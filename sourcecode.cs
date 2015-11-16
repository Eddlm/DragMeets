using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Globalization;


/// <summary>
/// CARS SHOULD OPEN THEIR TRUNK WHEN WAITING
/// CARS SHOULD DO BURNOUTS WHEN WAITING IN THE STARTING LINE (UPDATE)
/// </summary>




public class DragMeets : Script
{
    private XmlDocument dragmeetinfo = new XmlDocument();
    private XmlElement root;

    Ped winner;
    int DragSetupPatience = 0;
    Ped leftdriver;
    bool RaceInProgress = false;
    bool CanMeet = true;
    int meet_relationshipgroup = World.AddRelationshipGroup("Meet_relationshipgroup");
    Ped betondriver;
    Prop finish;
    Ped countdownped;
    List<Ped> drivers = new List<Ped>();
    List<Ped> waitingdrivers = new List<Ped>();
    private int interval5sec = 0; //        interval2sec = DateTime.Now;
    private int interval2sec = 0; //        interval2sec = DateTime.Now;
    private int interval5min = Game.GameTime; //        interval2sec = DateTime.Now;

    List<String> ambientscenarios = new List<String>();
    List<Vehicle> ambientvehicles = new List<Vehicle>();
    List<String> driverscenarios = new List<String>();

    List<Ped> crowd = new List<Ped>();
    List<Vector3> MeetsTriggers = new List<Vector3>();
    List<String> MeetsNames = new List<String>();
    List<String> NotifiedMeets = new List<String>();

    Vector3 disruptedpos;
    Model weaponAsset;
    string MeetName;
    float meetlimitrange = 600f;
    bool unpredictable_races = true;
    bool police_disbands_meets = true;

    string DragPhase = "MeetNotStarted"; //MeetNotStarted - MeetJustStarted - RacersGoingToStartPos - RaceCountDown - RaceInProgress - RaceFinished
    public DragMeets()
    {
        Tick += OnTick;
        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;

        ScriptSettings config = ScriptSettings.Load(@"scripts\DragMeets\config.ini");
        unpredictable_races = config.GetValue<bool>("SETTINGS", "unpredictable_races", true);
        police_disbands_meets = config.GetValue<bool>("SETTINGS", "police_disbands_meets", true);


        ambientscenarios.Add("WORLD_HUMAN_AA_SMOKE");
        ambientscenarios.Add("WORLD_HUMAN_CHEERING");
        ambientscenarios.Add("WORLD_HUMAN_DRINKING");
        ambientscenarios.Add("WORLD_HUMAN_PARTYING");
        ambientscenarios.Add("WORLD_HUMAN_SMOKING");

        driverscenarios.Add("WORLD_HUMAN_AA_SMOKE");
        driverscenarios.Add("WORLD_HUMAN_DRINKING");
        driverscenarios.Add("WORLD_HUMAN_PARTYING");
        driverscenarios.Add("WORLD_HUMAN_SMOKING");

        World.SetRelationshipBetweenGroups(Relationship.Companion, meet_relationshipgroup, Game.Player.Character.RelationshipGroup);

        if (police_disbands_meets)
        {
            World.SetRelationshipBetweenGroups(Relationship.Hate, meet_relationshipgroup, GetHash("COP"));
        }


        MeetsTriggers = GetMeetsTriggers();
        MeetsNames = GetMeetsNames();


    }

    List<Vector3> GetMeetsTriggers()
    {
        XmlDocument meettrigger = new XmlDocument();
        meettrigger.Load(@"scripts\\DragMeets\Meets\MeetTriggers.xml");
        // root = dragmeetinfo.DocumentElement;

        List<Vector3> positions = new List<Vector3>();
        Vector3 parkpos;
        XmlNodeList nodelist = meettrigger.SelectNodes("//Meets/*");
        foreach (XmlElement element1 in nodelist)
        {
            parkpos = new Vector3(int.Parse(element1.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
            positions.Add(parkpos);
        }
        return positions;
    }





    void HandleLamarNotifications()
    {
        if (!IsMeetLoaded() && CanMeet)
        {
            foreach (string meet in MeetsNames)
            {
                if (!NotifiedMeets.Contains(meet) && IsWithinTimeLimits(meet))
                {
                    NotifiedMeets.Add(meet);
                    GTA.Native.Function.Call(GTA.Native.Hash._SET_NOTIFICATION_TEXT_ENTRY, "STRING");
                    List<string> text = new List<string>();
                    text.Add("I've seen a bunch of cool cars racing near " + meet + ". ~n~JFYI");
                    text.Add("Wassup? Some cool rides at " + meet + ". ~n~ You goin? I might");
                    text.Add("Have you seen what's cookin in " + meet + "? Wonderful rides man, check them out!");
                    text.Add("You like betting? 'cause there's a Meet in " + meet + " right now!");
                    text.Add("Really good ambient here in " + meet + ". Give us a visit if you have time");
                    text.Add("Dude, you don't know what you're missing here on " + meet + ". Fuck!");
                    text.Add("Money passing hands at " + meet + ". U might be interested.");
                    text.Add("Some good rides burning tires on " + meet + ".");
                    text.Add("Looks like there's a Drag Meet at " + meet + " with bettings and shit. You going?~n~Easy money");
                    GTA.Native.Function.Call(GTA.Native.Hash._ADD_TEXT_COMPONENT_STRING, text[RandomInt(0, text.Count)]);
                    GTA.Native.Function.Call(GTA.Native.Hash._SET_NOTIFICATION_MESSAGE_CLAN_TAG_2, "CHAR_LAMAR", "CHAR_LAMAR", true, 9, "~b~Lamar", "~c~Drag meets", 1f, "", -1);
                    break;
                }
            }
        }
    }

    bool IsWithinTimeLimits(string MeetName)
    {
        XmlDocument meettrigger = new XmlDocument();
        meettrigger.Load(@"scripts\\DragMeets\Meets\MeetTriggers.xml");
        XmlNode data = meettrigger.SelectSingleNode("//Meet[@MeetName='" + MeetName + "']");
        int day = Function.Call<int>(Hash.GET_CLOCK_DAY_OF_WEEK);
        int hour = Function.Call<int>(Hash.GET_CLOCK_HOURS);
        bool correctday = false;
        bool correcthour = false;

        if ((bool.Parse(data.SelectSingleNode("Limits/WeekendsOnly").InnerText) && (day==6||day==0)) || !bool.Parse(data.SelectSingleNode("Limits/WeekendsOnly").InnerText))
        {
            correctday = true;
        }

        if ((bool.Parse(data.SelectSingleNode("Limits/NightOnly").InnerText) && (hour > 20 || hour < 7)) || !bool.Parse(data.SelectSingleNode("Limits/NightOnly").InnerText))
        {
            correcthour = true;
        }
        return correcthour && correctday;
    }

    List<String> GetMeetsNames()
    {
        //UI.Notify("getting meets names");

        XmlDocument meettrigger = new XmlDocument();
        meettrigger.Load(@"scripts\\DragMeets\Meets\MeetTriggers.xml");
        //root = dragmeetinfo.DocumentElement;

        List<string> positions = new List<string>();
        string parkpos;
        XmlNodeList nodelist = meettrigger.SelectNodes("//Meets/*");
        foreach (XmlElement element1 in nodelist)
        {
            parkpos = element1.GetAttribute("MeetName");
            //UI.Notify("got meet name" + parkpos);

            positions.Add(parkpos);
        }
        return positions;
    }
    void DisplayHelpTextThisFrame(string text)
    {
        Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, "STRING");
        Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, text);
        Function.Call(Hash._0x238FFE5C7B0498A6, 0, 0, 1, -1);
    }
    bool CanWeUse(Entity entity)
    {
        return entity != null && entity.Exists();
    }

    void SetRelationshipGroup(int Group1, int Group2, Relationship relationship)
    {
        World.SetRelationshipBetweenGroups(relationship, Group1, Group2);
        World.SetRelationshipBetweenGroups(relationship, Group2, Group1);
    }

    void CleanMeet()
    {

        foreach (Ped driver in drivers)
        {

            GetLastVehicle(driver).IsPersistent=false;
            driver.IsPersistent = false;

            Vector3 pos = driver.Position.Around(900f);
            TaskSequence RaceSequence = new TaskSequence();
            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, 0, GetLastVehicle(driver), pos.X, pos.Y, pos.Z, 300f, 262196, 2f);            
            RaceSequence.Close();
            driver.Task.PerformSequence(RaceSequence);
            RaceSequence.Dispose();

        }
        foreach (Ped driver in waitingdrivers)
        {
            GetLastVehicle(driver).IsPersistent = false;
            driver.IsPersistent = false;

            Vector3 pos = driver.Position.Around(900f);
            TaskSequence RaceSequence = new TaskSequence();
            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, 0, GetLastVehicle(driver), pos.X, pos.Y, pos.Z, 300f, 262196, 2f);
            RaceSequence.Close();
            driver.Task.PerformSequence(RaceSequence);
            RaceSequence.Dispose();


        }
        foreach (Ped driver in crowd)
        {

            driver.MarkAsNoLongerNeeded();
            if (CanWeUse(GetLastVehicle(driver)))
            {
                Vector3 pos = driver.Position.Around(900f);
                TaskSequence RaceSequence = new TaskSequence();
                Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, 0, GetLastVehicle(driver), pos.X, pos.Y, pos.Z, 300f, 262196, 2f);
                RaceSequence.Close();
                driver.Task.PerformSequence(RaceSequence);
                RaceSequence.Dispose();
            }
            else
            {
                TaskSequence RaceSequence = new TaskSequence();
                Function.Call(Hash.TASK_REACT_AND_FLEE_PED, 0, Game.Player.Character);
                RaceSequence.Close();
                driver.Task.PerformSequence(RaceSequence);
                RaceSequence.Dispose();
            }


        }
        crowd.Clear();
        drivers.Clear();
        waitingdrivers.Clear();

        if (CanWeUse(countdownped))
        {
            countdownped.MarkAsNoLongerNeeded();

            TaskSequence RaceSequence = new TaskSequence();
            Function.Call(Hash.TASK_REACT_AND_FLEE_PED, 0, Game.Player.Character);
            RaceSequence.Close();
            countdownped.Task.PerformSequence(RaceSequence);
            RaceSequence.Dispose();
        }
        foreach (Vehicle veh in ambientvehicles)
        {
            veh.MarkAsNoLongerNeeded();
        }
        finish.MarkAsNoLongerNeeded();
    }
    bool AnyCopNear(Vector3 pos, float radius)
    {
        return Function.Call<bool>(Hash.IS_COP_PED_IN_AREA_3D, pos.X - radius, pos.Y - radius, pos.Z - radius, pos.X + radius, pos.Y + radius, pos.Z + radius);
    }

    bool IsMeetLoaded()
    {
        return root != null;
    }

    bool IsWeekend()
    {
        int day= Function.Call<int>(Hash.GET_CLOCK_DAY_OF_WEEK);
        if (day==0 || day==6)
        {
            return true;
        }
        return false;
    }
    void OnTick(object sender, EventArgs e)
    {

        //Function.Call(GTA.Native.Hash._0x231C8F89D0539D8F, true,false);
        if (CanMeet)
        {
            if (!IsMeetLoaded())
            {

                int i = -1;
                foreach (Vector3 pos in MeetsTriggers)
                {
                    i++;
                    if (Game.Player.Character.IsInRangeOf(pos, 400f) && !AnyCopNear(pos, 100f))
                    {
                        MeetName = MeetsNames[i];

                        if (IsWithinTimeLimits(MeetName))
                        {
                            XmlDocument dragmeetinfo = new XmlDocument();
                            //UI.Notify(MeetName+" loaded");
                            dragmeetinfo.Load(@"scripts\\DragMeets\Meets\" + MeetName + ".xml"); // on key
                            root = dragmeetinfo.DocumentElement;
                            meetlimitrange = GetRaceDistanceLimit();
                            break;
                        }
                    }
                }
            }
        }
        else if (disruptedpos != null)
        {
            if (!Game.Player.Character.IsInRangeOf(disruptedpos, 400f))
            {
                CanMeet = true;
            }
        }

        if (IsMeetLoaded())
        {
            bool shouldinterrupt = false;
            if (AnyCopNear(GetWaitingZone(), 150f) && police_disbands_meets)
            {
               
                foreach (Ped ped in World.GetNearbyPeds(GetWaitingZone(),150f))
                {
                    if (ped.IsInCombat && ped.IsOnScreen)
                    {
                        shouldinterrupt = true;
                        break;
                    }
                }
            }

            if (RaceInProgress && winner == null)
            {
                foreach (Ped ped in drivers)
                {
                    if (GetLastVehicle(ped).IsInRangeOf(GetFinishLine(), GetFinishLineRadius()) && winner == null)
                    {
                        winner = ped;
                        break;
                    }
                }
            }
            if (!RaceInProgress)
            {
                foreach (Ped ped in drivers)
                {
                    if (Game.Player.Character.IsInRangeOf(ped.Position, 2.5f) && betondriver == null)
                    {
                        if (Game.Player.Money >= 200)
                        {
                            DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to bet ~g~$200~w~ on this " + GetLastVehicle(ped).FriendlyName + ".");
                            if (Game.IsControlJustPressed(2, GTA.Control.Context))
                            {
                                Function.Call(GTA.Native.Hash._PLAY_AMBIENT_SPEECH1, ped.Handle, "GENERIC_THANKS", "SPEECH_PARAMS_FORCE");
                                betondriver = ped;
                            }
                        }
                        else
                        {
                            DisplayHelpTextThisFrame("You don't have enough money to bet on this " + GetLastVehicle(ped).FriendlyName + ".");
                        }
                        break;
                    }
                }
            }
            if (Game.GameTime - interval5sec > 5000f)
            {
                    HandleDragRace();
                    if (RandomInt(0, 3) == 1)
                    {
                        HandleAmbient();
                    }
                interval5sec = Game.GameTime;

            }
            /*
            if (IsMeetLoaded() && Game.IsKeyPressed(Keys.X))
            {

                foreach (Vector3 pos in GetCrowdAreas())
                {
                    World.DrawMarker(MarkerType.VerticalCylinder, pos + new Vector3(0, 0, -1.2f), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(5, 5, 0.3f), Color.White);
                }
                World.DrawMarker(MarkerType.UpsideDownCone, GetStartingPoint("Left") + new Vector3(0, 0, 0.5f), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Yellow);
                World.DrawMarker(MarkerType.UpsideDownCone, GetStartingPoint("Right") + new Vector3(0, 0, 0.5f), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Yellow);
                World.DrawMarker(MarkerType.UpsideDownCone, GetFinishLine() + new Vector3(0, 0, 0.5f), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Yellow);
                foreach (Vector3 pos in GetSequenceToWaitingZone("Left"))
                {
                    World.DrawMarker(MarkerType.UpsideDownCone, pos+new Vector3(0, 0, +1), new Vector3(0, 0,0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Orange);
                }
                foreach (Vector3 pos in GetSequenceToWaitingZone("Right"))
                {
                    World.DrawMarker(MarkerType.UpsideDownCone, pos+ new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Orange);
                }
                foreach (Vector3 pos in GetSequenceToStartingPos("Left"))
                {
                    World.DrawMarker(MarkerType.UpsideDownCone, pos + new Vector3(0, 0, +1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Orange);
                }
                foreach (Vector3 pos in GetSequenceToStartingPos("Right"))
                {
                    World.DrawMarker(MarkerType.UpsideDownCone, pos + new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Orange);
                }
                World.DrawMarker(MarkerType.VerticalCylinder, GetWaitingZone() + new Vector3(0, 0, 0.5f), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(GetWaitingZoneRadius() * 2f, GetWaitingZoneRadius() * 2f, 1), Color.Yellow);

            }
            */
            if (!Game.Player.Character.IsInRangeOf(GetStartingLine(), meetlimitrange) || shouldinterrupt)
            {
                if (shouldinterrupt)
                {
                    disruptedpos = GetWaitingZone();
                    CanMeet = false;

                    if (CanWeUse(GetLastVehicle(Game.Player.Character)) && Game.Player.Character.IsInRangeOf(GetLastVehicle(Game.Player.Character).Position, 20f))
                    {
                        Game.Player.WantedLevel = 2;
                    }
                }
                CleanMeet();
                DragPhase = "MeetNotStarted";
                //UI.Notify("Meet stopped");
                dragmeetinfo = null;
                root = null;
            }
        }
        if (Game.GameTime - interval5min > 20000f)
        {
            HandleLamarNotifications();
            interval5min = Game.GameTime + (RandomInt(190000,120000)) ;
        }

        /*
        if (Game.IsKeyPressed(Keys.ShiftKey))
        {
            Function.Call(Hash._SET_VEHICLE_ENGINE_TORQUE_MULTIPLIER, GetLastVehicle(Game.Player.Character), 90000f);          
        }
        */

    }
    void OnKeyDown(object sender, KeyEventArgs e)
    {


    }
    void OnKeyUp(object sender, KeyEventArgs e)
    {

        if (IsMeetLoaded() && e.KeyCode == Keys.Space && CanWeUse(Game.Player.Character.CurrentVehicle) && Game.Player.Character.IsStopped)
        {
            List<string> text = new List<string>();
            Vehicle veh = Game.Player.Character.CurrentVehicle;
            text.Add("<Driver DriverName='"+veh.FriendlyName+"'>");
            text.Add("<PedModel>random</PedModel>");
            text.Add("<RacesAllowed>");
            text.Add("<Kind>Gang</Kind>");
            text.Add("<Kind>Street</Kind>");
            text.Add("<Kind>Country</Kind>");
            text.Add("<Kind>Normal</Kind>");
            text.Add("</RacesAllowed>");
            text.Add("<Vehicle>");
            text.Add("<Model>"+ veh.DisplayName+ "</Model>");
            text.Add("<Colors>");
            text.Add("<PrimaryColor>"+ ((int)veh.PrimaryColor).ToString()+ "</PrimaryColor>");
            text.Add("<SecondaryColor>"+ ((int)veh.SecondaryColor).ToString()+ "</SecondaryColor>");
            text.Add("<PearlescentColor>"+ ((int)veh.PearlescentColor).ToString()+ "</PearlescentColor>");
            text.Add("<RimColor>"+ ((int)veh.RimColor).ToString()+ "</RimColor>");
            int dash = 0;
            int trim = 0;
            unsafe
            {
                Function.Call((Hash)0x7D1464D472D32136, veh, &trim);
                Function.Call((Hash)0xB7635E80A5C31BFF, veh, &dash);
            }
            text.Add("<TrimColor>" + trim + "</TrimColor>");

            text.Add("<DashColor>" + dash + "</DashColor>");

            text.Add("<NeonColor>");
            text.Add("<Color>");
            text.Add("<R>"+ veh.NeonLightsColor.R+ "</R>");
            text.Add("<G>" + veh.NeonLightsColor.G + "</G>");
            text.Add("<B>" + veh.NeonLightsColor.B + "</B>");
            text.Add("</Color>");
            text.Add("</NeonColor>");
            
            text.Add("</Colors>");
            text.Add("<LicensePlate>"+ Function.Call<int>(Hash.GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, veh) + "</LicensePlate>");
            text.Add("<LicensePlateText>"+ veh.NumberPlate+"</LicensePlateText>");
            text.Add("<Components>");

            for (int i = 0; i <= 25; i++)
            {
                if (Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, veh, i))
                {
                    text.Add("<Component ComponentIndex='" + i + "'>" + "0" + "</Component>");

                }
                else
                {
                    text.Add("<Component ComponentIndex='" + i + "'>" + "-1" + "</Component>");
                }
            }
            text.Add("</Components>");

            text.Add("<ModToggles>");
            for (int i = 0; i <= 25; i++)
            {
                if (Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, veh, i))
                {
                    text.Add("<Toggle ToggleIndex='" + i + "'>" + "true" + "</Toggle>");
                }
            }
            text.Add("</ModToggles>");

            text.Add("<WheelType>"+((int)veh.WheelType).ToString()+"</WheelType>");
            text.Add("<Mods>");

            for (int i = 0; i <= 500; i++)
            {
                if (Function.Call<int>(Hash.GET_VEHICLE_MOD, veh, i) != -1)
                {
                    text.Add("<Mod ModIndex='" + i + "'>" + Function.Call<int>(Hash.GET_VEHICLE_MOD, veh, i).ToString() + "</Mod>");

                }
            }
            text.Add("</Mods>");
            text.Add("<CustomTires>false</CustomTires>");

            text.Add("<WindowsTint>"+(int)veh.WindowTint+"</WindowsTint>");
            text.Add("<Neons>");
            text.Add("<Left>" + Function.Call<bool>(Hash._IS_VEHICLE_NEON_LIGHT_ENABLED, veh, 0).ToString() + "</Left>");
            text.Add("<Right>" + Function.Call<bool>(Hash._IS_VEHICLE_NEON_LIGHT_ENABLED, veh, 1).ToString() + "</Right>");
            text.Add("<Front>" + Function.Call<bool>(Hash._IS_VEHICLE_NEON_LIGHT_ENABLED, veh, 2).ToString() + "</Front>");
            text.Add("<Back>" + Function.Call<bool>(Hash._IS_VEHICLE_NEON_LIGHT_ENABLED, veh, 3).ToString() + "</Back>");
            text.Add("</Neons>");
            text.Add("</Vehicle>");
            text.Add("</Driver>");

            File.WriteAllLines(@"scripts\\DragMeets\vehicleinfo.txt", text);
            UI.Notify(veh.FriendlyName+" info saved to /scripts/DragMeets/vehicleinfo.txt");
        }
    }


    void FixStuckDrivers()
    {
        foreach (Ped ped in waitingdrivers)
        {
            if ((Function.Call<bool>(Hash.IS_VEHICLE_ON_ALL_WHEELS, GetLastVehicle(ped)) == false && GetLastVehicle(ped).IsStopped) || Function.Call<bool>(Hash.IS_ENTITY_IN_WATER, GetLastVehicle(ped)) == true)
            {
                GetLastVehicle(ped).PlaceOnGround();
            }
            if (!GetLastVehicle(ped).IsInRangeOf(GetWaitingZone(), GetWaitingZoneRadius()) && IsIdle(ped))
            {
                DriveThisPedToTheMeetingArea(ped);
            }
        }

        foreach (Ped ped in drivers)
        {
            if ((Function.Call<bool>(Hash.IS_VEHICLE_ON_ALL_WHEELS, GetLastVehicle(ped)) == false && GetLastVehicle(ped).IsStopped) || Function.Call<bool>(Hash.IS_ENTITY_IN_WATER, GetLastVehicle(ped)) == true)
            {
                GetLastVehicle(ped).PlaceOnGround();
            }
        }
    }

    void HandleAmbient()
    {
        foreach (Ped ped in crowd)
        {
            if (ped.IsOnFoot && !ped.IsInCombat && Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, ped) == -1)
            {
                //UI.Notify("Scenario applied, " +ped.Handle.ToString());
                Vector3 faceto = GetStartingLine();
                TaskSequence dd = new TaskSequence();
                Function.Call(GTA.Native.Hash.TASK_TURN_PED_TO_FACE_COORD, 0, faceto.X, faceto.Y, faceto.Z, 2000);
                Function.Call(GTA.Native.Hash.TASK_START_SCENARIO_IN_PLACE, 0, GetRandomAmbientScenario(), -1, false);
                dd.Close();
                ped.Task.PerformSequence(dd);
                dd.Dispose();
            }

        }

        foreach (Ped ped in waitingdrivers)
        {
            Function.Call(Hash.SET_SCENARIO_PEDS_TO_BE_RETURNED_BY_NEXT_COMMAND, true);
            if (ped.IsOnFoot && !ped.IsInCombat && Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, ped) == -1 && Function.Call<bool>(Hash.IS_PED_USING_ANY_SCENARIO, countdownped) == false)
            {
                Vector3 faceto = GetWaitingZone();
                TaskSequence dd = new TaskSequence();
                Function.Call(GTA.Native.Hash.TASK_TURN_PED_TO_FACE_COORD, 0, faceto.X, faceto.Y, faceto.Z, 2000);
                Function.Call(GTA.Native.Hash.TASK_START_SCENARIO_IN_PLACE, 0, GetRandomAmbientDriverScenario(), 0, false);
                dd.Close();
                ped.Task.PerformSequence(dd);
                dd.Dispose();
            }
        }
    }

    void UnlockDriver(Ped ped)
    {
        ped.Task.ClearAll();        
        TaskSequence dd = new TaskSequence();
        Function.Call(GTA.Native.Hash.TASK_ENTER_VEHICLE, ped, GetLastVehicle(ped), -1, -1, 2f, 3, false);
        dd.Close();
        ped.Task.PerformSequence(dd);
        dd.Dispose();        
        GetLastVehicle(ped).FreezePosition = false;
        Function.Call(GTA.Native.Hash.SET_VEHICLE_BURNOUT, GetLastVehicle(ped), false);
    }

    bool IsInBurnout(Ped ped)
    {
        return GetLastVehicle(ped).IsInBurnout();
    }

    void HandleDriverShowOff()
    {
        UI.Notify("Driver showoff initiated");
        foreach (Ped ped in drivers)
        {
            if (ped.IsInRangeOf(GetStartingLine(), GetStartingLineRadius()))
            {
                if (IsIdle(ped))
                {
                    if (IsInBurnout(ped))
                    {
                        UI.Notify("Driver showoff UNLOCK");
                        UnlockDriver(ped);
                    }
                    else
                    {
                        Vector3 pos = ped.Position + ped.ForwardVector * 30;
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_BURNOUT, GetLastVehicle(ped), true);
                        TaskSequence dd2 = new TaskSequence();
                        Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, 0, GetLastVehicle(ped), pos.X, pos.Y, pos.Z, 100f, true, GetLastVehicle(ped).GetHashCode(), 16777216, 5f, 7f);
                        dd2.Close();
                        ped.Task.PerformSequence(dd2);
                        dd2.Dispose();
                        /*
                        TaskSequence dd = new TaskSequence();
                        Function.Call(Hash.TASK_PAUSE, 0, 2000); dd.Close();
                        ped.Task.PerformSequence(dd);
                        dd.Dispose();
                        */
                    }
                }

            }

        }
    }


    bool IsPlayerNearStartingLine()
    {
        if (Game.Player.Character.IsInRangeOf(GetStartingLine(), 150f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool IsPlayerNearFinishgLine()
    {
        if (Game.Player.Character.IsInRangeOf(GetFinishLine(), 150f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsBarrelWhereItShould()
    {
        if (finish.IsInRangeOf(GetFinishLine(), 10f))
        {
            return true;
        }
        else
        {
            finish.Position = GetFinishLine();
            return false;
        }
    }
    bool CountDownGuyIsInPos()
    {
        if (countdownped.IsInRangeOf(GetCountDownGuyPos(), 2f))
        {
            return true;
        }
        else
        {
            if (IsPlayerNearStartingLine())
            {
                Vector3 pos = GetCountDownGuyPos();
                Vector3 faceto = GetStartingLine();
                TaskSequence RaceSequence = new TaskSequence();
                Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, 0, pos.X, pos.Y, pos.Z, 1.0f, -1, 0.0f, 0, 0.0f);
                Function.Call(GTA.Native.Hash.TASK_TURN_PED_TO_FACE_COORD, 0, faceto.X, faceto.Y, faceto.Z, 2000);
                RaceSequence.Close();
                countdownped.Task.PerformSequence(RaceSequence);
                RaceSequence.Dispose();
            }
            else
            {
                countdownped.Position = GetCountDownGuyPos();
            }
            return false;
        }
    }
    string GetRandomAmbientScenario()
    {
        return ambientscenarios[RandomInt(0, ambientscenarios.Count)];
    }
    string GetRandomAmbientDriverScenario()
    {
        return driverscenarios[RandomInt(0, driverscenarios.Count)];
    }
    void HandleDragRace()
    {
        FixStuckDrivers();


        switch (DragPhase)
        {
            case "MeetNotStarted":
                {
                    SpawnCountDownPed();

                    SpawnCrowd();
                    SpawnDriversFromFile();
                    SpawnParkedVehicles();
                    DragPhase = "MeetJustStarted";
                    NotifiedMeets.Clear();
                    HandleAmbient();

                    finish = World.CreateProp("prop_offroad_barrel01", GetFinishLine()+ new Vector3(0, 0,3), new Vector3(0, 0, GetRaceHeading()), false, true);
                    finish.IsPersistent = true;
                    Function.Call(GTA.Native.Hash._0x0DC7CABAB1E9B67E, finish, true); //Load Collision
                    return;
                }

            case "MeetJustStarted":
                {


                    foreach (Vehicle veh in ambientvehicles)
                    {
                        if (veh.DisplayName == "MOONBEAM2" || veh.DisplayName == "FACTION2")
                        {
                            SetRadioLoud(veh);
                        }
                    }
                    //AddNewCarsToMeet(); //Doesn't work properly for now
                    if (drivers.Count == 2)
                    {
                        CountDownGuyIsInPos();
                        DragPhase = "RacersGoingToStartPos";
                        leftdriver = drivers[0];
                    }
                    else
                    {
                        GetNextDriverPair();
                    }
                    return;
                }

            case "RacersGoingToStartPos":
                {
                    if (AreDriversInStartingLine() == true)
                    {
                        if (CountDownGuyIsInPos() == true)
                        {
                            DragSetupPatience = 0;
                            DragPhase = "RaceCountDown";

                            if (CanWeUse(countdownped))
                            {
                                Vector3 faceto = GetStartingLine();
                                TaskSequence dd = new TaskSequence();
                                Function.Call(GTA.Native.Hash.TASK_SWAP_WEAPON, 0, true);
                                Function.Call(GTA.Native.Hash.TASK_TURN_PED_TO_FACE_COORD, 0, faceto.X, faceto.Y, faceto.Z, 2000);
                                dd.Close();
                                countdownped.Task.PerformSequence(dd);
                                dd.Dispose();
                            }
                        }
                    }
                    else
                    {
                        if (IsPlayerNearStartingLine() || countdownped.IsOnScreen)
                        {
                            DragSetupPatience++;

                            if (DragSetupPatience > 15)
                            {
                                DragSetupPatience = 0;
                                foreach (Ped driver in drivers)
                                {
                                    Function.Call(GTA.Native.Hash.SET_VEHICLE_FIXED, GetLastVehicle(driver));
                                    Function.Call(GTA.Native.Hash.SET_VEHICLE_DEFORMATION_FIXED, GetLastVehicle(driver));
                                    if (driver.Handle == drivers[0].Handle)
                                    {
                                        if (!GetLastVehicle(driver).IsInRangeOf(GetStartingPoint("Left"), 3f))
                                        {
                                            GetLastVehicle(driver).Position = GetSequenceToStartingPos("Left")[0];
                                        }
                                    }
                                    else
                                    {
                                        if (!GetLastVehicle(driver).IsInRangeOf(GetStartingPoint("Right"), 3f))
                                        {
                                            GetLastVehicle(driver).Position = GetSequenceToStartingPos("Right")[0];
                                        }
                                    }
                                    GetLastVehicle(driver).Heading = GetRaceHeading();
                                    driver.Task.EnterVehicle(GetLastVehicle(driver), VehicleSeat.Driver, -1);
                                }

                            }
                        }
                        else
                        {

                            foreach (Ped driver in drivers)
                            {
                                Function.Call(GTA.Native.Hash.SET_VEHICLE_FIXED, GetLastVehicle(driver));
                                Function.Call(GTA.Native.Hash.SET_VEHICLE_DEFORMATION_FIXED, GetLastVehicle(driver));
                                driver.Task.ClearAll();

                                if (driver.CurrentVehicle == null)
                                {
                                    driver.SetIntoVehicle(GetLastVehicle(driver), VehicleSeat.Driver);
                                }

                                if (driver.Handle == drivers[0].Handle)
                                {
                                    if (!GetLastVehicle(driver).IsInRangeOf(GetStartingPoint("Left"), 3f))
                                    {
                                        GetLastVehicle(driver).Position = GetStartingPoint("Left");
                                    }
                                }
                                else
                                {
                                    if (!GetLastVehicle(driver).IsInRangeOf(GetStartingPoint("Right"), 3f))
                                    {
                                        GetLastVehicle(driver).Position = GetStartingPoint("Right");
                                    }
                                }
                                GetLastVehicle(driver).Heading = GetRaceHeading();
                                driver.Task.EnterVehicle(GetLastVehicle(driver), VehicleSeat.Driver, -1);
                            }
                        }
                    }
                    return;
                }
            case "RaceCountDown":
                {
                    IsBarrelWhereItShould();
                    if (AreDriversInStartingLine() == true && !Game.Player.Character.IsInRangeOf(GetStartingLine(), GetStartingLineRadius() / 2f))
                    {
                        if (CanWeUse(countdownped))
                        {
                            Vector3 aim = countdownped.Position + countdownped.ForwardVector + countdownped.UpVector * 2;
                            Vector3 faceto = GetFinishLine();
                            TaskSequence dd = new TaskSequence();
                            Function.Call(GTA.Native.Hash.TASK_SHOOT_AT_COORD, 0, aim.X, aim.Y, aim.Z, 500, GetHash("FIRING_PATTERN_BURST_FIRE_PISTOL"));
                            Function.Call(GTA.Native.Hash.TASK_TURN_PED_TO_FACE_COORD, 0, faceto.X, faceto.Y, faceto.Z, 2000);
                            dd.Close();
                            countdownped.Task.PerformSequence(dd);
                            dd.Dispose();
                        }
                        string side;
                        foreach (Ped driver in drivers)
                        {
                            if (GetLastVehicle(driver).Heading - GetRaceHeading() > 20f)
                            {
                                GetLastVehicle(driver).Heading = GetRaceHeading();
                            }
                            if (unpredictable_races)
                            {
                                if (RandomInt(0,2)==3)
                                {
                                    Function.Call(Hash._SET_VEHICLE_ENGINE_POWER_MULTIPLIER, GetLastVehicle(driver), RandomFloat(0, 1));
                                    UI.Notify(RandomFloat(0, 1).ToString());
                                }
                                else
                                {
                                    Function.Call(Hash._SET_VEHICLE_ENGINE_POWER_MULTIPLIER, GetLastVehicle(driver), RandomInt(1, 10));

                                }
                            }
                            float speed = 80f;
                            if (driver == drivers[0])
                            {
                                side = "Left";
                            }
                            else
                            {
                                side = "Right";
                            }
                            TaskSequence RaceSequence = new TaskSequence();
                            foreach (Vector3 pos in GetSequenceToEnd(side))
                            {
                                if (unpredictable_races)
                                {
                                    Function.Call(Hash.TASK_PAUSE, 0, RandomInt(0, 800));
                                }
                                Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, 0, GetLastVehicle(driver), pos.X, pos.Y, pos.Z, speed, 16777236, 5f);
                                speed = 10;
                            }
                            RaceSequence.Close();
                            driver.Task.PerformSequence(RaceSequence);
                            RaceSequence.Dispose();
                        }
                        DragPhase = "RaceInProgress";
                        RaceInProgress = true;
                    }
                    return;
                }
            case "RaceInProgress":
                {
                    if (HaveDriversFinished())
                    {
                        Function.Call(Hash._SET_VEHICLE_ENGINE_POWER_MULTIPLIER, GetLastVehicle(drivers[0]), 0f);
                        Function.Call(Hash._SET_VEHICLE_ENGINE_POWER_MULTIPLIER, GetLastVehicle(drivers[1]), 0f);
                        if (IsPlayerNearFinishgLine() || IsPlayerNearStartingLine())
                        {
                            UI.Notify("~g~The " + GetLastVehicle(winner).FriendlyName + " won!");
                        }
                        DriveRacersToTheWaitingArea();
                        DragPhase = "RaceFinished";
                        RaceInProgress = false;
                        Vector3 faceto = GetStartingLine();

                        TaskSequence countdownguysequence = new TaskSequence();
                        Function.Call(GTA.Native.Hash.TASK_TURN_PED_TO_FACE_COORD, 0, faceto.X, faceto.Y, faceto.Z, 2000);
                        Function.Call(GTA.Native.Hash.TASK_RELOAD_WEAPON, 0, true);
                        countdownguysequence.Close();
                        countdownped.Task.PerformSequence(countdownguysequence);
                        countdownguysequence.Dispose();
                        if (CanWeUse(betondriver))
                        {
                            if (winner.Handle == betondriver.Handle)
                            {
                                UI.Notify("~g~You won the bet!");
                                Game.Player.Money = Game.Player.Money + 200;
                                PedsCongratulatePlayer();
                            }
                            else
                            {
                                UI.Notify("~r~You lost the bet.");
                                Game.Player.Money = Game.Player.Money - 200;
                            }
                            betondriver = null;
                        }
                        GetNextDriverPair();
                        AreDriversInStartingLine();
                    }
                    return;
                }
            case "RaceFinished":
                {
                    winner = null;
                    DragPhase = "MeetJustStarted";
                    return;
                }
        }
    }

    void PedsCongratulatePlayer()
    {
        foreach (Ped ped in World.GetNearbyPeds(Game.Player.Character, 10f))
        {
            Function.Call(GTA.Native.Hash._PLAY_AMBIENT_SPEECH1, ped.Handle, "ROLLERCOASTER_CHAT_EXCITED", "SPEECH_PARAMS_FORCE_FRONTEND");

        }
    }
    void SetRadioLoud(Vehicle veh)
    {
        Function.Call(GTA.Native.Hash.SET_VEHICLE_ENGINE_ON, veh, true, true);
        Function.Call(GTA.Native.Hash.SET_VEHICLE_RADIO_ENABLED, veh, true);
        Function.Call(GTA.Native.Hash.SET_VEHICLE_RADIO_LOUD, veh, true);
        Function.Call(GTA.Native.Hash.SET_VEH_RADIO_STATION, veh, GetMeetRadio());
    }
    int GetHash(string thing)
    {
        return Function.Call<int>(GTA.Native.Hash.GET_HASH_KEY, thing);
    }

    int RandomInt(int start, int end)
    {
        return Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, start, end);
    }

    float RandomFloat(float start, float end)
    {
        return Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, start, end);
    }


    void GetNextDriverPair()
    {
        List<Ped> driverstoremove = new List<Ped>();
        List<Ped> driverstoadd = new List<Ped>();
        List<Ped> pickeddrivers = new List<Ped>();
        drivers.Clear();

        foreach (Vehicle veh in World.GetNearbyVehicles(GetWaitingZone(), GetWaitingZoneRadius()))
        {
            Ped dude = VehiclePropietary(veh);
            if ( CanWeUse(dude) && dude != Game.Player.Character && IsInWaitingList(dude) && !IsInDriversList(dude))
            {
                Function.Call(GTA.Native.Hash.SET_VEHICLE_FIXED, veh);
                Function.Call(GTA.Native.Hash.SET_VEHICLE_DEFORMATION_FIXED, veh);
                driverstoadd.Add(dude);
            }
        }

        if (driverstoadd.Count > 1)
        {
            var copyDrivers = new List<Ped>(driverstoadd);
            for (int _ = 0; _ < 2; _++)
            {
                int i = RandomInt(0, copyDrivers.Count);
                pickeddrivers.Add(copyDrivers[i]);
                copyDrivers.RemoveAt(i);
            }

        }




        if (pickeddrivers.Count == 2)
        {
            if(IsPlayerNearFinishgLine() || IsPlayerNearStartingLine())
            {
                UI.Notify(GetLastVehicle(pickeddrivers[0]).FriendlyName + " vs " + GetLastVehicle(pickeddrivers[1]).FriendlyName);

            }
            foreach (Ped driver in pickeddrivers)
            {
                AddToDriversList(driver);
                RemoveFromWaitingList(driver);
                driver.Task.ClearAll();
            }
        }

    }

    bool AreDriversInStartingPos()
    {
        int driversok = 0;
        foreach (Ped driver in drivers)
        {
            if (Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, driver) == -1 && driver.IsInRangeOf(GetStartingLine(), GetStartingLineRadius()))
            {
                driversok++;
            }
        }

        if (driversok == 2)
        {
            return true;
        }

        return false;
    }

    bool AreDriversIdle()
    {
        int driversok = 0;
        foreach (Ped driver in drivers)
        {
            if (IsIdle(driver))
            {
                driversok++;
            }
        }

        if (driversok == 2)
        {
            return true;
        }

        return false;
    }

    void DriveThisManToItsStartingPos(Ped driver)
    {
        string side;
        if (driver.Handle == drivers[0].Handle)
        {
            side = "Left";
        }
        else
        {
            side = "Right";
        }

        Vector3 waitingcenter = GetStartingLine();
        TaskSequence RaceSequence = new TaskSequence();
        Function.Call(GTA.Native.Hash.TASK_ENTER_VEHICLE, 0, GetLastVehicle(driver), -1, -1, 2f, 1, false);
/*
        if (Function.Call<Vector3>(GTA.Native.Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, GetLastVehicle(driver), waitingcenter.X, waitingcenter.Y, waitingcenter.Z).Y < 0)
        {
            RaycastResult back = RaycastDrive(GetLastVehicle(driver).Position + (GetLastVehicle(driver).ForwardVector * -3), GetLastVehicle(driver).Position + (GetLastVehicle(driver).ForwardVector * -20));

            Vector3 pos = GetLastVehicle(driver).Position + (GetLastVehicle(driver).ForwardVector * -10);
            if (back.DitHitAnything)
            {
               pos = back.HitCoords;
            }
            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, 0, GetLastVehicle(driver), pos.X, pos.Y, pos.Z, 4f, 16778240, 2f);
        }
        */
        foreach (Vector3 pos in GetSequenceToStartingPos(side))
        {
            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, 0, GetLastVehicle(driver), pos.X, pos.Y, pos.Z, 4f, 4194357, 3f);
        }
        Vector3 startpoint = GetStartingPoint(side);
        Function.Call(GTA.Native.Hash.TASK_VEHICLE_PARK, 0, GetLastVehicle(driver), startpoint.X, startpoint.Y, startpoint.Z, GetRaceHeading(), 1, 10f, true);
        RaceSequence.Close();
        driver.Task.PerformSequence(RaceSequence);
        RaceSequence.Dispose();
    }


    bool AreDriversInStartingLine()
    {
        int driversok = 0;
        foreach (Ped driver in drivers)
        {
            if (IsIdle(driver))
            {
                if (driver.IsInRangeOf(GetStartingLine(), GetStartingLineRadius()) && CanWeUse(driver.CurrentVehicle))
                {
                    driversok++;
                }
                else
                {
                    DriveThisManToItsStartingPos(driver);
                }
            }
        }

        if (driversok == 2)
        {
            return true;
        }

        return false;
    }


    bool IsIdle(Ped ped)
    {
        Function.Call(Hash.SET_SCENARIO_PEDS_TO_BE_RETURNED_BY_NEXT_COMMAND, true);

        if (Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, ped) == -1 && !ped.IsInCombat)
        {
            return true;
        }
        if (Function.Call<bool>(Hash.IS_PED_USING_ANY_SCENARIO, ped) == true)
        {
            return true;
        }

        return false;
    }

    bool HaveDriversFinished()
    {
        int driversok = 0;
        foreach (Ped driver in drivers)
        {
            if (Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, driver) == -1)
            {
                driversok++;
            }
        }
        if (driversok == 2)
        {
            return true;
        }
        return false;
    }


    void PreparePed(Ped ped)
    {
        if (!ped.IsPersistent)
        {
            ped.IsPersistent = true;
        }

        ped.AlwaysKeepTask = true;
        ped.IsInvincible = true;
        ped.BlockPermanentEvents = false;
        Function.Call(GTA.Native.Hash.SET_PED_DIES_INSTANTLY_IN_WATER, ped, false);
        Function.Call(GTA.Native.Hash.SET_PED_DIES_IN_SINKING_VEHICLE, ped, false);
        Function.Call(GTA.Native.Hash.SET_PED_GET_OUT_UPSIDE_DOWN_VEHICLE, ped, false);

        Function.Call(GTA.Native.Hash.SET_DRIVER_ABILITY, ped, 100f);
        Function.Call(GTA.Native.Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 46,true);
        ped.Weapons.Give(WeaponHash.MicroSMG, -1, false, true);
        ped.RelationshipGroup = meet_relationshipgroup;
        /*
        ped.AddBlip();
        ped.CurrentBlip.Scale = 0.4f;
        ped.CurrentBlip.Color = BlipColor.Blue;
        ped.CurrentBlip.IsShortRange = true;
        */
    }


    Ped VehiclePropietary(Vehicle veh)
    {
        Ped pedfinal = null;

        foreach (Ped ped in World.GetAllPeds())
        {
            if (veh.Handle == GetLastVehicle(ped).Handle)
            {
                pedfinal = ped;
                break;
            }
        }
        if (pedfinal == null)
        {
            foreach (Ped ped in World.GetAllPeds())
            {
                if (ped.Handle == Function.Call<Ped>(GTA.Native.Hash.GET_LAST_PED_IN_VEHICLE_SEAT, veh, -1).Handle)
                {
                    pedfinal = ped;
                    break;
                }
            }
        }
        return pedfinal;
    }
    void AddNewCarsToMeet()
    {
        foreach (Vehicle veh in World.GetNearbyVehicles(GetWaitingZone(), GetWaitingZoneRadius()))
        {
            if (VehiclePropietary(veh) == Game.Player.Character && Game.Player.Character.CurrentVehicle!=veh)
            {
                Ped dude = GTA.Native.Function.Call<Ped>(GTA.Native.Hash.CREATE_RANDOM_PED, veh.Position.X, veh.Position.Y, veh.Position.Z + 5f);
                PreparePed(dude);
                dude.SetIntoVehicle(veh, VehicleSeat.Driver);
                SetRadioLoud(veh);
                UI.Notify(veh.FriendlyName + "~g~'s driver has joined the Meet.");
                AddToWaitingList(dude);
                PrepareVeh(veh);
                DriveThisPedToTheMeetingArea(dude);
            }
        }
    }
    void PrepareVeh(Vehicle veh)
    {
        veh.IsInvincible = true;
        veh.IsPersistent = true;
        //Function.Call(Hash.SET_ENTITY_PROOFS, veh, true, true, true, true, true, true, true, true);

        foreach (Vehicle veh2 in World.GetNearbyVehicles(veh.Position, 3f))
        {
            if (veh.Handle != veh2.Handle)
            {
                veh.Position = veh.Position.Around(5f) + veh.UpVector * 3;
                veh.PlaceOnGround();
            }
        }
    }

    void SpawnCountDownPed()
    {
        Vector3 zone = GetWaitingZone().Around(3f);

        countdownped = World.CreatePed((Model)"a_f_y_bevhills_03", zone, 0f);
        countdownped.Weapons.Give((WeaponHash)GetHash("WEAPON_FLAREGUN"), -1, true, true);
        countdownped.AlwaysKeepTask = true;
        countdownped.IsInvincible = true;
        Function.Call(GTA.Native.Hash.SET_PED_SHOOT_RATE, countdownped, 1000);

        Function.Call(GTA.Native.Hash.SET_DRIVER_ABILITY, countdownped, 100f);
        Function.Call(GTA.Native.Hash.SET_PED_COMBAT_ATTRIBUTES, countdownped, 46, true);
        countdownped.RelationshipGroup = meet_relationshipgroup;
    }


    void SpawnCrowd()
    {
        Ped dude;
        int number = 0;
        foreach (Vector3 pos in GetCrowdAreas())
        {
            number++;
            for (int i = 0; i < GetCrowdAreaRadius(number) ; i++)
            {
                Vector3 pos2 = pos.Around((float)RandomInt(0, GetCrowdAreaRadius(number)));
                dude = GTA.Native.Function.Call<Ped>(GTA.Native.Hash.CREATE_RANDOM_PED, pos2.X, pos2.Y, pos2.Z);
                PreparePed(dude);
                crowd.Add(dude);
            }
        }
    }
    

    void SpawnParkedVehicles()
    {

        List<Vector3> positions = new List<Vector3>();
        List<float> headings = new List<float>();
        foreach (float pos in GetAmbientVehicleParkZonesHeading())
        {
            headings.Add(pos);

        }
        foreach (Vector3 pos in GetAmbientVehicleParkZones())
        {
            positions.Add(pos);
        }

        for (int i = 0; i < positions.Count; i++)
        {
            XmlDocument document = new XmlDocument();
            document.Load(@"scripts\\DragMeets\Drivers\Drivers.xml");
            XmlElement docroot = document.DocumentElement;
            while (docroot == null || root==null)
            {
                Script.Wait(0);
            }
            XmlNodeList nodelist = docroot.SelectNodes("//AmbientVehicles/*");
            XmlNode driver = nodelist.Item(RandomInt(0, nodelist.Count));

            Vehicle veh = World.CreateVehicle(driver.SelectSingleNode("Vehicle/Model").InnerText, positions[i], headings[i]);

            if (driver.SelectSingleNode("Vehicle/WindowsTint") != null)
            {
                Function.Call(Hash.SET_VEHICLE_MOD_KIT, veh, 0);
                veh.WheelType = (VehicleWheelType)int.Parse(driver.SelectSingleNode("Vehicle/WheelType").InnerText, CultureInfo.InvariantCulture);
                veh.PrimaryColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/Colors/PrimaryColor").InnerText, CultureInfo.InvariantCulture);
                veh.SecondaryColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/Colors/SecondaryColor").InnerText, CultureInfo.InvariantCulture);
                veh.PearlescentColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/Colors/PearlescentColor").InnerText, CultureInfo.InvariantCulture);
                Function.Call((Hash)0xF40DD601A65F7F19, veh.Handle, int.Parse(driver.SelectSingleNode("Vehicle/Colors/TrimColor").InnerText, CultureInfo.InvariantCulture));
                Function.Call((Hash)0x6089CDF6A57F326C, veh.Handle, int.Parse(driver.SelectSingleNode("Vehicle/Colors/DashColor").InnerText, CultureInfo.InvariantCulture));
                veh.RimColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/Colors/RimColor").InnerText, CultureInfo.InvariantCulture);
                veh.NumberPlate = driver.SelectSingleNode("Vehicle/LicensePlateText").InnerText;
                Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, veh, int.Parse(driver.SelectSingleNode("Vehicle/LicensePlate").InnerText, CultureInfo.InvariantCulture));
                veh.WindowTint = (VehicleWindowTint)int.Parse(driver.SelectSingleNode("Vehicle/WindowsTint").InnerText, CultureInfo.InvariantCulture);
                if (driver.SelectSingleNode("Vehicle/Colors/SmokeColor") != null)
                {
                    Color color = Color.FromArgb(255, int.Parse(driver.SelectSingleNode("Vehicle/Colors/SmokeColor/Color/R").InnerText), int.Parse(driver.SelectSingleNode("Vehicle/Colors/SmokeColor/Color/G").InnerText), int.Parse(driver.SelectSingleNode("Vehicle/Colors/SmokeColor/Color/B").InnerText));
                    veh.TireSmokeColor = color;
                }
                if (driver.SelectSingleNode("Vehicle/Colors/NeonColor") != null)
                {
                    Color color = Color.FromArgb(255, int.Parse(driver.SelectSingleNode("Vehicle/Colors/NeonColor/Color/R").InnerText), int.Parse(driver.SelectSingleNode("Vehicle/Colors/NeonColor/Color/G").InnerText), int.Parse(driver.SelectSingleNode("Vehicle/Colors/NeonColor/Color/B").InnerText));
                    veh.NeonLightsColor = color;
                }

                veh.SetNeonLightsOn(VehicleNeonLight.Back, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Back").InnerText));
                veh.SetNeonLightsOn(VehicleNeonLight.Front, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Front").InnerText));
                veh.SetNeonLightsOn(VehicleNeonLight.Left, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Left").InnerText));
                veh.SetNeonLightsOn(VehicleNeonLight.Right, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Right").InnerText));
                foreach (XmlElement component in driver.SelectNodes("Vehicle/Components/*"))
                {
                    Function.Call(Hash.SET_VEHICLE_EXTRA, veh, int.Parse(component.GetAttribute("ComponentIndex")), int.Parse(component.InnerText, CultureInfo.InvariantCulture));
                }
                foreach (XmlElement component in driver.SelectNodes("Vehicle/ModToggles/*"))
                {
                    Function.Call(Hash.TOGGLE_VEHICLE_MOD, veh, int.Parse(component.GetAttribute("ToggleIndex")), bool.Parse(component.InnerText));
                }
                foreach (XmlElement component in driver.SelectNodes("Vehicle/Mods/*"))
                {
                    veh.SetMod((VehicleMod)int.Parse(component.GetAttribute("ModIndex")), int.Parse(component.InnerText, CultureInfo.InvariantCulture), bool.Parse(driver.SelectSingleNode("Vehicle/CustomTires").InnerText));
                }
            }

            ambientvehicles.Add(veh);
            PrepareVeh(veh);
            /*
            Ped ped = World.CreateRandomPed(veh.Position.Around(2f));
            ped.SetIntoVehicle(veh, VehicleSeat.Driver);
            Vector3 waitingzone = GetCrowdAreas()[RandomInt(0, GetCrowdAreas().Count)].Around(4f);
            TaskSequence RaceSequence = new TaskSequence();
            Function.Call(Hash.TASK_PAUSE, 0, 20000);
            Function.Call(Hash.TASK_LEAVE_VEHICLE, 0, GetLastVehicle(ped), 0);
            Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, 0, waitingzone.X, waitingzone.Y, waitingzone.Z, 1.0f, -1, 0.0f, 0, 0.0f);
            RaceSequence.Close();
            ped.Task.PerformSequence(RaceSequence);
            RaceSequence.Dispose();
            crowd.Add(ped);
            */
            if (veh.DisplayName == "MOONBEAM2" || veh.DisplayName == "FACTION2")
            {
                veh.OpenDoor(VehicleDoor.Trunk, false, false);
                veh.OpenDoor(VehicleDoor.BackLeftDoor, false, false);

            }
        }
    }


    void SpawnDriversFromFile()
    {
        XmlDocument document = new XmlDocument();
        document.Load(@"scripts\\DragMeets\Drivers\Drivers.xml");
        while (document==null)
        {
            Script.Wait(0);
        }
        XmlElement docroot = document.DocumentElement;

        XmlNodeList nodelist = docroot.SelectNodes("//Drivers/*");
        XmlNodeList nodelist2 = nodelist;
        List<String> drivers = new List<String>();
        List<String> pickeddrivers = new List<String>();
        int limit = GetMaxCars();

        foreach (XmlElement driver in nodelist)
        {
            if (driver.SelectSingleNode("RacesAllowed") != null)
            {
                foreach (XmlElement component in driver.SelectNodes("RacesAllowed/*"))
                {
                    if (GetCarsAllowed().Contains(component.InnerText))
                    {
                        drivers.Add(driver.GetAttribute("DriverName"));
                        break;
                    }
                }
            }
        }


        if (limit > drivers.Count) { limit = drivers.Count; UI.Notify("Not enough cars, lowering limit"); }

        var copyDrivers = new List<string>(drivers);
        for (int _ = 0; _ < limit; _++)
        {
            int i = RandomInt(0, copyDrivers.Count);
            pickeddrivers.Add(copyDrivers[i]);
            copyDrivers.RemoveAt(i);
        }
        

        float num = 1;

        foreach (XmlElement driver in nodelist)
        {
            if (pickeddrivers.Contains(driver.GetAttribute("DriverName")))
            {
                File.WriteAllText(@"scripts\\DragMeets\debug.txt", driver.GetAttribute("DriverName"));
                num = num + 3f;
                Vector3 position;
                if (num > GetWaitingZoneRadius())
                {
                    position = World.GetNextPositionOnStreet(GetWaitingZone().Around(num));
                }
                else
                {
                    position = GetWaitingZone().Around(num);
                }
                Vehicle veh = World.CreateVehicle(driver.SelectSingleNode("Vehicle/Model").InnerText, position);
                Ped ped;
                if (driver.SelectSingleNode("PedModel").InnerText == "random")
                {
                    ped = World.CreateRandomPed(veh.Position.Around(2f));
                }
                else
                {
                    ped = World.CreatePed(driver.SelectSingleNode("PedModel").InnerText, veh.Position.Around(2f));
                }

                Function.Call(Hash.SET_VEHICLE_MOD_KIT, veh, 0);
                veh.WheelType = (VehicleWheelType)int.Parse(driver.SelectSingleNode("Vehicle/WheelType").InnerText, CultureInfo.InvariantCulture);
                veh.PrimaryColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/Colors/PrimaryColor").InnerText, CultureInfo.InvariantCulture);
                veh.SecondaryColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/Colors/SecondaryColor").InnerText, CultureInfo.InvariantCulture);
                veh.PearlescentColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/Colors/PearlescentColor").InnerText, CultureInfo.InvariantCulture);
                Function.Call((Hash)0xF40DD601A65F7F19, veh, int.Parse(driver.SelectSingleNode("Vehicle/Colors/TrimColor").InnerText, CultureInfo.InvariantCulture));
                Function.Call((Hash)0x6089CDF6A57F326C, veh, int.Parse(driver.SelectSingleNode("Vehicle/Colors/DashColor").InnerText, CultureInfo.InvariantCulture));
                veh.RimColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/Colors/RimColor").InnerText, CultureInfo.InvariantCulture);
                veh.NumberPlate = driver.SelectSingleNode("Vehicle/LicensePlateText").InnerText;
                Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, veh, int.Parse(driver.SelectSingleNode("Vehicle/LicensePlate").InnerText, CultureInfo.InvariantCulture));
                veh.WindowTint = (VehicleWindowTint)int.Parse(driver.SelectSingleNode("Vehicle/WindowsTint").InnerText, CultureInfo.InvariantCulture);

                if (driver.SelectSingleNode("Vehicle/Colors/SmokeColor") != null)
                {
                    Color color = Color.FromArgb(255, int.Parse(driver.SelectSingleNode("Vehicle/Colors/SmokeColor/Color/R").InnerText), int.Parse(driver.SelectSingleNode("Vehicle/Colors/SmokeColor/Color/G").InnerText), int.Parse(driver.SelectSingleNode("Vehicle/Colors/SmokeColor/Color/B").InnerText));
                    veh.TireSmokeColor = color;
                }

                if (driver.SelectSingleNode("Vehicle/Colors/NeonColor") != null)
                {
                    Color color = Color.FromArgb(255, int.Parse(driver.SelectSingleNode("Vehicle/Colors/NeonColor/Color/R").InnerText), int.Parse(driver.SelectSingleNode("Vehicle/Colors/NeonColor/Color/G").InnerText), int.Parse(driver.SelectSingleNode("Vehicle/Colors/NeonColor/Color/B").InnerText));
                    veh.NeonLightsColor = color;
                }

                veh.SetNeonLightsOn(VehicleNeonLight.Back, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Back").InnerText));
                veh.SetNeonLightsOn(VehicleNeonLight.Front, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Front").InnerText));
                veh.SetNeonLightsOn(VehicleNeonLight.Left, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Left").InnerText));
                veh.SetNeonLightsOn(VehicleNeonLight.Right, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Right").InnerText));

                foreach (XmlElement component in driver.SelectNodes("Vehicle/Components/*"))
                {
                    Function.Call(Hash.SET_VEHICLE_EXTRA, veh, int.Parse(component.GetAttribute("ComponentIndex")), int.Parse(component.InnerText, CultureInfo.InvariantCulture));
                }
                foreach (XmlElement component in driver.SelectNodes("Vehicle/ModToggles/*"))
                {
                    Function.Call(Hash.TOGGLE_VEHICLE_MOD, veh, int.Parse(component.GetAttribute("ToggleIndex")), bool.Parse(component.InnerText));
                }
                foreach (XmlElement component in driver.SelectNodes("Vehicle/Mods/*"))
                {
                    veh.SetMod((VehicleMod)int.Parse(component.GetAttribute("ModIndex")), int.Parse(component.InnerText, CultureInfo.InvariantCulture), bool.Parse(driver.SelectSingleNode("Vehicle/CustomTires").InnerText));
                }
                ped.SetIntoVehicle(veh, VehicleSeat.Driver);
                DriveThisPedToTheMeetingArea(ped);
                AddToWaitingList(ped);
                PreparePed(ped);
                PrepareVeh(veh);
                if (RandomInt(0, 3) != 1 || veh.DisplayName=="FACTION2" || veh.DisplayName == "BUCCANEER2")
                {
                    SetRadioLoud(veh);
                }


                Function.Call(GTA.Native.Hash._0x0DC7CABAB1E9B67E, GetLastVehicle(ped), true); //Load Collision
                Function.Call(GTA.Native.Hash._0x0DC7CABAB1E9B67E, ped, true);

            }

        }

    }
    public RaycastResult RaycastDrive(Vector3 pos1, Vector3 pos2)
    {
        RaycastResult ray = World.Raycast(pos1, pos2 + new Vector3(0, 0, 1), IntersectOptions.Everything);
        return ray;
    }


    bool IsInDriversList(Ped ped)
    {
        foreach (Ped man in drivers)
        {
            if (man.Handle == ped.Handle)
            {
                return true;
            }
        }
        return false;
    }

    bool IsInWaitingList(Ped ped)
    {
        foreach (Ped man in waitingdrivers)
        {
            if (man.Handle == ped.Handle)
            {
                return true;
            }
        }
        return false;
    }


    bool IsInAmbientVehsList(Vehicle ent)
    {
        foreach (Vehicle ent2 in ambientvehicles)
        {
            if (ent.Handle == ent2.Handle)
            {
                return true;
            }
        }
        return false;
    }

    void AddToWaitingList(Ped ped)
    {
        bool can = true;
        foreach (Ped man in waitingdrivers)
        {
            if (man.Handle == ped.Handle)
            {
                //UI.Notify(GetLastVehicle(ped).FriendlyName + " already existed on waitingdrivers");
                can = false;
                break;
            }
        }
        if (can)
        {
            waitingdrivers.Add(ped);
            //UI.Notify(GetLastVehicle(ped).FriendlyName + " added to waitingdrivers ");
        }
    }


    void AddToDriversList(Ped ped)
    {
        bool can = true;
        foreach (Ped man in drivers)
        {
            if (man.Handle == ped.Handle)
            {
                //UI.Notify(GetLastVehicle(ped).FriendlyName + " already existed on drivers");
                can = false;
                break;
            }
        }
        if (can)
        {
            drivers.Add(ped);
            //UI.Notify(GetLastVehicle(ped).FriendlyName + "  added to drivers");
        }
    }


    void RemoveFromWaitingList(Ped ped)
    {
        foreach (Ped man in waitingdrivers)
        {
            if (man.Handle == ped.Handle)
            {
                //UI.Notify(GetLastVehicle(ped).FriendlyName + " Removed from waitingdrivers");
                waitingdrivers.Remove(man);
                break;
            }
        }
    }

    void RemoveFromDriversList(Ped ped)
    {
        foreach (Ped man in drivers)
        {
            if (man.Handle == ped.Handle)
            {
                //UI.Notify(GetLastVehicle(ped).FriendlyName + " removed from drivers");
                drivers.Remove(ped);
                break;
            }
        }
    }


    void DriveThisPedToTheMeetingArea(Ped ped)
    {
        if (CanWeUse(ped) && CanWeUse(GetLastVehicle(ped)))
        {
            AddToWaitingList(ped);
            Vector3 random = GetWaitingZone().Around(GetWaitingZoneRadius() * (RandomInt(3, 8) / 10.0f));

            Vector3 waitingzone = GetWaitingZone().Around(GetWaitingZoneRadius() * (RandomInt(4,10) / 10.0f));
            int how= 262199;
            if (ped.IsInRangeOf(GetWaitingZone(), 40f))
            {
                how = 4194365;
            }

            TaskSequence RaceSequence = new TaskSequence();
             Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, 0, GetLastVehicle(ped), waitingzone.X, waitingzone.Y, waitingzone.Z, 10f, true, GetLastVehicle(ped).GetHashCode(), how, 5f, 0f);
            Function.Call(Hash.TASK_LEAVE_VEHICLE, 0, GetLastVehicle(ped), 0);
            Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, 0, random.X, random.Y, random.Z, 1.0f, -1, 0.0f, 0, 0.0f);


            RaceSequence.Close();
            ped.Task.PerformSequence(RaceSequence);
            RaceSequence.Dispose();
        }
    }


    void DriveRacersToTheWaitingArea()
    {
        string side;
        foreach (Ped driver in drivers)
        {
            Vector3 random = GetWaitingZone().Around(GetWaitingZoneRadius() * (RandomInt(3,8) / 10.0f));
            Vector3 nearwaitingzone = GetWaitingZone().Around(GetWaitingZoneRadius() * (RandomInt(4, 10) / 10.0f));
            Vector3 waitingzone = GetWaitingZone();

            AddToWaitingList(driver);

            if (driver == drivers[0])
            {
                side = "Left";
            }
            else
            {
                side = "Right";
            }

            TaskSequence RaceSequence = new TaskSequence();

            foreach (Vector3 pos in GetSequenceToWaitingZone(side))
            {
                Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, 0, GetLastVehicle(driver), pos.X, pos.Y, pos.Z, 10f, 4194365, 4f);
            }

            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, 0, GetLastVehicle(driver), nearwaitingzone.X, nearwaitingzone.Y, nearwaitingzone.Z, 3f, 4194365,5f);
            Function.Call(Hash.TASK_LEAVE_VEHICLE, 0, GetLastVehicle(driver), 0);
            Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, 0, random.X, random.Y, random.Z, 1.0f, -1, 0.0f, 0, 0.0f);

            RaceSequence.Close();
            driver.Task.PerformSequence(RaceSequence);
            RaceSequence.Dispose();
        }

        drivers.Clear();
    }

    Vector3 GetGoal(string side)
    {
        Vector3 goal;
        XmlNode node = root.SelectSingleNode("//Drag/" + side + "/EndPoint");
        goal = new Vector3(int.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
        return goal;
    }
    Vector3 GetStartingLine()
    {
        Vector3 goal;
        XmlNode node = root.SelectSingleNode("//Drag/StartingLine");
        goal = new Vector3(int.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
        return goal;
    }


    float GetStartingLineRadius()
    {
        float goal;
        XmlNode node = root.SelectSingleNode("//Drag/StartingLine");
        goal = float.Parse(node.SelectSingleNode("Radius").InnerText, CultureInfo.InvariantCulture);
        return goal;
    }

    float GetFinishLineRadius()
    {
        float goal;
        XmlNode node = root.SelectSingleNode("//Drag/FinishLine");
        goal = float.Parse(node.SelectSingleNode("Radius").InnerText, CultureInfo.InvariantCulture);
        return goal;
    }

    Vector3 GetFinishLine()
    {
        Vector3 goal;
        XmlNode node = root.SelectSingleNode("//Drag/FinishLine");
        goal = new Vector3(int.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
        return goal;
    }
    float GetRaceHeading()
    {
        return float.Parse(root.SelectSingleNode("//Drag/Heading").InnerText, CultureInfo.InvariantCulture);
    }
    float GetRaceDistanceLimit()
    {
        return float.Parse(root.SelectSingleNode("//Drag/DistanceLimit").InnerText, CultureInfo.InvariantCulture);
    }
    Vector3 GetWaitingZone()
    {
        Vector3 goal;
        XmlNode node = root.SelectSingleNode("//Drag/WaitingZone");
        goal = new Vector3(int.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
        return goal;
    }
    Vector3 GetCountDownGuyPos()
    {
        Vector3 goal;
        XmlNode node = root.SelectSingleNode("//Drag/CountDownGuyPos");
        goal = new Vector3(int.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
        return goal;
    }
    float GetWaitingZoneRadius()
    {
        float goal;
        XmlNode node = root.SelectSingleNode("//Drag/WaitingZone");
        goal = float.Parse(node.SelectSingleNode("Radius").InnerText, CultureInfo.InvariantCulture);
        return goal;
    }

    List<Vector3> GetSequenceToStartingPos(string side)
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 parkpos;
        XmlNodeList nodelist = root.SelectNodes("//Drag/" + side + "/StartPointSequence/*");
        foreach (XmlElement element1 in nodelist)
        {
            parkpos = new Vector3(int.Parse(element1.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
            //UI.Notify(parkpos.ToString());
            positions.Add(parkpos);
        }
        return positions;
    }



    List<Vector3> GetSequenceToEnd(string side)
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 parkpos;
        XmlNodeList nodelist = root.SelectNodes("//Drag/" + side + "/EndPointSequence/*");
        foreach (XmlElement element1 in nodelist)
        {
            parkpos = new Vector3(int.Parse(element1.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
            //UI.Notify(parkpos.ToString());
            positions.Add(parkpos);
        }
        return positions;
    }


    Vector3 GetStartingPoint(string side)
    {
        Vector3 goal;
        XmlNode node = root.SelectSingleNode("//Drag/" + side + "/StartPoint");
        goal = new Vector3(int.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
        return goal;
    }


    List<Vector3> GetSequenceToWaitingZone(string side)
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 parkpos;
        XmlNodeList nodelist = root.SelectNodes("//Drag/" + side + "/BackToWaitSequence/*");
        foreach (XmlElement element1 in nodelist)
        {
            parkpos = new Vector3(int.Parse(element1.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
            //UI.Notify(parkpos.ToString());
            positions.Add(parkpos);
        }
        return positions;
    }


    Vehicle GetLastVehicle(Ped RecieveOrder)
    {
        Vehicle vehicle = null;
        if (GTA.Native.Function.Call<Vehicle>(GTA.Native.Hash.GET_VEHICLE_PED_IS_IN, RecieveOrder, true) != null)
        {
            vehicle = GTA.Native.Function.Call<Vehicle>(GTA.Native.Hash.GET_VEHICLE_PED_IS_IN, RecieveOrder, true);
            if (vehicle.IsAlive)
            {
                return vehicle;
            }
        }
        else
        {
            if (GTA.Native.Function.Call<Vehicle>(GTA.Native.Hash.GET_VEHICLE_PED_IS_IN, RecieveOrder, false) != null)
            {
                vehicle = GTA.Native.Function.Call<Vehicle>(GTA.Native.Hash.GET_VEHICLE_PED_IS_IN, RecieveOrder, false);
                if (vehicle.IsAlive)
                {
                    return vehicle;
                }
            }
        }
        return vehicle;
    }


    List<Vector3> GetCrowdAreas()
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 parkpos;
        XmlNodeList nodelist = root.SelectNodes("//CrowdAreas/*");
        foreach (XmlElement element1 in nodelist)
        {
            parkpos = new Vector3(int.Parse(element1.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
            positions.Add(parkpos);
        }
        return positions;
    }

    int GetCrowdAreaRadius(int i)
    {
        int radius = 5;
        int i2 = 0;
        XmlNodeList nodelist = root.SelectNodes("//CrowdAreas/*");
        foreach (XmlElement element1 in nodelist)
        {
            i2++;
            if (i == i2 && element1.SelectSingleNode("Radius") != null)
            {
                //UI.Notify("used");

                radius = int.Parse(element1.SelectSingleNode("Radius").InnerText, CultureInfo.InvariantCulture);
                break;
            }
        }
        return radius;
    }
    List<Vector3> GetAmbientVehicleParkZones()
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 parkpos;
        XmlNodeList nodelist = root.SelectNodes("//AmbientVehicleParkZones/*");
        foreach (XmlElement element1 in nodelist)
        {
            parkpos = new Vector3(int.Parse(element1.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
            positions.Add(parkpos);
        }
        return positions;
    }

    List<float> GetAmbientVehicleParkZonesHeading()
    {
        List<float> positions = new List<float>();
        float parkpos;
        XmlNodeList nodelist = root.SelectNodes("//AmbientVehicleParkZones/*");
        foreach (XmlElement element1 in nodelist)
        {
            parkpos = float.Parse(element1.SelectSingleNode("Heading").InnerText);
            positions.Add(parkpos);
        }
        return positions;
    }

    string GetMeetRadio()
    {
        List<string> RADIOS = new List<string>();
        XmlNodeList nodelist = root.SelectNodes("//Drag/Radios/*");
        foreach (XmlElement element1 in nodelist)
        {
            RADIOS.Add(element1.InnerText);
        }

        return RADIOS[RandomInt(0, RADIOS.Count-1)];
    }

    List<string> GetCarsAllowed()
    {
        List<string> positions = new List<string>();
        string parkpos;
        XmlNodeList nodelist = root.SelectNodes("//CarsAllowed/*");
        if (nodelist != null)
        {

            foreach (XmlElement element1 in nodelist)
            {
                parkpos = element1.InnerText;
                positions.Add(parkpos);
            }

        }
        else
        {
            UI.Notify("~r~List of cars allowed is empty.");
        }

        return positions;
    }


    int GetMaxCars()
    {
        int goal;
        XmlNode node = root.SelectSingleNode("//Drag/MaxCarLimit");
        goal = int.Parse(node.InnerText, CultureInfo.InvariantCulture);
        return goal;
    }
}