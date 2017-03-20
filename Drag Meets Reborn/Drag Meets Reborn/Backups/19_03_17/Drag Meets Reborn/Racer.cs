// Decompiled with JetBrains decompiler
// Type: Drag_Meets_Reborn.Racer
// Assembly: DragMeets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5A819CBA-2163-472B-873C-95AA7AB864F5
// Assembly location: C:\DragMeets.dll

using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Drag_Meets_Reborn
{
    public class Racer
    {
        public int max_willing_to_bet = 60;
        public int min_willing_to_bet = 30;
        public int DrivingStyle = 4456509;
        public TimeSpan TrackTime = new TimeSpan(0, 0, 0);
        public Vector3 Destination = DragMeets.WaitingArea;
        public List<Racer> BetsOnHim = new List<Racer>();
        public float WheelTemp = 30f;
        public int GearBoostTime = Game.GameTime;
        public int GearGrindTime = Game.GameTime;
        public Vector3 StuckReference = Vector3.Zero;
        public int Reaction = 50;
        public int Shifting = 250;
        public float EngineBonusMultiplier = 1f;
        public int CustomTurbo = 1;
        private Scaleform racertext = new Scaleform("mp_car_stats_02");
        public Ped Driver;
        public Vehicle Car;
        public bool RemoveMe;
        public int StuckScore;
        public int StuckTimeRef;
        public RacerStatus Picked;
        public RacerExpectations Expectations;
        public int ShiftBoostRef;
        public int RacesDone;
        public bool ReactTimeNotified;
        public string Name;
        public Prop Parachute;
        public int StartTime;
        public bool Penalized;
        public bool HasBet;
        public bool Forced;
        public int Offset;
        public int Wheelietime;
        public int bet;
        public float PerformancePoints;
        public string Reason;
        public int NitroTime;
        public bool UsedNitro;
        public int GearReference;
        public float GripBonus;
        public float PerformanceBonus;
        public int Wins;
        public int Loses;
        public bool GotToMidPoint;
        public bool RacingToFinish;

        public Racer(string name, Ped ped, Vehicle veh, int customTurbo, int reaction, int shifting, bool notify, int won, int lost, int money)
        {
            Driver = ped;
            Car = veh;
            Reaction = reaction;
            Shifting = shifting;
            Name = name;
            if (customTurbo > 1)
                CustomTurbo = customTurbo;
            if (Name == Car.FriendlyName)
                Name = Car.FriendlyName;
            if (!Driver.IsPersistent)
                Driver.IsPersistent = true;
            if (!Car.IsPersistent)
                Car.IsPersistent = true;
            Destination = DragMeets.WaitingArea;
            PerformancePoints = Util.CalculatePerformancePoints(Car);
            if (!Driver.IsPlayer)
            {
                Wins = won;
                Loses = lost;
                if (money > 0 && !Driver.IsPlayer)
                    Driver.Money = money;
                if (Driver.Money < DragMeets.Fee)
                {
                    if (Car.ClassType == VehicleClass.Super)
                        Driver.Money += Util.RandomInt(3000, 10000);
                    else if (Car.ClassType == VehicleClass.Sports)
                        Driver.Money += Util.RandomInt(500, 5000);
                    else if (Car.ClassType == VehicleClass.SportsClassics)
                        Driver.Money += Util.RandomInt(500, 5000);
                    else
                        Driver.Money += Util.RandomInt(200, 1000);
                }
                Driver.CanBeDraggedOutOfVehicle = false;
                Driver.RelationshipGroup = DragMeets.RacerRLGroup;
                if (Car.IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange + 30f))
                    GotToMidPoint = true;
                if (!Car.CurrentBlip.Exists())
                    Car.AddBlip();
                Car.CurrentBlip.Color = BlipColor.Blue;
                Car.CurrentBlip.Scale = 0.6f;
                Car.CurrentBlip.IsShortRange = true;
                if (!Driver.CurrentBlip.Exists())
                    Driver.AddBlip();
                Driver.CurrentBlip.Color = BlipColor.White;
                Driver.CurrentBlip.Scale = 0.3f;
                Driver.CurrentBlip.IsShortRange = true;
                Driver.AlwaysKeepTask = true;
                Function.Call(Hash._0x0DC7CABAB1E9B67E, Driver, 46, true);
                Function.Call(Hash._0x0DC7CABAB1E9B67E, Car, true);
                Function.Call(Hash._0xB195FFA8042FC5C3, Driver, 100);
                Function.Call(Hash._0x9F7794730795E019, Driver, 46, true);
                if ((double)Car.Position.DistanceTo(DragMeets.Finishline) < (double)Car.Position.DistanceTo(DragMeets.StartingLine))
                    TaskToWaitingArea();
            }
            else
                DragMeets.LoadPlayerStats(this);
            if (!notify)
                return;
            UI.Notify("~b~" + Name + " ~w~has joined the meet.");
        }

        public int GetRacerMoney()
        {
            if (Driver.IsPlayer)
                return Game.Player.Money;
            return Driver.Money;
        }

        public bool Ok()
        {
            if (!Util.CanWeUse((Entity)Car))
            {
                if (Reason == "")
                    Reason = "car dissapeared";
                return false;
            }
            if (!Util.CanWeUse((Entity)Driver))
            {
                if (Reason == "")
                    Reason = "driver dissapeared";
                return false;
            }
            if (!Car.IsAlive)
            {
                if (Reason == "")
                    Reason = "car dead";
                return false;
            }
            if (Driver.IsAlive)
                return true;
            if (Reason == "")
                Reason = "driver dead";
            return false;
        }

        public bool InPosition()
        {
            if (Car.IsInRangeOf(Util.ToGround(Util.GetEntityOffset((Entity)DragMeets.FlarePed, (float)DragMeets.ForwardOffset, (float)Offset), 5f), 4f))
                return (double)Car.Speed < 0.300000011920929;
            return false;
        }

        public void HandlePlayerBet()
        {
            if (!Game.Player.Character.IsOnFoot || Driver.IsPlayer || !Game.Player.Character.IsInRangeOf(Driver.Position, 4f))
                return;
            if (Game.Player.Money >= DragMeets.MoneyBet)
            {
                if (DragMeets.GetPlayerRacer() != null)
                {
                    if (!BetsOnHim.Contains(DragMeets.GetPlayerRacer()) && !DragMeets.GetPlayerRacer().HasBet)
                    {
                        if (DragMeets.GetPlayerRacer().Picked == RacerStatus.Picked)
                            Util.DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(DragMeets.InteractKey) + " to refuse to race.");
                        else
                            Util.DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(DragMeets.InteractKey) + " to bet ~g~$" + (object)DragMeets.MoneyBet + "~w~ on this guy.");
                        if (Game.IsControlJustPressed(2, DragMeets.InteractKey))
                        {
                            if (DragMeets.GetPlayerRacer().Picked != RacerStatus.Picked)
                            {
                                BetsOnHim.Add(DragMeets.GetPlayerRacer());
                                Util.AddQueuedHelpText("Bet placed. You can hop in the " + Car.FriendlyName + ".");
                                UI.Notify("~b~" + DragMeets.GetPlayerRacer().Name + "~w~ bets on ~g~" + Name);
                                Util.PlayAmbientSpeech(Driver, Util.ChatThanks);
                                DragMeets.GetPlayerRacer().HasBet = true;
                                Util.ChangePedMoney(DragMeets.GetPlayerRacer().Driver, -DragMeets.MoneyBet);
                            }
                            else
                                DragMeets.GetPlayerRacer().Picked = RacerStatus.NotPicked;
                        }
                    }
                }
                else
                    Util.DisplayHelpTextThisFrame("You need to register in the Meet to participate in bettings.");
            }
            else
                Util.DisplayHelpTextThisFrame("You don't have money to bet.");
            if (!Game.IsControlJustReleased(2, Control.VehicleExit) || !BetsOnHim.Contains(DragMeets.GetPlayerRacer()))
                return;
            Game.Player.Character.Task.ClearAll();
            Game.Player.Character.Task.EnterVehicle(Car, VehicleSeat.Passenger, 6000, 1f);
        }

        public float ReputationBalance()
        {
            int num1 = 10;
            float num2 = 50f + (float)(Wins * num1) - (float)(Loses * num1);
            if ((double)num2 < 0.0)
                num2 = 0.0f;
            if ((double)num2 > 100.0)
                num2 = 100f;
            return num2;
        }

        public void OnTick()
        {
            if (Util.CanWeUse((Entity)Parachute))
            {
                if ((double)Util.ForwardSpeed(Car) > 4.0)
                    Car.ApplyForceRelative(new Vector3(0.0f, -0.3f, 0.0f));
                else
                    Parachute.Delete();
            }
            if (DragMeets.FinishBarrel.IsInRangeOf(Game.Player.Character.Position, 4f))
            {
                Util.DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(DragMeets.InteractKey) + " fix the barrel's position.");
                if (Game.IsControlJustPressed(2, DragMeets.InteractKey))
                    Function.Call(Hash._0x58A850EAEE20FAA3, new InputArgument[1]
                    {
             DragMeets.FinishBarrel
                    });
            }
            if (DragMeets.GetPlayerRacer() != null && Picked == RacerStatus.NotPicked && (!Forced && !Driver.IsPlayer) && Util.EntitySeenByPlayer() == (Entity)Car)
            {
                Util.DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(DragMeets.InteractKey) + " to challenge " + Name + " to race you in the next turn.");
                if (Game.IsControlJustPressed(2, DragMeets.InteractKey))
                {
                    bool flag = true;
                    string text = "";
                    if (DragMeets.FairChallenges)
                    {
                        if (!Util.SimilarPerformance(this, DragMeets.GetPlayerRacer(), 10f))
                        {
                            if ((double)PerformancePoints > (double)DragMeets.GetPlayerRacer().PerformancePoints)
                            {
                                if ((double)ReputationBalance() > (double)DragMeets.GetPlayerRacer().ReputationBalance())
                                {
                                    text = "~b~" + Name + "~w~ laughs at your car's performance.";
                                    flag = false;
                                }
                                else
                                {
                                    text = "~b~" + Name + "~w~ feels confident in its car's performance.";
                                    flag = true;
                                }
                            }
                            else if ((double)ReputationBalance() < 60.0)
                            {
                                text = "~b~" + Name + "~w~ doesn't feel really confident.";
                                flag = false;
                            }
                            else
                            {
                                text = "~b~" + Name + "~w~ feels confident in its capabilities.";
                                flag = true;
                            }
                        }
                        else if ((double)System.Math.Abs(ReputationBalance() - DragMeets.GetPlayerRacer().ReputationBalance()) < 10.0)
                        {
                            text = "~b~" + Name + "~w~ thinks this will be a close race.";
                        }
                        else
                        {
                            if ((double)ReputationBalance() > (double)DragMeets.GetPlayerRacer().ReputationBalance() + 20.0)
                                text = "~b~" + Name + "~w~ expects you to lose.";
                            if ((double)DragMeets.GetPlayerRacer().ReputationBalance() > (double)ReputationBalance() + 20.0)
                                text = "~b~" + Name + "~w~ expects to lose.";
                        }
                    }
                    if (text.Length > 1)
                        Util.AddQueuedHelpText(text);
                    if (flag)
                    {
                        Util.AddQueuedHelpText("~b~" + Name + "~g~ has accepted the challenge.");
                        Forced = true;
                        UI.Notify("~b~" + DragMeets.GetPlayerRacer().Name + "~w~ challenges ~b~" + Name + "~w~!");
                        DragMeets.GetPlayerRacer().Forced = true;
                    }
                    else
                        Util.AddQueuedHelpText("~b~" + Name + "~o~ has refused the challenge.");
                }
            }
            if (Driver.IsPlayer && !Driver.IsInRangeOf(Destination, 4f))
                World.DrawMarker(MarkerType.UpsideDownCone, Destination + new Vector3(0.0f, 0.0f, 1f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1f, 1f, 1f), Color.Yellow);
            if (Game.Player.Character.IsOnFoot && (Entity)Car == Util.EntitySeenByPlayer())
            {
                if (!racertext.IsLoaded)
                    racertext = new Scaleform("mp_car_stats_02");
                if (Function.Call<bool>(Hash._0x7C2AC9CA66575FBF, new InputArgument[1]
                {
           Game.Player.Character
                }))
                {
                    float num1 = (float)System.Math.Round((double)Function.Call<float>(Hash._0xF417C2502FFFED43, new InputArgument[1]
                    {
             Car.Model.Hash
                    }) * 2.20000004768372);
                    float num2 = (float)System.Math.Round((double)Function.Call<float>(Hash._0xAD7E85FC227197C4, new InputArgument[1]
                    {
             Car
                    }), 2);
                    float num3 = (float)System.Math.Round((double)Function.Call<float>(Hash._0x8C044C5C84505B6A, new InputArgument[1]
                    {
             Car.Model
                    }), 2);
                    string str = num1.ToString();
                    if ((double)num1 > 100.0)
                        num1 = 99.9f;
                    if ((double)num3 > 100.0)
                        num3 = 99.9f;
                    racertext.CallFunction("SET_VEHICLE_INFOR_AND_STATS", (object)(Name + " ($" + (object)Driver.Money + ") - " + Car.FriendlyName), (object)("Wins/Loses:  " + (object)Wins + "/" + (object)Loses), (object)"MPCarHUD", (object)"Dinka", (object)("Wheel Temp: " + (object)WheelTemp + "º"), (object)("Speed: " + str + " mph"), (object)"Acceleration", (object)"Braking", (object)WheelTemp, (object)num1, (object)(float)((double)num3 * 100.0), (object)(float)((double)num2 * 100.0));
                    racertext.Render3D(Car.Position + new Vector3(0.0f, 0.0f, Car.Model.GetDimensions().Z + 1f), GameplayCamera.Rotation, new Vector3(6f, 3f, 1f));
                }
                else
                {
                    float num1 = (float)System.Math.Round((double)Shifting - (double)Reaction * 0.00999999977648258);
                    if ((double)num1 > 100.0)
                        num1 = 100f;
                    else if ((double)num1 < 0.0)
                        num1 = 0.0f;
                    float num2 = (float)((Car.GetMod(VehicleMod.Engine) + Car.GetMod(VehicleMod.Transmission)) * 10);
                    if ((double)num2 < 0.0)
                        num2 = 0.0f;
                    racertext.CallFunction("SET_VEHICLE_INFOR_AND_STATS", (object)Name, (object)Car.FriendlyName, (object)"MPCarHUD", (object)"Dinka", (object)"Reputation", (object)"Skill", (object)"Performance ", (object)"Upgrades", (object)ReputationBalance(), (object)num1, (object)PerformancePoints, (object)num2);
                    racertext.Render3D(Car.Position + new Vector3(0.0f, 0.0f, Car.Model.GetDimensions().Z + 1f), GameplayCamera.Rotation, new Vector3(6f, 3f, 1f));
                }
            }
            switch (DragMeets.MeetState)
            {
                case DragRaceState.Idle:
                    if ((double)Car.EngineHealth >= 201.0)
                        break;
                    Reason = "~r~ engine broken";
                    RemoveMe = true;
                    break;
                case DragRaceState.Setup:
                    if (Picked == RacerStatus.Picked)
                        HandlePlayerBet();
                    if (!Driver.IsPlayer || Picked != RacerStatus.Picked)
                        break;
                    string str1 = "";
                    if ((double)WheelTemp < (double)DragMeets.IdealWheelTemp)
                        str1 = "~b~";
                    if ((double)WheelTemp > (double)DragMeets.IdealWheelTemp)
                        str1 = "~g~";
                    if ((double)WheelTemp > (double)(DragMeets.IdealWheelTemp + 20))
                        str1 = "~y~";
                    if ((double)WheelTemp > (double)(DragMeets.IdealWheelTemp + 30))
                        str1 = "~o~";
                    if ((double)WheelTemp > (double)(DragMeets.IdealWheelTemp + 40))
                        str1 = "~r~";
                    UI.ShowSubtitle(str1 + "Wheel temp: " + (object)WheelTemp + "º", 900);
                    break;
                case DragRaceState.CountDown:
                    if (Picked != RacerStatus.Picked)
                        break;
                    HandlePlayerBet();
                    break;
                case DragRaceState.RaceInProgress:
                    if (Picked != RacerStatus.Picked)
                        break;
                    if (Driver.IsPlayer && DragMeets.SimulateManualTransmission && (DragMeets.SpectCamera == (Camera)null && !DragMeets.MT_Support))
                    {
                        Function.Call<Vector3>(Hash._0x3A618A217E5154F0, 0.5f, 0.85f, 0.205f, 0.055f, 0, 0, 0, ((int)byte.MaxValue));
                        Function.Call<Vector3>(Hash._0x3A618A217E5154F0, 0.5f, 0.85f, 0.2f, 0.05f, 100, 100, 100, ((int)byte.MaxValue));
                    }
                    if (Wheelietime > Game.GameTime & Function.Call<bool>(Hash._0x5333F526F6AB19AA, Car, 10f) && (double)Car.Speed > 0.2)
                    {
                        if ((double)GripBonus >= (double)PerformanceBonus || !DragMeets.RealisticWheelies)
                        {
                            if (Wheelietime > Game.GameTime + 5500)
                                Wheelietime = Game.GameTime + 5000;
                            float num = (float)(1.0 + (double)Car.HeightAboveGround * 3.0);
                            if ((double)num > 3.0)
                                num = 3f;
                            if (Car.ClassType != VehicleClass.Super || Car.IsOnAllWheels || !DragMeets.RealisticWheelies)
                                Function.Call(Hash._0xC5F68BE9613E2D18, Car, 3, 0.0f, 0.1f, 0.0f, 0.0f, 0.0f, (-num), 0, true, true, true, true);
                        }
                        else
                        {
                            if (Wheelietime > Game.GameTime + 1500)
                                Wheelietime = Game.GameTime + 1000;
                            Car.EngineTorqueMultiplier = 50f;
                            Function.Call(Hash._0xC5F68BE9613E2D18, Car, 3, 0.0f, -0.01f, 0.0f, 0.0f, 0.0f, 0.0f, 0, true, true, true, true);
                        }
                    }
                    if (DragMeets.AllowNitro && !UsedNitro && (Driver.IsPlayer && Game.IsControlJustPressed(2, DragMeets.InteractKey)))
                    {
                        UsedNitro = true;
                        NitroTime = Game.GameTime + 2000;
                    }
                    if (NitroTime > Game.GameTime && (double)Car.Acceleration > 0.0)
                        Util.ForceNitro(Car);
                    if (Driver.IsPlayer && DragMeets.SimulateManualTransmission && (!DragMeets.MT_Support && GearBoostTime < Game.GameTime))
                    {
                        if (ShiftBoostRef > Game.GameTime)
                        {
                            if (Driver.IsPlayer && DragMeets.SpectCamera == (Camera)null)
                                Function.Call<Vector3>(Hash._0x3A618A217E5154F0, 0.5f, 0.85f, 0.2f, 0.05f, 0, ((int)byte.MaxValue), 0, ((int)byte.MaxValue));
                            if (Game.IsControlJustPressed(0, DragMeets.ShiftingKey))
                            {
                                GearBoostTime = Game.GameTime + 1000;
                                if (DragMeets.ShowGoodBadShifting)
                                    UI.Notify("~b~" + Car.FriendlyName + " ~g~perfect shift to " + (object)(GearReference + 1));
                                Util.DisplayHelpTextThisFrame("~g~Good shift.");
                            }
                        }
                        else if (Game.GameTime - ShiftBoostRef < 700)
                        {
                            if (Game.IsControlJustPressed(0, DragMeets.ShiftingKey))
                            {
                                Util.DisplayHelpTextThisFrame("~o~Too late.");
                                GearGrindTime = Game.GameTime + 1000;
                                Util.PlayAmbientSpeech(Driver, Util.ChatCurse);
                                GearGrindTime = Game.GameTime + 1000;
                                int num = Util.RandomInt(100, 500) - Car.GetMod(VehicleMod.Engine) * 100;
                                if (num > 0)
                                    Car.EngineHealth -= (float)num;
                                else
                                    num = 0;
                                string str2 = (double)Car.EngineHealth >= 200.0 ? " - engine grind (damage:" + (object)num + ")" : " - ~r~engine blowup";
                                if (DragMeets.ShowGoodBadShifting)
                                    UI.Notify("~b~" + Car.FriendlyName + " ~o~bad shift to " + (object)(GearReference + 1) + str2);
                            }
                        }
                        else if (Game.GameTime - ShiftBoostRef > 700 && Game.IsControlJustPressed(0, DragMeets.ShiftingKey))
                        {
                            Util.DisplayHelpTextThisFrame("~o~Out of timeframe.");
                            GearGrindTime = Game.GameTime + 1000;
                            Util.PlayAmbientSpeech(Driver, Util.ChatCurse);
                            GearGrindTime = Game.GameTime + 1000;
                            int num = Util.RandomInt(100, 500) - Car.GetMod(VehicleMod.Engine) * 100;
                            if (num > 0)
                                Car.EngineHealth -= (float)num;
                            else
                                num = 0;
                            string str2 = (double)Car.EngineHealth >= 200.0 ? " - engine grind (damage:" + (object)num + ")" : " - ~r~engine blowup";
                            if (DragMeets.ShowGoodBadShifting)
                                UI.Notify("~b~" + Car.FriendlyName + " ~o~bad shift to " + (object)(GearReference + 1) + str2);
                        }
                    }
                    if (DragMeets.SimulateManualTransmission && Car.CurrentGear > 1 && (Car.CurrentGear > GearReference && (double)Car.EngineHealth > 200.0))
                    {
                        ShiftBoostRef = Game.GameTime + Util.RandomInt(400, 700);
                        if (!Driver.IsPlayer)
                        {
                            int num1 = Shifting + Util.RandomInt(-25, 25);
                            if (num1 > 75 && !DragMeets.MT_Support)
                            {
                                Util.PlayAmbientSpeech(Driver, Util.ChatTaunt);
                                GearBoostTime = Game.GameTime + 1000;
                                if (DragMeets.ShowGoodBadShifting)
                                    UI.Notify("~b~" + Car.FriendlyName + " ~g~perfect shift to " + (object)(GearReference + 1));
                            }
                            if (num1 < 25)
                            {
                                Util.PlayAmbientSpeech(Driver, Util.ChatCurse);
                                GearGrindTime = Game.GameTime + 1000;
                                int num2 = Util.RandomInt(100, 500) - Car.GetMod(VehicleMod.Engine) * 100;
                                if (num2 > 0)
                                    Car.EngineHealth -= (float)num2;
                                else
                                    num2 = 0;
                                string str2 = (double)Car.EngineHealth >= 200.0 ? " - engine grind (damage:" + (object)num2 + ")" : " - ~r~engine blowup";
                                if (DragMeets.ShowGoodBadShifting)
                                    UI.Notify("~b~" + Car.FriendlyName + " ~o~bad shift to " + (object)(GearReference + 1) + str2);
                            }
                        }
                    }
                    if ((double)Car.EngineHealth <= 205.0 && (double)Car.Speed > 20.0)
                        Driver.DrivingSpeed = 15f;
                    if (GearGrindTime > Game.GameTime)
                    {
                        if (Driver.IsPlayer && DragMeets.SpectCamera == (Camera)null)
                            Function.Call<Vector3>(Hash._0x3A618A217E5154F0, 0.5f, 0.85f, 0.2f, 0.05f, ((int)byte.MaxValue), 0, 0, ((int)byte.MaxValue));
                        Car.EngineTorqueMultiplier = -0.4f;
                    }
                    else if (GearBoostTime > Game.GameTime && (double)Car.Acceleration > 0.0)
                    {
                        Car.EngineTorqueMultiplier = 2f;
                        if (Driver.IsPlayer && DragMeets.SpectCamera == (Camera)null)
                            Function.Call<Vector3>(Hash._0x3A618A217E5154F0, 0.5f, 0.85f, 0.2f, 0.05f, 100, 100, 0, ((int)byte.MaxValue));
                    }
                    GearReference = Car.CurrentGear;
                    break;
            }
        }

        public void GetToStartingPos(bool doburnouts)
        {
            if ((double)Car.BodyHealth < 1000.0)
                Car.Repair();
            if (Driver.IsPlayer)
            {
                Destination = Util.ToGround(Util.GetEntityOffset((Entity)DragMeets.FlarePed, (float)DragMeets.ForwardOffset, (float)Offset), 5f);
            }
            else
            {
                if (Util.IsDriving(Driver))
                    return;
                if (GotToMidPoint || DragMeets.PathToStartingLine == Vector3.Zero)
                {
                    if (!DragMeets.FlarePedInPosition() || DragMeets.IsStartLineBusy)
                        return;
                    Vector3 ground = Util.ToGround(Util.GetEntityOffset((Entity)DragMeets.FlarePed, 0.0f, (float)Offset), 5f);
                    Destination = Util.ToGround(Util.GetEntityOffset((Entity)DragMeets.FlarePed, 15f, (float)Offset), 5f);
                    if (!Car.IsInRangeOf(ground, 30f))
                    {
                        if (Driver.IsPlayer)
                            return;
                        float speed = (float)Util.RandomInt(5, 8) * DragMeets.SpeedMultiplier;
                        if (!Car.IsInRangeOf(Destination, 60f))
                            speed *= 1.4f;
                        if (!Driver.IsInRangeOf(Game.Player.Character.Position, 50f) && !Driver.IsOnScreen)
                            Driver.SetIntoVehicle(Car, VehicleSeat.Driver);
                        Driver.Task.DriveTo(Car, Destination, 5f, speed, DrivingStyle);
                    }
                    else
                    {
                        Destination = Util.ToGround(Util.GetEntityOffset((Entity)DragMeets.FlarePed, (float)DragMeets.ForwardOffset, (float)Offset), 5f);
                        if (!Driver.IsInRangeOf(Destination, 2f) && (Picked == RacerStatus.Picked || DragMeets.MeetState == DragRaceState.RaceInProgress))
                        {
                            if (Driver.IsPlayer || DragMeets.MeetState != DragRaceState.Setup)
                                return;
                            if (Util.IsPosAheadentity((Entity)Car, Destination, 0.0f))
                                Function.Call(Hash._0x0F3E34E968EA374E, Driver, Car, Destination.X, Destination.Y, Destination.Z, DragMeets.DragHeading, 1, 15f, true);
                            else
                                Driver.Task.DriveTo(Car, Util.ToGround(Util.GetEntityOffset((Entity)DragMeets.FlarePed, 20f, (float)Offset), 5f), 5f, (float)Util.RandomInt(2, 4) * DragMeets.SpeedMultiplier, DrivingStyle + 1024);
                        }
                        else
                        {
                            if (Util.IsDriving(Driver))
                                return;
                            if (Util.ThrowDice(20) && Car.GetMod(VehicleMod.Hydraulics) != -1 && (Car.IsOnAllWheels && Car.IsStopped))
                            {
                                float num1 = 0.0f;
                                float num2 = 0.0f;
                                int num3 = Util.RandomInt(0, 4);
                                int num4 = 1;
                                Model model;
                                if (num3 == num4)
                                {
                                    model = Car.Model;
                                    num1 = model.GetDimensions().X;
                                }
                                int num5 = 2;
                                if (num3 == num5)
                                {
                                    model = Car.Model;
                                    num1 = -model.GetDimensions().X;
                                }
                                int num6 = 3;
                                if (num3 == num6)
                                {
                                    model = Car.Model;
                                    num2 = model.GetDimensions().Y;
                                }
                                int num7 = 4;
                                if (num3 == num7)
                                {
                                    model = Car.Model;
                                    num2 = -model.GetDimensions().Y;
                                }
                                Function.Call(Hash._0xC5F68BE9613E2D18, Car, 3, 0.0f, 0.0f, 1f, num1, num2, 0.0f, 0.0f, true, true, true, true);
                            }
                            else if (Car.GetMod(VehicleMod.Engine) > 1 & doburnouts && Util.ThrowDice(20 + (DragMeets.IdealWheelTemp - (int)WheelTemp)))
                            {
                                if (Driver.IsPlayer)
                                    return;
                                Util.TempAction(Driver, Car, 30, Util.RandomInt(1400, 3000));
                                WheelTemp = WheelTemp + 10f;
                                if ((double)WheelTemp + 10.0 <= (double)DragMeets.IdealWheelTemp)
                                    return;
                                UI.Notify("~b~" + Name + "~w~ warms up the " + Car.FriendlyName + "'s tires.");
                            }
                            else
                            {
                                if (!Util.ThrowDice(10) || Driver.IsPlayer)
                                    return;
                                Util.TempAction(Driver, Car, 31, Util.RandomInt(1000, 2500));
                            }
                        }
                    }
                }
                else
                {
                    if (Driver.IsPlayer)
                        return;
                    if (Car.IsInRangeOf(DragMeets.PathToStartingLine, 20f))
                    {
                        GotToMidPoint = true;
                    }
                    else
                    {
                        float speed = (float)Util.RandomInt(5, 10) * DragMeets.SpeedMultiplier;
                        if (Car.IsInRangeOf(DragMeets.PathToStartingLine, 70f))
                            speed *= 2f;
                        Driver.Task.DriveTo(Car, DragMeets.PathToStartingLine, 10f, speed, DrivingStyle);
                    }
                }
            }
        }

        private void HandleRacerSpectating()
        {
        }

        private void HandleWaitingAreaBehavior()
        {
            if (Driver.IsPlayer)
                return;
            if ((double)Car.BodyHealth < 1000.0)
                Function.Call(Hash._0x953DA1E1B12C0491, new InputArgument[1]
                {
           Car
                });
            if (Driver.IsOnFoot && Car.IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange))
            {
                if (DragMeets.MeetState == DragRaceState.Idle && Driver.TaskSequenceProgress == -1)
                {
                    if (!Function.Call<bool>(Hash._0x57AB4A3080F85143, new InputArgument[1]
                    {
             Driver
                    }) && Util.ThrowDice(20))
                    {
                        TaskSequence sequence = new TaskSequence();
                        Vector3 vector3 = Car.Position.Around(3f);
                        Function.Call(Hash._0x15D3A79D4E44B913, 0, vector3.X, vector3.Y, vector3.Z, 1f, 30000, 0.0f, 0, 0.0f);
                        Function.Call(Hash._0x1DDA930A0AC38571, 0, Car.Position.X, Car.Position.Y, Car.Position.Z, 3000);
                        Function.Call(Hash._0x142A02425FF02BD9, 0, "WORLD_HUMAN_SMOKING", 5000, true);
                        sequence.Close();
                        Driver.Task.PerformSequence(sequence);
                        sequence.Dispose();
                    }
                }
                if (DragMeets.MeetState == DragRaceState.RaceFinished)
                {
                    if (Function.Call<bool>(Hash._0x57AB4A3080F85143, new InputArgument[1]
                    {
             Driver
                    }))
                        Driver.Task.ClearAll();
                }
            }
            if (Car.IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange + 20f))
            {
                if ((double)Car.Speed > 6.0)
                    Driver.DrivingSpeed = 5f;
            }
            else if (Car.IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange * 10f) && (double)Car.Speed > 15.0)
                Driver.DrivingSpeed = 14f;
            if (Car.IsInRangeOf(DragMeets.PathToWaitingArea, 40f) && !GotToMidPoint)
                GotToMidPoint = true;
            if (Util.IsDriving(Driver) || Driver.IsPlayer)
                return;
            if (Car.IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange))
            {
                if (Driver.IsOnFoot || Util.IsDriving(Driver) || !Car.IsStopped)
                    return;
                if (World.GetNearbyVehicles(Car.Position, 7f).Length > 3)
                {
                    Destination = Util.GetEntityOffset((Entity)Car, (float)(int)System.Math.Round((double)DragMeets.WaitingAreRange * 0.5), (float)Util.RandomInt(-10, 10));
                    Driver.Task.DriveTo(Car, Destination, 5f, 2f, DrivingStyle);
                }
                else
                {
                    Util.SetRadioLoud(Car, DragMeets.Radio);
                    if (Car.GetMod(VehicleMod.Speakers) > -1 || Car.GetMod(VehicleMod.DoorSpeakers) > -1)
                        Car.OpenDoor(VehicleDoor.Trunk, false, false);
                    if (Car.GetMod(VehicleMod.EngineBlock) > -1)
                        Car.OpenDoor(VehicleDoor.Hood, false, false);
                    Driver.Task.GoTo(Util.GetEntityOffset((Entity)Car, 4f, 0.0f));
                }
            }
            else
            {
                if (Util.IsDriving(Driver))
                    return;
                int drivingstyle = DrivingStyle;
                if (Driver.IsInRangeOf(DragMeets.WaitingArea, 200f))
                {
                    Destination = DragMeets.WaitingArea.Around(DragMeets.WaitingAreRange / (float)Util.RandomInt(1, 4));
                    if (!Util.CarCanSeePos(Car, Destination))
                        drivingstyle = 61;
                    Driver.Task.DriveTo(Car, Destination, 4f, (float)Util.RandomInt(4, 8) * DragMeets.SpeedMultiplier, drivingstyle);
                }
                else
                {
                    if (!Util.CarCanSeePos(Car, DragMeets.WaitingArea))
                        drivingstyle = 61;
                    Driver.Task.DriveTo(Car, DragMeets.WaitingArea, DragMeets.WaitingAreRange + 10f, (float)Util.RandomInt(20, 35) * DragMeets.SpeedMultiplier, drivingstyle);
                }
            }
        }

        public void HandleAI()
        {
            if (Driver.IsInVehicle(Car) && (double)Car.Speed > 1.0)
            {
                if (Car.IsDoorOpen(VehicleDoor.Trunk))
                    Car.CloseDoor(VehicleDoor.Trunk, false);
                if (Car.IsDoorOpen(VehicleDoor.Hood))
                    Car.CloseDoor(VehicleDoor.Hood, false);
            }
            if (Driver.IsInCombat)
            {
                if (Driver.Weapons.HasWeapon(WeaponHash.Pistol))
                    return;
                Driver.Weapons.Give(WeaponHash.Pistol, 500, true, true);
            }
            else
            {
                if (Driver.IsPlayer)
                {
                    if (Car.IsInBurnout())
                    {
                        WheelTemp = WheelTemp + 2f;
                        if ((double)WheelTemp > 70.0 && 1 != 0)
                        {
                            int wheel = Util.RandomInt(4, 5);
                            if (Util.CannotWheelie.Contains(Car.Model))
                                wheel = Util.RandomInt(0, 1);
                            if (!Car.IsTireBurst(wheel))
                                Car.BurstTire(wheel);
                        }
                    }
                    else if ((double)WheelTemp > (double)(DragMeets.IdealWheelTemp - 15))
                        WheelTemp = WheelTemp - 0.1f;
                }
                else
                {
                    if ((double)WheelTemp > (double)(DragMeets.IdealWheelTemp - 15))
                        WheelTemp = WheelTemp - 0.1f;
                    if (Picked == RacerStatus.NotPicked && Car.CurrentBlip.Color != BlipColor.Blue)
                        Car.CurrentBlip.Color = BlipColor.Blue;
                    if (Picked == RacerStatus.Picked && Car.CurrentBlip.Color != BlipColor.Green)
                        Car.CurrentBlip.Color = BlipColor.Green;
                    if (Picked == RacerStatus.PickedNext && Car.CurrentBlip.Color != BlipColor.Yellow)
                        Car.CurrentBlip.Color = BlipColor.Yellow;
                    if (Forced && Car.CurrentBlip.Color != BlipColor.Red)
                        Car.CurrentBlip.Color = BlipColor.Red;
                }
                WheelTemp = (float)System.Math.Round((double)WheelTemp, 1);
                if (StuckTimeRef < Game.GameTime)
                {
                    if (Util.ThrowDice(10) && racertext.IsLoaded)
                        racertext.Unload();
                    StuckTimeRef = Game.GameTime + 5000;
                    if (Car.IsUpsideDown && Car.IsStopped)
                        Car.PlaceOnGround();
                    if (Picked != RacerStatus.Picked || DragMeets.MeetState != DragRaceState.CountDown)
                    {
                        StuckScore = !Util.IsDriving(Driver) || !Util.CanWeUse((Entity)Car.GetPedOnSeat(VehicleSeat.Driver)) || (!Car.IsInRangeOf(StuckReference, 5f) || Driver.IsOnFoot) ? 0 : StuckScore + 1;
                        StuckReference = Car.Position;
                    }
                }
                if (StuckScore == 3)
                {
                    Function.Call(Hash._0xFB8794444A7D60FB, Car, false);
                    Driver.Task.DriveTo(Car, Util.GetEntityOffset((Entity)Car, -9f, (float)Util.RandomInt(-5, 5)), 5f, 4f, 4195388);
                    StuckScore = StuckScore + 1;
                    if (Game.Player.Character.IsInRangeOf(Car.Position, 50f))
                        UI.Notify("~b~" + Name + "~w~: Help me I'm stuck");
                }
                if (StuckScore == 10)
                {
                    if (Picked == RacerStatus.Picked && DragMeets.PathToStartingLine != Vector3.Zero)
                        Car.Position = DragMeets.PathToStartingLine.Around(5f);
                    else
                        Car.Position = Util.GetEntityOffset((Entity)DragMeets.FlarePed, 10f, 0.0f);
                    StuckScore = 0;
                    Driver.Task.DriveTo(Car, Car.Position, 10f, 10f, DrivingStyle);
                }
                switch (DragMeets.MeetState)
                {
                    case DragRaceState.NotStarted:
                        if (Driver.IsInRangeOf(Game.Player.Character.Position, 50f))
                        {
                            if (Driver.IsPlayer || Util.IsDriving(Driver))
                                break;
                            Function.Call(Hash._0x480142959D337D00, Driver, Car, 20f, DrivingStyle);
                            break;
                        }
                        Clear(false);
                        break;
                    case DragRaceState.Idle:
                        if (!Driver.IsPlayer && Driver.Money == 0 && Picked == RacerStatus.NotPicked)
                        {
                            Function.Call(Hash._0x480142959D337D00, Driver, Car, 20f, DrivingStyle);
                            Reason = "~r~Lost all the money";
                            RemoveMe = true;
                        }
                        if (Picked == RacerStatus.Picked)
                            GetToStartingPos(true);
                        if (Picked == RacerStatus.PickedNext)
                            GetToStartingPos(false);
                        if (Picked != RacerStatus.NotPicked)
                            break;
                        HandleWaitingAreaBehavior();
                        break;
                    case DragRaceState.Setup:
                        if (Picked == RacerStatus.Picked)
                            GetToStartingPos(true);
                        if (Picked == RacerStatus.PickedNext)
                            GetToStartingPos(false);
                        if (Picked != RacerStatus.NotPicked)
                            break;
                        HandleWaitingAreaBehavior();
                        break;
                    case DragRaceState.CountDown:
                        if (Picked == RacerStatus.NotPicked)
                        {
                            HandleWaitingAreaBehavior();
                            break;
                        }
                        if (!Util.IsSubttaskActive(Driver, Subtask.CTaskVehicleTempAction) || !Util.InTaskSequence(Driver))
                        {
                            if (!Driver.IsPlayer && Util.ThrowDice(5))
                                Util.TempAction(Driver, Car, 31, Util.RandomInt(1000, 2500));
                        }
                        else
                            UI.Notify(Car.FriendlyName + "already in burnout");
                        if (!UsedNitro)
                            break;
                        UsedNitro = false;
                        break;
                    case DragRaceState.RaceInProgress:
                        if (Picked == RacerStatus.NotPicked)
                            HandleWaitingAreaBehavior();
                        if (Picked != RacerStatus.Picked)
                            break;
                        if (!RacingToFinish && !Util.CanWeUse((Entity)Parachute))
                        {
                            Parachute = World.CreateProp((Model)"p_cargo_chute_s", Car.Position, false, false);
                            Parachute.AttachTo((Entity)Car, Util.GetBoneIndex((Entity)Car, "boot"), Vector3.Zero, new Vector3((float)(-(double)Car.Rotation.X + 85.0), -Car.Rotation.Y, 0.0f));
                        }
                        if (!Driver.IsPlayer)
                        {
                            if (!Util.IsDriving(Driver) && Car.IsStopped && RacingToFinish)
                                TaskRace();
                            if (DragMeets.AllowNitro && !UsedNitro && (Car.CurrentGear > 2 && Util.RandomInt(0, 10) < 2))
                            {
                                UsedNitro = true;
                                NitroTime = Game.GameTime + 2000;
                            }
                        }
                        if (Driver.IsPlayer || Util.IsDriving(Driver) || (RacingToFinish || (double)Util.ForwardSpeed(Car) <= 5.0) || !Util.IsSliding(Car, 0.2f))
                            break;
                        Util.TempAction(Driver, Car, 24, 1000);
                        break;
                    case DragRaceState.RaceFinished:
                        if (Picked == RacerStatus.NotPicked)
                            HandleWaitingAreaBehavior();
                        RacingToFinish = false;
                        break;
                }
            }
        }

        public void TaskFinish()
        {
            RacesDone = RacesDone + 1;
            Expectations = RacerExpectations.None;
            max_willing_to_bet = 60;
            min_willing_to_bet = 20;
            Offset = 0;
            Car.EnginePowerMultiplier = 1f;
            GripBonus = 0.0f;
            PerformanceBonus = 0.0f;
            RacingToFinish = false;
            Destination = DragMeets.WaitingArea;
            GotToMidPoint = false;
            StartTime = 0;
            TrackTime = new TimeSpan();
            ReactTimeNotified = false;
            Penalized = false;
            Picked = RacerStatus.NotPicked;
            if (Driver.IsPlayer)
                DragMeets.SavePlayerstats();
            else if (RacesDone >= 2 && (double)ReputationBalance() < 30.0)
            {
                RemoveMe = true;
                Reason = "~y~Bored of losing";
            }
            else if (RacesDone > 5)
            {
                RemoveMe = true;
                Reason = "~g~Enough racing";
            }
            else
                TaskToWaitingArea();
        }

        public void TaskToWaitingArea()
        {
            if (DragMeets.ToWaitingArea.Count <= 0 || Driver.IsPlayer)
                return;
            TaskSequence sequence = new TaskSequence();
            foreach (Vector3 pos in DragMeets.ToWaitingArea)
            {
                int num = 262205;
                if (Util.CarCanSeePos(Car, pos))
                    num += 4194304;
                Function.Call(Hash._0x158BB33F920D360C, 0, Car, pos.X, pos.Y, pos.Z, 30f, num, 3f);
            }
            sequence.Close();
            Driver.Task.PerformSequence(sequence);
            sequence.Dispose();
        }

        public void TaskRace()
        {
            StartTime = Game.GameTime;
            StuckScore = 0;
            EngineBonusMultiplier = 1f;
            GotToMidPoint = false;
            string friendlyName = Car.FriendlyName;
            Vector3 position = DragMeets.FlarePed.Position;
            float num1 = 30f;
            float num2 = 0.0f;
            if (DragMeets.RealisticWheelies)
                num1 = 70f;
            if (Car.GetMod(VehicleMod.Engine) + Car.GetMod(VehicleMod.Transmission) > 0)
                PerformanceBonus = (float)((Car.GetMod(VehicleMod.Engine) + Car.GetMod(VehicleMod.Transmission)) * 10);
            if (DragMeets.SimulateTirePerformance)
            {
                if (Function.Call<bool>(Hash._0x125BF4ABFC536B09, position.X, position.Y, position.Z, DragMeets.FlarePed))
                {
                    if ((double)WheelTemp > (double)DragMeets.IdealWheelTemp)
                    {
                        friendlyName += "~n~Tires warm, +25%";
                        GripBonus = GripBonus + 25f;
                    }
                    if (Util.HasCustomTires(Car))
                    {
                        friendlyName += "~n~Custom Tires, +25%";
                        GripBonus = GripBonus + 25f;
                    }
                    int mod = Car.GetMod(VehicleMod.Suspension);
                    if (mod > 0 && !DragMeets.RealisticWheelies)
                    {
                        friendlyName += "~n~Vehicle Suspension Lowered";
                        num2 += (float)mod * 10f;
                    }
                    if (Car.ClassType == VehicleClass.Sports && Car.WheelType == VehicleWheelType.Sport && DragMeets.RealisticWheelies)
                    {
                        friendlyName += "~n~Sport Tires in Sport Car, +30%";
                        GripBonus = GripBonus + 30f;
                    }
                    if (Car.ClassType == VehicleClass.SportsClassics && Car.WheelType == VehicleWheelType.Sport && DragMeets.RealisticWheelies)
                    {
                        friendlyName += "~n~Sport Tires in Classic Sports Car, +30%";
                        GripBonus = GripBonus + 30f;
                    }
                    if (Car.ClassType == VehicleClass.Super && Car.WheelType == VehicleWheelType.HighEnd && DragMeets.RealisticWheelies)
                    {
                        friendlyName += "~n~High End Tires in HE car, +30%";
                        GripBonus = GripBonus + 30f;
                    }
                    if (Car.ClassType == VehicleClass.Muscle && Car.WheelType == VehicleWheelType.Muscle && DragMeets.RealisticWheelies)
                    {
                        friendlyName += "~n~Muscle Tires in Muscle car, +30%";
                        GripBonus = GripBonus + 30f;
                    }
                }
                else if (Car.WheelType == VehicleWheelType.Offroad)
                {
                    friendlyName += "~n~Offroad Tyres, +1";
                    GripBonus = GripBonus + 10f;
                }
            }
            EngineBonusMultiplier = GripBonus + PerformanceBonus;
            if ((double)EngineBonusMultiplier < 1.0)
                EngineBonusMultiplier = 1f;
            string message = friendlyName + "~n~~b~Performance:" + (object)PerformanceBonus + "%" + "~n~~b~Grip: " + (object)GripBonus + "%" + "~n~~b~PowerMultiplier: " + (object)(float)((double)EngineBonusMultiplier + 100.0) + "%";
            Car.EnginePowerMultiplier = EngineBonusMultiplier * 0.1f + (float)CustomTurbo;
            if (DragMeets.RealisticWheelies)
            {
                if (!Util.CannotWheelie.Contains(Car.Model) && (double)GripBonus - (double)num2 >= (double)num1 && (Car.GetMod(VehicleMod.Engine) > 1 && Car.GetMod(VehicleMod.Transmission) > 1))
                {
                    int num3 = (int)(((double)WheelTemp - (double)DragMeets.IdealWheelTemp) * 100.0);
                    if (num3 > 600)
                        Wheelietime = Game.GameTime + num3;
                }
            }
            else if (!Util.CannotWheelie.Contains(Car.Model) && (double)GripBonus >= (double)num1)
                Wheelietime = Game.GameTime + Util.RandomInt(2, 4) * 1000;
            Function.Call(Hash._0xFB8794444A7D60FB, Car, false);
            RacingToFinish = true;
            Destination = Util.ToGround(Util.GetEntityOffset((Entity)DragMeets.FinishBarrel, 10f, (float)-Offset), 10f);
            if (Destination == Vector3.Zero)
                Destination = Util.GetEntityOffset((Entity)DragMeets.FinishBarrel, 10f, (float)-Offset);
            if (!Driver.IsPlayer)
            {
                int num3 = 1;
                int num4 = 0;
                if (DragMeets.ReactionTimes)
                {
                    num3 = 1;
                    num4 = Util.RandomInt(Reaction - 100, Reaction + 100);
                    if (num4 < 50)
                        num4 = Util.RandomInt(20, 90);
                    if (DragMeets.ShowReactionTimes)
                        message = message + "~n~~w~React.Time: ~y~" + (object)num4;
                }
                int num5 = 16777244;
                if (DragMeets.TougeMode)
                    num5 = 262172;
                TaskSequence sequence = new TaskSequence();
                Function.Call(Hash._0xC429DCEEB339E129, 0, Car, num3, num4);
                Function.Call(Hash._0x158BB33F920D360C, 0, Car, Destination.X, Destination.Y, Destination.Z, 200f, num5, 8f);
                sequence.Close();
                Driver.Task.PerformSequence(sequence);
                sequence.Dispose();
            }
            if (!DragMeets.ShowGrip && !DragMeets.ShowReactionTimes)
                return;
            UI.Notify(message);
        }

        public void Clear(bool HardDeletion)
        {
            if (!Driver.IsPlayer)
            {
                if (Util.CanWeUse((Entity)Car))
                {
                    if (HardDeletion)
                        Car.Delete();
                    else
                        Car.MarkAsNoLongerNeeded();
                }
                if (Util.CanWeUse((Entity)Driver))
                {
                    if (HardDeletion)
                        Driver.Delete();
                    else
                        Driver.MarkAsNoLongerNeeded();
                }
            }
            else if (Util.CanWeUse((Entity)Car) && Car.IsPersistent)
                Car.IsPersistent = false;
            RemoveMe = true;
        }
    }
}
