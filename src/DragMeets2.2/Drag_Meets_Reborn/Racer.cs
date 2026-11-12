using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Drag_Meets_Reborn
{
public class Racer
{
	private bool UsesParachute;

	private bool ParachuteWasted;

	public bool IsPersistent;

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

	public Prop SuperEngine;

	public Racer(string name, Ped ped, Vehicle veh, int customTurbo, int reaction, int shifting, bool notify, int won, int lost, int money, bool parachute, bool persistent)
	{
		//IL_00a0: Expected Obj, but got Unknown
		//IL_01aa: Invalid comparison between Unknown and I4
		//IL_01dc: Invalid comparison between Unknown and I4
		//IL_01ea: Invalid comparison between Unknown and I4
		//IL_021c: Invalid comparison between Unknown and I4
		Driver = ped;
		Car = veh;
		Reaction = reaction;
		Shifting = shifting;
		Name = name;
		IsPersistent = persistent;
		UsesParachute = parachute;
		if (customTurbo > 1)
		{
			CustomTurbo = customTurbo;
		}
		if (Name == Car.FriendlyName)
		{
			Name = Car.FriendlyName;
		}
		if (!((Entity)Driver).IsPersistent)
		{
			((Entity)Driver).IsPersistent = true;
		}
		if (!((Entity)Car).IsPersistent)
		{
			((Entity)Car).IsPersistent = true;
		}
		Destination = DragMeets.WaitingArea;
		PerformancePoints = Util.CalculatePerformancePoints(Car);
		if (!Driver.IsPlayer)
		{
			Wins = won;
			Loses = lost;
			if (!Driver.IsPlayer)
			{
				Driver.Money = money;
				int num = 0;
				if ((int)Car.ClassType == 7 && Driver.Money < DragMeets.MinRacerMoneySuper)
				{
					num = DragMeets.MinRacerMoneySuper + Util.RandomInt(0, DragMeets.MinRacerMoneySuper);
				}
				if (((int)Car.ClassType == 6 || (int)Car.ClassType == 5) && Driver.Money < DragMeets.MinRacerMoneySports)
				{
					num = DragMeets.MinRacerMoneySports + Util.RandomInt(0, DragMeets.MinRacerMoneySports);
				}
				if ((int)Car.ClassType == 4 && Driver.Money < DragMeets.MinRacerMoneyMuscle)
				{
					num = DragMeets.MinRacerMoneyMuscle + Util.RandomInt(0, DragMeets.MinRacerMoneyMuscle);
				}
				if (Driver.Money < DragMeets.MinRacerMoneyOther)
				{
					num = DragMeets.MinRacerMoneyOther + Util.RandomInt(0, DragMeets.MinRacerMoneyOther);
				}
				if (num != 0)
				{
					Util.ChangePedMoney(Driver, num);
				}
			}
			Driver.CanBeDraggedOutOfVehicle = false;
			Driver.RelationshipGroup = DragMeets.RacerRLGroup;
			if (((Entity)Car).IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange + 30f))
			{
				GotToMidPoint = true;
			}
			if (DragMeets.RacerBlips && !((Entity)Car).CurrentBlip.Exists())
			{
				((Entity)Car).AddBlip();
				((Entity)Car).CurrentBlip.Color = (BlipColor)3;
				((Entity)Car).CurrentBlip.Scale = 0.6f;
				((Entity)Car).CurrentBlip.IsShortRange = true;
			}
			if (DragMeets.RacerBlips && !((Entity)Driver).CurrentBlip.Exists())
			{
				((Entity)Driver).AddBlip();
				((Entity)Driver).CurrentBlip.Color = (BlipColor)0;
				((Entity)Driver).CurrentBlip.Scale = 0.3f;
				((Entity)Driver).CurrentBlip.IsShortRange = true;
			}
			Driver.AlwaysKeepTask = true;
			Function.Call((Hash)992985146056095358L, new InputArgument[3]
			{
				Driver,
				46,
				true
			});
			Function.Call((Hash)992985146056095358L, new InputArgument[2]
			{
				Car,
				true
			});
			Function.Call((Hash)unchecked((ulong)(-5650329060376590909L)), new InputArgument[2]
			{
				Driver,
				100
			});
			Function.Call((Hash)unchecked((ulong)(-6955927877681029095L)), new InputArgument[3]
			{
				Driver,
				46,
				true
			});
			Vector3 position = ((Entity)Car).Position;
			float num2 = position.DistanceTo(DragMeets.Finishline);
			position = ((Entity)Car).Position;
			if (num2 < position.DistanceTo(DragMeets.StartingLine))
			{
				TaskToWaitingArea();
			}
		}
		else
		{
			DragMeets.LoadPlayerStats(this);
		}
		if (notify && DragMeets.ActivityNotifications)
		{
			UI.Notify("~b~" + Name + " ~w~has joined the meet.");
		}
	}

	public int GetRacerMoney()
	{
		if (Driver.IsPlayer)
		{
			return Game.Player.Money;
		}
		return Driver.Money;
	}

	public bool Ok()
	{
		if (!Util.CanWeUse((Entity)(object)Car))
		{
			if (Reason == "")
			{
				Reason = "car dissapeared";
			}
			return false;
		}
		if (!Util.CanWeUse((Entity)(object)Driver))
		{
			if (Reason == "")
			{
				Reason = "driver dissapeared";
			}
			return false;
		}
		if (!((Entity)Car).IsAlive)
		{
			if (Reason == "")
			{
				Reason = "car dead";
			}
			return false;
		}
		if (((Entity)Driver).IsAlive)
		{
			return true;
		}
		if (Reason == "")
		{
			Reason = "driver dead";
		}
		return false;
	}

	public bool InPosition()
	{
		if (((Entity)Car).IsInRangeOf(Util.ToGround(Util.GetEntityOffset((Entity)(object)DragMeets.FlarePed, DragMeets.ForwardOffset, Offset), 5f), 4f))
		{
			return (double)Car.Speed < 0.300000011920929;
		}
		return false;
	}

	public void HandlePlayerBet()
	{
		if (!Game.Player.Character.IsOnFoot || Driver.IsPlayer || !((Entity)Game.Player.Character).IsInRangeOf(((Entity)Driver).Position, 4f))
		{
			return;
		}
		if (Game.Player.Money >= DragMeets.MoneyBet)
		{
			if (DragMeets.GetPlayerRacer() != null)
			{
				if (!BetsOnHim.Contains(DragMeets.GetPlayerRacer()) && !DragMeets.GetPlayerRacer().HasBet)
				{
					if (DragMeets.GetPlayerRacer().Picked == RacerStatus.Picked)
					{
						Util.DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(DragMeets.InteractKey) + " to refuse to race.");
					}
					else
					{
						Util.DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(DragMeets.InteractKey) + " to bet ~g~$" + DragMeets.MoneyBet + "~w~ on this guy.");
					}
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
						{
							DragMeets.GetPlayerRacer().Picked = RacerStatus.NotPicked;
						}
					}
				}
			}
			else
			{
				Util.DisplayHelpTextThisFrame("You need to register in the Meet to participate in bettings.");
			}
		}
		else
		{
			Util.DisplayHelpTextThisFrame("You don't have money to bet.");
		}
		if (Game.IsControlJustReleased(2, (Control)75) && BetsOnHim.Contains(DragMeets.GetPlayerRacer()))
		{
			Game.Player.Character.Task.ClearAll();
			Game.Player.Character.Task.EnterVehicle(Car, (VehicleSeat)0, 6000, 1f);
		}
	}

	public float ReputationBalance()
	{
		int num = 10;
		float num2 = 50f + (float)(Wins * num) - (float)(Loses * num);
		if ((double)num2 < 0.0)
		{
			num2 = 0f;
		}
		if ((double)num2 > 100.0)
		{
			num2 = 100f;
		}
		return num2;
	}

	public void OnTick()
	{
		//IL_044e: Expected Obj, but got Unknown
		//IL_0b36: Invalid comparison between Unknown and I4
		if (Util.CanWeUse((Entity)(object)Parachute))
		{
			if ((double)Util.ForwardSpeed(Car) > 4.0)
			{
				((Entity)Car).ApplyForceRelative(new Vector3(0f, -0.3f, 0f));
			}
			else
			{
				ParachuteWasted = true;
				((Entity)Parachute).Delete();
			}
		}
		if (((Entity)DragMeets.FinishBarrel).IsInRangeOf(((Entity)Game.Player.Character).Position, 4f))
		{
			Util.DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(DragMeets.InteractKey) + " fix the barrel's position.");
			if (Game.IsControlJustPressed(2, DragMeets.InteractKey))
			{
				Function.Call((Hash)6388445041372756643L, new InputArgument[1] { DragMeets.FinishBarrel });
			}
		}
		if (DragMeets.GetPlayerRacer() != null && Picked == RacerStatus.NotPicked && !Forced && !Driver.IsPlayer && Util.EntitySeenByPlayer() == (Entity)(object)Car)
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
						if (PerformancePoints > DragMeets.GetPlayerRacer().PerformancePoints)
						{
							if (ReputationBalance() > DragMeets.GetPlayerRacer().ReputationBalance())
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
					else if ((double)Math.Abs(ReputationBalance() - DragMeets.GetPlayerRacer().ReputationBalance()) < 10.0)
					{
						text = "~b~" + Name + "~w~ thinks this will be a close race.";
					}
					else
					{
						if ((double)ReputationBalance() > (double)DragMeets.GetPlayerRacer().ReputationBalance() + 20.0)
						{
							text = "~b~" + Name + "~w~ expects you to lose.";
						}
						if ((double)DragMeets.GetPlayerRacer().ReputationBalance() > (double)ReputationBalance() + 20.0)
						{
							text = "~b~" + Name + "~w~ expects to lose.";
						}
					}
				}
				if (text.Length > 1 && DragMeets.Guides)
				{
					Util.AddQueuedHelpText(text);
				}
				if (flag)
				{
					Util.AddQueuedHelpText("~b~" + Name + "~g~ has accepted the challenge.");
					Forced = true;
					UI.Notify("~b~" + DragMeets.GetPlayerRacer().Name + "~w~ challenges ~b~" + Name + "~w~!");
					DragMeets.GetPlayerRacer().Forced = true;
				}
				else
				{
					Util.AddQueuedHelpText("~b~" + Name + "~o~ has refused the challenge.");
				}
			}
		}
		if (Driver.IsPlayer && !((Entity)Driver).IsInRangeOf(Destination, 2f) && Picked == RacerStatus.Picked && DragMeets.MeetState == DragRaceState.Setup)
		{
			World.DrawMarker((MarkerType)0, Destination + new Vector3(0f, 0f, 1f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 1f), Color.Yellow);
		}
		if (Game.Player.Character.IsOnFoot && (Entity)(object)Car == Util.EntitySeenByPlayer())
		{
			if (!racertext.IsLoaded)
			{
				racertext = new Scaleform("mp_car_stats_02");
			}
			Model model;
			if (Function.Call<bool>((Hash)8947185480862490559L, new InputArgument[1] { Game.Player.Character }))
			{
				InputArgument[] array = new InputArgument[1];
				model = ((Entity)Car).Model;
				array[0] = model.Hash;
				float num = (float)Math.Round((double)Function.Call<float>((Hash)unchecked((ulong)(-858003554332316349L)), array) * 2.20000004768372);
				float num2 = (float)Math.Round(Function.Call<float>((Hash)unchecked((ulong)(-5945167140126353468L)), new InputArgument[1] { Car }), 2);
				float num3 = (float)Math.Round(Function.Call<float>((Hash)unchecked((ulong)(-8357471048252236950L)), new InputArgument[1] { ((Entity)Car).Model }), 2);
				string text2 = num.ToString();
				if ((double)num > 100.0)
				{
					num = 99.9f;
				}
				if ((double)num3 > 100.0)
				{
					num3 = 99.9f;
				}
				racertext.CallFunction("SET_VEHICLE_INFOR_AND_STATS", new object[12]
				{
					Name + " ($" + Driver.Money + ") - " + Car.FriendlyName,
					"Wins/Loses:  " + Wins + "/" + Loses,
					"MPCarHUD",
					"Dinka",
					"Wheel Temp: " + WheelTemp + "º",
					"Speed: " + text2 + " mph",
					"Acceleration",
					"Braking",
					WheelTemp,
					num,
					(float)((double)num3 * 100.0),
					(float)((double)num2 * 100.0)
				});
				Scaleform val = racertext;
				Vector3 position = ((Entity)Car).Position;
				model = ((Entity)Car).Model;
				val.Render3D(position + new Vector3(0f, 0f, model.GetDimensions().Z + 1f), GameplayCamera.Rotation, new Vector3(6f, 3f, 1f));
			}
			else
			{
				float num4 = (float)Math.Round((double)Shifting - (double)Reaction * 0.00999999977648258);
				if ((double)num4 > 100.0)
				{
					num4 = 100f;
				}
				else if ((double)num4 < 0.0)
				{
					num4 = 0f;
				}
				float num5 = (Car.GetMod((VehicleMod)11) + Car.GetMod((VehicleMod)13)) * 10;
				if ((double)num5 < 0.0)
				{
					num5 = 0f;
				}
				racertext.CallFunction("SET_VEHICLE_INFOR_AND_STATS", new object[12]
				{
					Name,
					Car.FriendlyName,
					"MPCarHUD",
					"Dinka",
					"Reputation",
					"Skill",
					"Performance ",
					"Upgrades",
					ReputationBalance(),
					num4,
					PerformancePoints,
					num5
				});
				Scaleform val2 = racertext;
				Vector3 position2 = ((Entity)Car).Position;
				model = ((Entity)Car).Model;
				val2.Render3D(position2 + new Vector3(0f, 0f, model.GetDimensions().Z + 1f), GameplayCamera.Rotation, new Vector3(6f, 3f, 1f));
			}
		}
		switch (DragMeets.MeetState)
		{
		case DragRaceState.Idle:
			if (!((double)Car.EngineHealth >= 201.0))
			{
				Reason = "~r~ engine broken";
				RemoveMe = true;
			}
			break;
		case DragRaceState.Setup:
			if (Picked == RacerStatus.Picked)
			{
				HandlePlayerBet();
			}
			if (Driver.IsPlayer && Picked == RacerStatus.Picked)
			{
				string text6 = "";
				if (WheelTemp < (float)DragMeets.IdealWheelTemp)
				{
					text6 = "~b~";
				}
				if (WheelTemp > (float)DragMeets.IdealWheelTemp)
				{
					text6 = "~g~";
				}
				if (WheelTemp > (float)(DragMeets.IdealWheelTemp + 20))
				{
					text6 = "~y~";
				}
				if (WheelTemp > (float)(DragMeets.IdealWheelTemp + 30))
				{
					text6 = "~o~";
				}
				if (WheelTemp > (float)(DragMeets.IdealWheelTemp + 40))
				{
					text6 = "~r~";
				}
				UI.ShowSubtitle(text6 + "Wheel temp: " + WheelTemp + "º", 900);
			}
			break;
		case DragRaceState.CountDown:
			if (Picked == RacerStatus.Picked)
			{
				HandlePlayerBet();
			}
			break;
		case DragRaceState.RaceInProgress:
			if (Picked != RacerStatus.Picked)
			{
				break;
			}
			if (Driver.IsPlayer && DragMeets.SimulateManualTransmission && DragMeets.SpectCamera == (Camera)null && !DragMeets.MT_Support)
			{
				Function.Call<Vector3>((Hash)4206795403398567152L, new InputArgument[8]
				{
					0.5f,
					0.85f,
					0.205f,
					0.055f,
					0,
					0,
					0,
					255
				});
				Function.Call<Vector3>((Hash)4206795403398567152L, new InputArgument[8]
				{
					0.5f,
					0.85f,
					0.2f,
					0.05f,
					100,
					100,
					100,
					255
				});
			}
			if (Wheelietime > Game.GameTime && (double)Car.Speed > 0.2)
			{
				if (GripBonus >= PerformanceBonus || !DragMeets.RealisticWheelies)
				{
					if (Wheelietime > Game.GameTime + 5500)
					{
						Wheelietime = Game.GameTime + 5000;
					}
					float num6 = 3f * DragMeets.WheelieForce;
					if (((int)Car.ClassType != 7 || Car.IsOnAllWheels || !DragMeets.RealisticWheelies) && !Function.Call<bool>((Hash)unchecked((ulong)(-8382040100735967724L)), new InputArgument[1] { Car }))
					{
						Function.Call((Hash)unchecked((ulong)(-4182001369491821288L)), new InputArgument[13]
						{
							Car,
							3,
							0f,
							0.2f,
							0f,
							0f,
							0f,
							0f - num6,
							0,
							true,
							true,
							true,
							true
						});
					}
				}
				else
				{
					if (Wheelietime > Game.GameTime + 1500)
					{
						Wheelietime = Game.GameTime + 1000;
					}
					Car.EngineTorqueMultiplier = 50f;
					Function.Call((Hash)unchecked((ulong)(-4182001369491821288L)), new InputArgument[13]
					{
						Car,
						3,
						0f,
						-0.01f,
						0f,
						0f,
						0f,
						0f,
						0,
						true,
						true,
						true,
						true
					});
				}
			}
			if (DragMeets.AllowNitro && !UsedNitro && Driver.IsPlayer && Game.IsControlJustPressed(2, DragMeets.InteractKey))
			{
				UsedNitro = true;
				NitroTime = Game.GameTime + 2000;
			}
			if (NitroTime > Game.GameTime && (double)Car.Acceleration > 0.0)
			{
				Util.ForceNitro(Car);
			}
			if (!DragMeets.MT_Support && Driver.IsPlayer && DragMeets.SimulateManualTransmission && GearBoostTime < Game.GameTime)
			{
				if (ShiftBoostRef > Game.GameTime)
				{
					if (Driver.IsPlayer && DragMeets.SpectCamera == (Camera)null)
					{
						Function.Call<Vector3>((Hash)4206795403398567152L, new InputArgument[8]
						{
							0.5f,
							0.85f,
							0.2f,
							0.05f,
							0,
							255,
							0,
							255
						});
					}
					if (Game.IsControlJustPressed(0, DragMeets.ShiftingKey))
					{
						GearBoostTime = Game.GameTime + 1000;
						if (DragMeets.ShowGoodBadShifting)
						{
							UI.Notify("~b~" + Car.FriendlyName + " ~g~perfect shift to " + (GearReference + 1));
						}
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
						int num7 = Util.RandomInt(100, 500) - Car.GetMod((VehicleMod)11) * 100;
						if (num7 > 0)
						{
							Vehicle car = Car;
							car.EngineHealth -= (float)num7;
						}
						else
						{
							num7 = 0;
						}
						string text3 = (((double)Car.EngineHealth >= 200.0) ? (" - engine grind (damage:" + num7 + ")") : " - ~r~engine blowup");
						if (DragMeets.ShowGoodBadShifting)
						{
							UI.Notify("~b~" + Car.FriendlyName + " ~o~bad shift to " + (GearReference + 1) + text3);
						}
					}
				}
				else if (Game.GameTime - ShiftBoostRef > 700 && Game.IsControlJustPressed(0, DragMeets.ShiftingKey))
				{
					Util.DisplayHelpTextThisFrame("~o~Out of timeframe.");
					GearGrindTime = Game.GameTime + 1000;
					Util.PlayAmbientSpeech(Driver, Util.ChatCurse);
					GearGrindTime = Game.GameTime + 1000;
					int num8 = Util.RandomInt(100, 500) - Car.GetMod((VehicleMod)11) * 100;
					if (num8 > 0)
					{
						Vehicle car2 = Car;
						car2.EngineHealth -= (float)num8;
					}
					else
					{
						num8 = 0;
					}
					string text4 = (((double)Car.EngineHealth >= 200.0) ? (" - engine grind (damage:" + num8 + ")") : " - ~r~engine blowup");
					if (DragMeets.ShowGoodBadShifting)
					{
						UI.Notify("~b~" + Car.FriendlyName + " ~o~bad shift to " + (GearReference + 1) + text4);
					}
				}
			}
			if (DragMeets.SimulateManualTransmission && Car.CurrentGear > 1 && Car.CurrentGear > GearReference && (double)Car.EngineHealth > 200.0)
			{
				ShiftBoostRef = Game.GameTime + Util.RandomInt(400, 700);
				if (!Driver.IsPlayer)
				{
					int num9 = Shifting + Util.RandomInt(-25, 25);
					if (num9 > 75 && !DragMeets.MT_Support)
					{
						Util.PlayAmbientSpeech(Driver, Util.ChatTaunt);
						GearBoostTime = Game.GameTime + 1000;
						if (DragMeets.ShowGoodBadShifting)
						{
							UI.Notify("~b~" + Car.FriendlyName + " ~g~perfect shift to " + (GearReference + 1));
						}
					}
					if (num9 < 25)
					{
						Util.PlayAmbientSpeech(Driver, Util.ChatCurse);
						GearGrindTime = Game.GameTime + 1000;
						int num10 = Util.RandomInt(100, 500) - Car.GetMod((VehicleMod)11) * 100;
						if (num10 > 0)
						{
							Vehicle car3 = Car;
							car3.EngineHealth -= (float)num10;
						}
						else
						{
							num10 = 0;
						}
						string text5 = (((double)Car.EngineHealth >= 200.0) ? (" - engine grind (damage:" + num10 + ")") : " - ~r~engine blowup");
						if (DragMeets.ShowGoodBadShifting)
						{
							UI.Notify("~b~" + Car.FriendlyName + " ~o~bad shift to " + (GearReference + 1) + text5);
						}
					}
				}
			}
			if ((double)Car.EngineHealth <= 205.0 && (double)Car.Speed > 20.0)
			{
				Driver.DrivingSpeed = 15f;
			}
			if (GearGrindTime > Game.GameTime)
			{
				if (Driver.IsPlayer && DragMeets.SpectCamera == (Camera)null)
				{
					Function.Call<Vector3>((Hash)4206795403398567152L, new InputArgument[8]
					{
						0.5f,
						0.85f,
						0.2f,
						0.05f,
						255,
						0,
						0,
						255
					});
				}
				Car.EngineTorqueMultiplier = -0.4f;
			}
			else if (GearBoostTime > Game.GameTime && (double)Car.Acceleration > 0.0)
			{
				Car.EngineTorqueMultiplier = 2f;
				if (Driver.IsPlayer && DragMeets.SpectCamera == (Camera)null)
				{
					Function.Call<Vector3>((Hash)4206795403398567152L, new InputArgument[8]
					{
						0.5f,
						0.85f,
						0.2f,
						0.05f,
						100,
						100,
						0,
						255
					});
				}
			}
			GearReference = Car.CurrentGear;
			break;
		}
	}

	public void GetToStartingPos(bool doburnouts)
	{
		if ((double)Car.BodyHealth < 600.0)
		{
			Car.Repair();
		}
		if (Util.CanWeUse((Entity)(object)SuperEngine) && !Car.IsDoorBroken((VehicleDoor)4))
		{
			Function.Call((Hash)unchecked((ulong)(-3110590255496472013L)), new InputArgument[3]
			{
				Car,
				4,
				true
			});
		}
		if (Driver.IsPlayer)
		{
			Destination = Util.ToGround(Util.GetEntityOffset((Entity)(object)DragMeets.FlarePed, DragMeets.ForwardOffset, Offset), 5f);
		}
		else
		{
			if (Util.IsDriving(Driver))
			{
				return;
			}
			if (GotToMidPoint || DragMeets.PathToStartingLine == Vector3.Zero)
			{
				if (!DragMeets.FlarePedInPosition() || DragMeets.IsStartLineBusy)
				{
					return;
				}
				Vector3 val = Util.ToGround(Util.GetEntityOffset((Entity)(object)DragMeets.FlarePed, 0f, Offset), 5f);
				Destination = Util.ToGround(Util.GetEntityOffset((Entity)(object)DragMeets.FlarePed, 15f, Offset), 5f);
				if (!((Entity)Car).IsInRangeOf(val, 30f))
				{
					if (!Driver.IsPlayer)
					{
						float num = (float)Util.RandomInt(5, 8) * DragMeets.SpeedMultiplier;
						if (!((Entity)Car).IsInRangeOf(Destination, 60f))
						{
							num *= 1.4f;
						}
						if (!((Entity)Driver).IsInRangeOf(((Entity)Game.Player.Character).Position, 50f) && !((Entity)Driver).IsOnScreen)
						{
							Driver.SetIntoVehicle(Car, (VehicleSeat)(-1));
						}
						Driver.Task.DriveTo(Car, Destination, 5f, num, DrivingStyle);
					}
					return;
				}
				Destination = Util.ToGround(Util.GetEntityOffset((Entity)(object)DragMeets.FlarePed, DragMeets.ForwardOffset, Offset), 5f);
				if (!((Entity)Driver).IsInRangeOf(Destination, 2f) && (Picked == RacerStatus.Picked || DragMeets.MeetState == DragRaceState.RaceInProgress))
				{
					if (!Driver.IsPlayer && DragMeets.MeetState == DragRaceState.Setup)
					{
						if (Util.IsPosAheadentity((Entity)(object)Car, Destination, 0f))
						{
							Function.Call((Hash)1098373536217184078L, new InputArgument[9]
							{
								Driver,
								Car,
								Destination.X,
								Destination.Y,
								Destination.Z,
								DragMeets.DragHeading,
								1,
								15f,
								true
							});
						}
						else
						{
							Tasks task = Driver.Task;
							Vehicle car = Car;
							Ped flarePed = DragMeets.FlarePed;
							Vector3 position = ((Entity)Car).Position;
							task.DriveTo(car, Util.ToGround(Util.GetEntityOffset((Entity)(object)flarePed, position.DistanceTo2D(((Entity)DragMeets.FlarePed).Position) + 10f, Offset), 5f), 5f, (float)Util.RandomInt(2, 4) * DragMeets.SpeedMultiplier, DrivingStyle + 1024);
						}
					}
				}
				else
				{
					if (Util.IsDriving(Driver))
					{
						return;
					}
					if (Util.ThrowDice(20) && Car.GetMod((VehicleMod)38) != -1 && Car.IsOnAllWheels && Car.IsStopped)
					{
						float num2 = 0f;
						float num3 = 0f;
						int num4 = Util.RandomInt(0, 4);
						int num5 = 1;
						Model model;
						if (num4 == num5)
						{
							model = ((Entity)Car).Model;
							num2 = model.GetDimensions().X;
						}
						int num6 = 2;
						if (num4 == num6)
						{
							model = ((Entity)Car).Model;
							num2 = 0f - model.GetDimensions().X;
						}
						int num7 = 3;
						if (num4 == num7)
						{
							model = ((Entity)Car).Model;
							num3 = model.GetDimensions().Y;
						}
						int num8 = 4;
						if (num4 == num8)
						{
							model = ((Entity)Car).Model;
							num3 = 0f - model.GetDimensions().Y;
						}
						Function.Call((Hash)unchecked((ulong)(-4182001369491821288L)), new InputArgument[13]
						{
							Car,
							3,
							0f,
							0f,
							1f,
							num2,
							num3,
							0f,
							0f,
							true,
							true,
							true,
							true
						});
					}
					else if (((Car.GetMod((VehicleMod)11) > 1) & doburnouts) && WheelTemp < (float)(DragMeets.IdealWheelTemp + 20) && Util.ThrowDice(30))
					{
						if (!Driver.IsPlayer)
						{
							Util.TempAction(Driver, Car, 30, Util.RandomInt(1400, 3000));
							WheelTemp += 10f;
							_ = (double)WheelTemp + 10.0;
							_ = (double)DragMeets.IdealWheelTemp;
						}
					}
					else if (Util.ThrowDice(10) && !Driver.IsPlayer)
					{
						Util.TempAction(Driver, Car, 31, Util.RandomInt(1000, 2500));
					}
				}
			}
			else
			{
				if (Driver.IsPlayer)
				{
					return;
				}
				if (((Entity)Car).IsInRangeOf(DragMeets.PathToStartingLine, 20f))
				{
					GotToMidPoint = true;
					return;
				}
				float num9 = (float)Util.RandomInt(5, 10) * DragMeets.SpeedMultiplier;
				if (((Entity)Car).IsInRangeOf(DragMeets.PathToStartingLine, 70f))
				{
					num9 *= 2f;
				}
				Driver.Task.DriveTo(Car, DragMeets.PathToStartingLine, 10f, num9, DrivingStyle);
			}
		}
	}

	private void HandleRacerSpectating()
	{
	}

	private void HandleWaitingAreaBehavior()
	{
		//IL_00c6: Expected Obj, but got Unknown
		if (Driver.IsPlayer)
		{
			return;
		}
		if ((double)Car.BodyHealth < 1000.0)
		{
			Function.Call((Hash)unchecked((ulong)(-7692814597766773615L)), new InputArgument[1] { Car });
		}
		if (Driver.IsOnFoot && ((Entity)Car).IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange))
		{
			if (DragMeets.MeetState == DragRaceState.Idle && Driver.TaskSequenceProgress == -1 && !Function.Call<bool>((Hash)6317224474499895619L, new InputArgument[1] { Driver }) && Util.ThrowDice(20))
			{
				TaskSequence val = new TaskSequence();
				Vector3 position = ((Entity)Car).Position;
				Vector3 val2 = position.Around(3f);
				Function.Call((Hash)1572784988947265811L, new InputArgument[9]
				{
					0,
					val2.X,
					val2.Y,
					val2.Z,
					1f,
					30000,
					0f,
					0,
					0f
				});
				Function.Call((Hash)2151193443362375025L, new InputArgument[5]
				{
					0,
					((Entity)Car).Position.X,
					((Entity)Car).Position.Y,
					((Entity)Car).Position.Z,
					3000
				});
				Function.Call((Hash)1452976313881078745L, new InputArgument[4]
				{
					0,
					"WORLD_HUMAN_SMOKING",
					5000,
					true
				});
				val.Close();
				Driver.Task.PerformSequence(val);
				val.Dispose();
			}
			if (DragMeets.MeetState == DragRaceState.RaceFinished && Function.Call<bool>((Hash)6317224474499895619L, new InputArgument[1] { Driver }))
			{
				Driver.Task.ClearAll();
			}
		}
		if (((Entity)Car).IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange + 20f))
		{
			if ((double)Car.Speed > 6.0)
			{
				Driver.DrivingSpeed = 5f;
			}
		}
		else if (((Entity)Car).IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange * 10f) && (double)Car.Speed > 15.0)
		{
			Driver.DrivingSpeed = 14f;
		}
		if (((Entity)Car).IsInRangeOf(DragMeets.PathToWaitingArea, 40f) && !GotToMidPoint)
		{
			GotToMidPoint = true;
		}
		if (Util.IsDriving(Driver) || Driver.IsPlayer)
		{
			return;
		}
		if (((Entity)Car).IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange))
		{
			if (Driver.IsOnFoot || Util.IsDriving(Driver) || !Car.IsStopped)
			{
				return;
			}
			if (World.GetNearbyVehicles(((Entity)Car).Position, 7f).Length > 3)
			{
				Destination = Util.GetEntityOffset((Entity)(object)Car, (int)Math.Round((double)DragMeets.WaitingAreRange * 0.5), Util.RandomInt(-10, 10));
				Driver.Task.DriveTo(Car, Destination, 5f, 2f, DrivingStyle);
				return;
			}
			Util.SetRadioLoud(Car, DragMeets.Radio);
			if (Car.GetMod((VehicleMod)36) > -1 || Car.GetMod((VehicleMod)31) > -1)
			{
				Car.OpenDoor((VehicleDoor)5, false, false);
			}
			if (Car.GetMod((VehicleMod)39) > -1)
			{
				Car.OpenDoor((VehicleDoor)4, false, false);
			}
			Driver.Task.GoTo(Util.GetEntityOffset((Entity)(object)Car, 4f, 0f));
		}
		else
		{
			if (Util.IsDriving(Driver))
			{
				return;
			}
			int num = DrivingStyle;
			if (((Entity)Driver).IsInRangeOf(DragMeets.WaitingArea, 200f))
			{
				Destination = DragMeets.WaitingArea.Around(DragMeets.WaitingAreRange / (float)Util.RandomInt(1, 4));
				if (!Util.CarCanSeePos(Car, Destination))
				{
					num = 61;
				}
				Driver.Task.DriveTo(Car, Destination, 4f, (float)Util.RandomInt(4, 8) * DragMeets.SpeedMultiplier, num);
			}
			else
			{
				if (!Util.CarCanSeePos(Car, DragMeets.WaitingArea))
				{
					num = 61;
				}
				Driver.Task.DriveTo(Car, DragMeets.WaitingArea, DragMeets.WaitingAreRange + 10f, (float)Util.RandomInt(20, 35) * DragMeets.SpeedMultiplier, num);
			}
		}
	}

	public void HandleAI()
	{
		//IL_01a1: Invalid comparison between Unknown and I4
		//IL_01ce: Invalid comparison between Unknown and I4
		//IL_01fc: Invalid comparison between Unknown and I4
		//IL_0229: Invalid comparison between Unknown and I4
		if (Driver.IsInVehicle(Car) && (double)Car.Speed > 1.0)
		{
			if (Car.IsDoorOpen((VehicleDoor)5))
			{
				Car.CloseDoor((VehicleDoor)5, false);
			}
			if (Car.IsDoorOpen((VehicleDoor)4))
			{
				Car.CloseDoor((VehicleDoor)4, false);
			}
		}
		if (Driver.IsInCombat)
		{
			if (!Driver.Weapons.HasWeapon((WeaponHash)453432689))
			{
				Driver.Weapons.Give((WeaponHash)453432689, 500, true, true);
			}
			return;
		}
		if (Driver.IsPlayer)
		{
			if (Car.IsInBurnout())
			{
				WheelTemp += 2f;
				if ((double)WheelTemp > 70.0)
				{
					int num = Util.RandomInt(4, 5);
					if (Util.CannotWheelie.Contains(((Entity)Car).Model))
					{
						num = Util.RandomInt(0, 1);
					}
					if (!Car.IsTireBurst(num))
					{
						Car.BurstTire(num);
					}
				}
			}
			else if (WheelTemp > (float)(DragMeets.IdealWheelTemp - 15))
			{
				WheelTemp -= 0.1f;
			}
		}
		else
		{
			if (WheelTemp > (float)(DragMeets.IdealWheelTemp - 15))
			{
				WheelTemp -= 0.1f;
			}
			if (DragMeets.RacerBlips)
			{
				if (Picked == RacerStatus.NotPicked && (int)((Entity)Car).CurrentBlip.Color != 3)
				{
					((Entity)Car).CurrentBlip.Color = (BlipColor)3;
				}
				if (Picked == RacerStatus.Picked && (int)((Entity)Car).CurrentBlip.Color != 2)
				{
					((Entity)Car).CurrentBlip.Color = (BlipColor)2;
				}
				if (Picked == RacerStatus.PickedNext && (int)((Entity)Car).CurrentBlip.Color != 66)
				{
					((Entity)Car).CurrentBlip.Color = (BlipColor)66;
				}
				if (Forced && (int)((Entity)Car).CurrentBlip.Color != 1)
				{
					((Entity)Car).CurrentBlip.Color = (BlipColor)1;
				}
			}
		}
		WheelTemp = (float)Math.Round(WheelTemp, 1);
		if (StuckTimeRef < Game.GameTime)
		{
			if (Util.ThrowDice(10) && racertext.IsLoaded)
			{
				racertext.Unload();
			}
			StuckTimeRef = Game.GameTime + 5000;
			if (((Entity)Car).IsUpsideDown && Car.IsStopped)
			{
				Car.PlaceOnGround();
			}
			if (Picked != RacerStatus.Picked || DragMeets.MeetState != DragRaceState.CountDown)
			{
				StuckScore = ((Util.IsDriving(Driver) && Util.CanWeUse((Entity)(object)Car.GetPedOnSeat((VehicleSeat)(-1))) && ((Entity)Car).IsInRangeOf(StuckReference, 5f) && !Driver.IsOnFoot) ? (StuckScore + 1) : 0);
				StuckReference = ((Entity)Car).Position;
			}
		}
		if (StuckScore == 3)
		{
			Function.Call((Hash)unchecked((ulong)(-322125827305283333L)), new InputArgument[2]
			{
				Car,
				false
			});
			Driver.Task.DriveTo(Car, Util.GetEntityOffset((Entity)(object)Car, -9f, Util.RandomInt(-5, 5)), 5f, 4f, 4195388);
			StuckScore++;
			if (((Entity)Game.Player.Character).IsInRangeOf(((Entity)Car).Position, 50f) && DragMeets.DebugNotifications)
			{
				UI.Notify("~b~" + Name + "~w~: Help me I'm stuck");
			}
		}
		if (StuckScore == 10)
		{
			if (Picked == RacerStatus.Picked && DragMeets.PathToStartingLine != Vector3.Zero)
			{
				((Entity)Car).Position = DragMeets.PathToStartingLine.Around(5f);
			}
			else
			{
				((Entity)Car).Position = Util.GetEntityOffset((Entity)(object)DragMeets.FlarePed, 10f, 0f);
			}
			StuckScore = 0;
			Driver.Task.DriveTo(Car, ((Entity)Car).Position, 10f, 10f, DrivingStyle);
		}
		switch (DragMeets.MeetState)
		{
		case DragRaceState.NotStarted:
			if (((Entity)Driver).IsInRangeOf(((Entity)Game.Player.Character).Position, 50f))
			{
				if (!Driver.IsPlayer && !Util.IsDriving(Driver))
				{
					Function.Call((Hash)5188501456062479616L, new InputArgument[4]
					{
						Driver,
						Car,
						20f,
						DrivingStyle
					});
				}
			}
			else
			{
				Clear(HardDeletion: false);
			}
			break;
		case DragRaceState.Idle:
			if (!Driver.IsPlayer && Driver.Money == 0 && Picked == RacerStatus.NotPicked)
			{
				Function.Call((Hash)5188501456062479616L, new InputArgument[4]
				{
					Driver,
					Car,
					20f,
					DrivingStyle
				});
				Reason = "~r~Lost all the money";
				RemoveMe = true;
			}
			if (Picked == RacerStatus.Picked)
			{
				GetToStartingPos(doburnouts: true);
			}
			if (Picked == RacerStatus.PickedNext)
			{
				GetToStartingPos(doburnouts: false);
			}
			if (Picked == RacerStatus.NotPicked)
			{
				HandleWaitingAreaBehavior();
			}
			break;
		case DragRaceState.Setup:
			if (Picked == RacerStatus.Picked)
			{
				GetToStartingPos(doburnouts: true);
			}
			if (Picked == RacerStatus.PickedNext)
			{
				GetToStartingPos(doburnouts: false);
			}
			if (Picked == RacerStatus.NotPicked)
			{
				HandleWaitingAreaBehavior();
			}
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
				{
					Util.TempAction(Driver, Car, 31, Util.RandomInt(1000, 2500));
				}
			}
			else if (DragMeets.DebugNotifications)
			{
				UI.Notify(Car.FriendlyName + "already in burnout");
			}
			if (UsedNitro)
			{
				UsedNitro = false;
			}
			break;
		case DragRaceState.RaceInProgress:
			if (Picked == RacerStatus.NotPicked)
			{
				HandleWaitingAreaBehavior();
			}
			if (Picked != RacerStatus.Picked)
			{
				break;
			}
			if (!ParachuteWasted && !RacingToFinish && !Util.CanWeUse((Entity)(object)Parachute) && (Util.ForwardSpeed(Car) > 70f || UsesParachute))
			{
				Parachute = World.CreateProp("p_cargo_chute_s", ((Entity)Car).Position, false, false);
				((Entity)Parachute).AttachTo((Entity)(object)Car, Util.GetBoneIndex((Entity)(object)Car, "boot"), Vector3.Zero, new Vector3((float)((double)(0f - ((Entity)Car).Rotation.X) + 85.0), 0f - ((Entity)Car).Rotation.Y, 0f));
			}
			if (!Driver.IsPlayer)
			{
				if (!Util.IsDriving(Driver) && Car.IsStopped && RacingToFinish)
				{
					TaskRace();
				}
				if (DragMeets.AllowNitro && !UsedNitro && Car.CurrentGear > 2 && Util.RandomInt(0, 10) < 2)
				{
					UsedNitro = true;
					NitroTime = Game.GameTime + 2000;
				}
			}
			if (!Driver.IsPlayer && !Util.IsDriving(Driver) && !RacingToFinish && !((double)Util.ForwardSpeed(Car) <= 5.0) && Util.IsSliding(Car, 0.2f))
			{
				Util.TempAction(Driver, Car, 24, 1000);
			}
			break;
		case DragRaceState.RaceFinished:
			if (Picked == RacerStatus.NotPicked)
			{
				HandleWaitingAreaBehavior();
			}
			RacingToFinish = false;
			break;
		}
	}

	public void TaskFinish()
	{
		PerformancePoints = Util.CalculatePerformancePoints(Car);
		ParachuteWasted = false;
		RacesDone++;
		Expectations = RacerExpectations.None;
		max_willing_to_bet = 60;
		min_willing_to_bet = 20;
		Offset = 0;
		Car.EnginePowerMultiplier = 1f;
		GripBonus = 0f;
		PerformanceBonus = 0f;
		RacingToFinish = false;
		Destination = DragMeets.WaitingArea;
		GotToMidPoint = false;
		StartTime = 0;
		TrackTime = default;
		ReactTimeNotified = false;
		Penalized = false;
		Picked = RacerStatus.NotPicked;
		if (Driver.IsPlayer)
		{
			DragMeets.SavePlayerstats();
		}
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
		{
			TaskToWaitingArea();
		}
	}

	public void TaskToWaitingArea()
	{
		//IL_0021: Expected Obj, but got Unknown
		if (DragMeets.ToWaitingArea.Count <= 0 || Driver.IsPlayer)
		{
			return;
		}
		TaskSequence val = new TaskSequence();
		foreach (Vector3 item in DragMeets.ToWaitingArea)
		{
			int num = 262205;
			if (Util.CarCanSeePos(Car, item))
			{
				num += 4194304;
			}
			Function.Call((Hash)1552531582173918732L, new InputArgument[8]
			{
				0,
				Car,
				item.X,
				item.Y,
				item.Z,
				30f,
				num,
				3f
			});
		}
		val.Close();
		Driver.Task.PerformSequence(val);
		val.Dispose();
	}

	public void TaskRace()
	{
		//IL_0299: Invalid comparison between Unknown and I4
		//IL_0187: Invalid comparison between Unknown and I4
		//IL_0195: Invalid comparison between Unknown and I4
		//IL_01d5: Invalid comparison between Unknown and I4
		//IL_0547: Expected Obj, but got Unknown
		//IL_0215: Invalid comparison between Unknown and I4
		//IL_0256: Invalid comparison between Unknown and I4
		//IL_0223: Invalid comparison between Unknown and I4
		//IL_0264: Invalid comparison between Unknown and I4
		StartTime = Game.GameTime;
		StuckScore = 0;
		EngineBonusMultiplier = 1f;
		GotToMidPoint = false;
		string text = Car.FriendlyName;
		Vector3 position = ((Entity)DragMeets.FlarePed).Position;
		float num = 30f;
		float num2 = 0f;
		if (DragMeets.RealisticWheelies)
		{
			num = 70f;
		}
		if (Car.GetMod((VehicleMod)11) + Car.GetMod((VehicleMod)13) > 0)
		{
			PerformanceBonus = (Car.GetMod((VehicleMod)11) + Car.GetMod((VehicleMod)13)) * 10;
		}
		if (DragMeets.SimulateTirePerformance)
		{
			if (Function.Call<bool>((Hash)1322919935073282825L, new InputArgument[4]
			{
				position.X,
				position.Y,
				position.Z,
				DragMeets.FlarePed
			}))
			{
				if (WheelTemp > (float)DragMeets.IdealWheelTemp)
				{
					text += "~n~Tires warm, +25%";
					GripBonus += 25f;
				}
				if (Util.HasCustomTires(Car))
				{
					text += "~n~Custom Tires, +25%";
					GripBonus += 25f;
				}
				int mod = Car.GetMod((VehicleMod)15);
				if (mod > 0 && !DragMeets.RealisticWheelies)
				{
					text += "~n~Vehicle Suspension Lowered";
					num2 += (float)mod * 10f;
				}
				if (((int)Car.ClassType == 6 || (int)Car.ClassType == 1) && (int)Car.WheelType == 0 && DragMeets.RealisticWheelies)
				{
					text += "~n~Sport Tires in Sport Car, +30%";
					GripBonus += 30f;
				}
				if ((int)Car.ClassType == 5 && (int)Car.WheelType == 0 && DragMeets.RealisticWheelies)
				{
					text += "~n~Sport Tires in Classic Sports Car, +30%";
					GripBonus += 30f;
				}
				if ((int)Car.ClassType == 7 && (int)Car.WheelType == 7 && DragMeets.RealisticWheelies)
				{
					text += "~n~High End Tires in HE car, +30%";
					GripBonus += 30f;
				}
				if ((int)Car.ClassType == 4 && (int)Car.WheelType == 1 && DragMeets.RealisticWheelies)
				{
					text += "~n~Muscle Tires in Muscle car, +30%";
					GripBonus += 30f;
				}
			}
			else if ((int)Car.WheelType == 4)
			{
				text += "~n~Offroad Tyres, +1";
				GripBonus += 10f;
			}
		}
		EngineBonusMultiplier = GripBonus + PerformanceBonus;
		if ((double)EngineBonusMultiplier < 1.0)
		{
			EngineBonusMultiplier = 1f;
		}
		string text2 = text + "~n~~b~Performance:" + PerformanceBonus + "%~n~~b~Grip: " + GripBonus + "%~n~~b~PowerMultiplier: " + (float)((double)EngineBonusMultiplier + 100.0) + "%";
		Car.EnginePowerMultiplier = EngineBonusMultiplier * 0.1f + (float)CustomTurbo;
		if (DragMeets.RealisticWheelies)
		{
			if (!Util.CannotWheelie.Contains(((Entity)Car).Model) && GripBonus - num2 >= num && Car.GetMod((VehicleMod)11) > 1 && Car.GetMod((VehicleMod)13) > 1)
			{
				int num3 = (int)((double)(WheelTemp - (float)DragMeets.IdealWheelTemp) * 200.0);
				if (num3 > 0)
				{
					if (num3 < 600)
					{
						num3 = 600;
					}
					Wheelietime = Game.GameTime + num3;
				}
			}
		}
		else if (!Util.CannotWheelie.Contains(((Entity)Car).Model) && GripBonus >= num)
		{
			Wheelietime = Game.GameTime + Util.RandomInt(4, 6) * 1000;
		}
		Function.Call((Hash)unchecked((ulong)(-322125827305283333L)), new InputArgument[2]
		{
			Car,
			false
		});
		RacingToFinish = true;
		Destination = Util.ToGround(Util.GetEntityOffset((Entity)(object)DragMeets.FinishBarrel, 10f, -Offset), 10f);
		if (Destination == Vector3.Zero)
		{
			Destination = Util.GetEntityOffset((Entity)(object)DragMeets.FinishBarrel, 10f, -Offset);
		}
		if (!Driver.IsPlayer)
		{
			int num4 = 1;
			int num5 = 0;
			if (DragMeets.ReactionTimes)
			{
				num4 = 1;
				num5 = Util.RandomInt(Reaction - 100, Reaction + 100);
				if (num5 < 50)
				{
					num5 = Util.RandomInt(20, 90);
				}
				if (DragMeets.ShowReactionTimes)
				{
					text2 = text2 + "~n~~w~React.Time: ~y~" + num5;
				}
			}
			int num6 = 16777244;
			if (DragMeets.TougeMode)
			{
				num6 = 262172;
			}
			TaskSequence val = new TaskSequence();
			Function.Call((Hash)unchecked((ulong)(-4311672250463297239L)), new InputArgument[4]
			{
				0,
				Car,
				num4,
				num5
			});
			Function.Call((Hash)1552531582173918732L, new InputArgument[8]
			{
				0,
				Car,
				Destination.X,
				Destination.Y,
				Destination.Z,
				200f,
				num6,
				8f
			});
			val.Close();
			Driver.Task.PerformSequence(val);
			val.Dispose();
		}
		if (DragMeets.ShowGrip || DragMeets.ShowReactionTimes)
		{
			UI.Notify(text2);
		}
	}

	public void Clear(bool HardDeletion)
	{
		if (Util.CanWeUse((Entity)(object)SuperEngine))
		{
			((Entity)SuperEngine).Delete();
		}
		if (!Driver.IsPlayer)
		{
			if (Util.CanWeUse((Entity)(object)Car))
			{
				if (HardDeletion)
				{
					((Entity)Car).Delete();
				}
				else
				{
					((Entity)Car).MarkAsNoLongerNeeded();
				}
			}
			if (Util.CanWeUse((Entity)(object)Driver))
			{
				if (HardDeletion)
				{
					((Entity)Driver).Delete();
				}
				else
				{
					((Entity)Driver).MarkAsNoLongerNeeded();
				}
			}
		}
		else if (Util.CanWeUse((Entity)(object)Car) && ((Entity)Car).IsPersistent)
		{
			((Entity)Car).IsPersistent = false;
		}
		RemoveMe = true;
	}
}

}