using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.Globalization;

namespace Drag_Meets_Reborn
{
    /// <summary>
    /// --- TO DO ---
    /// - Release:
    /// - Lower wins/loses values at the Racers' panel
    /// - Crowd cheers
    /// - Make sure Racers don't spawn near the player (GenerateSpawnPoint)
    /// - Introductory texts
    /// - Respect system - kinda done
    /// - Guards
    /// - Ped comments (they're creepily silent for now)
    /// - Persistent respect & money for both player and Racers - DONE, needs optional toogle
    /// - Improve configuration file
    /// - Add fuckton of designed/persistent racers, its basically empty for now
    /// 
    /// - Eventual Update:
    /// - Activities pre-Meet
    /// 
    /// --- Info ---
    /// 
    /// 
    /// 
    /// </summary>
    public class DragMeets : Script
    {
        
        
        public static string ScriptName = "Drag Meets";
        public static string ScriptVer = "1.9.4";
        public static bool IsStartLineBusy = false;
        public static string RacersFile = @"scripts\\DragMeets\Racers\Racers.xml";
        public static string PlayerFile = @"scripts\\DragMeets\Racers\Player.xml";

        public string configpath = @"scripts\DragMeets\config.ini";
        public int BetTimeFrame = 0;

        public static int IdealWheelTemp = 35;


        public static Camera SpectCamera;


        public static List<Model> RacerModel = new List<Model>
        {
            "a_m_m_eastsa_01",
"a_m_m_eastsa_02",
"a_m_m_malibu_01",
"a_m_m_mexcntry_01",
"a_m_m_mexlabor_01",
"a_m_m_og_boss_01",
"a_m_m_polynesian_01",
"a_m_m_soucent_01",
"a_m_m_soucent_03",
"a_m_m_soucent_04",
"a_m_m_stlat_02",
"s_m_m_bouncer_01",
"s_m_m_lifeinvad_01",
"u_m_m_aldinapoli",
"u_m_m_bikehire_01",
"u_m_m_filmdirector",
"u_m_m_rivalpap",
"u_m_m_willyfist",
"u_m_y_baygor",
"u_m_y_chip",
"u_m_y_cyclist_01",
"u_m_y_fibmugger_01",
"u_m_y_guido_01",
"u_m_y_gunvend_01",
"u_m_y_hippie_01",
"u_m_y_paparazzi",
"u_m_y_party_01",
"u_m_y_sbike",
"u_m_y_tattoo_01",
        };


        //Debug
        public static bool DebugNotifications = false; //Not really used yet, should link more notifications to this

        
        

        //Options loaded from file
        public static float SpeedMultiplier = 1f; //Option to make all driving slower, helps with realistic handling settings
        public static int Separation = 3; //Meters of separation between cars
        public static int DistanceMultiplier= 1; //Modifies the finish line distance;

        public static bool RealisticWheelies = true;
        public static bool MT_Support = false;
        public static bool PreventPreDesignedTuning = false;

        
        public static bool ManualMeetSpawn = false;
        public static bool ForceRandomTuning = false;
        
        public static bool SimulateManualTransmission = false;
        public static bool SimulateTirePerformance = false;
        public static bool ShowGoodBadShifting = false;
        public static bool AllowNitro = false;
        public static bool PersistentRacers = false;


        public static bool VehicleListUpdater = false;


        //Drag Meet
        //        PadNitroKey = config.GetValue<GTA.Control>("KEYS", "PadNitroKey", GTA.Control.ScriptRDown);

        public static GTA.Control ShiftingKey = GTA.Control.Context;
        public static GTA.Control InteractKey = GTA.Control.Context;

        public static bool DespanwnRequested = false;
        public static bool WarnedOfPerformance = false;
        public static bool OfficialIntroduction = false;
        public static bool StreetIntroduction = false;

        public static bool TougeMode = false;

        public static bool OfficialMeet = false;

        public static Vector3 PathToStartingLine = Vector3.Zero;
        public static Vector3 PathToWaitingArea = Vector3.Zero;
        public static List<Vector3> CrowdAreas = new List<Vector3>();
        public static List<Vector3> DynamicCrowdAreas = new List<Vector3>();

        public static List<Vector3> ToWaitingArea = new List<Vector3>();

        public static List<Prop> MeetProps = new List<Prop>();


        public static List<string> KindsAllowed = new List<string>();
        public static int RacerRLGroup;

        public static List<Ped> Partners = new List<Ped>();
        public static int MoneyBet=200;

        public static int Prize = 200;
        public static int ChallengePrize = 200;
        public static int Fee = 1000;

        public static Prop FinishBarrel;

        public static bool PlayerAccepted=false;

        public static int PlayerAcceptedTime = 0;
        public static bool CinematicCam = false;
        public static bool FairChallenges = false;



        //StartingLine references
        public static int ForwardOffset = 6;

        public static bool SelfRadio = false;
        public static string Radio = "RADIO_07_DANCE_01";
        public static List<Vector3> MeetTriggers = new List<Vector3>();
        public static List<Blip> MeetBlips = new List<Blip>();

        public static DragRaceState MeetState = DragRaceState.NotStarted;

        public static int PickingGameRef = 0;
        public static int PerformanceTreshold=0;
        public static int ReputationTreshold = 0;

        public static int PickSameDriversTries = 0;
        public static int SameDriverReference = 0;
        public static int RacersPerRace = 2;

        public static List<Spectator> Spectators = new List<Spectator>();


        public static List<Racer> Racers = new List<Racer>();

        //public static List<String> VehiclesNotPermitted = new List<String>();

        public static List<Racer> Winners = new List<Racer>();

        public static Vector3 WaitingArea;
        public static float WaitingAreRange=30;
        public static Vector3 LeftRacerPos;
        public static Vector3 RightRacerPos;


        public static Vector3 StartingLine;
        public static Vector3 Finishline;


        public static float TougeHeading = 0;

        public static float DragHeading=0;
        public static int RacerCap = 10;
        public static int GameTimeRef = 0;
        public static int GameInterval = 500;
        public static int FinishCamTime = 0;


        public static int GameTimeRefLong = 0;
        public static int GameIntervalLong = 20000;

        public static Ped FlarePed;
        public int CountDown = 0;
        public int CoolDown = 0;
        public int SomethingWrongTimer =0;
        public static int RaceStartedTime = 0;
        public int PerformanceDifferenceMin = 3;

        public static bool ReactionTimes = false;
        public static bool ShowReactionTimes = false;
        public static bool ShowGrip = false;
        public DragMeets()
        {
            Tick += OnTick;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            LoadMeetTriggersFromFile();
            Game.FadeScreenIn(200);

            RacerRLGroup = World.AddRelationshipGroup("DragMeetsRacers");
            World.SetRelationshipBetweenGroups(Relationship.Companion, RacerRLGroup, Function.Call<int>(Hash.GET_HASH_KEY, "PLAYER"));
            World.SetRelationshipBetweenGroups(Relationship.Companion, Function.Call<int>(Hash.GET_HASH_KEY, "PLAYER"), RacerRLGroup);

            World.SetRelationshipBetweenGroups(Relationship.Companion, RacerRLGroup, Function.Call<int>(Hash.GET_HASH_KEY, "CIVMALE"));
            World.SetRelationshipBetweenGroups(Relationship.Companion, Function.Call<int>(Hash.GET_HASH_KEY, "CIVMALE"), RacerRLGroup);

            World.SetRelationshipBetweenGroups(Relationship.Companion, RacerRLGroup, Function.Call<int>(Hash.GET_HASH_KEY, "CIVFEMALE"));
            World.SetRelationshipBetweenGroups(Relationship.Companion, Function.Call<int>(Hash.GET_HASH_KEY, "CIVFEMALE"), RacerRLGroup);

            World.SetRelationshipBetweenGroups(Relationship.Neutral, RacerRLGroup, Function.Call<int>(Hash.GET_HASH_KEY, "COP"));
            World.SetRelationshipBetweenGroups(Relationship.Neutral, Function.Call<int>(Hash.GET_HASH_KEY, "COP"), RacerRLGroup);

            World.SetRelationshipBetweenGroups(Relationship.Companion, RacerRLGroup, RacerRLGroup);


            LoadSettings(configpath);

            //Function.Call(Hash.LOAD_STREAM, "INTRO_STREAM", "ROAD_RACE_SOUNDSET");
            Function.Call(Hash.LOAD_STREAM, "INTRO_STREAM", "DIRT_RACES_SOUNDSET");
            File.WriteAllText(@"scripts\\DragMeets\debug.txt", "Script started -"+DateTime.Now+"-");



        }

        public static void SavePlayerstats()
        {

            if (!DragMeets.PersistentRacers) return;

            XmlDocument document = new XmlDocument();
            document.Load(PlayerFile);
            int pat = 0;
            while (document == null && pat < 500)
            {
                document.Load(PlayerFile);
                Script.Wait(0);
            }
            XmlElement root = document.DocumentElement;


            XmlNode list = root.SelectSingleNode("//Wins");
            if (list == null)
            {
               XmlNode wins= document.CreateNode(XmlNodeType.Element, "Wins", null);
                document.AppendChild(wins);
                list = wins;
            }

            list.InnerText = GetPlayerRacer().Wins.ToString();


            list = root.SelectSingleNode("//Loses");
            if (list == null)
            {
                XmlNode loses = document.CreateNode(XmlNodeType.Element, "Loses", null);
                document.AppendChild(loses);
                list = loses;
            }

            list.InnerText = GetPlayerRacer().Loses.ToString();


            document.Save(PlayerFile);
        }



        public static void SaveRacerstats(Racer racer)
        {

            if (!DragMeets.PersistentRacers) return;

                XmlDocument document = new XmlDocument();
            document.Load(RacersFile);
            int pat = 0;
            while (document == null && pat < 500)
            {
                document.Load(RacersFile);
                Script.Wait(0);
            }
            if (pat >= 500)
            {
                WarnPlayer(ScriptName + " " + ScriptVer, "Racers.xml LOAD ERROR", "~r~Couldn't load the file.");
                return;
            }
            XmlElement docroot = document.DocumentElement;


            XmlNodeList nodelist = docroot.SelectNodes("//Racers/*");
            if (nodelist.Count == 0) nodelist = docroot.SelectNodes("//Drivers/*");

            if (nodelist.Count == 0)
            {
                WarnPlayer(ScriptName + " " + ScriptVer, "Racers.xml LOAD ERROR", "~r~No vehicles found in Racers.xml.");
                return;
            }
            else
            {
                foreach (XmlElement driver in nodelist)
                {

                    if (driver.HasAttribute("Name") && driver.GetAttribute("Name") == racer.Name && driver.HasAttribute("Reaction"))
                    {


                        if (driver.HasAttribute("Reaction")) driver.SetAttribute("Reaction", racer.Reaction.ToString());
                        if (driver.HasAttribute("Shifting")) driver.SetAttribute("Shifting", racer.Shifting.ToString());

                        if (driver.HasAttribute("RacesWon")) driver.SetAttribute("RacesWon", racer.Wins.ToString());
                        else
                        {
                            XmlAttribute Attribute = document.CreateAttribute("RacesWon");
                            driver.Attributes.Append(Attribute);
                            driver.SetAttribute("RacesWon", racer.Wins.ToString());
                        }
                        if (driver.HasAttribute("RacesLost")) driver.SetAttribute("RacesLost", racer.Loses.ToString());
                        else
                        {
                            XmlAttribute Attribute = document.CreateAttribute("RacesLost");
                            driver.Attributes.Append(Attribute);
                            driver.SetAttribute("RacesLost", racer.Loses.ToString());
                        }
                        if (driver.HasAttribute("Money")) driver.SetAttribute("Money", racer.Driver.Money.ToString());
                        else
                        {
                            XmlAttribute Attribute = document.CreateAttribute("Money");
                            driver.Attributes.Append(Attribute);
                            driver.SetAttribute("Money", racer.Driver.Money.ToString());
                        }
                    }
                }
                //UI.Notify("~b~"+racer.Name+"~w~ stats saved.");
                document.Save(RacersFile);
            }
        }

        public static void LoadPlayerStats(Racer racer)
        {

            XmlDocument document = new XmlDocument();
            document.Load(PlayerFile);
            int pat = 0;
            while (document == null && pat < 500)
            {
                document.Load(PlayerFile);
                Script.Wait(0);
            }
            XmlElement root = document.DocumentElement;

            XmlNode list = root.SelectSingleNode("//Wins");
            if (list != null) racer.Wins= int.Parse(list.InnerText);

            list = root.SelectSingleNode("//Loses");
            if (list != null) racer.Loses = int.Parse(list.InnerText);
        }
        public bool IsAnyMeetLoaded()
        {
            return (StartingLine != Vector3.Zero && Finishline != Vector3.Zero && WaitingArea != Vector3.Zero && WaitingAreRange != 0);
        }
        public bool IsInRacerList(Vehicle veh, bool checkHash, bool checkName, int maxItems)
        {
            XmlDocument document = new XmlDocument();
            document.Load(RacersFile);
            int pat = 0;
            while (document == null && pat < 500)
            {
                document.Load(RacersFile);
                Script.Wait(0);
            }
            if (pat >= 500)
            {
                return true;
            }
            XmlElement docroot = document.DocumentElement;
            int i = 0;
            XmlNodeList nodelist = docroot.SelectNodes("//Vehicle/*");
            foreach (XmlElement element1 in nodelist)
            {
                if ((checkHash && element1.InnerText == veh.Model.Hash.ToString()) || (checkName && element1.GetAttribute("Name") == veh.FriendlyName)) i++;
            }
            if (i >= maxItems) return true;
            return false;
        }
        Scaleform text = new Scaleform("mp_car_stats_01");
        void OnTick(object sender, EventArgs e)
        {



            if (Util.WasCheatStringJustEntered("save car details"))
            {
                if (CanWeUse(Game.Player.Character.CurrentVehicle)) SmartVehicleAddition(Game.Player.Character, Game.Player.Character.CurrentVehicle, true); else UI.Notify("You're not in a vehicle.");
            }
            if (Util.WasCheatStringJustEntered("save car"))
            {
                if (CanWeUse(Game.Player.Character.CurrentVehicle)) SmartVehicleAddition(Game.Player.Character, Game.Player.Character.CurrentVehicle, false); else UI.Notify("You're not in a vehicle.");
            }
            if (Util.WasCheatStringJustEntered("help"))
            {
                UI.Notify("Current commands available:");
                UI.Notify("~b~shorten track:~w~ Reduces the track length.");
                UI.Notify("~b~base bet:~w~ Changes the spectator bet");
                UI.Notify("~b~bet:~w~ If you're racing, changes your bet on the race.");
                UI.Notify("If you're not, it lets you select who you're betting on.");

                UI.Notify("~b~leave meet:~w~ Makes your character leave the Meet.");
                UI.Notify("~b~despawn meet:~w~ Force-despawns the entire Meet.");
                UI.Notify("~b~save car:~w~ Saves your current car as a generic racer.");
                UI.Notify("~b~save car details:~w~ Saves your current car as a defined racer.");
            }
            if (Util.WasCheatStringJustEntered("shorten track"))
            {
                UI.Notify("Enter an integer from 0 to 100 to shorten the track by that.");
                int temp;
                if (int.TryParse(Game.GetUserInput(10), out temp))
                {
                    DistanceMultiplier = temp;
                    UI.Notify("~g~New distance accepted.~n~~w~The track will be shortened in the next heat.");
                }
            }
            if (Util.WasCheatStringJustEntered("base bet"))
            {
                UI.Notify("Enter the new betting base.");
                int temp;
                if (int.TryParse(Game.GetUserInput(10), out temp))
                {
                    MoneyBet = temp;
                    UI.Notify("~g~New base~n~~w~Any new bet will be $"+MoneyBet+".");
                }
            }
            if (Util.WasCheatStringJustEntered("bet"))
            {
                if (GetPlayerRacer() != null && MeetState == DragRaceState.Setup)
                {
                    if (GetPlayerRacer().Picked == RacerStatus.Picked)
                    {
                        int temp;
                        if (!OfficialMeet && int.TryParse(Game.GetUserInput(10), out temp))
                        {
                            bool Accepted = true;
                            string RacerName = "";
                            string reason ="";
                            foreach (Racer racer in RacersInState(RacerStatus.Picked))
                            {
                                if (racer.Driver.IsPlayer)
                                {
                                    if (temp > Util.PedMoney(racer.Driver))
                                    {
                                        Accepted = false;
                                        reason = "You don't have that much money.";
                                    }
                                }
                                else
                                {
                                    RacerName = racer.Name;
                                    if(temp > (Util.PedMoney(racer.Driver) * racer.max_willing_to_bet) /100)
                                    {
                                        Accepted = false;
                                        reason = racer.Name + " is not willing to bet more than ~g~$"+ (Util.PedMoney(racer.Driver) * racer.max_willing_to_bet) / 100 + "~w~.";
                                    }

                                    if (temp <(Util.PedMoney(racer.Driver) * racer.min_willing_to_bet) / 100)
                                    {
                                        Accepted = false;
                                        reason = racer.Name + " thinks you're a pussy and won't accept a bet lower than ~g~$"+ (Util.PedMoney(racer.Driver) * racer.min_willing_to_bet) / 100 + "~w~.";
                                    }
/*
                                    if ((temp > Util.PedMoney(racer.Driver) * 0.5f && racer.Expectations==RacerExpectations.Lose))
                                    {
                                        Accepted = false;
                                        reason = racer.Name + " is not willing to bet more than ~g~$" + Util.PedMoney(racer.Driver) * 0.5f + "~w~.";
                                    }
                                    if(GetPlayerRacer().ReputationBalance() < racer.ReputationBalance())
                                    {
                                        if(temp < ChallengePrize * 0.75 && racer.Expectations==RacerExpectations.Win)
                                        {
                                            Accepted = false;
                                            reason = racer.Name + " thinks you're a pussy and won't accept a bet lower than ~g~$" + ChallengePrize * 0.75 + "~w~.";
                                        }
                                    }
                                    */
                                }
                            }
                            if (Accepted)
                            {
                                ChallengePrize = temp;
                                UI.Notify(RacerName+" accepted the bet. ~n~The bet is now ~g~$" + ChallengePrize + "~w~.");
                            }
                            else UI.Notify("~o~"+reason+"");
                        }
                    }
                    else
                    {
                        UI.Notify("Enter the name of the racer you want to bet on.");
                        string name = Game.GetUserInput(50).ToLowerInvariant();
                        foreach (Racer racer in RacersInState(RacerStatus.Picked))
                        {
                            if (name.ToLowerInvariant() == racer.Name.ToLowerInvariant())
                            {
                                if (!racer.BetsOnHim.Contains(DragMeets.GetPlayerRacer()) && !DragMeets.GetPlayerRacer().HasBet && Game.Player.Money >= DragMeets.MoneyBet)
                                {
                                    racer.BetsOnHim.Add(DragMeets.GetPlayerRacer());
                                    Util.AddQueuedHelpText("Bet placed. You can hop in the " + racer.Car.FriendlyName + ".");
                                    UI.Notify("~b~" + DragMeets.GetPlayerRacer().Name + "~w~ bets on ~g~" + racer.Name);
                                    Util.PlayAmbientSpeech(racer.Driver, Util.ChatThanks);
                                    DragMeets.GetPlayerRacer().HasBet = true;
                                    Util.ChangePedMoney(DragMeets.GetPlayerRacer().Driver, -DragMeets.MoneyBet);
                                }
                            }
                        }
                    }
                }
                else UI.Notify("~r~You cannot bet (nor change bets) now.");
            }


            if (Util.WasCheatStringJustEntered("leave meet") && GetPlayerRacer() != null) GetPlayerRacer().RemoveMe = true;
            if (Util.WasCheatStringJustEntered("despawn meet") && IsAnyMeetLoaded())
            {
                UI.Notify("~g~Meet despawned.");
                DespanwnRequested = true;
            }



            if (IsAnyMeetLoaded())                
            {

                Function.Call(Hash.SET_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, 0.0f);
                Function.Call(Hash.SET_RANDOM_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, 0.0f);
                Function.Call(Hash.SET_PARKED_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, 0.0f);
                //Function.Call(Hash._0x90B6DA738A9A25DA, 0.0f);

                if (!Util.CanWeUse(FlarePed) || FlarePed.IsDead)
                {
                    if (CanWeUse(FlarePed))  FlarePed.MarkAsNoLongerNeeded();
                    FlarePed = World.CreatePed(PedHash.Bartender01SFY, WaitingArea.Around(20));

                    FlarePed.RelationshipGroup = RacerRLGroup;
                }
                else if (FlarePed.Weapons.Current.Hash == WeaponHash.Unarmed) FlarePed.Weapons.Give(WeaponHash.FlareGun, -1, true, true);

                
                if(Util.CanWeUse(FlarePed) && FlarePed.IsOnScreen && Game.Player.Character.IsOnFoot && Game.Player.Character.IsInRangeOf(FlarePed.Position, 3f) && ManualMeetSpawn)
                {
                    DisplayHelpTextThisFrame("Press "+Util.GetInstructionalButton(InteractKey)+" to despawn this meet.");
                    if (Game.IsControlJustPressed(2, InteractKey))
                    {
                        DespanwnRequested = true;
                    }                        
                }

                //Player join
                if (GetPlayerRacer() == null)
                {
                    Vehicle veh = Game.Player.Character.CurrentVehicle;
                    if (CanWeUse(veh))
                    {
                        if (veh.IsInRangeOf(WaitingArea, WaitingAreRange))
                        {
                            if(OfficialMeet) DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(DragMeets.InteractKey) + " to register.~n~The fee is $" + Fee + ", with a prize of ~g~$" + Prize + "~w~.");
                            else DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(DragMeets.InteractKey) + " to join.~n~No fee, you'll gain or lose your money in ~g~bets~w~.");
                            if (Game.IsControlJustPressed(2, InteractKey))
                            {
                                bool CanRegister = true;
                                if (OfficialMeet && Game.Player.Money < Fee)
                                {
                                    CanRegister = false;
                                    WarnPlayer(ScriptName,"NOT ENOUGH MONEY","You don't have enough money to pay for the fee.");
                                }

                                if (CanRegister)
                                {

                                    if (OfficialMeet) Util.ChangePedMoney(Game.Player.Character, -Fee);

                                    Racers.Add(new Racer(Game.Player.Name, Game.Player.Character, veh, 0, 50, 50, true,0,0,0));

                                    if (VehicleListUpdater) SmartVehicleAddition(Game.Player.Character, veh, false);
                                    if(!IsInRacerList(veh,true,true, 1))
                                    {
                                        if (DebugNotifications) Util.AddQueuedHelpText("~g~New car detected~w~, a new racer with that car should join shortly.");

                                        Vehicle newcar = World.CreateVehicle(veh.Model, Game.Player.Character.Position.Around(200));
                                        Ped newped = World.CreatePed(RacerModel[0], newcar.Position.Around(3));  
                                        if(CanWeUse(newcar) && CanWeUse(newped)) Racers.Add(new Racer(newcar.FriendlyName, newped, newcar, 0, 150, 60, true, 0, 0, 10000));
                                    }
                                    if (OfficialMeet)
                                    {
                                        Util.AddQueuedHelpText("You can wait in the ~y~Waiting area~w~ to be be picked for the races, or challenge other ~b~Racers~w~ yourself.");
                                    }
                                    else
                                    {
                                        Util.AddQueuedHelpText("You can wait in the ~y~Waiting area~w~ to be challenged, or challenge other ~b~Racers~w~ yourself.");
                                    }
                                    /*
                                    Util.AddQueuedHelpText("You have registered in this Meet.");
                                    Util.AddQueuedHelpText("You can participate in it leaving your car parked inside the ~y~Waiting area~w~.");
                                    Util.AddQueuedHelpText("Alternatively, you can make bets on any ~b~participant~w~ if you so desire.");
                                    Util.AddQueuedHelpText("While each is set up, you'll be given some time to do so.");

                                    Util.AddQueuedHelpText("You can leave the Meet by going far from it.");
                                    Util.AddQueuedHelpText("Or, by talking with the starting line girl.");
                                    */

                                    int partners = 0;
                                    foreach (Ped ped in World.GetNearbyPeds(Game.Player.Character, 100f))
                                    {
                                        if (ped.CurrentPedGroup == Game.Player.Character.CurrentPedGroup)
                                        {
                                            partners++;
                                        }
                                    }

                                    if (partners > 0)
                                    {
                                        Util.AddQueuedHelpText("Partners can join the Meet if you provide them a vehicle to participate with.");
                                        Util.AddQueuedHelpText("To do so, simply enter a new car.");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {

                    Vehicle veh = Game.Player.Character.CurrentVehicle;

                    //Partner join
                    if(CanWeUse(veh) && veh != GetPlayerRacer().Car)
                    {
                        bool CanDo = true;
                        foreach (Racer racer in Racers)
                        {
                            if (veh == racer.Car) CanDo = false;
                        }
                        if (CanDo)
                        {
                            Ped ped = null;
                            int partners = 0;
                            foreach (Ped partner in World.GetNearbyPeds(Game.Player.Character, 100f))
                            {
                                if (partner.CurrentPedGroup == Game.Player.Character.CurrentPedGroup)
                                {
                                    ped = partner;
                                    partner.LeaveGroup();
                                }
                            }

                            if(!CanWeUse(ped)) ped = World.CreateRandomPed(veh.Position.Around(4));
                            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, veh, true, true);
                            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, ped, true, true);

                            Racers.Add(new Racer(Game.Player.Name + "'s friend", ped, veh, 0, 250, 50, true,0,0,500));
                            //Game.Player.Character.Task.LeaveVehicle();

                            //if (DebugNotifications) UI.Notify(veh.IsPersistent.ToString()+" . "+ped.IsPersistent.ToString());
                        }
                    }

                }


                /*
                //Partner join
                if (GetPlayerRacer() != null && Partners.Count > 0)
                {
                    Vehicle veh = Game.Player.Character.CurrentVehicle;
                    if (CanWeUse(veh) && veh.Handle != GetPlayerRacer().Car.Handle)
                    {

                        DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to make your partner join the Meet using this vehicle.");
                        if (Game.IsControlJustPressed(2, GTA.Control.Context))
                        {
                            Util.AddNotification("","","","Your partner has joined the race.");
                            Racers.Add(new Racer("[Partner] " + veh.FriendlyName, Partners[0], veh, 0, 50, 50,true));
                            Partners[0].LeaveGroup();
                            Partners.RemoveAt(0);
                        }
                    }
                }
                */
            }


            //Crowds

            int numb = 0;
            foreach (Spectator spect in Spectators)
            {
                numb++;
                if (spect.Ok()) spect.HandleAI();
                else
                {
                    spect.Clear(!spect.SpectatorPed.IsOnScreen);
                    Spectators.Remove(spect);
                    break;
                }
            }
            

            HandleMeetSpawnDespawn(!DragMeets.ManualMeetSpawn);


            if (GameTimeRefLong < Game.GameTime)
            {
                GameTimeRefLong = Game.GameTime + GameIntervalLong;
                /*
                foreach (Blip blip in MeetBlips) blip.Remove();
                MeetBlips.Clear();

                foreach(Vector3 trigger in MeetTriggers)
                {
                    if (trigger.DistanceTo(Game.Player.Character.Position) < 40)
                    {
                        Blip blip = World.CreateBlip(trigger);
                        blip.Sprite = BlipSprite.DollarSign;
                        blip.Alpha = 0;
                        MeetBlips.Add(blip);
                    }
                }
                */
                if (Util.ThrowDice(20) && RacersNotParticipating(true).Count > 0)
                {
                    Vehicle checkout = DragMeets.RacersNotParticipating(true)[Util.RandomInt(0, DragMeets.RacersNotParticipating(true).Count)].Car;
                    {
                        List<Spectator> CheckingOut = new List<Spectator>();

                        for (int i = 0; i < CrowdAreas.Count * 2; i++)
                        {
                            Spectator spect = DragMeets.Spectators[Util.RandomInt(0, Spectators.Count)];
                            if (!CheckingOut.Contains(spect)) CheckingOut.Add(spect);
                        }
                        foreach (Spectator newspect in CheckingOut) if (newspect.Idle()) newspect.TaskCheckCarOut(checkout);
                    }
                }
                if (IsAnyMeetLoaded() && Racers.Count < RacerCap && Util.ThrowDice(50))
                {
                    List<string> filter = new List<string>();
                    foreach (Racer racer in Racers) filter.Add(racer.Name);
                    SpawnDriversFromFile(filter, 1);
                }
            }
            if (GameTimeRef < Game.GameTime)
            {
                GameTimeRef = Game.GameTime + GameInterval;

                foreach (Racer racer in Racers)
                {
                    if (racer.Ok()) racer.HandleAI();
                    else
                    {
                        racer.RemoveMe = true;
                    }
                }

                for (int i = 0; i < Racers.Count; i++)
                {
                    //UI.Notify(Racers[i].Car.FriendlyName);
                    if (Racers[i].RemoveMe)
                    {
                        if (Util.CanWeUse(Racers[i].Driver) && Racers[i].Driver.IsPlayer) Util.WarnPlayer(ScriptName, "Racer leaves", "You have left the ~y~Meet~w~."); else
                        {
                           if(IsAnyMeetLoaded())
                            {
                                //Util.WarnPlayer(ScriptName, "Racer leaves", "~b~" + Racers[i].Name + "~w~ has left the  ~y~Meet~w~. Reason:"+Racers[i].Reason);
                                string text = "~b~"+Racers[i].Name + "~w~ has left the  ~y~Meet.~w~";
                                if (Racers[i].Reason != "") text += "~n~Reason: " + Racers[i].Reason;
                                UI.Notify(text);
                            }
                        }

                        Racers[i].Clear(false);
                        Racers.Remove(Racers[i]);
                        break;
                    }
                }


                if (CanWeUse(FlarePed)) HandleFlarePedAI();

                if (CanWeUse(FinishBarrel))
                {
                    if (!FinishBarrel.IsInRangeOf(Finishline, 2f))
                    {
                        FinishBarrel.Position = Finishline;
                            Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, FinishBarrel);
                        FinishBarrel.FreezePosition = true;
                    }
                    if (FinishBarrel.Heading != TougeHeading) FinishBarrel.Heading = TougeHeading;
                }

                if (IsAnyMeetLoaded())
                {
                    HandleMeet();


                    //Performance warning
                    if (Game.Player.Character.IsInRangeOf(WaitingArea, WaitingAreRange + 30))
                    {
                        if(OfficialMeet && !OfficialIntroduction)
                        {
                            Util.AddQueuedHelpText("This is an ~y~Official Drag Meet~w~.");
                            Util.AddQueuedHelpText("Here, the Organizer chooses which ~b~Racers~w~ will race in the next heat.");
                            Util.AddQueuedHelpText("Registering has a ~g~$"+Fee+"~w~ fee.");

                            OfficialIntroduction = true;                          
                        }
                        if (!OfficialMeet && !StreetIntroduction)
                        {
                            Util.AddQueuedHelpText("This is a ~y~Street Drag Meet~w~.");
                            Util.AddQueuedHelpText("Here ~b~Racers~w~ challenge each other to race for money.");
                            Util.AddQueuedHelpText("Currently, the money bet is calculated from the money each ~b~Racer~w~ has.");
                            StreetIntroduction = true;
                        }

                        Vehicle veh = Game.Player.Character.CurrentVehicle;
                        if (CanWeUse(veh) && !WarnedOfPerformance)
                        {
                            float AvgPerformance = 0;
                            foreach (Racer racer in Racers)
                            {
                                AvgPerformance += (float)Math.Round(racer.PerformancePoints, 2);
                            }
                            AvgPerformance = AvgPerformance / Racers.Count;

                            float MyPerformance = Util.CalculatePerformancePoints(veh);

                            if (MyPerformance > AvgPerformance + 5)
                            {
                                Util.AddQueuedHelpText("~y~Your current car outperforms most cars in this Meet.");
                                Util.AddQueuedHelpText("Average: " + AvgPerformance + ", yours: " + MyPerformance);

                            }
                            if (MyPerformance + 5 < AvgPerformance)
                            {
                                Util.AddQueuedHelpText("~y~Most of the cars in this Meet outperform yours.");
                                Util.AddQueuedHelpText("Average: " + AvgPerformance + ", yours: " + MyPerformance);
                            }
                            WarnedOfPerformance = true;

                        }
                    }
                }




            }



            if (MeetState == DragRaceState.CountDown)
            {
                if (FlarePed.IsShooting || Util.IsSubttaskActive(FlarePed, (Subtask)126))
                {
                  //if(CanWeUse(FinishBarrel)) Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, FinishBarrel);

                    foreach (Racer racer in RacersParticipating())
                    {
                        racer.TaskRace();
                    }
                    //foreach(Racer racer in RacersNotParticipating(false)) Function.Call(Hash.TASK_TURN_PED_TO_FACE_ENTITY, racer.Driver, DragMeets.FinishBarrel, 3000);


                    FlarePed.Task.Cower(3000);
                    //if (GetPlayerRacer() == null) Game.Player.Character.Position = Finishline;
                    MeetState = DragRaceState.RaceInProgress;
                    Function.Call(Hash.SET_PAD_SHAKE,0,500,200);

                    SomethingWrongTimer = Game.GameTime + (int)(StartingLine.DistanceTo(Finishline)*100f);
                    RaceStartedTime = Game.GameTime;



                    if (SpectCamera != null && World.RenderingCamera == SpectCamera)
                    {
                        int number = RacersInState(RacerStatus.Picked).Count - 1;
                        Function.Call(Hash.SET_FOCUS_ENTITY, RacersInState(RacerStatus.Picked)[0].Car);
                        //Function.Call(Hash.ATTACH_CAM_TO_ENTITY, SpectCamera, RacersInState(RacerStatus.Picked)[number-1].Car, 0f, 0f, RacersInState(RacerStatus.Picked)[number-1].Car.Model.GetDimensions().Z * 0.64f, false);
                        Vector3 offset = Util.GetEntityOffset(RacersInState(RacerStatus.Picked)[1].Car,0,5)- RacersInState(RacerStatus.Picked)[1].Car.Position;
                        
                        if (Util.IsPosRightOf(RacersInState(RacerStatus.Picked)[0].Car, RacersInState(RacerStatus.Picked)[1].Car.Position, 0f)) offset = Util.GetEntityOffset(RacersInState(RacerStatus.Picked)[1].Car, 0, -5) - RacersInState(RacerStatus.Picked)[1].Car.Position;


                        Function.Call(Hash.ATTACH_CAM_TO_ENTITY, SpectCamera, RacersInState(RacerStatus.Picked)[number - 1].Car, offset.X, offset.Y, 3f, false);
                        SpectCamera.PointAt(RacersInState(RacerStatus.Picked)[number].Driver, 0, RacersInState(RacerStatus.Picked)[number].Car.UpVector);
                    }
                }

            }
            if (!Game.Player.Character.IsOnFoot && Game.Player.Character.IsInRangeOf(WaitingArea, WaitingAreRange * 2))
            {
                World.DrawMarker(MarkerType.VerticalCylinder, WaitingArea + new Vector3(0, 0, -0.5f), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(WaitingAreRange * 2f, WaitingAreRange* 2f, 1.5f), Color.Yellow);
            }
            if (MeetState == DragRaceState.RaceInProgress)
            {
                
                Function.Call(Hash.LOCK_MINIMAP_ANGLE, DragHeading);
                Vector3 pos = (RacersParticipating()[0].Car.Position + RacersParticipating()[1].Car.Position )/2; //Util.LerpByDistance(RacersParticipating()[0].Car.Position,RacersParticipating()[1].Car.Position, 0.5f);
                Function.Call(Hash.LOCK_MINIMAP_POSITION, pos.X, pos.Y);
                

              
                if (SomethingWrongTimer < Game.GameTime && RacersParticipating().Count != Winners.Count)
                {
                    WarnPlayer(ScriptName + " " + ScriptVer, "RACE DIDN'T FINISH PROPERLY", ScriptName + " will skip this race.");
                    foreach (Racer racer in RacersParticipating())
                    {
                        racer.Driver.Money += Prize;
                        racer.TaskFinish();
                    }
                    Winners.Clear();
                    MeetState = DragRaceState.Idle;


                    //foreach (Racer racer in RacersNotParticipating(false)) if (racer.Driver.IsOnFoot) racer.Driver.Task.RunTo(WaitingArea);

                }

                if (FinishCamTime!=0)
                {
                    if (FinishCamTime < Game.GameTime)
                    {                    
                        Game.TimeScale = 1;
                        FinishCamTime = 0;
                        if(SpectCamera != null)
                        {
                            SpectCamera.Destroy();
                            SpectCamera = null;
                            World.RenderingCamera = null;
                            Function.Call(Hash.CLEAR_FOCUS);
                        }
                    }
                    Util.DrawLine(Util.GetEntityOffset(FinishBarrel, 0, 10) - new Vector3(0,0,FinishBarrel.HeightAboveGround * 0.9f), Util.GetEntityOffset(FinishBarrel, 0, -10) - new Vector3(0, 0, FinishBarrel.HeightAboveGround*0.9f));
                }
                foreach (Racer racer in RacersParticipating())
                {

                    if(racer.Car.IsInRangeOf(Finishline, 30f))
                    {
                        if (Util.IsPosAheadentity(FinishBarrel, racer.Car.Position, -10f) && FinishCamTime == 0 && Winners.Count == 0 && SpectCamera != null)
                        {

                            if (SpectCamera == null) SpectCamera = World.CreateCamera(Util.GetEntityOffset(FinishBarrel, 20, 15) + new Vector3(0, 0, 3), new Vector3(0, 0, -DragHeading), GameplayCamera.FieldOfView - 30);
                            float Scale =1f;
                            int ms = 5000;
                            SpectCamera.Detach();
                            float dist = racer.Car.Position.DistanceTo(FinishBarrel.Position);
                            if (World.GetNearbyVehicles(FinishBarrel.Position, dist + 5).Length < 2) SpectCamera.Position = Util.GetEntityOffset(FinishBarrel, -2f, 0) +new Vector3(0,0, FinishBarrel.Model.GetDimensions().Z); //Util.GetEntityOffset(FinishBarrel, 20, 15) + new Vector3(0, 0, 3);
                            else
                            {
                                SpectCamera.Position = Util.GetEntityOffset(FinishBarrel, 5, 15) + new Vector3(0, 0, 1);
                                SpectCamera.FieldOfView -= GameplayCamera.FieldOfView - 40;
                                Scale = 0.03f;
                                ms = 400;
                            }
                            SpectCamera.PointAt(FinishBarrel,FinishBarrel.UpVector);
                            World.RenderingCamera = SpectCamera;
                            Game.TimeScale = Scale;
                            FinishCamTime = Game.GameTime + ms;
                        }


                        if (Util.IsPosAheadentity(FinishBarrel, racer.Car.Position, 0f) && !Winners.Contains(racer))
                        {
                            if (SpectCamera != null) SpectCamera.Shake(CameraShake.SmallExplosion, 0.04f);
                            racer.TrackTime = racer.TrackTime.Add(new TimeSpan(0, 0, 0, 0, Game.GameTime - racer.StartTime));
                            SomethingWrongTimer += 5000;
                            Winners.Add(racer);
                            racer.RacingToFinish = false;
                            //if (!racer.Driver.IsPlayer) Util.TempAction(racer.Driver, racer.Car, 1, Util.RandomInt(1500, 3000));
                            if (!racer.Driver.IsPlayer) if (TougeMode) Function.Call(Hash.TASK_VEHICLE_DRIVE_WANDER, racer.Driver, racer.Car, 1f, 4 + 8 + 16); else Util.TempAction(racer.Driver, racer.Car, 24, 1000);
                            //string TrackTime = racer.TrackTime.Seconds +":"+racer.TrackTime.Milliseconds.ToString().Substring(0,3);
                            UI.Notify("~b~" + racer.Name + "~w~ - ~g~" + racer.TrackTime);

                            /*
                            if (Winners.Count == 0)
                            {

                                string tracktimeSeconds = (racer.TrackTime / 1000).ToString();
                                string tracktime = tracktimeSeconds + "," + racer.TrackTime.ToString().Substring(2);

                                UI.Notify("~b~" + racer.Name + "~w~ - ~g~" + tracktime + "s");
                            }
                            else
                            {
                            }
                            */
                        }
                    }
                   

                    if (racer.Car.IsInRangeOf(racer.Destination, 7f) && !Winners.Contains(racer))
                    {


                    }
                }
            }
            if(MeetState == DragRaceState.CountDown)
            {
                int time = (((CountDown - Game.GameTime) / 1000) + 1);
                //DisplayHelpTextThisFrame(time.ToString());

                if(time < 3)
                {
                    foreach (Racer racer in RacersParticipating())
                    {
                        if (Util.ForwardSpeed(racer.Car)>1f && !racer.Penalized)
                        {
                            UI.Notify(racer.Name+"~o~ penalized");
                            racer.Penalized = true;
                        }
                    }
                }

            }
            foreach (Racer racer in Racers)
            {
                //Util.DrawTextHere(Util.IsDriving(racer.Driver).ToString(), racer.Driver.Position);
                //Util.DrawLine(racer.Car.Position, racer.Destination);
                racer.OnTick();
            }




            //Util.DrawLine(Game.Player.Character.Position, WaitingArea);
            //Util.DrawLine(WaitingArea, StartingLine);
            if (DebugNotifications)
            {
                Util.DrawLine(StartingLine, Finishline);
                if (PathToWaitingArea != Vector3.Zero) Util.DrawLine(Finishline, PathToWaitingArea);
                if (PathToWaitingArea != Vector3.Zero) Util.DrawLine(PathToWaitingArea, WaitingArea);

                if (PathToStartingLine != Vector3.Zero) Util.DrawLine(WaitingArea, PathToStartingLine);
                Util.DrawLine(PathToStartingLine, StartingLine);
            }
            Util.HandleMessages();
            Util.HandleNotifications();

        }

        public void SmartVehicleAddition(Ped ped, Vehicle veh, bool details)
        {
            Model PlayerModel = ped.Model;

               List<Model> MainCharacters = new List<Model> { PedHash.Michael,PedHash.Franklin,PedHash.Trevor,PedHash.FreemodeMale01,PedHash.FreemodeFemale01};

            if (MainCharacters.Contains(PlayerModel)) PlayerModel = null;

            if (IsInRacerList(veh, true, true, 2) && !details)
            {
                if(DebugNotifications) WarnPlayer(ScriptName, "Vehicle already saved", "Racers.xml alredy contains 2 ~b~" + veh.FriendlyName + "s~w~.");
            }
            else
            {
                string Name = veh.FriendlyName;
                if (Name == "NULL") Name = veh.DisplayName;
                string CarKind="Average";
                if(KindsAllowed.Count>0) CarKind = KindsAllowed[0];
                int Shift = 50;
                int ReactionTime = 250;
                
                if (details)
                {
                    UI.Notify("~b~[Oponent Creator]~w~: Set the Racer's display name.");
                    Name = Game.GetUserInput(veh.FriendlyName, 20);

                    UI.Notify("~b~[Oponent Creator]~w~: Set the Racer's kind.");
                    CarKind = Game.GetUserInput(KindsAllowed[0], 20);

                    UI.Notify("~b~[Oponent Creator]~w~: Set the Racer's Shifting ability (0 - 100). Its measured as a percentage.");
                    int.TryParse(Game.GetUserInput((50).ToString(), 20), out Shift);

                    UI.Notify("~b~[Oponent Creator]~w~: Set the Racer's Reaction time (0 - 1000). Its measured in miliseconds.");
                    int.TryParse(Game.GetUserInput((250).ToString(), 20), out ReactionTime);

                }

                if (details)
                {
                    if (PlayerModel == null)
                    {
                        Util.SaveDriverToFile(Name, null, veh, RacersFile, true, true, CarKind, Shift, ReactionTime);
                        WarnPlayer(ScriptName, "New vehicle saved", "~b~" + veh.FriendlyName + "~w~ (with tuning) added to Racers.xml.");

                    }
                    else
                    {
                        Util.SaveDriverToFile(Name, Game.Player.Character, veh, RacersFile, true, true, CarKind, Shift, ReactionTime);
                        WarnPlayer(ScriptName, "New vehicle saved", "~b~Ped model~w~ and ~b~" + veh.FriendlyName + "~w~ (with tuning) added to Racers.xml.");

                    }
                }
                else
                {
                    Util.SaveDriverToFile(Name, null, veh, RacersFile, false, false, CarKind, Shift, ReactionTime);
                    WarnPlayer(ScriptName, "New vehicle saved", "~b~" + veh.FriendlyName + "~w~ (generic) saved to Racers.xml.");

                }
            }
        }
        public void LoadMeetTriggersFromFile()
        {

            XmlDocument filetrigger = new XmlDocument();

            int number = 0;
            foreach (string meet in Directory.GetFiles(@"scripts\\DragMeets\Meets"))
            {
                if (File.Exists(meet))
                {
                    number++;
                    filetrigger.Load(meet);
                    if (filetrigger != null)
                    {
                        XmlNode list = filetrigger.SelectSingleNode("//Trigger");
                        if (list == null) list = filetrigger.SelectSingleNode("//WaitingZone");
                        if (list == null) UI.Notify("Failed to load " + meet);
                        else
                        {
                            Vector3 pos = new Vector3(int.Parse(list.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(list.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(list.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
                            MeetTriggers.Add(pos);
                            Blip blip = World.CreateBlip(pos);
                            blip.Sprite = BlipSprite.DollarSign;
                            if (filetrigger.SelectSingleNode("//Touge") == null)
                            {
                                blip.Color = BlipColor.Green;
                                blip.Name = "Drag Meet"; //"[DEV] " + World.GetStreetName(pos) + 

                            }
                            else
                            {
                                blip.Color = BlipColor.Yellow;
                                blip.Name = "Street Race Meet";
                            }
                            blip.IsShortRange = true;
                            MeetBlips.Add(blip);
                        }
                    }

                }
            }

            if (DebugNotifications) UI.Notify(number.ToString() + " meets loaded.");
        }
        public void HandleMeetSpawnDespawn(bool immersive)
        {

            //Meet despawn;
            if (IsAnyMeetLoaded())
            {
                float distance = StartingLine.DistanceTo(Finishline) * 1.5f;
                if (distance < 900) distance = 900;
                if (Game.Player.Character.Position.DistanceTo(WaitingArea) > (distance) || DespanwnRequested)
                {
                    foreach (Blip blip in MeetBlips) blip.Alpha = 255;
                    ClearMeet(true);
                }
            }//Meet Spawn
            else if (MeetTriggers.Count > 0)
            {
                foreach (Vector3 meetPos in MeetTriggers)
                {
                    bool Spawn = false;
                    if (immersive)
                    {
                        if (Game.Player.Character.IsInRangeOf(meetPos, 300f)) Spawn = true;
                    }
                    else
                    {

                        if (Game.Player.Character.IsInRangeOf(meetPos, 30f))
                        {
                            Util.DrawLine(Game.Player.Character.Position, meetPos);
                            DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(DragMeets.InteractKey) + " to spawn this meet.");
                            if (Game.IsControlJustPressed(2, InteractKey)) Spawn = true;
                        }

                    }

                    if (Spawn)
                    {
                        File.AppendAllText(@"scripts\\DragMeets\debug.txt", "\n Spawning meet ");

                        LoadSettings(configpath);

                        XmlDocument filetrigger = new XmlDocument();
                        int number = 0;
                        foreach (string meet in Directory.GetFiles(@"scripts\\DragMeets\Meets"))
                        {
                            if (File.Exists(meet))
                            {
                                number++;
                                filetrigger.Load(meet);
                                if (filetrigger == null)
                                {
                                    File.AppendAllText(@"scripts\\DragMeets\debug.txt", " - ERROR, no meet file found.");
                                    UI.Notify("~r~Failed to load " + meet);
                                }
                                else
                                {
                                    XmlNode list = filetrigger.SelectSingleNode("//Trigger");
                                    if (list == null)
                                    {
                                        File.AppendAllText(@"scripts\\DragMeets\debug.txt", " - NO MEET TRIGGER FOUND, looking for WaitingZone");

                                        list = filetrigger.SelectSingleNode("//WaitingZone");
                                    }

                                    if (list == null)
                                    {
                                        UI.Notify("~r~Failed to load " + meet);
                                    }
                                    else
                                    {
                                        File.AppendAllText(@"scripts\\DragMeets\debug.txt", " - WaitingZone loaded");

                                        Vector3 pos = new Vector3(float.Parse(list.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), float.Parse(list.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), float.Parse(list.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
                                        if (meetPos == pos)
                                        {
                                           if(!immersive) Game.FadeScreenOut(500);

                                            //Load all the info




                                            Vector3 position;
                                            XmlNode node = filetrigger.SelectSingleNode("//StartingLine");
                                            position = new Vector3(float.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), float.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), float.Parse(node.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
                                            StartingLine = position;

                                            string StartingLineProp="";
                                            node = filetrigger.SelectSingleNode("//StartingLine/Prop");
                                            if (node != null) StartingLineProp = node.InnerText; else if (DebugNotifications) UI.Notify("~r~StartingLine/Prop not found");


                                            node = filetrigger.SelectSingleNode("//FinishLine");
                                            position = new Vector3(float.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), float.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), float.Parse(node.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
                                            Finishline = position;


                                            node = filetrigger.SelectSingleNode("//WaitingZone");
                                            position = new Vector3(float.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), float.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), float.Parse(node.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
                                            WaitingArea = position;

                                            string WaitingZoneProp = "";
                                            node = filetrigger.SelectSingleNode("//WaitingZone/Prop");
                                            if (node != null) WaitingZoneProp = node.InnerText; else if (DebugNotifications) UI.Notify("~r~WaitingZone/Prop not found");


                                            WaitingAreRange = float.Parse(filetrigger.SelectSingleNode("//WaitingZone/Radius").InnerText, CultureInfo.InvariantCulture);

                                            node = filetrigger.SelectSingleNode("//Heading");
                                            if (node != null) DragHeading = int.Parse(node.InnerText, CultureInfo.InvariantCulture);
                                            else
                                            {
                                                Vector2 angle = new Vector2(Finishline.X - StartingLine.X, Finishline.Y - StartingLine.Y);
                                                DragHeading = (Function.Call<float>(Hash.GET_HEADING_FROM_VECTOR_2D, angle.X, angle.Y));
                                            }

                                            node = filetrigger.SelectSingleNode("//TougeHeading");
                                            if (node != null) TougeHeading = int.Parse(node.InnerText, CultureInfo.InvariantCulture);
                                            else TougeHeading = DragHeading;

                                            node = filetrigger.SelectSingleNode("//SeparationBetweenCars");
                                            if (node != null) Separation = int.Parse(node.InnerText, CultureInfo.InvariantCulture); else if (DebugNotifications) UI.Notify("~r~SeparationBetweenCars not found");

                                            node = filetrigger.SelectSingleNode("//MaxCarLimit");
                                            if (node != null) RacerCap = int.Parse(node.InnerText, CultureInfo.InvariantCulture); else if (DebugNotifications) UI.Notify("~r~MaxCarLimit not found");


                                            node = filetrigger.SelectSingleNode("//PathToStartingLine");
                                            Vector3 Midpoint = Vector3.Zero;
                                            if (node != null) Midpoint = new Vector3(int.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
                                            else if(DebugNotifications) UI.Notify("~r~PathToStartingLine not found");
                                            PathToStartingLine = Midpoint;


                                            /*
                                            node = filetrigger.SelectSingleNode("//PathToWaitingArea");
                                            Midpoint = Vector3.Zero;
                                            if (node != null) Midpoint = new Vector3(int.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(node.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
                                            else UI.Notify("~r~PathToWaitingArea not found");
                                            PathToWaitingArea = Midpoint;
                                            */

                                            //Path to waiting area

                                            //Radios
                                            XmlNodeList radios = filetrigger.SelectNodes("//Radios/*");
                                            foreach (XmlElement radio in radios)
                                            {
                                                Radio = radio.InnerText;
                                            }
                                            if (SelfRadio) Radio = "RADIO_19_USER";

                                            Vector3 path;
                                            XmlNodeList paths = filetrigger.SelectNodes("//PathToWaitingArea/*");
                                            foreach (XmlElement element1 in paths)
                                            {
                                                path = new Vector3(int.Parse(element1.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
                                                ToWaitingArea.Add(path);
                                            }

                                            node = filetrigger.SelectSingleNode("//CarsPerRace");
                                            if (node != null) RacersPerRace = int.Parse(node.InnerText, CultureInfo.InvariantCulture); else if (DebugNotifications) UI.Notify("~r~CarsPerRace not found");


                                            //Fee
                                            node = filetrigger.SelectSingleNode("//Fee");
                                            if (node != null) Fee = int.Parse(node.InnerText, CultureInfo.InvariantCulture); else if (DebugNotifications) UI.Notify("~r~Fee not found");

                                            //Prize
                                            node = filetrigger.SelectSingleNode("//Prize");
                                            if (node != null) Prize = int.Parse(node.InnerText, CultureInfo.InvariantCulture); else if (DebugNotifications) UI.Notify("~r~Prize not found");


                                            foreach (Blip blip in MeetBlips) blip.Alpha = 0;

                                            //KindsAllowed
                                            foreach (XmlNode kind in filetrigger.SelectNodes("//CarsAllowed/*"))
                                            {
                                                KindsAllowed.Add(kind.InnerText);
                                                if (kind.InnerText.Contains("Official")) OfficialMeet = true;
                                            }

                                            
                                            //TougeMode
                                            node = filetrigger.SelectSingleNode("//Touge");
                                            if (node != null) TougeMode = true;//bool.Parse(node.InnerText);
                                            
                                            //CrowdArea
                                            Vector3 parkpos;
                                            XmlNodeList nodelist = filetrigger.SelectNodes("//CrowdAreas/*");
                                            foreach (XmlElement element1 in nodelist)
                                            {
                                                parkpos = new Vector3(int.Parse(element1.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture), int.Parse(element1.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture));
                                                CrowdAreas.Add(parkpos);
                                            }

                                            List<string> nothing = new List<string>();

                                            SpawnDriversFromFile(nothing, (int)Math.Round(RacerCap*0.7f));


                                            if (CrowdAreas.Count > 0)
                                            {
                                                foreach(Vector3 area in CrowdAreas)
                                                {
                                                    for (int i = 0; i < WaitingAreRange/4; i++)
                                                    {
                                                        Spectators.Add(new Spectator(World.CreateRandomPed(area.Around(5)), null));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int i = 0; i < 6; i++)
                                                {
                                                    Spectators.Add(new Spectator(World.CreateRandomPed(WaitingArea.Around(5)), null));
                                                }
                                            }

                                            MeetState = DragRaceState.Idle;
                                            FinishBarrel = World.CreateProp("prop_offroad_barrel01", DragMeets.Finishline, new Vector3(0, 0, TougeHeading), false, true);
                                            FinishBarrel.IsPersistent = true;

                                            Function.Call(GTA.Native.Hash._0x0DC7CABAB1E9B67E, FinishBarrel, true); //Load Collision

                                            FinishBarrel.ApplyForce(new Vector3(0, 0, -1), new Vector3(0, 0, 0));

                                            Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, FinishBarrel);



                                            if (!Util.CanWeUse(FlarePed)) FlarePed = World.CreatePed(PedHash.Bartender01SFY, StartingLine);
                                            FlarePed.Heading = DragHeading + 180;

                                            if (!immersive) Game.FadeScreenIn(2000);
                                            if (DebugNotifications) UI.Notify("~g~" + meet + " loaded.");


                                            //""
                                            if (StartingLineProp != "")
                                            {
                                                Prop prop = World.CreateProp(StartingLineProp, Util.GetEntityOffset(FlarePed, -3, 0), false, true);
                                                if (prop.Model == "stt_prop_race_gantry_01") prop.Heading = DragHeading + 90; else prop.Heading = DragHeading;
                                                if (prop.Model == "prop_start_gate_01") prop.Position += new Vector3(0, 0, -1);

                                                MeetProps.Add(prop);
                                            }
                                            if (WaitingZoneProp != "")
                                            {
                                                Prop prop = World.CreateProp(WaitingZoneProp, WaitingArea, false, true);
                                                prop.Heading = DragHeading + 90;

                                                MeetProps.Add(prop);
                                            }
                                            DevChatter = true;
                                            if (!DevChatter)
                                            {

                                            Util.AddQueuedHelpText("~g~[Eddlm]:~w~ Thanks for trying this out! Remember, this is an ~r~ALPHA~w~. Bugs and stuff.");
                                            Util.AddQueuedHelpText("~g~[Eddlm]:~w~ Be sure to report anything you like/dislike back to GTA5Forums.");
                                            Util.AddQueuedHelpText("~g~[Eddlm]:~w~ Your opinion is important.");
                                            Util.AddQueuedHelpText("~g~[Eddlm]:~w~ BTW, while you're registered in the meet, you can enter new cars to spawn opponents.");

                                                DevChatter = true;
                                            }

                                            File.AppendAllText(@"scripts\\DragMeets\debug.txt", " - Meet loaded");

                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        bool DevChatter = false;
        public void PickForRace(Racer racer)
        {
            racer.Car.Repair();
            racer.Picked = RacerStatus.Picked;
            racer.Offset = 1;
            racer.GotToMidPoint = false;
            racer.Forced = false;

            if (SameDriverReference == 0)
            {
              if(DebugNotifications)  UI.Notify("Got Performance sample: " + racer.PerformancePoints + "~n~Looking for similar vehicles in Racers...");

                SameDriverReference = (int)racer.PerformancePoints;
                //SameDriverReference = (int)racer.Car.ClassType;
            }

            if (DebugNotifications)
            {
                WarnPlayer(ScriptName, racer.Car.FriendlyName,"Performance:"+racer.PerformancePoints+ "~n~Shifting: " + racer.Shifting.ToString() + "~n~Reaction: " + racer.Reaction.ToString());
            }

        }

        public void PickForRace(Racer racer, RacerStatus state)
        {
            racer.Car.Repair();
            racer.Picked = state;
            int i = RacersInState(state).Count;

            if (i == 1) racer.Offset = Separation * 1;
            if (i == 2) racer.Offset = Separation * -1;
            if (i == 3) racer.Offset = Separation * 2;
            if (i == 4) racer.Offset = Separation * -2;
            if (i == 5) racer.Offset = Separation * 3;
            if (i == 6) racer.Offset = Separation * -3;
            if (i == 7) racer.Offset = Separation * 4;
            if (i == 8) racer.Offset = Separation * -4;
            if (i == 9) racer.Offset = Separation * 5;
            if (i == 10) racer.Offset = Separation * -5;

            racer.GotToMidPoint = false;
            racer.Forced = false;

            if (SameDriverReference == 0)
            {
                if (DebugNotifications) UI.Notify("Got Performance sample: " + racer.PerformancePoints + "~n~Looking for similar vehicles in Racers...");

                SameDriverReference = (int)racer.PerformancePoints;
                //SameDriverReference = (int)racer.Car.ClassType;
            }

            if (DebugNotifications)
            {
                WarnPlayer(ScriptName, racer.Car.FriendlyName, "Performance:" + racer.PerformancePoints + "~n~Shifting: " + racer.Shifting.ToString() + "~n~Reaction: " + racer.Reaction.ToString());
            }
            if (GetPlayerRacer() == racer)
            {
                if(state==RacerStatus.Picked && OfficialMeet) Util.AddQueuedHelpText("~g~You've been picked for this race.~w~ Go to your starting position.");
                if (state == RacerStatus.PickedNext) Util.AddQueuedHelpText("~g~You've been picked for the next race.~w~ Be ready when you're called to the starting line.");
            }
        }
        public void HandleMeet()
        {
            if (Util.CanWeUse(FlarePed) && FlarePedInPosition())
            {

                if (DynamicCrowdAreas.Count == 0 && Spectators.Count>0)
                {
                    Vector3 specialcrowd = Util.GetEntityOffset(FlarePed, -5, RacersPerRace * 5);
                    DynamicCrowdAreas.Add(specialcrowd);

                    for (int spectint = 0; spectint < 5; spectint++)
                    {
                        Spectator spec = Spectators[Util.RandomInt(0, Spectators.Count - 1)];
                        spec.DesiredPos = specialcrowd;
                    }

                    specialcrowd = Util.GetEntityOffset(FlarePed, -5, -(RacersPerRace * 5));
                    DynamicCrowdAreas.Add(specialcrowd);

                    for (int spectint = 0; spectint < 5; spectint++)
                    {
                        Spectator spec = Spectators[Util.RandomInt(0, Spectators.Count - 1)];
                        spec.DesiredPos = specialcrowd;
                    }


                   if(DebugNotifications) WarnPlayer(ScriptName + " " + ScriptVer, "Start Line Crowd Areas", ScriptName + " has created two custom crowd areas near the starting line.");
                }
            }
            IsStartLineBusy = false;
            foreach (Racer racer in RacersParticipating()) if (racer.Car.Speed>0.5f && racer.Car.IsInRangeOf(StartingLine, RacersPerRace*4 )) { IsStartLineBusy = true; break; }

            switch (MeetState)
            {
                case DragRaceState.NotStarted:
                    {

                        break;
                    }
                case DragRaceState.Idle:
                    {
                        if (StartingLine == Vector3.Zero) StartingLine =  Util.ToGround(Util.GetEntityOffset(Game.Player.Character, 0, 0),3f);
                        if (Finishline == Vector3.Zero) Finishline = World.GetCrosshairCoordinates().HitCoords;// Util.ToGround(Util.GetPosAheadOf(Game.Player.Character, 200));
                        if (WaitingArea == Vector3.Zero) WaitingArea = Util.ToGround(Util.GetEntityOffset(Game.Player.Character, -80, 0),3f);
                        if (DragHeading == 0) DragHeading = Game.Player.Character.Heading;
                        if (Racers.Count > 0 && !Util.CanWeUse(FlarePed)) FlarePed = World.CreatePed(PedHash.Bartender01SFY, StartingLine.Around(20));

                        ChallengePrize = 0 ;

                        if (RacersInState(RacerStatus.PickedNext).Count > 0)
                        {
                            foreach (Racer racer in RacersInState(RacerStatus.PickedNext)) racer.Picked = RacerStatus.Picked;
                        }
                        int tempCarsPerRace = RacersPerRace;
                        if (tempCarsPerRace > Racers.Count)
                        {
                            tempCarsPerRace = Racers.Count;
                        }                        
                        if (RacersNotParticipating(true).Count > 0)
                        {
                            if (RacersParticipating().Count < tempCarsPerRace)
                            {
                                HandleRacerPicking(RacerStatus.Picked, tempCarsPerRace);
                            }
                            else
                            {
                                if (DebugNotifications) UI.Notify("Finished picking racers.");                            
                                SameDriverReference = 0;
                                PickSameDriversTries = 0;
                                PerformanceDifferenceMin = 3;
                                MeetState = DragRaceState.Setup;

                                //Expectations
                                foreach (Racer racer in RacersParticipating())
                                {
                                    racer.max_willing_to_bet = 60;
                                    racer.min_willing_to_bet = 30;

                                    if (!racer.Driver.IsPlayer)
                                    {
                                        foreach (Racer expect in RacersParticipating())
                                        {
                                            float performance = 0;
                                            int i = 0;
                                            if (racer.Driver.Handle != expect.Driver.Handle)
                                            {
                                                i++;
                                                performance += racer.PerformancePoints;
                                                //if(part.PerformancePoints-10>expect.PerformancePoints)
                                            }
                                            performance = performance / i;
                                            if (performance > expect.PerformancePoints + 10)
                                            {
                                                racer.max_willing_to_bet = 50;
                                                racer.min_willing_to_bet = 0;
                                                expect.Expectations = RacerExpectations.Lose;
                                            }
                                            if (performance < expect.PerformancePoints - 10)
                                            {
                                                racer.max_willing_to_bet = 80;
                                                racer.min_willing_to_bet = 40;
                                                expect.Expectations = RacerExpectations.Win;
                                            }
                                        }
                                    }                                   
                                }

                                string text ="";
                                foreach (Racer part in RacersParticipating())
                                {
                                    if (ChallengePrize == 0)
                                    {
                                        ChallengePrize = (part.GetRacerMoney() * Util.RandomInt(part.min_willing_to_bet, part.max_willing_to_bet)) / 100;
                                    }
                                    if ((part.GetRacerMoney() * part.max_willing_to_bet) / 100 < ChallengePrize)
                                    {
                                        ChallengePrize = (part.GetRacerMoney() * Util.RandomInt(part.min_willing_to_bet, part.max_willing_to_bet)) / 100;
                                    }



                                    if (part == RacersParticipating()[0])
                                    {
                                        text += "~b~" + part.Name;
                                        //text += "-" + part.PerformancePoints;
                                    }
                                    else
                                    {
                                        text += "~w~ vs ~b~" + part.Name;
                                        //text += "-" + part.PerformancePoints;

                                    }
                                }
                                if (!OfficialMeet)
                                {

                                    //ChallengePrize = RacersInState(RacerStatus.Picked)[0].GetRacerMoney() / Util.RandomInt(1, 4);
                                    //if (ChallengePrize > RacersInState(RacerStatus.Picked)[1].GetRacerMoney()) ChallengePrize = RacersInState(RacerStatus.Picked)[1].GetRacerMoney();
                                    if (ChallengePrize > 0) text += "~n~Bet: ~g~$" + ChallengePrize; else text += "~n~Bet: ~g~No bet";
                                }
                                else
                                {
                                    text += "~n~Prize: ~g~$" + Prize;
                                }
                                
                                if (GetPlayerRacer() != null)
                                {

                                    if (RacersInState(RacerStatus.Picked).Contains(GetPlayerRacer())) Util.AddQueuedHelpText("~g~Its your turn.~w~ Go to your ~y~starting position~w~.");
                                    else if(CinematicCam) Util.AddQueuedHelpText("Aim to the ~y~Starting Line Girl~w~ if you want to enable cinematic mode for this race.");

                                    if (RacersInState(RacerStatus.Picked).Contains(GetPlayerRacer()))
                                    {
                                        if (!OfficialMeet)
                                        {
                                            Util.AddQueuedHelpText("The bet for this race is set at ~g~$" + ChallengePrize+".~w~ You can change it introducing the cheat '~b~bet~w~'."); 
                                        }
                                        else Util.AddQueuedHelpText("The prize for this race is $~g~"+Prize);
                                    }
                                }
                                else if (CinematicCam) Util.AddQueuedHelpText("Aim to the ~y~Starting Line Girl~w~ if you want to enable cinematic mode for this race.");

                                Util.Notify("CHAR_LAMAR", "~b~Lamar", "Next race", text);                                
                            }
                        }
                        break;
                    }
                case DragRaceState.Setup:
                    {

                        if (RacersParticipating().Count == RacersPerRace)
                        {
                            int NearStartingLine=0;
                            int i = 0;
                            int inposition = 0;
                            foreach (Racer racer in RacersParticipating())
                            {
                                i++;
                                if (racer.Driver.IsInRangeOf(StartingLine, 20f)) NearStartingLine++;
                                if (racer.InPosition()) inposition++;
                            }

                            if (NearStartingLine > 0 && OfficialMeet && RacersInState(RacerStatus.PickedNext).Count < RacersPerRace) HandleRacerPicking(RacerStatus.PickedNext, RacersPerRace);

                            if (inposition >= RacersParticipating().Count)
                            {
                                if (BetTimeFrame == 0)
                                {
                                    Util.Notify("CHAR_LAMAR", "~b~Lamar", "BET TIME", "Make your bets, ladies and gentlemen.");
                                    BetTimeFrame = Game.GameTime + 6000;
                                }
                                else if(RacersNotParticipating(true).Count>0)
                                {
                                    Racer better = RacersNotParticipating(true)[Util.RandomInt(0, RacersNotParticipating(true).Count)];

                                    if (!better.Driver.IsPlayer && !better.HasBet)
                                    {
                                        if (Util.ThrowDice(60) && better.Driver.Money >= MoneyBet)
                                        {
                                            Racer bet = RacersParticipating()[Util.RandomInt(0, RacersParticipating().Count)];
                                            UI.Notify("~b~" + better.Name + "~w~ bets on ~g~" + bet.Name);
                                            bet.BetsOnHim.Add(better);
                                            better.HasBet = true;
                                            Util.ChangePedMoney(better.Driver, -MoneyBet);
                                            break;
                                        }

                                    }
                                }


                                if (BetTimeFrame < Game.GameTime && TrackReady() && !PlayerNearRacers())
                                {
                                    BetTimeFrame = 0;
                                    UI.Notify("Bet time is over.");



                                    CountDown = Game.GameTime + 5000;
                                    MeetState = DragRaceState.CountDown;

                                    SameDriverReference = 0;
                                    PickSameDriversTries = 0;
                                    PerformanceDifferenceMin = 3;


                                    if (CinematicCam && SpectCamera == null && (GetPlayerRacer() == null || GetPlayerRacer().Picked != RacerStatus.Picked))
                                    {
                                        if (FlarePed.IsOnScreen && (GameplayCamera.IsAimCamActive || GameplayCamera.IsFirstPersonAimCamActive))
                                        {
                                            SpectCamera = World.CreateCamera(StartingLine, new Vector3(0, 0, DragHeading), GTA.GameplayCamera.FieldOfView);
                                            SpectCamera.Position = Util.GetEntityOffset(FlarePed, -(ForwardOffset + 8), 0);
                                            SpectCamera.PointAt(FlarePed);
                                            World.RenderingCamera = SpectCamera;
                                        }
                                    }
                                    foreach (Racer racer in RacersParticipating())
                                    {
                                        
                                        //Util.SetRadioQuiet(racer.Car);
                                        //racer.Car.IsRadioEnabled = false;
                                        if (Util.DiffBetweenAngles(racer.Car.Heading, DragHeading) > 3 && !racer.Driver.IsPlayer)
                                        {
                                            racer.Car.Position = racer.Car.Position + new Vector3(0, 0, 1);
                                            racer.Car.Heading = DragMeets.DragHeading;
                                        }
                                    }
                                    foreach (Racer better in RacersNotParticipating(false)) if (better.HasBet) better.HasBet = false;
                                }
                            }
                        }
                        else
                        {
                            foreach (Racer racer in RacersParticipating())
                            {
                                racer.Picked = RacerStatus.NotPicked;
                            }
                            WarnPlayer(ScriptName + " " + ScriptVer, "ERROR IN CURRENT PARTICIPANTS", ScriptName + " will try to get new participants now.");
                            MeetState = DragRaceState.Idle;
                            
                        }
                        break;
                    }
                case DragRaceState.CountDown:
                    {

                        if (DistanceMultiplier != 100 && !TougeMode)
                        {
                            if (FlarePedInPosition())
                            {
                                Vector3 Finish1 = Util.LerpByDistance(StartingLine, Finishline, StartingLine.DistanceTo(Finishline)*(DistanceMultiplier*0.01f));

                                if (DebugNotifications)
                                {
                                    UI.Notify("Track length reduction requested");
                                    UI.Notify("Old distance:" + Finishline.DistanceTo(StartingLine));
                                }
                               
                                Finishline = Util.ToGround(Finish1, 30f);
                                if (Finishline == Vector3.Zero) Finishline = Finish1;
                                if (DebugNotifications) UI.Notify("New distance:" + Finishline.DistanceTo(StartingLine));

                                Util.AddNotification("CHAR_LAMAR", "~b~Lamar", "Shorter track requested", "Due to " + Game.Player.Name + "'s request, the track will now be " + Math.Round(Finishline.DistanceTo(StartingLine)) + "m long.");

                                DistanceMultiplier = 100;
                            }
                        }
                        if (CountDown < Game.GameTime)
                        {

                            Vector3 pos = Util.GetEntityOffset(FlarePed, 10, 0) + (FlarePed.UpVector * 10);
                            FlarePed.Task.ShootAt(pos, 1000);
                        }
                        else if (CountDown - 3000 < Game.GameTime)
                        {
                            Vector3 pos = Util.GetEntityOffset(FlarePed, 10, 0) + (FlarePed.UpVector * 10);
                            if (!Util.IsSubttaskActive(FlarePed, Subtask.AIMING_GUN)) Function.Call(Hash.TASK_AIM_GUN_AT_COORD, FlarePed, pos.X, pos.Y, pos.Z, -1, false, true);
                        }
                        break;
                    }
                case DragRaceState.RaceInProgress:
                    {
                        if (Winners.Count == RacersParticipating().Count)
                        {
                            bool canfinish = true;
                            foreach (Racer winner in RacersParticipating())
                            {
                                if (winner.Car.Velocity.Length() > 3f && !winner.Driver.IsPlayer) canfinish = false;
                            }
                            if (canfinish)
                            {
                                Function.Call(Hash.UNLOCK_MINIMAP_ANGLE);
                                Function.Call(Hash.UNLOCK_MINIMAP_POSITION);
                                MeetState = DragRaceState.RaceFinished;
                                //if(CanWeUse(FinishBarrel)) Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, FinishBarrel);
                            }
                        }
                        break;
                    }
                case DragRaceState.RaceFinished:
                    {
                        //if (GetPlayerRacer() == null) Game.Player.Character.Position = StartingLine;


                       if (SpectCamera != null && World.RenderingCamera == SpectCamera)
                        {
                            SpectCamera.StopPointing();
                            SpectCamera.Detach();
                            SpectCamera.Destroy();
                            SpectCamera = null;
                            World.RenderingCamera = null;
                            Function.Call(Hash.CLEAR_FOCUS);

                        }
                        int BetPrize = 0;

                        foreach (Racer winner in Winners)
                        {

                            if (Winners[0].Penalized && winner != Winners[0]) Util.ChangePedMoney(winner.Driver, 200);

                            //Race Prize
                            if (winner == Winners[0])
                            {
                                if (winner.Penalized)
                                {
                                    Util.Notify("CHAR_LAMAR", "~b~Lamar", "INVALID RACE", winner.Name+" has clearly cheated and can't claim the Prize. Participants will have their fee returned to them.");
                                }
                                else
                                {
                                    winner.Wins++;
                                    Util.PlayAmbientSpeech(winner.Driver, Util.ChatTaunt);
                                    if (OfficialMeet)
                                    {
                                        Util.Notify("CHAR_LAMAR", "~b~Lamar", winner.Name + " won", "~b~" + winner.Name + "~w~ got the " + (Prize) + " prize! Now, for the bets...");
                                        if (winner.Driver.IsPlayer)
                                        Util.ChangePedMoney(winner.Driver, Prize);
                                    }
                                    else
                                    {
                                        Util.Notify("CHAR_LAMAR", "~b~Lamar", winner.Name + " won", "~b~" + winner.Name + "~w~ won the ~g~$" +ChallengePrize+ "~w~ bet! Now, for the other bets...");
                                        Util.ChangePedMoney(winner.Driver, ChallengePrize * (RacersPerRace - 1));
                                    }
                                }
                            }
                            else
                            {
                                winner.Loses++;
                                Util.PlayAmbientSpeech(winner.Driver, Util.ChatCurse);
                                if (!OfficialMeet) Util.ChangePedMoney(winner.Driver, -ChallengePrize);
                            }

                            //Bet Prize
                            foreach (Racer racer in winner.BetsOnHim)
                            {
                                BetPrize += MoneyBet;
                            }                          
                        }


                         //UI.Notify("~b~" + Winners[0].Name + "~w~ has won the ~g~" + (Prize * RacersParticipating().Count) + "~w~ Prize.~n~Money from Bets: ~g~" + BetPrize);

                        foreach (Racer winner in Winners)
                        {

                            if(winner == Winners[0])
                            {
                                if (Winners[0].BetsOnHim.Count > 0)
                                {
                                    BetPrize = (BetPrize / Winners[0].BetsOnHim.Count);
                                    foreach (Racer racer in Winners[0].BetsOnHim)
                                    {
                                        UI.Notify("~b~" + racer.Name + "~w~ wins ~g~" + BetPrize);
                                        if (racer.Driver.IsPlayer) Util.ChangePedMoney(racer.Driver, BetPrize);// Game.Player.Money += BetPrize; else racer.Driver.Money += BetPrize;
                                        SaveRacerstats(racer);
                                    }
                                }
                                else if (BetPrize > 0)
                                {
                                    Util.ChangePedMoney(Winners[0].Driver, BetPrize);
                                    UI.Notify("Nobody betted on the winner! ~b~" + Winners[0].Name + "~w~ wins ~g~" + BetPrize);
                                }
                            }
                            else
                            {
                                foreach (Racer racer in winner.BetsOnHim)
                                {
                                    //UI.Notify("~b~" + racer.Name + "~o~ loses ~r~" + MoneyBet);
                                    SaveRacerstats(racer);
                                }
                            }

                            if (winner.Reaction > 100) winner.Reaction -= 50;
                            if (winner.Shifting < 70) winner.Shifting++;

                            winner.TaskFinish();
                            winner.bet = 0;
                            winner.Offset = 0;
                            winner.Picked = RacerStatus.NotPicked;
                            winner.BetsOnHim.Clear();
                            SaveRacerstats(winner);
                        }


                        if (CanWeUse(FlarePed)) FlarePed.Task.ReloadWeapon();
                        Winners.Clear();
                        MeetState = DragRaceState.Idle;


                        if (GetPlayerRacer() != null)
                        {
                            //if(Game.Player.Money<Prize) Util.WarnPlayer(DragMeets.ScriptName, "Not enough money", "You don't have enough $ to pay for participating so you won't get picked.");
                        }
                        break;
                    }
            }
        }


        public void HandleRacerPicking(RacerStatus status, int goal)
        {
            
            int currentracersnum = RacersInState(status).Count;
            if (currentracersnum == 0)
            {
                PickingGameRef = Game.GameTime;
                PerformanceTreshold = 2;
                ReputationTreshold = 10;
            }
            else
            {
                if (Game.GameTime > PickingGameRef + 6000 && ReputationTreshold < 100) { ReputationTreshold = 100;}

                if (Game.GameTime > PickingGameRef + 10000 && PerformanceTreshold < 5) { PerformanceTreshold = 5;}
                if (Game.GameTime > PickingGameRef + 15000 && PerformanceTreshold < 10) PerformanceTreshold = 10;
                if (Game.GameTime > PickingGameRef + 30000 && PerformanceTreshold < 20) PerformanceTreshold = 20;
                if (Game.GameTime > PickingGameRef + 35000 && PerformanceTreshold < 100) PerformanceTreshold = 100;

            }

            if (currentracersnum < goal && RacersNotParticipating(true).Count > 0)
            {
                foreach (Racer forcedracer in Racers) if (forcedracer.Forced) { PickForRace(forcedracer, status); return; }

                if (RacersNotParticipating(true).Contains(GetPlayerRacer())) // && (Game.Player.Money >= Prize || !OfficialMeet)
                {
                    PickForRace(GetPlayerRacer(), status);
                    if(!OfficialMeet) UI.Notify("~b~" + GetPlayerRacer().Name + "~w~ awaits to be challenged.");
                    if (!OfficialMeet && GetPlayerRacer().ReputationBalance() > 70) UI.Notify("~b~" + GetPlayerRacer().Name + "~w~ dares to challenge higher tier Racers!");

                    //if(OfficialMeet) Game.Player.Money -= Prize;
                    return;
                }
       
                Racer racer = RacersNotParticipating(true)[Util.RandomInt(0, RacersNotParticipating(true).Count - 1)];

                if (currentracersnum == 0)
                {
                    PickForRace(racer, status);
                    if(!OfficialMeet) UI.Notify("~b~" + racer.Name + "~w~ looks for a challenge.");
                    if(racer.ReputationBalance()>70 && !OfficialMeet) UI.Notify("~b~" + racer.Name + "~w~ dares to challenge higher tier Racers!");
                    return;
                }
                else
                {
                    int RepPerfTrade = 0;
                    if (RacersInState(status)[0].ReputationBalance() > 70 && !OfficialMeet) RepPerfTrade += 20; //(int)((RacersInState(status)[0].PerformancePoints*50)/100);
                    for (int _ = 0; _ < 10; _++)
                    {
                        racer = RacersNotParticipating(true)[Util.RandomInt(0, RacersNotParticipating(true).Count - 1)];
                        if (racer.Picked == RacerStatus.NotPicked && ((Util.SimilarPerformance(RacersInState(status)[0], racer, PerformanceTreshold, RepPerfTrade, 0))))
                        {
                            if ((Util.SimilarRep(racer, RacersInState(status)[0], ReputationTreshold) || PerformanceTreshold > 4))
                            {
                                if (1 == 1)//racer.Driver.Money >= Prize || (!OfficialMeet)
                                {
                                    PickForRace(racer, status);
                                    // if (OfficialMeet) Util.ChangePedMoney(racer.Driver, -Prize);

                                    if (!OfficialMeet)
                                    {

                                            if (RacersInState(status).Contains(GetPlayerRacer()))
                                            {
                                                UI.Notify("~b~" + racer.Name + "~w~ challenges ~b~" + GetPlayerRacer().Name + "~w~!");
                                            }
                                            else
                                            {
                                                UI.Notify("~b~" + RacersInState(status)[1].Name + "~w~ challenges ~b~" + RacersInState(status)[0].Name + "~w~!");
                                            }
                                    }
                                    return;
                                }
                            }
                        }
                    }
                }
               
            }
        }


        public static bool TrackReady()
        {
            /*
            for (int _ = 4; _ < 10; _++)
            {
                Vector3 pos = Util.LerpByDistance(StartingLine, Finishline, _ * 0.1f);
                if (World.GetNearbyVehicles(pos, 10).Length > 0) return false;
            }


                    List<Entity> Interrupting = new List<Entity>();

            foreach (Entity ent in World.GetNearbyEntities(Util.GetEntityOffset(FlarePed, -10, 0), 5)) if (ent.IsPersistent) Interrupting.Add(ent);
            foreach (Entity ent in World.GetNearbyEntities(Util.GetEntityOffset(FlarePed, -20, 0), 5)) if (ent.IsPersistent) Interrupting.Add(ent);
            foreach (Entity ent in World.GetNearbyEntities(Util.GetEntityOffset(FlarePed, -30, 0), 5)) if (ent.IsPersistent) Interrupting.Add(ent);

            if (Interrupting.Count > 1) return false;
            */
            return true;
            }


        public bool RaceCanStart()
        {

            return true;
        }
        public bool PlayerNearRacers()
        {
            //RaycastResult raycast = World.RaycastCapsule(Util.GetEntityOffset(FlarePed, -1, 0),FlarePed.ForwardVector*-1, 5, IntersectOptions.Everything);
            //if (CanWeUse(raycast.HitEntity)) { DisplayHelpTextThisFrame("busy;" + raycast.HitEntity); return false; }
            if (GetPlayerRacer() != null && GetPlayerRacer().Picked == RacerStatus.NotPicked && Game.Player.Character.IsOnFoot)
            {
                foreach (Racer racer in RacersParticipating()) if (Game.Player.Character.IsInRangeOf(racer.Driver.Position, 5f)) return true;
            }

            return false;
        }
        public static Racer GetPlayerRacer()
        {
            foreach (Racer racer in Racers) if (racer.Driver.IsPlayer) return racer;
            return null;
        }
       public static bool FlarePedInPosition()
        {
            return FlarePed.IsInRangeOf(StartingLine, 5f) && FlarePed.IsStopped &&  Util.DiffBetweenAngles(FlarePed.Heading, DragHeading-180) < 4;
        }
        void HandleFlarePedAI()
        {
            if (FlarePed.RelationshipGroup != RacerRLGroup) FlarePed.RelationshipGroup = RacerRLGroup;
            if (FlarePed.IsInCombat) return;
            switch (MeetState)
            {
                case DragRaceState.NotStarted:
                    {

                        break;
                    }
                case DragRaceState.Idle:
                    {
                        if (RacersParticipating().Count > 0 && FlarePed.IsStopped)
                        {
                            if (!FlarePed.IsInRangeOf(StartingLine, 2f))
                            {
                                FlarePed.Task.RunTo(StartingLine, false);
                            }
                            else if (Util.DiffBetweenAngles(FlarePed.Heading, DragHeading - 180) > 1)
                            {
                                FlarePed.Task.AchieveHeading(DragHeading-180);
                            }
                            if (FlarePed.Weapons.Current.AmmoInClip == 0) FlarePed.Task.ReloadWeapon();
                        }
                        break;
                    }
                case DragRaceState.Setup:
                    {
                        if (FlarePed.IsStopped)
                        {
                            if (!FlarePed.IsInRangeOf(StartingLine, 2f))
                            {
                                FlarePed.Task.RunTo(StartingLine, false);
                            }
                            else if ((FlarePed.Heading - 180) != DragHeading)
                            {
                                FlarePed.Task.AchieveHeading(DragHeading - 180);
                            }
                        }
                        break;
                    }
                case DragRaceState.CountDown:
                    {

                        break;
                    }
                case DragRaceState.RaceInProgress:
                    {

                        break;
                    }
                case DragRaceState.RaceFinished:
                    {

                        break;
                    }
            }
        }



        public static List<Racer> RacersParticipating()
        {
             List<Racer> participating = new List<Racer>();
            foreach (Racer racer in Racers) if (racer.Picked == RacerStatus.Picked) participating.Add(racer);
            return participating;
        }
        public static List<Racer> RacersInState(RacerStatus state)
        {
            List<Racer> RacerState = new List<Racer>();

            foreach (Racer racer in Racers) if (racer.Picked == state) RacerState.Add(racer);
            return RacerState;
        }
        public static List<Racer> RacersNotParticipating(bool inWaitingArea)
        {
            List<Racer> participating = new List<Racer>();
            foreach (Racer racer in Racers) if (racer.Picked == RacerStatus.NotPicked && (racer.Car.IsInRangeOf(WaitingArea, WaitingAreRange) || !inWaitingArea)) participating.Add(racer);
            return participating;
        }
        void OnKeyDown(object sender, KeyEventArgs e)
        {

        }
        void OnKeyUp(object sender, KeyEventArgs e)
        {


        }

        void ClearMeet(bool DeleteEverything)
        {
            foreach (Spectator spect in Spectators) spect.Clear(DeleteEverything);
            Spectators.Clear();
            foreach (Racer racer in Racers) racer.Clear(DeleteEverything);
            Racers.Clear();
            MeetState = DragRaceState.NotStarted;

            if (Util.CanWeUse(FlarePed)) FlarePed.MarkAsNoLongerNeeded();
            FlarePed = null;
            if (Util.CanWeUse(FinishBarrel)) FinishBarrel.MarkAsNoLongerNeeded();
            FinishBarrel = null;
            KindsAllowed.Clear();

            WarnedOfPerformance = false;
            StreetIntroduction = false;
            OfficialIntroduction = false;

            foreach (Prop prop in MeetProps) prop.Delete();
            MeetProps.Clear();
            OfficialMeet = false;
            ToWaitingArea.Clear();
            DespanwnRequested = false;
            CrowdAreas.Clear();
            DynamicCrowdAreas.Clear();
            TougeMode = false;
            StartingLine = Vector3.Zero;
            Finishline = Vector3.Zero;
            
            RacersPerRace = 2;
            WaitingArea = Vector3.Zero;
            WaitingAreRange = 0f;
            DistanceMultiplier = 100;
            PathToStartingLine = Vector3.Zero;
            PathToWaitingArea = Vector3.Zero;
        }
        protected override void Dispose(bool dispose)
        {
            /*
            if (SpectCamera.Exists())
            {
                SpectCamera.Destroy();
                SpectCamera = null;
            }
            */
            World.RenderingCamera = null;


            Function.Call(Hash.CLEAR_FOCUS);
            Function.Call(Hash.UNLOCK_MINIMAP_ANGLE);
            Function.Call(Hash.UNLOCK_MINIMAP_POSITION);

            foreach (Blip blip in MeetBlips) blip.Remove();

            ClearMeet(true);

            text.Dispose();


            if (CanWeUse(FlarePed)) FlarePed.Delete();

            WarnPlayer(ScriptName + " " + ScriptVer, "SCRIPT RESET", ScriptName +" has been reset successfully.");

            base.Dispose(dispose);
        }




        /// TOOLS ///
        void LoadSettings(string file)
        {
            if (File.Exists(file))
            {

                ScriptSettings config = ScriptSettings.Load(file);


                PersistentRacers = config.GetValue<bool>("GENERAL_SETTINGS", "PersistentRacers", false);


                MT_Support = config.GetValue<bool>("COMPATIBLITY", "MT_Support", false);
                PreventPreDesignedTuning = config.GetValue<bool>("COMPATIBLITY", "PreventPreDesignedTuning", false);

               
                SpeedMultiplier = config.GetValue<float>("MEET_TWEAKS", "SpeedMultiplier", 100f);
                SpeedMultiplier = SpeedMultiplier * 0.01f;

                SelfRadio = config.GetValue<bool>("MEET_TWEAKS", "AlwaysSelfRadio", false);

                DistanceMultiplier = config.GetValue<int>("MEET_TWEAKS", "DistancePercent", 100);
                ManualMeetSpawn = config.GetValue<bool>("MEET_TWEAKS", "ManualMeetSpawn", false);
                ForceRandomTuning = config.GetValue<bool>("MEET_TWEAKS", "ForceRandomTuning", false);
                AllowNitro = config.GetValue<bool>("MEET_TWEAKS", "AllowNitro", false);
                Fee = config.GetValue<int>("MEET_TWEAKS", "Fee", 800);
                Prize = config.GetValue<int>("MEET_TWEAKS", "Prize", 5000);

                MoneyBet = config.GetValue<int>("MEET_TWEAKS", "Bet", 200);
                CinematicCam = config.GetValue<bool>("MEET_TWEAKS", "CinematicCam", true);

                FairChallenges = config.GetValue<bool>("SIMULATION", "FairChallenges", false);
                ReactionTimes = config.GetValue<bool>("SIMULATION", "ReactionTimes", false);
                ShowReactionTimes = config.GetValue<bool>("SIMULATION", "ShowReactionTimes", false);
                SimulateManualTransmission = config.GetValue<bool>("SIMULATION", "SimulateManualTransmission", false);
                ShowGoodBadShifting = config.GetValue<bool>("SIMULATION", "ShowGoodBadShifting", false);
                SimulateTirePerformance = config.GetValue<bool>("SIMULATION", "SimulateTirePerformance", false);
                RealisticWheelies = config.GetValue<bool>("SIMULATION", "RealisticWheelies", false);
                ShowGrip = config.GetValue<bool>("SIMULATION", "ShowGrip", false);

                VehicleListUpdater = config.GetValue<bool>("VEHICLE_LIST_UPDATER", "VehicleListUpdater", false);

                InteractKey = config.GetValue<GTA.Control>("KEYS", "InteractKey", GTA.Control.Context);
                ShiftingKey = config.GetValue<GTA.Control>("KEYS", "ShiftingKey", GTA.Control.Context);

                WarnPlayer(ScriptName + " " + ScriptVer, "Settings loaded", "~g~All settings have been loaded.");

                //WarnPlayer(ScriptName + " " + ScriptVer, "Settings loaded", "SpeedMultiplier: " + SpeedMultiplier + "~n~DistanceMultiplier: " + DistanceMultiplier + "~n~ShowReactionTimes: " + ShowReactionTimes + "~n~ReactionTimes: " + ReactionTimes);

            }
            else
            {
                WarnPlayer(ScriptName + " " + ScriptVer, "Settings error", "Settings file in "+configpath+" doesn't exist.");

            }
        }

        public static void WarnPlayer(string script_name, string title, string message)
        {
            Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "STRING");
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, message);
            Function.Call(Hash._SET_NOTIFICATION_MESSAGE, "CHAR_SOCIAL_CLUB", "CHAR_SOCIAL_CLUB", true, 0, title, "~b~" + script_name);
        }

        bool CanWeUse(Entity entity)
        {
            return entity != null && entity.Exists();
        }

        void DisplayHelpTextThisFrame(string text)
        {
            Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, "STRING");
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, text);
            Function.Call(Hash._DISPLAY_HELP_TEXT_FROM_STRING_LABEL, 0, false, true, -1);
        }



        List<string> GetCarsAllowed()
        {
            List<string> allowed = new List<string> { "Street", "Super", "Gang" };
            return allowed;


            /*
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
                        */

        }

        

        void SpawnDriversFromFile(List<String> NotPermitted,int maxtoadd)
        {
            File.AppendAllText(@"scripts\\DragMeets\debug.txt", "\n Spawning cars");

            XmlDocument document = new XmlDocument();
            document.Load(RacersFile);
            int pat = 0;
            while (document == null && pat < 500)
            {
                document.Load(RacersFile);
                Script.Wait(0);
            }
            if (pat >= 500)
            {

                 WarnPlayer(ScriptName + " " + ScriptVer, "Racers.xml LOAD ERROR","~r~Couldn't load the file.");
                ClearMeet(true);
                return;
            }
            XmlElement docroot = document.DocumentElement;


            XmlNodeList nodelist = docroot.SelectNodes("//Racers/*");
            if(nodelist.Count==0) nodelist = docroot.SelectNodes("//Drivers/*");

            if(nodelist.Count==0)
            {
                WarnPlayer(ScriptName + " " + ScriptVer, "Racers.xml LOAD ERROR", "~r~No vehicles found in Racers.xml.");


                return;
            }
            XmlNodeList nodelist2 = nodelist;
            List<String> drivers = new List<String>();
            List<String> pickeddrivers = new List<String>();
            int limit = maxtoadd;

            foreach (XmlElement driver in nodelist)
            {

                if (driver.GetAttribute("Name") != null && !NotPermitted.Contains(driver.GetAttribute("Name")))
                {

                    if (driver.SelectSingleNode("RacesAllowed") == null)
                    {
                        drivers.Add(driver.GetAttribute("Name"));
                      if(DebugNotifications)  UI.Notify("Added generic driver: " + driver.GetAttribute("Name"));
                    }
                    else
                    {
                        //foreach (string kind in KindsAllowed) UI.Notify("Allowed:" +kind);
                        foreach (XmlElement component in driver.SelectNodes("RacesAllowed/*"))
                        {
                            //UI.Notify(driver.GetAttribute("Name")+": "+component.InnerText);
                            if (KindsAllowed.Contains(component.InnerText))
                            {
                                drivers.Add(driver.GetAttribute("Name"));
                                break;
                            }
                        }
                    }

                }
                else if(DebugNotifications) UI.Notify("~o~" + driver.GetAttribute("Name") + " is already in the Meet");


            }
            if (limit > drivers.Count)
            {
                limit = drivers.Count;
                if (DebugNotifications) UI.Notify("Not enough cars for this Meet, lowering limit to  "+ limit);
            }

            var copyDrivers = new List<string>(drivers);
            for (int _ = 0; _ < limit; _++)
            {
                int i = Util.RandomInt(0, copyDrivers.Count);
                pickeddrivers.Add(copyDrivers[i]);
                copyDrivers.RemoveAt(i);
            }

            bool FirstTime = Racers.Count == 0;
            float num = 1;
            if (limit != maxtoadd) num = 200;
            foreach (XmlElement driver in nodelist)
            {
                //Script.Wait(50);
                if (pickeddrivers.Contains(driver.GetAttribute("Name")))
                {
                    
                   File.AppendAllText(@"scripts\\DragMeets\debug.txt","\n"+driver.GetAttribute("Name"));
                    Vector3 position = WaitingArea.Around(4);

                    num += 3f;
                    if(Game.Player.Character.IsInRangeOf(WaitingArea, 200))
                    {
                       if(ManualMeetSpawn && FirstTime)
                        {
                            if (num > WaitingAreRange)
                            {
                                num += 50f;
                                position = World.GetNextPositionOnStreet(DragMeets.WaitingArea.Around(num));
                            }
                            else
                            {
                                position = DragMeets.WaitingArea.Around(num);
                            }
                        }
                       else
                        {
                            if (num < 500) num = 500;
                            num += 50f;
                            position = World.GetNextPositionOnStreet(DragMeets.WaitingArea.Around(num));
                        }
                    }
                    else
                    {
                        position = DragMeets.WaitingArea.Around(num);

                    }


                    /*
if (ManualMeetSpawn)
{
    position = DragMeets.WaitingArea.Around(num);
}
else if (num > WaitingAreRange || Game.Player.Character.IsInRangeOf(WaitingArea, 200))
{
    if (num < 500) num = 500;
    num += 50f;
    position = World.GetNextPositionOnStreet(DragMeets.WaitingArea.Around(num));
}


if (FirstTime)
{
    if (num > WaitingAreRange || Game.Player.Character.IsInRangeOf(WaitingArea, 200))
    {
        if (num < 500) num = 500;
        num += 50f;
        position = World.GetNextPositionOnStreet(DragMeets.WaitingArea.Around(num));
    }
    else
    {
        position = DragMeets.WaitingArea.Around(num);
    }
}
else 
{
    num += 50f;
    if (num < 500) num = 500;
    position = World.GetNextPositionOnStreet(DragMeets.WaitingArea.Around(num));
}
*/
                    bool IsRandomVehicle = false;
                    Vehicle veh = null;
                    int patience = 0;
                    int maxpatience = 1000;
                    while (!CanWeUse(veh) && patience< maxpatience)
                    {
                        patience++;
                       if(patience> maxpatience/2) DisplayHelpTextThisFrame("Trying to spawn " + driver.SelectSingleNode("Vehicle/Model").InnerText + " for the Drag Meet...(" + patience + "/"+ maxpatience + ")");


                        int n;
                        if (int.TryParse(driver.SelectSingleNode("Vehicle/Model").InnerText, out n))
                        {
                            veh = World.CreateVehicle(n, position);

                        }
                        else
                        {
                            veh = World.CreateVehicle(driver.SelectSingleNode("Vehicle/Model").InnerText, position);
                        }
                    }

                    bool Error = false;
                    if (patience >= maxpatience)
                    {
                        if (DebugNotifications) WarnPlayer(ScriptName + " " + ScriptVer, "VEHICLE LOAD ERROR", "Error trying to load " + driver.SelectSingleNode("Vehicle/Model").InnerText + ". Skipping this car.");
                        File.AppendAllText(@"scripts\\DragMeets\debug.txt", " - " + " ERROR vehicle model not found in game files, SKIPPED");

                        Error = true;
                    }
                    if (Error) continue;
                    File.AppendAllText(@"scripts\\DragMeets\debug.txt", " - " + " created");

                    Ped ped=null;

                    if(driver.SelectSingleNode("PedModel") != null)
                    {
                        if (driver.SelectSingleNode("PedModel").InnerText == "random")
                        {
                            ped = World.CreatePed(RacerModel[Util.RandomInt(0,RacerModel.Count)], veh.Position.Around(2f));
                            //ped = Function.Call<Ped>(Hash.CREATE_RANDOM_PED_AS_DRIVER, veh, true);
                        }
                        else
                        {
                            int nped;
                            if(int.TryParse(driver.SelectSingleNode("PedModel").InnerText, out nped))
                            {
                                ped = World.CreatePed(nped, veh.Position.Around(2f));
                            }
                            else
                            {
                                ped = World.CreatePed(driver.SelectSingleNode("PedModel").InnerText, veh.Position.Around(2f));
                            }
                        }
                    }
                    if(!CanWeUse(ped)) ped = Function.Call<Ped>(Hash.CREATE_RANDOM_PED_AS_DRIVER, veh, true);

                    while (veh == null || ped == null)
                    {
                        Script.Wait(0);
                    }
                    File.AppendAllText(@"scripts\\DragMeets\debug.txt", " - " + " ped created");

                    
                    if (!ForceRandomTuning && driver.SelectSingleNode("Vehicle//PrimaryColor") != null && !PreventPreDesignedTuning)
                    {
                        
                        Function.Call(Hash.SET_VEHICLE_MOD_KIT, veh, 0);
                        if (driver.SelectSingleNode("Vehicle/WheelType") != null) veh.WheelType = (VehicleWheelType)int.Parse(driver.SelectSingleNode("Vehicle/WheelType").InnerText, CultureInfo.InvariantCulture);
                        if (driver.SelectSingleNode("Vehicle/PrimaryColor") != null) veh.PrimaryColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/PrimaryColor").InnerText, CultureInfo.InvariantCulture);
                        if (driver.SelectSingleNode("Vehicle/SecondaryColor") != null) veh.SecondaryColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/SecondaryColor").InnerText, CultureInfo.InvariantCulture);
                        if (driver.SelectSingleNode("Vehicle/PearlescentColor") != null) veh.PearlescentColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/PearlescentColor").InnerText, CultureInfo.InvariantCulture);
                        if (driver.SelectSingleNode("Vehicle/TrimColor") != null) Function.Call((Hash)0xF40DD601A65F7F19, veh, int.Parse(driver.SelectSingleNode("Vehicle/TrimColor").InnerText, CultureInfo.InvariantCulture));
                        if (driver.SelectSingleNode("Vehicle/DashColor") != null) Function.Call((Hash)0x6089CDF6A57F326C, veh, int.Parse(driver.SelectSingleNode("Vehicle/DashColor").InnerText, CultureInfo.InvariantCulture));
                        if (driver.SelectSingleNode("Vehicle/RimColor") != null) veh.RimColor = (VehicleColor)int.Parse(driver.SelectSingleNode("Vehicle/RimColor").InnerText, CultureInfo.InvariantCulture);
                        if (driver.SelectSingleNode("Vehicle/LicensePlateText") != null) veh.NumberPlate = driver.SelectSingleNode("Vehicle/LicensePlateText").InnerText;
                        if (driver.SelectSingleNode("Vehicle/LicensePlate") != null) Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, veh, int.Parse(driver.SelectSingleNode("Vehicle/LicensePlate").InnerText, CultureInfo.InvariantCulture));
                        if (driver.SelectSingleNode("Vehicle/WindowsTint") != null) veh.WindowTint = (VehicleWindowTint)int.Parse(driver.SelectSingleNode("Vehicle/WindowsTint").InnerText, CultureInfo.InvariantCulture);

                        if (driver.SelectSingleNode("Vehicle//SmokeColor") != null)
                        {
                            Color color = Color.FromArgb(255, int.Parse(driver.SelectSingleNode("Vehicle//SmokeColor/Color/R").InnerText), int.Parse(driver.SelectSingleNode("Vehicle//SmokeColor/Color/G").InnerText), int.Parse(driver.SelectSingleNode("Vehicle//SmokeColor/Color/B").InnerText));
                            veh.TireSmokeColor = color;
                        }

                        if (driver.SelectSingleNode("Vehicle//NeonColor") != null)
                        {

                            if (driver.SelectSingleNode("Vehicle//NeonColor/Color") != null)
                            {
                                Color color = Color.FromArgb(255, int.Parse(driver.SelectSingleNode("Vehicle//NeonColor/Color/R").InnerText), int.Parse(driver.SelectSingleNode("Vehicle//NeonColor/Color/G").InnerText), int.Parse(driver.SelectSingleNode("//Vehicle/NeonColor/Color/B").InnerText));
                                veh.NeonLightsColor = color;
                            }
                            else
                            {
                                Color color = Color.FromArgb(255, int.Parse(driver.SelectSingleNode("Vehicle//NeonColor/R").InnerText), int.Parse(driver.SelectSingleNode("Vehicle//NeonColor/G").InnerText), int.Parse(driver.SelectSingleNode("Vehicle//NeonColor/B").InnerText));
                                veh.NeonLightsColor = color;
                            }

                        }

                        if (driver.SelectSingleNode("Vehicle/Neons/Back") != null) veh.SetNeonLightsOn(VehicleNeonLight.Back, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Back").InnerText));
                        if (driver.SelectSingleNode("Vehicle/Neons/Front") != null) veh.SetNeonLightsOn(VehicleNeonLight.Front, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Front").InnerText));
                        if (driver.SelectSingleNode("Vehicle/Neons/Left") != null) veh.SetNeonLightsOn(VehicleNeonLight.Left, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Left").InnerText));
                        if (driver.SelectSingleNode("Vehicle/Neons/Right") != null) veh.SetNeonLightsOn(VehicleNeonLight.Right, bool.Parse(driver.SelectSingleNode("Vehicle/Neons/Right").InnerText));

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
                        File.AppendAllText(@"scripts\\DragMeets\debug.txt", " - " + "tuned");
                    }
                    else
                    {
                        IsRandomVehicle = true;
                        Script.Wait(40);
                        Util.RandomTuning(veh, false,false);
                        File.AppendAllText(@"scripts\\DragMeets\debug.txt", " - " + "tuned (random)");
                    }

                    int turbo = 1;
                    if (driver.SelectSingleNode("CustomTurbo") != null)
                    {
                        turbo= int.Parse(driver.SelectSingleNode("CustomTurbo").InnerText, CultureInfo.InvariantCulture);
                    }

                    //Shifting
                    int shiftskill = 50;
                    if (driver.GetAttribute("Shifting") != "" && driver.GetAttribute("Shifting") != "50")
                    {
                        shiftskill = int.Parse(driver.GetAttribute("Shifting"));
                    }
                    else
                    {
                        shiftskill = Util.RandomInt(15, 80);
                    }

                    //Reaction
                    int reactSkill = 250;
                    if (driver.GetAttribute("Reaction") != "" && driver.GetAttribute("Reaction") != "250")
                    {
                        reactSkill = int.Parse(driver.GetAttribute("Reaction"));
                    }
                    else
                    {
                        reactSkill = Util.RandomInt(150, 850);

                    }

                    File.AppendAllText(@"scripts\\DragMeets\debug.txt", " - " + "skilled");

                    //Name
                    string name = veh.FriendlyName;
                    if (driver.GetAttribute("Name") != "") name = driver.GetAttribute("Name");
                    if (driver.GetAttribute("DriverName") != "") name = driver.GetAttribute("Name");

                   // if (turbo > 1) name = "Supercharged "+name;
                    veh.Heading = Util.RandomInt(-180, 180);


                    int Money=0;
                    if (driver.HasAttribute("Money")) Money = int.Parse(driver.GetAttribute("Money"));

                    int Won=0;
                    if (driver.HasAttribute("RacesWon")) Won = int.Parse(driver.GetAttribute("RacesWon"));
                    int Lost=0;
                    if (driver.HasAttribute("RacesLost")) Lost = int.Parse(driver.GetAttribute("RacesLost"));

                    Racers.Add(new Racer(name, ped, veh, turbo, reactSkill, shiftskill, !FirstTime, Won, Lost, Money));
                    File.AppendAllText(@"scripts\\DragMeets\debug.txt", " - " + "finished");
                }

            }

        }

    }
}
