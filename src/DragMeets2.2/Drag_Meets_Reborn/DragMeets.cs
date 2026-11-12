using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using GTA;
using GTA.Math;
using GTA.Native;
using Control = GTA.Control;

namespace Drag_Meets_Reborn
{
public class DragMeets : Script
{
	public static string ScriptName = "Drag Meets";

	public static string ScriptVer = "2.2";

	public static bool IsStartLineBusy = false;

	public static string RacersFile = Paths.RacersFile;

	public static string PlayerFile = Paths.PlayerFile;

	public static bool PrivateMeet = false;

	public string configpath = Paths.ConfigFile;

	public int BetTimeFrame;

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
		"u_m_y_tattoo_01"
	};

	public static bool DebugNotifications = false;

	public static float SpeedMultiplier = 1f;

	public static int Separation = 3;

	public static int DistanceMultiplier = 1;

	public static int MinRacerMoneyMuscle = 1;

	public static int MinRacerMoneySports = 1;

	public static int MinRacerMoneySuper = 1;

	public static int MinRacerMoneyOther = 1;

	public static bool RealisticWheelies = true;

	public static bool MT_Support = false;

	public static float WheelieForce = 1f;

	public static bool Guides = false;

	public static bool ActivityNotifications = false;

	public static bool FPSFriendly = false;

	public static bool RacerBlips = false;

	public static bool ManualMeetSpawn = false;

	public static bool UseDesigns = false;

	public static bool RandomTuningAll = false;

	public static bool RandomTuningGeneric = false;

	public static bool SimulateManualTransmission = false;

	public static bool SimulateTirePerformance = false;

	public static bool ShowGoodBadShifting = false;

	public static bool AllowNitro = false;

	public static bool PersistentRacers = false;

	public static bool AllowParachutes = false;

	public static bool SupercarsUseParachutes = false;

	public static bool VehicleListUpdater = false;

	public static int LamarNotificationTime = 0;

	public static List<string> LamarComments = new List<string>();

	public static bool LamarNotifications = true;


	public static Control ShiftingKey = (Control)51;

	public static Control InteractKey = (Control)51;

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

	public static int MoneyBet = 200;

	public static int Prize = 200;

	public static int ChallengePrize = 200;

	public static int Fee = 1000;

	public static Prop FinishBarrel;

	public static bool PlayerAccepted = false;

	public static int PlayerAcceptedTime = 0;

	public static bool CinematicCam = false;

	public static bool FairChallenges = false;

	public static int ForwardOffset = 6;

	public static bool SelfRadio = false;

	public static string Radio = "RADIO_07_DANCE_01";

	public static List<Vector3> MeetTriggers = new List<Vector3>();

	public static List<Blip> MeetBlips = new List<Blip>();

	public static DragRaceState MeetState = DragRaceState.NotStarted;

	public static int PickingGameRef = 0;

	public static int PerformanceTreshold = 0;

	public static int ReputationTreshold = 0;

	public static int PickSameDriversTries = 0;

	public static int SameDriverReference = 0;

	public static int RacersPerRace = 2;

	public static List<Spectator> Spectators = new List<Spectator>();

	public static List<Racer> Racers = new List<Racer>();

	public static List<Racer> Winners = new List<Racer>();

	public static Vector3 WaitingArea;

	public static float WaitingAreRange = 30f;

	public static Vector3 LeftRacerPos;

	public static Vector3 RightRacerPos;

	public static Vector3 StartingLine;

	public static Vector3 Finishline;

	public static float TougeHeading = 0f;

	public static float DragHeading = 0f;

	public static int RacerCap = 10;

	public static int GameTimeRef = 0;

	public static int GameInterval = 500;

	public static int FinishCamTime = 0;

	public static int GameTimeRefLong = 0;

	public static int GameIntervalLong = 20000;

	public static Ped FlarePed;

	public int CountDown;

	public int CoolDown;

	public int SomethingWrongTimer;

	public static int RaceStartedTime = 0;

	public int PerformanceDifferenceMin = 3;

	public static bool ReactionTimes = false;

	public static bool ShowReactionTimes = false;

	public static bool ShowGrip = false;

	private Scaleform text = new Scaleform("mp_car_stats_01");

	private bool DevChatter;

	public DragMeets()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected Obj, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		((Script)this).Tick += OnTick;
		((Script)this).KeyDown += OnKeyDown;
		((Script)this).KeyUp += OnKeyUp;
		LoadMeetTriggersFromFile();
		Game.FadeScreenIn(200);
		RacerRLGroup = World.AddRelationshipGroup("DragMeetsRacers");
		World.SetRelationshipBetweenGroups((Relationship)0, RacerRLGroup, Function.Call<int>((Hash)unchecked((ulong)(-3292914402564945716L)), new InputArgument[1] { "PLAYER" }));
		World.SetRelationshipBetweenGroups((Relationship)0, Function.Call<int>((Hash)unchecked((ulong)(-3292914402564945716L)), new InputArgument[1] { "PLAYER" }), RacerRLGroup);
		World.SetRelationshipBetweenGroups((Relationship)0, RacerRLGroup, Function.Call<int>((Hash)unchecked((ulong)(-3292914402564945716L)), new InputArgument[1] { "CIVMALE" }));
		World.SetRelationshipBetweenGroups((Relationship)0, Function.Call<int>((Hash)unchecked((ulong)(-3292914402564945716L)), new InputArgument[1] { "CIVMALE" }), RacerRLGroup);
		World.SetRelationshipBetweenGroups((Relationship)0, RacerRLGroup, Function.Call<int>((Hash)unchecked((ulong)(-3292914402564945716L)), new InputArgument[1] { "CIVFEMALE" }));
		World.SetRelationshipBetweenGroups((Relationship)0, Function.Call<int>((Hash)unchecked((ulong)(-3292914402564945716L)), new InputArgument[1] { "CIVFEMALE" }), RacerRLGroup);
		World.SetRelationshipBetweenGroups((Relationship)3, RacerRLGroup, Function.Call<int>((Hash)unchecked((ulong)(-3292914402564945716L)), new InputArgument[1] { "COP" }));
		World.SetRelationshipBetweenGroups((Relationship)3, Function.Call<int>((Hash)unchecked((ulong)(-3292914402564945716L)), new InputArgument[1] { "COP" }), RacerRLGroup);
		World.SetRelationshipBetweenGroups((Relationship)0, RacerRLGroup, RacerRLGroup);
		LoadSettings(configpath);
		Function.Call((Hash)2242675453442413023L, new InputArgument[2]
		{
			"INTRO_STREAM",
			"DIRT_RACES_SOUNDSET"
		});
		File.WriteAllText(Paths.DebugLog, string.Concat("Script started -", DateTime.Now, "-GameVer ", ((object)Game.Version/*cast due to constrained. prefix*/).ToString()));
		LamarNotificationTime = Game.GameTime + Util.RandomInt(1, 5) * 1000 * 60;
	}

	public static void SavePlayerstats()
	{
		if (PersistentRacers)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(PlayerFile);
			XmlElement documentElement = xmlDocument.DocumentElement;
			XmlNode xmlNode = documentElement.SelectSingleNode("//Wins");
			if (xmlNode == null)
			{
				XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, "Wins", null);
				xmlDocument.AppendChild(xmlNode2);
				xmlNode = xmlNode2;
			}
			xmlNode.InnerText = GetPlayerRacer().Wins.ToString();
			xmlNode = documentElement.SelectSingleNode("//Loses");
			if (xmlNode == null)
			{
				XmlNode xmlNode3 = xmlDocument.CreateNode(XmlNodeType.Element, "Loses", null);
				xmlDocument.AppendChild(xmlNode3);
				xmlNode = xmlNode3;
			}
			xmlNode.InnerText = GetPlayerRacer().Loses.ToString();
			xmlDocument.Save(PlayerFile);
		}
	}

	public static void SaveRacerstats(Racer racer)
	{
		if (!PersistentRacers)
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(RacersFile);
		XmlElement documentElement = xmlDocument.DocumentElement;
		XmlNodeList xmlNodeList = documentElement.SelectNodes("//Racers/*");
		if (xmlNodeList.Count == 0)
		{
			xmlNodeList = documentElement.SelectNodes("//Drivers/*");
		}
		if (xmlNodeList.Count == 0)
		{
			WarnPlayer(ScriptName + " " + ScriptVer, "Racers.xml LOAD ERROR", "~r~No vehicles found in Racers.xml.");
			return;
		}
		foreach (XmlElement item in xmlNodeList)
		{
			if (item.HasAttribute("Name") && item.GetAttribute("Name") == racer.Name && item.HasAttribute("Reaction"))
			{
				if (item.HasAttribute("Reaction"))
				{
					item.SetAttribute("Reaction", racer.Reaction.ToString());
				}
				if (item.HasAttribute("Shifting"))
				{
					item.SetAttribute("Shifting", racer.Shifting.ToString());
				}
				if (item.HasAttribute("RacesWon"))
				{
					item.SetAttribute("RacesWon", racer.Wins.ToString());
				}
				else
				{
					XmlAttribute node = xmlDocument.CreateAttribute("RacesWon");
					item.Attributes.Append(node);
					item.SetAttribute("RacesWon", racer.Wins.ToString());
				}
				if (item.HasAttribute("RacesLost"))
				{
					item.SetAttribute("RacesLost", racer.Loses.ToString());
				}
				else
				{
					XmlAttribute node2 = xmlDocument.CreateAttribute("RacesLost");
					item.Attributes.Append(node2);
					item.SetAttribute("RacesLost", racer.Loses.ToString());
				}
				if (item.HasAttribute("Money"))
				{
					item.SetAttribute("Money", racer.Driver.Money.ToString());
					continue;
				}
				XmlAttribute node3 = xmlDocument.CreateAttribute("Money");
				item.Attributes.Append(node3);
				item.SetAttribute("Money", racer.Driver.Money.ToString());
			}
		}
		xmlDocument.Save(RacersFile);
	}

	public static void LoadPlayerStats(Racer racer)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(PlayerFile);
		XmlElement documentElement = xmlDocument.DocumentElement;
		XmlNode xmlNode = documentElement.SelectSingleNode("//Wins");
		if (xmlNode != null)
		{
			racer.Wins = int.Parse(xmlNode.InnerText);
		}
		xmlNode = documentElement.SelectSingleNode("//Loses");
		if (xmlNode != null)
		{
			racer.Loses = int.Parse(xmlNode.InnerText);
		}
	}

	public bool IsAnyMeetLoaded()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (StartingLine != Vector3.Zero && Finishline != Vector3.Zero && WaitingArea != Vector3.Zero)
		{
			return WaitingAreRange != 0f;
		}
		return false;
	}

	public bool IsInRacerList(Vehicle veh, bool checkHash, bool checkName, int maxItems)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(RacersFile);
		XmlElement documentElement = xmlDocument.DocumentElement;
		int num2 = 0;
		foreach (XmlElement item in documentElement.SelectNodes("//Vehicle/*"))
		{
			if (checkHash)
			{
				string innerText = item.InnerText;
				Model model = ((Entity)veh).Model;
				if (innerText == model.Hash.ToString())
				{
					goto IL_00a7;
				}
			}
			if (!checkName || !(item.GetAttribute("Name") == veh.FriendlyName))
			{
				continue;
			}
			goto IL_00a7;
			IL_00a7:
			num2++;
		}
		if (num2 >= maxItems)
		{
			return true;
		}
		return false;
	}

	private void OnTick(object sender, EventArgs e)
	{
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Invalid comparison between Unknown and I4
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_1579: Unknown result type (might be due to invalid IL or missing references)
		//IL_158e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1593: Unknown result type (might be due to invalid IL or missing references)
		//IL_159d: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_14cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_150e: Unknown result type (might be due to invalid IL or missing references)
		//IL_152e: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_176f: Unknown result type (might be due to invalid IL or missing references)
		//IL_178e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1793: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1360: Unknown result type (might be due to invalid IL or missing references)
		//IL_1376: Unknown result type (might be due to invalid IL or missing references)
		//IL_137b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1380: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1816: Unknown result type (might be due to invalid IL or missing references)
		//IL_142c: Unknown result type (might be due to invalid IL or missing references)
		//IL_143b: Unknown result type (might be due to invalid IL or missing references)
		//IL_148d: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_102b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0913: Unknown result type (might be due to invalid IL or missing references)
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1caa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_18db: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_186f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1883: Unknown result type (might be due to invalid IL or missing references)
		//IL_1888: Unknown result type (might be due to invalid IL or missing references)
		//IL_189d: Unknown result type (might be due to invalid IL or missing references)
		//IL_196c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1980: Unknown result type (might be due to invalid IL or missing references)
		//IL_1985: Unknown result type (might be due to invalid IL or missing references)
		//IL_1920: Unknown result type (might be due to invalid IL or missing references)
		//IL_1934: Unknown result type (might be due to invalid IL or missing references)
		//IL_1939: Unknown result type (might be due to invalid IL or missing references)
		//IL_193d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1947: Unknown result type (might be due to invalid IL or missing references)
		//IL_194c: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c8: Unknown result type (might be due to invalid IL or missing references)
		if (LamarComments.Count > 0 && LamarNotificationTime < Game.GameTime)
		{
			if (!IsAnyMeetLoaded())
			{
				Util.AddNotification("CHAR_LAMAR", "Lamar", "Drag Meets", LamarComments[0]);
				LamarComments.RemoveAt(0);
			}
			LamarNotificationTime = Game.GameTime + Util.RandomInt(1, 5) * 1000 * 60;
		}
		if (IsAnyMeetLoaded() && Util.WasCheatStringJustEntered("invite"))
		{
			List<string> notPermitted = new List<string>();
			UI.Notify("~b~Enter the name of the Racer you want to invite.");
			SpawnDriversFromFile(notPermitted, 1, Game.GetUserInput(90));
		}
		if (Util.WasCheatStringJustEntered("save car details"))
		{
			if (CanWeUse((Entity)(object)Game.Player.Character.CurrentVehicle))
			{
				SmartVehicleAddition(Game.Player.Character, Game.Player.Character.CurrentVehicle, details: true);
			}
			else
			{
				UI.Notify("You're not in a vehicle.");
			}
		}
		if (Util.WasCheatStringJustEntered("save car"))
		{
			if (CanWeUse((Entity)(object)Game.Player.Character.CurrentVehicle))
			{
				SmartVehicleAddition(Game.Player.Character, Game.Player.Character.CurrentVehicle, details: false);
			}
			else
			{
				UI.Notify("You're not in a vehicle.");
			}
		}
		if (Util.WasCheatStringJustEntered("help"))
		{
			UI.Notify("Current commands available:");
			UI.Notify("~b~shorten track:~w~ Reduces the track length.");
			UI.Notify("~b~invite:~w~ Calls a Racer from Racers.xml.");
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
			if (int.TryParse(Game.GetUserInput(10), out var result))
			{
				DistanceMultiplier = result;
				UI.Notify("~g~New distance accepted.~n~~w~The track will be shortened in the next heat.");
			}
		}
		if (Util.WasCheatStringJustEntered("base bet"))
		{
			UI.Notify("Enter the new betting base.");
			if (int.TryParse(Game.GetUserInput(10), out var result2))
			{
				MoneyBet = result2;
				UI.Notify("~g~New base~n~~w~Any new bet will be $" + MoneyBet + ".");
			}
		}
		if (Util.WasCheatStringJustEntered("bet"))
		{
			if (GetPlayerRacer() != null && MeetState == DragRaceState.Setup)
			{
				if (GetPlayerRacer().Picked == RacerStatus.Picked)
				{
					if (!OfficialMeet && int.TryParse(Game.GetUserInput(10), out var result3))
					{
						bool flag = true;
						string text = "";
						string text2 = "";
						foreach (Racer item2 in RacersInState(RacerStatus.Picked))
						{
							if (item2.Driver.IsPlayer)
							{
								if (result3 > Util.PedMoney(item2.Driver))
								{
									flag = false;
									text2 = "You don't have that much money.";
								}
								continue;
							}
							text = item2.Name;
							if (result3 > Util.PedMoney(item2.Driver) * item2.max_willing_to_bet / 100)
							{
								flag = false;
								text2 = item2.Name + " is not willing to bet more than ~g~$" + Util.PedMoney(item2.Driver) * item2.max_willing_to_bet / 100 + "~w~.";
							}
							if (result3 < Util.PedMoney(item2.Driver) * item2.min_willing_to_bet / 100)
							{
								flag = false;
								text2 = item2.Name + " thinks you're a pussy and won't accept a bet lower than ~g~$" + Util.PedMoney(item2.Driver) * item2.min_willing_to_bet / 100 + "~w~.";
							}
						}
						if (flag)
						{
							ChallengePrize = result3;
							UI.Notify(text + " accepted the bet. ~n~The bet is now ~g~$" + ChallengePrize + "~w~.");
						}
						else
						{
							UI.Notify(("~o~" + text2) ?? "");
						}
					}
				}
				else
				{
					UI.Notify("Enter the name of the racer you want to bet ~g~$" + MoneyBet + "~w~ on.");
					string text3 = Game.GetUserInput(50).ToLowerInvariant();
					foreach (Racer item3 in RacersInState(RacerStatus.Picked))
					{
						if (text3.ToLowerInvariant() == item3.Name.ToLowerInvariant() && !item3.BetsOnHim.Contains(GetPlayerRacer()) && !GetPlayerRacer().HasBet)
						{
							if (Game.Player.Money >= MoneyBet)
							{
								item3.BetsOnHim.Add(GetPlayerRacer());
								Util.AddQueuedHelpText("Bet placed. You can hop in the " + item3.Car.FriendlyName + ".");
								UI.Notify("~b~" + GetPlayerRacer().Name + "~w~ bets on ~g~" + item3.Name);
								Util.PlayAmbientSpeech(item3.Driver, Util.ChatThanks);
								GetPlayerRacer().HasBet = true;
								Util.ChangePedMoney(GetPlayerRacer().Driver, -MoneyBet);
							}
							else
							{
								Util.AddQueuedHelpText("~o~You don't have enough money to bet.~g~$(" + MoneyBet + ")");
							}
						}
					}
				}
			}
			else
			{
				UI.Notify("~r~You cannot bet (nor change bets) now.");
			}
		}
		if (Util.WasCheatStringJustEntered("leave meet") && GetPlayerRacer() != null)
		{
			GetPlayerRacer().RemoveMe = true;
		}
		if (Util.WasCheatStringJustEntered("despawn meet") && IsAnyMeetLoaded())
		{
			UI.Notify("~g~Meet despawned.");
			DespanwnRequested = true;
		}
		Vector3 position;
		if (IsAnyMeetLoaded())
		{
			Function.Call((Hash)2619521048766764343L, new InputArgument[1] { 0f });
			Function.Call((Hash)unchecked((ulong)(-5497991812566059053L)), new InputArgument[1] { 0f });
			Function.Call((Hash)unchecked((ulong)(-1520285072926450915L)), new InputArgument[1] { 0f });
			if (!Util.CanWeUse((Entity)(object)FlarePed) || ((Entity)FlarePed).IsDead)
			{
				if (CanWeUse((Entity)(object)FlarePed))
				{
					((Entity)FlarePed).MarkAsNoLongerNeeded();
				}
				FlarePed = World.CreatePed((PedHash)2014052797, WaitingArea.Around(20f));
				FlarePed.RelationshipGroup = RacerRLGroup;
			}
			else if ((int)FlarePed.Weapons.Current.Hash == -1569615261)
			{
				FlarePed.Weapons.Give((WeaponHash)1198879012, -1, true, true);
			}
			if (Util.CanWeUse((Entity)(object)FlarePed) && ((Entity)FlarePed).IsOnScreen && Game.Player.Character.IsOnFoot && ((Entity)Game.Player.Character).IsInRangeOf(((Entity)FlarePed).Position, 3f) && ManualMeetSpawn)
			{
				DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(InteractKey) + " to despawn this meet.");
				if (Game.IsControlJustPressed(2, InteractKey))
				{
					DespanwnRequested = true;
				}
			}
			if (GetPlayerRacer() == null)
			{
				Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
				if (CanWeUse((Entity)(object)currentVehicle) && ((Entity)currentVehicle).IsInRangeOf(WaitingArea, WaitingAreRange))
				{
					if (OfficialMeet)
					{
						DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(InteractKey) + " to register.~n~The fee is $" + Fee + ", with a prize of ~g~$" + Prize + "~w~.");
					}
					else
					{
						DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(InteractKey) + " to join.~n~No fee, you'll gain or lose your money in ~g~bets~w~.");
					}
					if (Game.IsControlJustPressed(2, InteractKey))
					{
						bool flag2 = true;
						if (OfficialMeet && Game.Player.Money < Fee)
						{
							flag2 = false;
							WarnPlayer(ScriptName, "NOT ENOUGH MONEY", "You don't have enough money to pay for the fee.");
						}
						if (flag2)
						{
							if (OfficialMeet)
							{
								Util.ChangePedMoney(Game.Player.Character, -Fee);
							}
							Racers.Add(new Racer(Game.Player.Name, Game.Player.Character, currentVehicle, 0, 50, 50, notify: true, 0, 0, 0, parachute: false, PersistentRacers));
							if (VehicleListUpdater)
							{
								SmartVehicleAddition(Game.Player.Character, currentVehicle, details: false);
							}
							if (!IsInRacerList(currentVehicle, checkHash: true, checkName: true, 1))
							{
								if (DebugNotifications)
								{
									Util.AddQueuedHelpText("~g~New car detected~w~, a new racer with that car should join shortly.");
								}
								Model model = ((Entity)currentVehicle).Model;
								position = ((Entity)Game.Player.Character).Position;
								Vehicle val = World.CreateVehicle(model, position.Around(200f));
								Model val2 = RacerModel[0];
								position = ((Entity)val).Position;
								Ped val3 = World.CreatePed(val2, position.Around(3f));
								if (CanWeUse((Entity)(object)val) && CanWeUse((Entity)(object)val3))
								{
									Racers.Add(new Racer(val.FriendlyName, val3, val, 0, 150, 60, notify: true, 0, 0, 10000, parachute: false, persistent: false));
								}
							}
							if (OfficialMeet)
							{
								if (Guides)
								{
									Util.AddQueuedHelpText("You can wait in the ~y~Waiting area~w~ to be be picked for the races, or challenge other ~b~Racers~w~ yourself.");
								}
							}
							else if (Guides)
							{
								Util.AddQueuedHelpText("You can wait in the ~y~Waiting area~w~ to be challenged, or challenge other ~b~Racers~w~ yourself.");
							}
							int num = 0;
							Ped[] nearbyPeds = World.GetNearbyPeds(Game.Player.Character, 100f);
							for (int i = 0; i < nearbyPeds.Length; i++)
							{
								if (nearbyPeds[i].CurrentPedGroup == Game.Player.Character.CurrentPedGroup)
								{
									num++;
								}
							}
							if (num > 0 && Guides)
							{
								Util.AddQueuedHelpText("Partners can join the Meet if you provide them a vehicle to participate with.");
								Util.AddQueuedHelpText("To do so, simply enter a new car.");
							}
						}
					}
				}
			}
			else
			{
				Vehicle currentVehicle2 = Game.Player.Character.CurrentVehicle;
				if (CanWeUse((Entity)(object)currentVehicle2) && (Entity)(object)currentVehicle2 != (Entity)(object)GetPlayerRacer().Car)
				{
					bool flag3 = true;
					foreach (Racer racer in Racers)
					{
						if ((Entity)(object)currentVehicle2 == (Entity)(object)racer.Car)
						{
							flag3 = false;
						}
					}
					if (flag3)
					{
						Ped val4 = null;
						Ped[] nearbyPeds = World.GetNearbyPeds(Game.Player.Character, 100f);
						foreach (Ped val5 in nearbyPeds)
						{
							if (val5.CurrentPedGroup == Game.Player.Character.CurrentPedGroup)
							{
								val4 = val5;
								val5.LeaveGroup();
							}
						}
						if (!CanWeUse((Entity)(object)val4))
						{
							position = ((Entity)currentVehicle2).Position;
							val4 = World.CreateRandomPed(position.Around(4f));
						}
						Function.Call((Hash)unchecked((ulong)(-5948256542791926255L)), new InputArgument[3]
						{
							currentVehicle2,
							true,
							true
						});
						Function.Call((Hash)unchecked((ulong)(-5948256542791926255L)), new InputArgument[3]
						{
							val4,
							true,
							true
						});
						string text4 = currentVehicle2.FriendlyName;
						if (text4 == "NULL" || text4 == "")
						{
							text4 = currentVehicle2.DisplayName;
						}
						if ((RandomTuningGeneric || RandomTuningAll) && currentVehicle2.GetMod((VehicleMod)11) < 1)
						{
							Util.RandomTuning(currentVehicle2, neons: false, horn: false);
						}
						Racers.Add(new Racer(text4, val4, currentVehicle2, 0, 250, 50, notify: true, 0, 0, 500, parachute: false, persistent: false));
					}
				}
			}
		}
		int num2 = 0;
		foreach (Spectator spectator in Spectators)
		{
			num2++;
			if (spectator.Ok())
			{
				spectator.HandleAI();
				continue;
			}
			spectator.Clear(!((Entity)spectator.SpectatorPed).IsOnScreen);
			Spectators.Remove(spectator);
			break;
		}
		HandleMeetSpawnDespawn(!ManualMeetSpawn);
		if (GameTimeRefLong < Game.GameTime)
		{
			GameTimeRefLong = Game.GameTime + GameIntervalLong;
			if (Util.ThrowDice(20) && RacersNotParticipating(inWaitingArea: true).Count > 0)
			{
				Vehicle car = RacersNotParticipating(inWaitingArea: true)[Util.RandomInt(0, RacersNotParticipating(inWaitingArea: true).Count)].Car;
				List<Spectator> list = new List<Spectator>();
				for (int j = 0; j < CrowdAreas.Count * 2; j++)
				{
					Spectator item = Spectators[Util.RandomInt(0, Spectators.Count)];
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
				foreach (Spectator item4 in list)
				{
					if (item4.Idle())
					{
						item4.TaskCheckCarOut(car);
					}
				}
			}
			if (IsAnyMeetLoaded() && !PrivateMeet && Racers.Count < RacerCap && Util.ThrowDice(50))
			{
				List<string> list2 = new List<string>();
				foreach (Racer racer2 in Racers)
				{
					list2.Add(racer2.Name);
				}
				SpawnDriversFromFile(list2, 1, "");
			}
		}
		if (GameTimeRef < Game.GameTime)
		{
			GameTimeRef = Game.GameTime + GameInterval;
			foreach (Racer racer3 in Racers)
			{
				if (racer3.Ok())
				{
					racer3.HandleAI();
				}
				else
				{
					racer3.RemoveMe = true;
				}
			}
			for (int k = 0; k < Racers.Count; k++)
			{
				if (!Racers[k].RemoveMe)
				{
					continue;
				}
				if (Util.CanWeUse((Entity)(object)Racers[k].Driver) && Racers[k].Driver.IsPlayer)
				{
					Util.WarnPlayer(ScriptName, "Racer leaves", "You have left the ~y~Meet~w~.");
				}
				else if (IsAnyMeetLoaded())
				{
					string text5 = "~b~" + Racers[k].Name + "~w~ has left the  ~y~Meet.~w~";
					if (Racers[k].Reason != "")
					{
						text5 = text5 + "~n~Reason: " + Racers[k].Reason;
					}
					if (ActivityNotifications)
					{
						UI.Notify(text5);
					}
				}
				Racers[k].Clear(HardDeletion: false);
				Racers.Remove(Racers[k]);
				break;
			}
			if (CanWeUse((Entity)(object)FlarePed))
			{
				HandleFlarePedAI();
			}
			if (CanWeUse((Entity)(object)FinishBarrel))
			{
				if (!((Entity)FinishBarrel).IsInRangeOf(Finishline, 2f))
				{
					((Entity)FinishBarrel).Position = Finishline;
					Function.Call((Hash)6388445041372756643L, new InputArgument[1] { FinishBarrel });
					((Entity)FinishBarrel).Velocity = Vector3.Zero;
				}
				if (((Entity)FinishBarrel).Heading != TougeHeading)
				{
					((Entity)FinishBarrel).Heading = TougeHeading;
				}
			}
			if (IsAnyMeetLoaded())
			{
				HandleMeet();
				if (((Entity)Game.Player.Character).IsInRangeOf(WaitingArea, WaitingAreRange + 30f))
				{
					if (OfficialMeet && !OfficialIntroduction)
					{
						if (Guides)
						{
							Util.AddQueuedHelpText("This is an ~y~Official Drag Meet~w~.");
							Util.AddQueuedHelpText("Here, the Organizer chooses which ~b~Racers~w~ will race in the next heat.");
							Util.AddQueuedHelpText("Registering has a ~g~$" + Fee + "~w~ fee.");
						}
						OfficialIntroduction = true;
					}
					if (!OfficialMeet && !StreetIntroduction)
					{
						if (Guides)
						{
							Util.AddQueuedHelpText("This is a ~y~Street Drag Meet~w~.");
							Util.AddQueuedHelpText("Here ~b~Racers~w~ challenge each other to race for money.");
							Util.AddQueuedHelpText("Currently, the money bet is calculated from the money each ~b~Racer~w~ has.");
						}
						StreetIntroduction = true;
					}
					Vehicle currentVehicle3 = Game.Player.Character.CurrentVehicle;
					if (CanWeUse((Entity)(object)currentVehicle3) && !WarnedOfPerformance)
					{
						float num3 = 0f;
						foreach (Racer racer4 in Racers)
						{
							num3 += (float)Math.Round(racer4.PerformancePoints, 2);
						}
						num3 /= (float)Racers.Count;
						float num4 = Util.CalculatePerformancePoints(currentVehicle3);
						if (num4 > num3 + 5f)
						{
							if (Guides)
							{
								Util.AddQueuedHelpText("~y~Your current car outperforms most cars in this Meet.");
							}
							if (DebugNotifications)
							{
								Util.AddQueuedHelpText("Average: " + num3 + ", yours: " + num4);
							}
						}
						if (num4 + 5f < num3)
						{
							if (Guides)
							{
								Util.AddQueuedHelpText("~y~Most of the cars in this Meet outperform yours.");
							}
							if (DebugNotifications)
							{
								Util.AddQueuedHelpText("Average: " + num3 + ", yours: " + num4);
							}
						}
						WarnedOfPerformance = true;
					}
				}
			}
		}
		if (MeetState == DragRaceState.CountDown && (FlarePed.IsShooting || Util.IsSubttaskActive(FlarePed, (Subtask)126)))
		{
			foreach (Racer item5 in RacersParticipating())
			{
				item5.TaskRace();
			}
			FlarePed.Task.Cower(3000);
			MeetState = DragRaceState.RaceInProgress;
			Function.Call((Hash)5238680789324452053L, new InputArgument[3]
			{
				0,
				500,
				200
			});
			SomethingWrongTimer = Game.GameTime + (int)(StartingLine.DistanceTo(Finishline) * 100f);
			RaceStartedTime = Game.GameTime;
			if (SpectCamera != (Camera)null && World.RenderingCamera == SpectCamera)
			{
				int num5 = RacersInState(RacerStatus.Picked).Count - 1;
				Function.Call((Hash)1841822097142223645L, new InputArgument[1] { RacersInState(RacerStatus.Picked)[0].Car });
				Vector3 val6 = Util.GetEntityOffset((Entity)(object)RacersInState(RacerStatus.Picked)[1].Car, 0f, 5f) - ((Entity)RacersInState(RacerStatus.Picked)[1].Car).Position;
				if (Util.IsPosRightOf((Entity)(object)RacersInState(RacerStatus.Picked)[0].Car, ((Entity)RacersInState(RacerStatus.Picked)[1].Car).Position, 0f))
				{
					val6 = Util.GetEntityOffset((Entity)(object)RacersInState(RacerStatus.Picked)[1].Car, 0f, -5f) - ((Entity)RacersInState(RacerStatus.Picked)[1].Car).Position;
				}
				Function.Call((Hash)unchecked((ulong)(-82334563353992989L)), new InputArgument[6]
				{
					SpectCamera,
					RacersInState(RacerStatus.Picked)[num5 - 1].Car,
					val6.X,
					val6.Y,
					3f,
					false
				});
				SpectCamera.PointAt(RacersInState(RacerStatus.Picked)[num5].Driver, 0, ((Entity)RacersInState(RacerStatus.Picked)[num5].Car).UpVector);
			}
		}
		if (!Game.Player.Character.IsOnFoot && ((Entity)Game.Player.Character).IsInRangeOf(WaitingArea, WaitingAreRange * 2f))
		{
			World.DrawMarker((MarkerType)1, WaitingArea + new Vector3(0f, 0f, -0.5f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(WaitingAreRange * 2f, WaitingAreRange * 2f, 1.5f), Color.Yellow);
		}
		if (MeetState == DragRaceState.RaceInProgress)
		{
			Function.Call((Hash)2999307995311693915L, new InputArgument[1] { DragHeading });
			Vector3 val7 = (((Entity)RacersParticipating()[0].Car).Position + ((Entity)RacersParticipating()[1].Car).Position) / 2f;
			Function.Call((Hash)1331350670911596351L, new InputArgument[2]
			{
				val7.X,
				val7.Y
			});
			if (SomethingWrongTimer < Game.GameTime && RacersParticipating().Count != Winners.Count)
			{
				if (Winners.Count > 0)
				{
					WarnPlayer(ScriptName + " " + ScriptVer, "Some racers DNF", "Some racers are considered DNF.~n~ Race finished.");
					foreach (Racer item6 in RacersParticipating())
					{
						if (!Winners.Contains(item6))
						{
							Winners.Add(item6);
						}
					}
				}
				else
				{
					WarnPlayer(ScriptName + " " + ScriptVer, "RACE DIDN'T FINISH PROPERLY", ScriptName + " will skip this race.");
					foreach (Racer item7 in RacersParticipating())
					{
						Ped driver = item7.Driver;
						driver.Money += Prize;
						item7.TaskFinish();
					}
					Winners.Clear();
					MeetState = DragRaceState.Idle;
				}
			}
			if (FinishCamTime != 0)
			{
				if (FinishCamTime < Game.GameTime)
				{
					Game.TimeScale = 1f;
					FinishCamTime = 0;
					if (SpectCamera != (Camera)null)
					{
						SpectCamera.Destroy();
						SpectCamera = null;
						World.RenderingCamera = null;
						Function.Call((Hash)3582399230505917858L, new InputArgument[0]);
					}
				}
				Util.DrawLine(Util.GetEntityOffset((Entity)(object)FinishBarrel, 0f, 10f) - new Vector3(0f, 0f, ((Entity)FinishBarrel).HeightAboveGround * 0.9f), Util.GetEntityOffset((Entity)(object)FinishBarrel, 0f, -10f) - new Vector3(0f, 0f, ((Entity)FinishBarrel).HeightAboveGround * 0.9f));
			}
			foreach (Racer item8 in RacersParticipating())
			{
				if (((Entity)item8.Car).IsInRangeOf(Finishline, 30f))
				{
					if (Util.IsPosAheadentity((Entity)(object)FinishBarrel, ((Entity)item8.Car).Position, -10f) && FinishCamTime == 0 && Winners.Count == 0 && SpectCamera != (Camera)null)
					{
						if (SpectCamera == (Camera)null)
						{
							SpectCamera = World.CreateCamera(Util.GetEntityOffset((Entity)(object)FinishBarrel, 20f, 15f) + new Vector3(0f, 0f, 3f), new Vector3(0f, 0f, 0f - DragHeading), GameplayCamera.FieldOfView - 30f);
						}
						float timeScale = 1f;
						int num6 = 5000;
						SpectCamera.Detach();
						position = ((Entity)item8.Car).Position;
						float num7 = position.DistanceTo(((Entity)FinishBarrel).Position);
						if (World.GetNearbyVehicles(((Entity)FinishBarrel).Position, num7 + 5f).Length < 2)
						{
							Camera spectCamera = SpectCamera;
							Vector3 entityOffset = Util.GetEntityOffset((Entity)(object)FinishBarrel, -2f, 0f);
							Model model2 = ((Entity)FinishBarrel).Model;
							spectCamera.Position = entityOffset + new Vector3(0f, 0f, model2.GetDimensions().Z);
						}
						else
						{
							SpectCamera.Position = Util.GetEntityOffset((Entity)(object)FinishBarrel, 5f, 15f) + new Vector3(0f, 0f, 1f);
							Camera spectCamera2 = SpectCamera;
							spectCamera2.FieldOfView -= GameplayCamera.FieldOfView - 40f;
							timeScale = 0.03f;
							num6 = 400;
						}
						SpectCamera.PointAt((Entity)(object)FinishBarrel, ((Entity)FinishBarrel).UpVector);
						World.RenderingCamera = SpectCamera;
						Game.TimeScale = timeScale;
						FinishCamTime = Game.GameTime + num6;
					}
					if (Util.IsPosAheadentity((Entity)(object)FinishBarrel, ((Entity)item8.Car).Position, 0f) && !Winners.Contains(item8))
					{
						if (SpectCamera != (Camera)null)
						{
							SpectCamera.Shake((CameraShake)1, 0.04f);
						}
						item8.TrackTime = item8.TrackTime.Add(new TimeSpan(0, 0, 0, 0, Game.GameTime - item8.StartTime));
						SomethingWrongTimer += 5000;
						Winners.Add(item8);
						item8.RacingToFinish = false;
						if (!item8.Driver.IsPlayer)
						{
							if (TougeMode)
							{
								Function.Call((Hash)5188501456062479616L, new InputArgument[4]
								{
									item8.Driver,
									item8.Car,
									1f,
									28
								});
							}
							else
							{
								Util.TempAction(item8.Driver, item8.Car, 24, 1000);
							}
						}
						UI.Notify("~b~" + item8.Name + "~w~ - ~g~" + item8.TrackTime);
					}
				}
				if (((Entity)item8.Car).IsInRangeOf(item8.Destination, 7f))
				{
					Winners.Contains(item8);
				}
			}
		}
		if (MeetState == DragRaceState.CountDown && (CountDown - Game.GameTime) / 1000 + 1 < 3)
		{
			foreach (Racer item9 in RacersParticipating())
			{
				if (Util.ForwardSpeed(item9.Car) > 1f && !item9.Penalized)
				{
					UI.Notify(item9.Name + "~o~ penalized");
					item9.Penalized = true;
				}
			}
		}
		foreach (Racer racer5 in Racers)
		{
			racer5.OnTick();
		}
		if (DebugNotifications)
		{
			Util.DrawLine(StartingLine, Finishline);
			if (PathToWaitingArea != Vector3.Zero)
			{
				Util.DrawLine(Finishline, PathToWaitingArea);
			}
			if (PathToWaitingArea != Vector3.Zero)
			{
				Util.DrawLine(PathToWaitingArea, WaitingArea);
			}
			if (PathToStartingLine != Vector3.Zero)
			{
				Util.DrawLine(WaitingArea, PathToStartingLine);
			}
			Util.DrawLine(PathToStartingLine, StartingLine);
		}
		Util.HandleMessages();
		Util.HandleNotifications();
	}

	public void SmartVehicleAddition(Ped ped, Vehicle veh, bool details)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		Model val = ((Entity)ped).Model;
		if (new List<Model>
		{
			(PedHash)225514697,
			unchecked((PedHash)(-1692214353)),
			unchecked((PedHash)(-1686040670)),
			(PedHash)1885233650,
			unchecked((PedHash)(-1667301416))
		}.Contains(val))
		{
			val = (string)null;
		}
		if (IsInRacerList(veh, checkHash: true, checkName: true, 2) && !details)
		{
			if (DebugNotifications)
			{
				WarnPlayer(ScriptName, "Vehicle already saved", "Racers.xml alredy contains 2 ~b~" + veh.FriendlyName + "s~w~.");
			}
			return;
		}
		string text = veh.FriendlyName;
		if (text == "NULL")
		{
			text = veh.DisplayName;
		}
		string text2 = "Average";
		if (KindsAllowed.Count > 0)
		{
			text2 = KindsAllowed[0];
		}
		int result = 50;
		int result2 = 250;
		if (details)
		{
			UI.Notify("~b~[Oponent Creator]~w~: Set the Racer's display name.");
			text = Game.GetUserInput(veh.FriendlyName, 20);
			UI.Notify("~b~[Oponent Creator]~w~: Set the Racer's kind.");
			text2 = Game.GetUserInput(text2, 20);
			UI.Notify("~b~[Oponent Creator]~w~: Set the Racer's Shifting ability (0 - 100). Its measured as a percentage.");
			int.TryParse(Game.GetUserInput(50.ToString(), 20), out result);
			UI.Notify("~b~[Oponent Creator]~w~: Set the Racer's Reaction time (0 - 1000). Its measured in miliseconds.");
			int.TryParse(Game.GetUserInput(250.ToString(), 20), out result2);
		}
		if (details)
		{
			if (val == (string)null)
			{
				Util.SaveDriverToFile(text, null, veh, RacersFile, SavePerfMods: true, SaveVisualMods: true, text2, result, result2);
				WarnPlayer(ScriptName, "New vehicle saved", "~b~" + veh.FriendlyName + "~w~ (with tuning) added to Racers.xml.");
			}
			else
			{
				Util.SaveDriverToFile(text, Game.Player.Character, veh, RacersFile, SavePerfMods: true, SaveVisualMods: true, text2, result, result2);
				WarnPlayer(ScriptName, "New vehicle saved", "~b~Ped model~w~ and ~b~" + veh.FriendlyName + "~w~ (with tuning) added to Racers.xml.");
			}
		}
		else
		{
			Util.SaveDriverToFile(text, null, veh, RacersFile, SavePerfMods: false, SaveVisualMods: false, text2, result, result2);
			WarnPlayer(ScriptName, "New vehicle saved", "~b~" + veh.FriendlyName + "~w~ (generic) saved to Racers.xml.");
		}
	}

	public void LoadMeetTriggersFromFile()
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		XmlDocument xmlDocument = new XmlDocument();
		int num = 0;
		string[] files = Directory.GetFiles(Paths.MeetsDir);
		foreach (string text in files)
		{
			if (!File.Exists(text))
			{
				continue;
			}
			num++;
			xmlDocument.Load(text);
			if (xmlDocument == null)
			{
				continue;
			}
			XmlNode xmlNode = xmlDocument.SelectSingleNode("//Trigger");
			if (xmlNode == null)
			{
				xmlNode = xmlDocument.SelectSingleNode("//WaitingZone");
			}
			if (xmlNode == null)
			{
				UI.Notify("Failed to load " + text);
				continue;
			}
			Vector3 val = XmlRead.TruncatedVector(xmlNode);
			MeetTriggers.Add(val);
			Blip val2 = World.CreateBlip(val);
			val2.Sprite = (BlipSprite)108;
			if (xmlDocument.SelectSingleNode("//Touge") == null)
			{
				val2.Color = (BlipColor)2;
				val2.Name = "Drag Meet";
			}
			else
			{
				val2.Color = (BlipColor)66;
				val2.Name = "Street Race Meet";
			}
			List<string> list = new List<string>();
			list.Add("Yo " + Game.Player.Name + " there's a great car meet here at ~y~" + World.GetStreetName(val) + "~w~, come say hi");
			list.Add("Money passing hands near ~y~" + World.GetZoneName(val) + "~w~, FYI");
			list.Add("Looks like there's a Drag Meet at ~y~" + World.GetStreetName(val) + "~w~ with bettings and shit. You going?~n~Easy money");
			list.Add("Wassup? Some cool rides at ~y~" + World.GetStreetName(val) + "~w~. ~n~ You goin? I might");
			list.Add("Really good ambient here in ~y~" + World.GetStreetName(val) + "~w~. Give us a visit if you have time");
			List<string> list2 = list;
			if (LamarNotifications)
			{
				LamarComments.Add(list2[Util.RandomInt(0, list2.Count - 1)]);
			}
			val2.IsShortRange = true;
			MeetBlips.Add(val2);
		}
		if (DebugNotifications)
		{
			UI.Notify(num + " meets loaded.");
		}
	}

	public void HandleMeetSpawnDespawn(bool immersive)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0997: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f66: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val;
		if (IsAnyMeetLoaded())
		{
			float num = StartingLine.DistanceTo(Finishline) * 1.5f;
			if (num < 900f)
			{
				num = 900f;
			}
			val = ((Entity)Game.Player.Character).Position;
			if (!(val.DistanceTo(WaitingArea) > num) && !DespanwnRequested)
			{
				return;
			}
			foreach (Blip meetBlip in MeetBlips)
			{
				meetBlip.Alpha = 255;
			}
			ClearMeet(DeleteEverything: true);
		}
		else
		{
			if (MeetTriggers.Count <= 0)
			{
				return;
			}
			foreach (Vector3 meetTrigger in MeetTriggers)
			{
				bool flag = false;
				if (immersive)
				{
					if (((Entity)Game.Player.Character).IsInRangeOf(meetTrigger, 300f))
					{
						flag = true;
					}
				}
				else if (((Entity)Game.Player.Character).IsInRangeOf(meetTrigger, 40f))
				{
					val = ((Entity)Game.Player.Character).Velocity;
					if (val.Length() < 1f)
					{
						DisplayHelpTextThisFrame("Press " + Util.GetInstructionalButton(InteractKey) + " to spawn this meet.");
						if (Game.IsControlJustPressed(2, InteractKey))
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					continue;
				}
				if (IsAnyMeetLoaded())
				{
					// 2.2 spawns inside this loop without re-checking, so a second meet whose
					// WaitingZone sits inside the same 300 m radius starts on top of the first
					// (MountainBesidesTrevorsAirfield and the Sandy Shores Airfield are 297.8 m apart).
					break;
				}
				File.AppendAllText(Paths.DebugLog, "\n Spawning new meet, looking for meet file...");
				LoadSettings(configpath);
				XmlDocument xmlDocument = new XmlDocument();
				int num2 = 0;
				string[] files = Directory.GetFiles(Paths.MeetsDir);
				foreach (string text in files)
				{
					if (!File.Exists(text))
					{
						continue;
					}
					num2++;
					xmlDocument.Load(text);
					if (xmlDocument == null)
					{
						File.AppendAllText(Paths.DebugLog, " - ERROR, no meet file found.");
						UI.Notify("~r~Failed to load " + text);
						continue;
					}
					XmlNode xmlNode = xmlDocument.SelectSingleNode("//Trigger");
					if (xmlNode == null)
					{
						File.AppendAllText(Paths.DebugLog, "\nNO MEET TRIGGER FOUND, looking for WaitingZone");
						xmlNode = xmlDocument.SelectSingleNode("//WaitingZone");
					}
					if (xmlNode == null)
					{
						UI.Notify("~r~Failed to load " + text);
						continue;
					}
					File.AppendAllText(Paths.DebugLog, " - WaitingZone found");
					Vector3 val2 = XmlRead.Vector(xmlNode);
					if (!(meetTrigger == val2))
					{
						continue;
					}
					File.AppendAllText(Paths.DebugLog, "\nMatching MeetFile found. Loading " + text + "...");
					if (!immersive)
					{
						Game.FadeScreenOut(500);
					}
					XmlNode xmlNode2 = xmlDocument.SelectSingleNode("//StartingLine");
					StartingLine = XmlRead.Vector(xmlNode2);
					string text2 = "";
					xmlNode2 = xmlDocument.SelectSingleNode("//StartingLine/Prop");
					if (xmlNode2 != null)
					{
						text2 = xmlNode2.InnerText;
					}
					else if (DebugNotifications)
					{
						UI.Notify("~r~StartingLine/Prop not found");
					}
					xmlNode2 = xmlDocument.SelectSingleNode("//FinishLine");
					Finishline = XmlRead.Vector(xmlNode2);
					xmlNode2 = xmlDocument.SelectSingleNode("//WaitingZone");
					WaitingArea = XmlRead.Vector(xmlNode2);
					string text3 = "";
					xmlNode2 = xmlDocument.SelectSingleNode("//WaitingZone/Prop");
					if (xmlNode2 != null)
					{
						text3 = xmlNode2.InnerText;
					}
					else if (DebugNotifications)
					{
						UI.Notify("~r~WaitingZone/Prop not found");
					}
					WaitingAreRange = float.Parse(xmlDocument.SelectSingleNode("//WaitingZone/Radius").InnerText, CultureInfo.InvariantCulture);
					xmlNode2 = xmlDocument.SelectSingleNode("//Heading");
					if (xmlNode2 != null)
					{
						DragHeading = int.Parse(xmlNode2.InnerText, CultureInfo.InvariantCulture);
					}
					else
					{
						Vector2 val3 = new Vector2(Finishline.X - StartingLine.X, Finishline.Y - StartingLine.Y);
						DragHeading = Function.Call<float>((Hash)3457474934040373542L, new InputArgument[2]
						{
							val3.X,
							val3.Y
						});
					}
					xmlNode2 = xmlDocument.SelectSingleNode("//TougeHeading");
					if (xmlNode2 != null)
					{
						TougeHeading = int.Parse(xmlNode2.InnerText, CultureInfo.InvariantCulture);
					}
					else
					{
						TougeHeading = DragHeading;
					}
					xmlNode2 = xmlDocument.SelectSingleNode("//SeparationBetweenCars");
					if (xmlNode2 != null)
					{
						Separation = int.Parse(xmlNode2.InnerText, CultureInfo.InvariantCulture);
					}
					else if (DebugNotifications)
					{
						UI.Notify("~r~SeparationBetweenCars not found");
					}
					xmlNode2 = xmlDocument.SelectSingleNode("//MaxCarLimit");
					if (xmlNode2 != null)
					{
						RacerCap = int.Parse(xmlNode2.InnerText, CultureInfo.InvariantCulture);
					}
					else if (DebugNotifications)
					{
						UI.Notify("~r~MaxCarLimit not found");
					}
					if (FPSFriendly && RacerCap > 12)
					{
						RacerCap = 12;
					}
					xmlNode2 = xmlDocument.SelectSingleNode("//PathToStartingLine");
					Vector3 pathToStartingLine = Vector3.Zero;
					if (xmlNode2 != null)
					{
						pathToStartingLine = XmlRead.TruncatedVector(xmlNode2);
					}
					else if (DebugNotifications)
					{
						UI.Notify("~r~PathToStartingLine not found");
					}
					PathToStartingLine = pathToStartingLine;
					foreach (XmlElement item3 in xmlDocument.SelectNodes("//Radios/*"))
					{
						Radio = item3.InnerText;
					}
					if (SelfRadio)
					{
						Radio = "RADIO_19_USER";
					}
					foreach (XmlElement item4 in xmlDocument.SelectNodes("//PathToWaitingArea/*"))
					{
						Vector3 item = XmlRead.TruncatedVector(item4);
						ToWaitingArea.Add(item);
					}
					xmlNode2 = xmlDocument.SelectSingleNode("//CarsPerRace");
					if (xmlNode2 != null)
					{
						RacersPerRace = int.Parse(xmlNode2.InnerText, CultureInfo.InvariantCulture);
					}
					else if (DebugNotifications)
					{
						UI.Notify("~r~CarsPerRace not found");
					}
					xmlNode2 = xmlDocument.SelectSingleNode("//Bets");
					if (xmlNode2 != null)
					{
						MoneyBet = int.Parse(xmlNode2.InnerText, CultureInfo.InvariantCulture);
					}
					else if (DebugNotifications)
					{
						UI.Notify("~r~Bets not found");
					}
					xmlNode2 = xmlDocument.SelectSingleNode("//Fee");
					if (xmlNode2 != null)
					{
						Fee = int.Parse(xmlNode2.InnerText, CultureInfo.InvariantCulture);
					}
					else if (DebugNotifications)
					{
						UI.Notify("~r~Fee not found");
					}
					xmlNode2 = xmlDocument.SelectSingleNode("//Prize");
					if (xmlNode2 != null)
					{
						Prize = int.Parse(xmlNode2.InnerText, CultureInfo.InvariantCulture);
					}
					else if (DebugNotifications)
					{
						UI.Notify("~r~Prize not found");
					}
					foreach (Blip meetBlip2 in MeetBlips)
					{
						meetBlip2.Alpha = 0;
					}
					foreach (XmlNode item5 in xmlDocument.SelectNodes("//CarsAllowed/*"))
					{
						KindsAllowed.Add(item5.InnerText);
						if (item5.InnerText.Contains("Official"))
						{
							OfficialMeet = true;
						}
					}
					xmlNode2 = xmlDocument.SelectSingleNode("//Touge");
					if (xmlNode2 != null)
					{
						TougeMode = true;
					}
					if (!FPSFriendly)
					{
						foreach (XmlElement item6 in xmlDocument.SelectNodes("//CrowdAreas/*"))
						{
							Vector3 item2 = XmlRead.TruncatedVector(item6);
							CrowdAreas.Add(item2);
						}
					}
					File.AppendAllText(Paths.DebugLog, "\nAll meet info loaded.");
					Vehicle[] nearbyVehicles = World.GetNearbyVehicles(((Entity)Game.Player.Character).Position, 40f);
					foreach (Vehicle val4 in nearbyVehicles)
					{
						if (val4.IsStopped && !Game.Player.Character.IsInVehicle(val4) && (Util.NeverBeenDriven(val4) || Util.LastDriverIsPed(val4, Game.Player.Character)))
						{
							val = ((Entity)val4).Position;
							Ped val5 = World.CreateRandomPed(val.Around(4f));
							Function.Call((Hash)unchecked((ulong)(-5948256542791926255L)), new InputArgument[3]
							{
								val4,
								true,
								true
							});
							Function.Call((Hash)unchecked((ulong)(-5948256542791926255L)), new InputArgument[3]
							{
								val5,
								true,
								true
							});
							string text4 = val4.FriendlyName;
							if (text4 == "NULL" || text4 == "")
							{
								text4 = val4.DisplayName;
							}
							if ((RandomTuningGeneric || RandomTuningAll) && val4.GetMod((VehicleMod)11) < 1)
							{
								Util.RandomTuning(val4, neons: false, horn: false);
							}
							Racers.Add(new Racer(text4, val5, val4, 0, 250, 50, notify: false, 0, 0, 500, parachute: false, persistent: false));
						}
					}
					if (Racers.Count < 2)
					{
						List<string> notPermitted = new List<string>();
						SpawnDriversFromFile(notPermitted, (int)Math.Round((float)RacerCap * 0.7f), "");
					}
					else
					{
						Util.AddQueuedHelpText("This is a ~b~Private Meet~w~. No Racers will join this meet unless they're added by you.");
						PrivateMeet = true;
					}
					File.AppendAllText(Paths.DebugLog, "\nAll cars spawned.");
					File.AppendAllText(Paths.DebugLog, "\nSetting up CrowdAreas.");
					if (!FPSFriendly)
					{
						if (CrowdAreas.Count > 0)
						{
							foreach (Vector3 crowdArea in CrowdAreas)
							{
								Vector3 current2 = crowdArea;
								for (int k = 0; (float)k < WaitingAreRange / 4f; k++)
								{
									Spectators.Add(new Spectator(World.CreateRandomPed(current2.Around(5f)), null));
								}
							}
						}
						else
						{
							for (int l = 0; l < 6; l++)
							{
								Spectators.Add(new Spectator(World.CreateRandomPed(WaitingArea.Around(5f)), null));
							}
						}
					}
					MeetState = DragRaceState.Idle;
					File.AppendAllText(Paths.DebugLog, "\nSpawning FinishBarrel...");
					FinishBarrel = World.CreateProp("prop_offroad_barrel01", Finishline, new Vector3(0f, 0f, TougeHeading), false, true);
					((Entity)FinishBarrel).IsPersistent = true;
					Function.Call((Hash)992985146056095358L, new InputArgument[2]
					{
						FinishBarrel,
						true
					});
					((Entity)FinishBarrel).ApplyForce(new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, 0f));
					Function.Call((Hash)6388445041372756643L, new InputArgument[1] { FinishBarrel });
					File.AppendAllText(Paths.DebugLog, "\nSpawning FlarePed...");
					if (!Util.CanWeUse((Entity)(object)FlarePed))
					{
						FlarePed = World.CreatePed((PedHash)2014052797, StartingLine);
					}
					Script.Wait(1000);
					int num3 = 0;
					while (Util.CanWeUse((Entity)(object)FlarePed) && num3 < 100)
					{
						if (!Util.CanWeUse((Entity)(object)FlarePed))
						{
							FlarePed = World.CreatePed((PedHash)2014052797, StartingLine);
						}
						num3++;
					}
					if (num3 == 100)
					{
						File.AppendAllText(Paths.DebugLog, "\nSpawning FlarePed failed! Random ped model selected.");
						num3 = 0;
						while (Util.CanWeUse((Entity)(object)FlarePed) && num3 < 100)
						{
							if (!Util.CanWeUse((Entity)(object)FlarePed))
							{
								FlarePed = World.CreateRandomPed(StartingLine);
							}
							num3++;
						}
					}
					if (!Util.CanWeUse((Entity)(object)FlarePed))
					{
						File.AppendAllText(Paths.DebugLog, "\nSpawning alternative FlarePed ALSO failed! Michael ped model selected.");
						FlarePed = World.CreatePed((PedHash)225514697, StartingLine);
					}
					if (!Util.CanWeUse((Entity)(object)FlarePed))
					{
						File.AppendAllText(Paths.DebugLog, "\nSpawning alternative FlarePed ALSO failed! Trevor ped model selected.");
						FlarePed = World.CreatePed(unchecked((PedHash)(-1686040670)), StartingLine);
					}
					((Entity)FlarePed).Heading = DragHeading + 180f;
					if (!immersive)
					{
						Game.FadeScreenIn(2000);
					}
					if (DebugNotifications)
					{
						UI.Notify("~g~" + text + " loaded.");
					}
					if (text2 != "")
					{
						File.AppendAllText(Paths.DebugLog, "\nSpawning StartingLineProp...");
						Prop val6 = World.CreateProp(text2, Util.GetEntityOffset((Entity)(object)FlarePed, -3f, 0f), false, true);
						if (CanWeUse((Entity)(object)val6))
						{
							if (((Entity)val6).Model == "stt_prop_race_gantry_01")
							{
								((Entity)val6).Heading = DragHeading + 90f;
							}
							else
							{
								((Entity)val6).Heading = DragHeading;
							}
							if (((Entity)val6).Model == "prop_start_gate_01")
							{
								Prop val7 = val6;
								((Entity)val7).Position = ((Entity)val7).Position + new Vector3(0f, 0f, -1f);
							}
							MeetProps.Add(val6);
						}
						else
						{
							File.AppendAllText(Paths.DebugLog, "\nWARNING, failed to spawn the props for the starting line.");
						}
					}
					if (text3 != "")
					{
						File.AppendAllText(Paths.DebugLog, "\nSpawning WaitingZoneProp...");
						Prop val8 = World.CreateProp(text3, WaitingArea, false, true);
						if (CanWeUse((Entity)(object)val8))
						{
							((Entity)val8).Heading = DragHeading + 90f;
							MeetProps.Add(val8);
						}
						else
						{
							File.AppendAllText(Paths.DebugLog, "\nWARNING, failed to spawn the props for the waiting zone.");
						}
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
					File.AppendAllText(Paths.DebugLog, " - Filling with sleepers...");
					FillWithSleepers();
					File.AppendAllText(Paths.DebugLog, " - Meet loaded. Have fun.");
					break;
				}
				break;
			}
		}
	}

	public void PickForRace(Racer racer)
	{
		racer.Car.Repair();
		racer.Picked = RacerStatus.Picked;
		racer.Offset = 1;
		racer.GotToMidPoint = false;
		racer.Forced = false;
		if (SameDriverReference == 0)
		{
			if (DebugNotifications)
			{
				UI.Notify("Got Performance sample: " + racer.PerformancePoints + "~n~Looking for similar vehicles in Racers...");
			}
			SameDriverReference = (int)racer.PerformancePoints;
		}
		if (DebugNotifications)
		{
			WarnPlayer(ScriptName, racer.Car.FriendlyName, "Performance:" + racer.PerformancePoints + "~n~Shifting: " + racer.Shifting.ToString() + "~n~Reaction: " + racer.Reaction.ToString());
		}
	}

	public void FillWithSleepers()
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Invalid comparison between Unknown and I4
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Invalid comparison between Unknown and I4
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Invalid comparison between Unknown and I4
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Invalid comparison between Unknown and I4
		int num = 0;
		foreach (Racer racer2 in Racers)
		{
			if (!racer2.Driver.IsPlayer && racer2.CustomTurbo > 2)
			{
				num++;
			}
		}
		int num2 = 0;
		while (num < 4 && num2 < 20)
		{
			Racer racer = Racers[Util.RandomInt(0, Racers.Count - 1)];
			if (!racer.Driver.IsPlayer && racer.CustomTurbo < 2 && ((int)racer.Car.ClassType == 0 || (int)racer.Car.ClassType == 2 || (int)racer.Car.ClassType == 3 || (int)racer.Car.ClassType == 1 || (int)racer.Car.ClassType == 12))
			{
				if (Util.ThrowDice(50))
				{
					Prop val = World.CreateProp("prop_car_engine_01", ((Entity)racer.Car).Position, ((Entity)racer.Car).Rotation, false, false);
					((Entity)val).AttachTo((Entity)(object)racer.Car, ((Entity)racer.Car).GetBoneIndex("engine"), new Vector3(0f, -0.04f, -0.1f), Vector3.Zero);
					racer.SuperEngine = val;
					racer.CustomTurbo = 200;
					if (DebugNotifications)
					{
						UI.Notify(racer.Name + " has EMS5");
					}
					racer.Car.SetMod((VehicleMod)23, 1, true);
					Function.Call((Hash)unchecked((ulong)(-3110590255496472013L)), new InputArgument[3]
					{
						racer.Car,
						4,
						true
					});
				}
				else
				{
					racer.CustomTurbo = Util.RandomInt(20, 100);
					if (DebugNotifications)
					{
						UI.Notify(racer.Name + " is a sleeper");
					}
				}
				num++;
				Script.Wait(20);
			}
			num2++;
		}
	}

	public void PickForRace(Racer racer, RacerStatus state)
	{
		racer.Car.Repair();
		racer.Picked = state;
		int count = RacersInState(state).Count;
		if (count == 1)
		{
			racer.Offset = Separation * 1;
		}
		if (count == 2)
		{
			racer.Offset = Separation * -1;
		}
		if (count == 3)
		{
			racer.Offset = Separation * 2;
		}
		if (count == 4)
		{
			racer.Offset = Separation * -2;
		}
		if (count == 5)
		{
			racer.Offset = Separation * 3;
		}
		if (count == 6)
		{
			racer.Offset = Separation * -3;
		}
		if (count == 7)
		{
			racer.Offset = Separation * 4;
		}
		if (count == 8)
		{
			racer.Offset = Separation * -4;
		}
		if (count == 9)
		{
			racer.Offset = Separation * 5;
		}
		if (count == 10)
		{
			racer.Offset = Separation * -5;
		}
		racer.GotToMidPoint = false;
		racer.Forced = false;
		if (SameDriverReference == 0)
		{
			if (DebugNotifications)
			{
				UI.Notify("Got Performance sample: " + racer.PerformancePoints + "~n~Looking for similar vehicles in Racers...");
			}
			SameDriverReference = (int)racer.PerformancePoints;
		}
		if (DebugNotifications)
		{
			WarnPlayer(ScriptName, racer.Car.FriendlyName, "Performance:" + racer.PerformancePoints + "~n~Shifting: " + racer.Shifting.ToString() + "~n~Reaction: " + racer.Reaction.ToString());
		}
		if (GetPlayerRacer() == racer)
		{
			if (state == RacerStatus.Picked && OfficialMeet)
			{
				Util.AddQueuedHelpText("~g~You've been picked for this race.~w~ Go to your starting position.");
			}
			if (state == RacerStatus.PickedNext)
			{
				Util.AddQueuedHelpText("~g~You've been picked for the next race.~w~ Be ready when you're called to the starting line.");
			}
		}
	}

	public void HandleMeet()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
		if (Util.CanWeUse((Entity)(object)FlarePed) && FlarePedInPosition() && DynamicCrowdAreas.Count == 0 && Spectators.Count > 0)
		{
			Vector3 entityOffset = Util.GetEntityOffset((Entity)(object)FlarePed, -5f, RacersPerRace * 5);
			DynamicCrowdAreas.Add(entityOffset);
			for (int i = 0; i < 5; i++)
			{
				Spectators[Util.RandomInt(0, Spectators.Count - 1)].DesiredPos = entityOffset;
			}
			entityOffset = Util.GetEntityOffset((Entity)(object)FlarePed, -5f, -(RacersPerRace * 5));
			DynamicCrowdAreas.Add(entityOffset);
			for (int j = 0; j < 5; j++)
			{
				Spectators[Util.RandomInt(0, Spectators.Count - 1)].DesiredPos = entityOffset;
			}
			if (DebugNotifications)
			{
				WarnPlayer(ScriptName + " " + ScriptVer, "Start Line Crowd Areas", ScriptName + " has created two custom crowd areas near the starting line.");
			}
		}
		IsStartLineBusy = false;
		foreach (Racer item in RacersParticipating())
		{
			if (item.Car.Speed > 0.5f && ((Entity)item.Car).IsInRangeOf(StartingLine, (float)(RacersPerRace * 4)))
			{
				IsStartLineBusy = true;
				break;
			}
		}
		switch (MeetState)
		{
		case DragRaceState.Idle:
		{
			if (StartingLine == Vector3.Zero)
			{
				StartingLine = Util.ToGround(Util.GetEntityOffset((Entity)(object)Game.Player.Character, 0f, 0f), 3f);
			}
			if (Finishline == Vector3.Zero)
			{
				RaycastResult crosshairCoordinates = World.GetCrosshairCoordinates();
				Finishline = crosshairCoordinates.HitCoords;
			}
			if (WaitingArea == Vector3.Zero)
			{
				WaitingArea = Util.ToGround(Util.GetEntityOffset((Entity)(object)Game.Player.Character, -80f, 0f), 3f);
			}
			if (DragHeading == 0f)
			{
				DragHeading = ((Entity)Game.Player.Character).Heading;
			}
			if (Racers.Count > 0 && !Util.CanWeUse((Entity)(object)FlarePed))
			{
				FlarePed = World.CreatePed((PedHash)2014052797, StartingLine.Around(20f));
			}
			ChallengePrize = 0;
			if (RacersInState(RacerStatus.PickedNext).Count > 0)
			{
				foreach (Racer item2 in RacersInState(RacerStatus.PickedNext))
				{
					item2.Picked = RacerStatus.Picked;
				}
			}
			int num5 = RacersPerRace;
			if (num5 > Racers.Count)
			{
				num5 = Racers.Count;
			}
			if (RacersNotParticipating(inWaitingArea: true).Count <= 0)
			{
				break;
			}
			if (RacersParticipating().Count < num5)
			{
				HandleRacerPicking(RacerStatus.Picked, num5);
				break;
			}
			if (DebugNotifications)
			{
				UI.Notify("Finished picking racers.");
			}
			SameDriverReference = 0;
			PickSameDriversTries = 0;
			PerformanceDifferenceMin = 3;
			MeetState = DragRaceState.Setup;
			foreach (Racer item3 in RacersParticipating())
			{
				item3.max_willing_to_bet = 60;
				item3.min_willing_to_bet = 30;
				if (item3.Driver.IsPlayer)
				{
					continue;
				}
				foreach (Racer item4 in RacersParticipating())
				{
					float num6 = 0f;
					int num7 = 0;
					if (((Entity)item3.Driver).Handle != ((Entity)item4.Driver).Handle)
					{
						num7++;
						num6 += item3.PerformancePoints;
					}
					num6 /= (float)num7;
					if (num6 > item4.PerformancePoints + 10f)
					{
						item3.max_willing_to_bet = 50;
						item3.min_willing_to_bet = 0;
						item4.Expectations = RacerExpectations.Lose;
					}
					if (num6 < item4.PerformancePoints - 10f)
					{
						item3.max_willing_to_bet = 80;
						item3.min_willing_to_bet = 40;
						item4.Expectations = RacerExpectations.Win;
					}
				}
			}
			string text = "";
			foreach (Racer item5 in RacersParticipating())
			{
				if (ChallengePrize == 0)
				{
					ChallengePrize = item5.GetRacerMoney() * Util.RandomInt(item5.min_willing_to_bet, item5.max_willing_to_bet) / 100;
				}
				if (item5.GetRacerMoney() * item5.max_willing_to_bet / 100 < ChallengePrize)
				{
					ChallengePrize = item5.GetRacerMoney() * Util.RandomInt(item5.min_willing_to_bet, item5.max_willing_to_bet) / 100;
				}
				text = ((item5 != RacersParticipating()[0]) ? (text + "~w~ vs ~b~" + item5.Name) : (text + "~b~" + item5.Name));
			}
			if (OfficialMeet)
			{
				text = text + "~n~Prize: ~g~$" + Prize;
			}
			else
			{
				text = ((ChallengePrize <= 0) ? (text + "~n~Bet: ~g~No bet") : (text + "~n~Bet: ~g~$" + ChallengePrize));
			}
			if (GetPlayerRacer() != null)
			{
				if (RacersInState(RacerStatus.Picked).Contains(GetPlayerRacer()))
				{
					Util.AddQueuedHelpText("~g~Its your turn.~w~ Go to your ~y~starting position~w~.");
				}
				else if (CinematicCam && Guides)
				{
					Util.AddQueuedHelpText("Aim to the ~y~Starting Line Girl~w~ if you want to enable cinematic mode for this race.");
				}
				if (RacersInState(RacerStatus.Picked).Contains(GetPlayerRacer()))
				{
					if (!OfficialMeet)
					{
						if (Guides)
						{
							if (ChallengePrize > 0)
							{
								Util.AddQueuedHelpText("The bet for this race is set at ~g~$" + ChallengePrize + "~w~. You can change it introducing the cheat '~b~bet~w~'.");
							}
							else
							{
								Util.AddQueuedHelpText("The bet for this race is set at ~g~$" + ChallengePrize + "~w~ because you don't have any money.");
							}
						}
					}
					else if (Guides)
					{
						Util.AddQueuedHelpText("The prize for this race is $~g~" + Prize);
					}
				}
			}
			else if (CinematicCam && Guides)
			{
				Util.AddQueuedHelpText("Aim to the ~y~Starting Line Girl~w~ if you want to enable cinematic mode for this race.");
			}
			Util.Notify("CHAR_LAMAR", "~b~Lamar", "Next race", text);
			break;
		}
		case DragRaceState.Setup:
			if (RacersParticipating().Count == RacersPerRace)
			{
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				foreach (Racer item6 in RacersParticipating())
				{
					num3++;
					if (((Entity)item6.Driver).IsInRangeOf(StartingLine, 20f))
					{
						num2++;
					}
					if (item6.InPosition())
					{
						num4++;
					}
				}
				if (num2 > 0 && OfficialMeet && RacersInState(RacerStatus.PickedNext).Count < RacersPerRace)
				{
					HandleRacerPicking(RacerStatus.PickedNext, RacersPerRace);
				}
				if (num4 < RacersParticipating().Count)
				{
					break;
				}
				if (BetTimeFrame == 0)
				{
					if (ActivityNotifications)
					{
						Util.Notify("CHAR_LAMAR", "~b~Lamar", "BET TIME", "Make your bets, ladies and gentlemen.");
					}
					BetTimeFrame = Game.GameTime + 6000;
				}
				else if (RacersNotParticipating(inWaitingArea: true).Count > 0)
				{
					Racer racer = RacersNotParticipating(inWaitingArea: true)[Util.RandomInt(0, RacersNotParticipating(inWaitingArea: true).Count)];
					if (!racer.Driver.IsPlayer && !racer.HasBet && Util.ThrowDice(60) && racer.Driver.Money >= MoneyBet)
					{
						Racer racer2 = RacersParticipating()[Util.RandomInt(0, RacersParticipating().Count)];
						if (ActivityNotifications)
						{
							UI.Notify("~b~" + racer.Name + "~w~ bets on ~g~" + racer2.Name);
						}
						racer2.BetsOnHim.Add(racer);
						racer.HasBet = true;
						Util.ChangePedMoney(racer.Driver, -MoneyBet);
						break;
					}
				}
				if (BetTimeFrame >= Game.GameTime || !TrackReady() || PlayerNearRacers())
				{
					break;
				}
				BetTimeFrame = 0;
				if (ActivityNotifications)
				{
					UI.Notify("Bet time is over.");
				}
				CountDown = Game.GameTime + 5000;
				MeetState = DragRaceState.CountDown;
				SameDriverReference = 0;
				PickSameDriversTries = 0;
				PerformanceDifferenceMin = 3;
				if (CinematicCam && SpectCamera == (Camera)null && (GetPlayerRacer() == null || GetPlayerRacer().Picked != RacerStatus.Picked) && ((Entity)FlarePed).IsOnScreen && (GameplayCamera.IsAimCamActive || GameplayCamera.IsFirstPersonAimCamActive))
				{
					SpectCamera = World.CreateCamera(StartingLine, new Vector3(0f, 0f, DragHeading), GameplayCamera.FieldOfView);
					SpectCamera.Position = Util.GetEntityOffset((Entity)(object)FlarePed, -(ForwardOffset + 8), 0f);
					SpectCamera.PointAt((Entity)(object)FlarePed);
					World.RenderingCamera = SpectCamera;
				}
				foreach (Racer item7 in RacersParticipating())
				{
					if (Util.DiffBetweenAngles(((Entity)item7.Car).Heading, DragHeading) > 5.0 && !item7.Driver.IsPlayer)
					{
						((Entity)item7.Car).Position = ((Entity)item7.Car).Position + ((Entity)item7.Car).ForwardVector * -3f + new Vector3(0f, 0f, 1f);
						((Entity)item7.Car).Heading = DragHeading;
					}
				}
				{
					foreach (Racer item8 in RacersNotParticipating(inWaitingArea: false))
					{
						if (item8.HasBet)
						{
							item8.HasBet = false;
						}
					}
					break;
				}
			}
			foreach (Racer item9 in RacersParticipating())
			{
				item9.Picked = RacerStatus.NotPicked;
			}
			WarnPlayer(ScriptName + " " + ScriptVer, "ERROR IN CURRENT PARTICIPANTS", ScriptName + " will try to get new participants now.");
			MeetState = DragRaceState.Idle;
			break;
		case DragRaceState.CountDown:
			if (DistanceMultiplier != 100 && !TougeMode && FlarePedInPosition())
			{
				Vector3 val = Util.LerpByDistance(StartingLine, Finishline, StartingLine.DistanceTo(Finishline) * ((float)DistanceMultiplier * 0.01f));
				if (DebugNotifications)
				{
					UI.Notify("Track length reduction requested");
					UI.Notify("Old distance:" + Finishline.DistanceTo(StartingLine));
				}
				Finishline = Util.ToGround(val, 30f);
				if (Finishline == Vector3.Zero)
				{
					Finishline = val;
				}
				if (DebugNotifications)
				{
					UI.Notify("New distance:" + Finishline.DistanceTo(StartingLine));
				}
				Util.AddNotification("CHAR_LAMAR", "~b~Lamar", "Shorter track requested", "Due to " + Game.Player.Name + "'s request, the track will now be " + Math.Round(Finishline.DistanceTo(StartingLine)) + "m long.");
				DistanceMultiplier = 100;
			}
			if (CountDown < Game.GameTime)
			{
				Vector3 val2 = Util.GetEntityOffset((Entity)(object)FlarePed, 10f, 0f) + ((Entity)FlarePed).UpVector * 10f;
				FlarePed.Task.ShootAt(val2, 1000);
			}
			else if (CountDown - 3000 < Game.GameTime)
			{
				Vector3 val3 = Util.GetEntityOffset((Entity)(object)FlarePed, 10f, 0f) + ((Entity)FlarePed).UpVector * 10f;
				if (!Util.IsSubttaskActive(FlarePed, Subtask.AIMING_GUN))
				{
					Function.Call((Hash)7381949471095111073L, new InputArgument[7]
					{
						FlarePed,
						val3.X,
						val3.Y,
						val3.Z,
						-1,
						false,
						true
					});
				}
			}
			break;
		case DragRaceState.RaceInProgress:
		{
			if (Winners.Count != RacersParticipating().Count)
			{
				break;
			}
			bool flag = true;
			foreach (Racer item10 in RacersParticipating())
			{
				Vector3 velocity = ((Entity)item10.Car).Velocity;
				if (velocity.Length() > 3f && !item10.Driver.IsPlayer)
				{
					flag = false;
				}
			}
			if (flag)
			{
				Function.Call((Hash)unchecked((ulong)(-9114364950456553926L)), new InputArgument[0]);
				Function.Call((Hash)4509194413786734384L, new InputArgument[0]);
				MeetState = DragRaceState.RaceFinished;
			}
			break;
		}
		case DragRaceState.RaceFinished:
		{
			if (SpectCamera != (Camera)null && World.RenderingCamera == SpectCamera)
			{
				SpectCamera.StopPointing();
				SpectCamera.Detach();
				SpectCamera.Destroy();
				SpectCamera = null;
				World.RenderingCamera = null;
				Function.Call((Hash)3582399230505917858L, new InputArgument[0]);
			}
			int num = 0;
			foreach (Racer winner in Winners)
			{
				if (Winners[0].Penalized && winner != Winners[0])
				{
					Util.ChangePedMoney(winner.Driver, 200);
				}
				if (winner == Winners[0])
				{
					if (winner.Penalized)
					{
						Util.Notify("CHAR_LAMAR", "~b~Lamar", "INVALID RACE", winner.Name + " has clearly cheated and can't claim the Prize. Participants will have their fee returned to them.");
					}
					else
					{
						winner.Wins++;
						Util.PlayAmbientSpeech(winner.Driver, Util.ChatTaunt);
						if (OfficialMeet)
						{
							if (Prize > 0)
							{
								Util.Notify("CHAR_LAMAR", "~b~Lamar", winner.Name + " won", "~b~" + winner.Name + "~w~ got the " + Prize + " prize! Now, for the bets...");
								Util.ChangePedMoney(winner.Driver, Prize);
							}
						}
						else
						{
							Util.Notify("CHAR_LAMAR", "~b~Lamar", winner.Name + " won", "~b~" + winner.Name + "~w~ won the ~g~$" + ChallengePrize + "~w~ bet!");
							Util.ChangePedMoney(winner.Driver, ChallengePrize * (RacersPerRace - 1));
						}
					}
				}
				else
				{
					winner.Loses++;
					Util.PlayAmbientSpeech(winner.Driver, Util.ChatCurse);
					if (!OfficialMeet)
					{
						Util.ChangePedMoney(winner.Driver, -ChallengePrize);
					}
				}
				foreach (Racer item11 in winner.BetsOnHim)
				{
					_ = item11;
					num += MoneyBet;
				}
			}
			foreach (Racer winner2 in Winners)
			{
				if (winner2 == Winners[0])
				{
					if (Winners[0].BetsOnHim.Count > 0)
					{
						num /= Winners[0].BetsOnHim.Count;
						foreach (Racer item12 in Winners[0].BetsOnHim)
						{
							if (ActivityNotifications)
							{
								UI.Notify("~b~" + item12.Name + "~w~ wins ~g~$" + num);
							}
							if (item12.Driver.IsPlayer)
							{
								Util.ChangePedMoney(item12.Driver, num);
							}
							SaveRacerstats(item12);
						}
					}
					else if (num > 0)
					{
						Util.ChangePedMoney(Winners[0].Driver, num);
						UI.Notify("Nobody betted on the winner! ~b~" + Winners[0].Name + "~w~ wins the ~g~$" + num + "~w~ pot!");
					}
				}
				else
				{
					foreach (Racer item13 in winner2.BetsOnHim)
					{
						SaveRacerstats(item13);
					}
				}
				if (winner2.Reaction > 100)
				{
					winner2.Reaction -= 50;
				}
				if (winner2.Shifting < 70)
				{
					winner2.Shifting++;
				}
				winner2.TaskFinish();
				winner2.bet = 0;
				winner2.Offset = 0;
				winner2.Picked = RacerStatus.NotPicked;
				winner2.BetsOnHim.Clear();
				SaveRacerstats(winner2);
			}
			if (CanWeUse((Entity)(object)FlarePed))
			{
				FlarePed.Task.ReloadWeapon();
			}
			Winners.Clear();
			MeetState = DragRaceState.Idle;
			((Entity)FinishBarrel).ApplyForce(new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, 0f));
			GetPlayerRacer();
			break;
		}
		case DragRaceState.NotStarted:
			break;
		}
	}

	public void HandleRacerPicking(RacerStatus status, int goal)
	{
		int count = RacersInState(status).Count;
		if (count == 0)
		{
			PickingGameRef = Game.GameTime;
			PerformanceTreshold = 2;
			ReputationTreshold = 10;
		}
		else
		{
			if (Game.GameTime > PickingGameRef + 6000 && ReputationTreshold < 100)
			{
				ReputationTreshold = 100;
			}
			if (Game.GameTime > PickingGameRef + 10000 && PerformanceTreshold < 5)
			{
				PerformanceTreshold = 5;
			}
			if (Game.GameTime > PickingGameRef + 15000 && PerformanceTreshold < 10)
			{
				PerformanceTreshold = 10;
			}
			if (Game.GameTime > PickingGameRef + 30000 && PerformanceTreshold < 20)
			{
				PerformanceTreshold = 20;
			}
			if (Game.GameTime > PickingGameRef + 35000 && PerformanceTreshold < 100)
			{
				PerformanceTreshold = 100;
			}
		}
		if (count >= goal || RacersNotParticipating(inWaitingArea: true).Count <= 0)
		{
			return;
		}
		foreach (Racer racer2 in Racers)
		{
			if (racer2.Forced)
			{
				PickForRace(racer2, status);
				return;
			}
		}
		if (RacersNotParticipating(inWaitingArea: true).Contains(GetPlayerRacer()))
		{
			PickForRace(GetPlayerRacer(), status);
			if (!OfficialMeet)
			{
				UI.Notify("~b~" + GetPlayerRacer().Name + "~w~ awaits to be challenged.");
			}
			if (!OfficialMeet && GetPlayerRacer().ReputationBalance() > 70f)
			{
				UI.Notify("~b~" + GetPlayerRacer().Name + "~w~ dares to challenge higher tier Racers!");
			}
			return;
		}
		Racer racer = RacersNotParticipating(inWaitingArea: true)[Util.RandomInt(0, RacersNotParticipating(inWaitingArea: true).Count - 1)];
		if (count == 0)
		{
			PickForRace(racer, status);
			if (!OfficialMeet)
			{
				UI.Notify("~b~" + racer.Name + "~w~ looks for a challenge.");
			}
			if (racer.ReputationBalance() > 70f && !OfficialMeet)
			{
				UI.Notify("~b~" + racer.Name + "~w~ dares to challenge higher tier Racers!");
			}
			return;
		}
		int num = 0;
		if (RacersInState(status)[0].ReputationBalance() > 70f && !OfficialMeet)
		{
			num += 20;
		}
		for (int i = 0; i < 10; i++)
		{
			racer = RacersNotParticipating(inWaitingArea: true)[Util.RandomInt(0, RacersNotParticipating(inWaitingArea: true).Count - 1)];
			if (racer.Picked != RacerStatus.NotPicked || !Util.SimilarPerformance(RacersInState(status)[0], racer, PerformanceTreshold, num, 0f) || (!Util.SimilarRep(racer, RacersInState(status)[0], ReputationTreshold) && PerformanceTreshold <= 4))
			{
				continue;
			}
			PickForRace(racer, status);
			if (OfficialMeet)
			{
				break;
			}
			if (RacersInState(status).Contains(GetPlayerRacer()))
			{
				if (ActivityNotifications)
				{
					UI.Notify("~b~" + racer.Name + "~w~ challenges ~b~" + GetPlayerRacer().Name + "~w~!");
				}
			}
			else if (ActivityNotifications)
			{
				UI.Notify("~b~" + RacersInState(status)[1].Name + "~w~ challenges ~b~" + RacersInState(status)[0].Name + "~w~!");
			}
			break;
		}
	}

	public static bool TrackReady()
	{
		return true;
	}

	public bool RaceCanStart()
	{
		return true;
	}

	public bool PlayerNearRacers()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if (GetPlayerRacer() != null && GetPlayerRacer().Picked == RacerStatus.NotPicked && Game.Player.Character.IsOnFoot)
		{
			foreach (Racer item in RacersParticipating())
			{
				if (((Entity)Game.Player.Character).IsInRangeOf(((Entity)item.Driver).Position, 5f))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static Racer GetPlayerRacer()
	{
		foreach (Racer racer in Racers)
		{
			if (racer.Driver.IsPlayer)
			{
				return racer;
			}
		}
		return null;
	}

	public static bool FlarePedInPosition()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (((Entity)FlarePed).IsInRangeOf(StartingLine, 5f) && FlarePed.IsStopped)
		{
			return Util.DiffBetweenAngles(((Entity)FlarePed).Heading, DragHeading - 180f) < 4.0;
		}
		return false;
	}

	private void HandleFlarePedAI()
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		if (FlarePed.RelationshipGroup != RacerRLGroup)
		{
			FlarePed.RelationshipGroup = RacerRLGroup;
		}
		if (FlarePed.IsInCombat)
		{
			return;
		}
		switch (MeetState)
		{
		case DragRaceState.Idle:
			if (RacersParticipating().Count > 0 && FlarePed.IsStopped)
			{
				if (!((Entity)FlarePed).IsInRangeOf(StartingLine, 2f))
				{
					FlarePed.Task.RunTo(StartingLine, false);
				}
				else if (Util.DiffBetweenAngles(((Entity)FlarePed).Heading, DragHeading - 180f) > 1.0)
				{
					FlarePed.Task.AchieveHeading(DragHeading - 180f);
				}
				if (FlarePed.Weapons.Current.AmmoInClip == 0)
				{
					FlarePed.Task.ReloadWeapon();
				}
			}
			break;
		case DragRaceState.Setup:
			if (FlarePed.IsStopped)
			{
				if (!((Entity)FlarePed).IsInRangeOf(StartingLine, 2f))
				{
					FlarePed.Task.RunTo(StartingLine, false);
				}
				else if (((Entity)FlarePed).Heading - 180f != DragHeading)
				{
					FlarePed.Task.AchieveHeading(DragHeading - 180f);
				}
			}
			break;
		case DragRaceState.NotStarted:
		case DragRaceState.CountDown:
		case DragRaceState.RaceInProgress:
		case DragRaceState.RaceFinished:
			break;
		}
	}

	public static List<Racer> RacersParticipating()
	{
		List<Racer> list = new List<Racer>();
		foreach (Racer racer in Racers)
		{
			if (racer.Picked == RacerStatus.Picked)
			{
				list.Add(racer);
			}
		}
		return list;
	}

	public static List<Racer> RacersInState(RacerStatus state)
	{
		List<Racer> list = new List<Racer>();
		foreach (Racer racer in Racers)
		{
			if (racer.Picked == state)
			{
				list.Add(racer);
			}
		}
		return list;
	}

	public static List<Racer> RacersNotParticipating(bool inWaitingArea)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		List<Racer> list = new List<Racer>();
		foreach (Racer racer in Racers)
		{
			if (racer.Picked == RacerStatus.NotPicked && (((Entity)racer.Car).IsInRangeOf(WaitingArea, WaitingAreRange) || !inWaitingArea))
			{
				list.Add(racer);
			}
		}
		return list;
	}

	private void OnKeyDown(object sender, KeyEventArgs e)
	{
	}

	private void OnKeyUp(object sender, KeyEventArgs e)
	{
	}

	private void ClearMeet(bool DeleteEverything)
	{
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		PrivateMeet = false;
		Function.Call((Hash)unchecked((ulong)(-9114364950456553926L)), new InputArgument[0]);
		Function.Call((Hash)4509194413786734384L, new InputArgument[0]);
		foreach (Spectator spectator in Spectators)
		{
			spectator.Clear(DeleteEverything);
		}
		Spectators.Clear();
		foreach (Racer racer in Racers)
		{
			racer.Clear(DeleteEverything);
		}
		Racers.Clear();
		MeetState = DragRaceState.NotStarted;
		if (Util.CanWeUse((Entity)(object)FlarePed))
		{
			((Entity)FlarePed).MarkAsNoLongerNeeded();
		}
		FlarePed = null;
		if (Util.CanWeUse((Entity)(object)FinishBarrel))
		{
			((Entity)FinishBarrel).MarkAsNoLongerNeeded();
		}
		FinishBarrel = null;
		KindsAllowed.Clear();
		WarnedOfPerformance = false;
		StreetIntroduction = false;
		OfficialIntroduction = false;
		foreach (Prop meetProp in MeetProps)
		{
			((Entity)meetProp).Delete();
		}
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
		World.RenderingCamera = null;
		Function.Call((Hash)3582399230505917858L, new InputArgument[0]);
		Function.Call((Hash)unchecked((ulong)(-9114364950456553926L)), new InputArgument[0]);
		Function.Call((Hash)4509194413786734384L, new InputArgument[0]);
		foreach (Blip meetBlip in MeetBlips)
		{
			meetBlip.Remove();
		}
		ClearMeet(DeleteEverything: true);
		text.Dispose();
		if (CanWeUse((Entity)(object)FlarePed))
		{
			((Entity)FlarePed).Delete();
		}
		WarnPlayer(ScriptName + " " + ScriptVer, "SCRIPT RESET", ScriptName + " has been reset successfully.");
		Dispose();
	}

	private void LoadSettings(string file)
	{
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		if (File.Exists(file))
		{
			ScriptSettings val = ScriptSettings.Load(file);
			DebugNotifications = val.GetValue<bool>("DEBUG", "DebugNotifications", false);
			Guides = val.GetValue<bool>("INFO", "Guides", false);
			ActivityNotifications = val.GetValue<bool>("INFO", "ActivityNotifications", false);
			ShowGoodBadShifting = val.GetValue<bool>("INFO", "ShowGoodBadShifting", false);
			ShowReactionTimes = val.GetValue<bool>("INFO", "ShowReactionTimes", false);
			ShowGrip = val.GetValue<bool>("INFO", "ShowGrip", false);
			LamarNotifications = val.GetValue<bool>("INFO", "LamarNotifications", true);
			MT_Support = val.GetValue<bool>("COMPATIBLITY", "MT_Support", false);
			FPSFriendly = val.GetValue<bool>("COMPATIBLITY", "FPSFriendly", false);
			MinRacerMoneyMuscle = val.GetValue<int>("RACERS", "MinRacerMoneyMuscle", 5000);
			MinRacerMoneySports = val.GetValue<int>("RACERS", "MinRacerMoneySports", 8000);
			MinRacerMoneySuper = val.GetValue<int>("RACERS", "MinRacerMoneySuper", 20000);
			MinRacerMoneyOther = val.GetValue<int>("RACERS", "MinRacerMoneyOther", 1000);
			UseDesigns = val.GetValue<bool>("RACERS", "UseDesigns", false);
			RacerBlips = val.GetValue<bool>("RACERS", "RacerBlips", true);
			AllowParachutes = val.GetValue<bool>("RACERS", "DragParachutes", false);
			SupercarsUseParachutes = val.GetValue<bool>("RACERS", "AllSupercarsUseParachute", false);
			SpeedMultiplier = val.GetValue<float>("RACERS", "SpeedMultiplier", 100f);
			SpeedMultiplier *= 0.01f;
			RandomTuningAll = val.GetValue<bool>("RACERS", "RandomTuningAll", false);
			RandomTuningGeneric = val.GetValue<bool>("RACERS", "RandomTuningGeneric", false);
			PersistentRacers = val.GetValue<bool>("RACERS", "PersistentRacers", false);
			SelfRadio = val.GetValue<bool>("MEETS", "AlwaysSelfRadio", false);
			DistanceMultiplier = val.GetValue<int>("MEETS", "TrackLength", 100);
			ManualMeetSpawn = val.GetValue<bool>("MEETS", "ManualMeetSpawn", false);
			AllowNitro = val.GetValue<bool>("MEETS", "AllowNitro", false);
			Fee = val.GetValue<int>("MEETS", "Fee", 800);
			Prize = val.GetValue<int>("MEETS", "Prize", 5000);
			MoneyBet = val.GetValue<int>("MEETS", "Bet", 200);
			CinematicCam = val.GetValue<bool>("MEETS", "CinematicCam", true);
			FairChallenges = val.GetValue<bool>("SIMULATION", "FairChallenges", false);
			ReactionTimes = val.GetValue<bool>("SIMULATION", "ReactionTimes", false);
			SimulateManualTransmission = val.GetValue<bool>("SIMULATION", "SimulateManualTransmission", false);
			SimulateTirePerformance = val.GetValue<bool>("SIMULATION", "SimulateTirePerformance", false);
			RealisticWheelies = val.GetValue<bool>("SIMULATION", "RealisticWheelies", false);
			WheelieForce = val.GetValue<float>("SIMULATION", "WheelieForce", 100f) * 0.01f;
			VehicleListUpdater = val.GetValue<bool>("RACERS", "AddNewRacers", false);
			InteractKey = val.GetValue<Control>("KEYS", "InteractKey", (Control)51);
			ShiftingKey = val.GetValue<Control>("KEYS", "ShiftingKey", (Control)51);
			WarnPlayer(ScriptName + " " + ScriptVer, "Settings loaded", "~g~All settings have been loaded.");
		}
		else
		{
			WarnPlayer(ScriptName + " " + ScriptVer, "Settings error", "Settings file in " + configpath + " doesn't exist.");
		}
	}

	public static void WarnPlayer(string script_name, string title, string message)
	{
		Function.Call((Hash)2316831480196236324L, new InputArgument[1] { "STRING" });
		Function.Call((Hash)7789129354908300458L, new InputArgument[1] { message });
		Function.Call((Hash)2075484565200204495L, new InputArgument[6]
		{
			"CHAR_SOCIAL_CLUB",
			"CHAR_SOCIAL_CLUB",
			true,
			0,
			title,
			"~b~" + script_name
		});
	}

	private bool CanWeUse(Entity entity)
	{
		if (entity != (Entity)null)
		{
			return entity.Exists();
		}
		return false;
	}

	private void DisplayHelpTextThisFrame(string text)
	{
		Function.Call((Hash)unchecked((ulong)(-8860350453193909743L)), new InputArgument[1] { "STRING" });
		Function.Call((Hash)7789129354908300458L, new InputArgument[1] { text });
		Function.Call((Hash)2562546386151446694L, new InputArgument[4]
		{
			0,
			false,
			true,
			-1
		});
	}

	private List<string> GetCarsAllowed()
	{
		return new List<string> { "Street", "Super", "Gang" };
	}

	private void SpawnDriversFromFile(List<string> NotPermitted, int maxtoadd, string LookFor)
	{
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eab: Invalid comparison between Unknown and I4
		File.AppendAllText(Paths.DebugLog, "\n Spawning cars");
		if (LookFor != "")
		{
			maxtoadd = 1;
		}
		bool flag = Racers.Count == 0;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(RacersFile);
		int num = 0;
		XmlElement documentElement = xmlDocument.DocumentElement;
		XmlNodeList xmlNodeList = documentElement.SelectNodes("//Racers/*");
		if (xmlNodeList.Count == 0)
		{
			xmlNodeList = documentElement.SelectNodes("//Drivers/*");
		}
		if (xmlNodeList.Count == 0)
		{
			WarnPlayer(ScriptName + " " + ScriptVer, "Racers.xml LOAD ERROR", "~r~No vehicles found in Racers.xml.");
			return;
		}
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		int num2 = maxtoadd;
		foreach (XmlElement item in xmlNodeList)
		{
			if ((item.GetAttribute("Name") != null && LookFor == "" && !NotPermitted.Contains(item.GetAttribute("Name"))) || item.GetAttribute("Name").ToLowerInvariant() == LookFor.ToLowerInvariant())
			{
				if (item.SelectSingleNode("RacesAllowed") == null)
				{
					list.Add(item.GetAttribute("Name"));
					if (DebugNotifications)
					{
						UI.Notify("Added generic driver: " + item.GetAttribute("Name"));
					}
					continue;
				}
				foreach (XmlElement item2 in item.SelectNodes("RacesAllowed/*"))
				{
					if (KindsAllowed.Contains(item2.InnerText) || LookFor != "")
					{
						list.Add(item.GetAttribute("Name"));
						break;
					}
				}
			}
			else if (DebugNotifications)
			{
				UI.Notify("~o~" + item.GetAttribute("Name") + " is already in the Meet");
			}
		}
		if (num2 > list.Count)
		{
			num2 = list.Count;
			if (DebugNotifications)
			{
				UI.Notify("Not enough cars for this Meet, lowering limit to  " + num2);
			}
		}
		List<string> list3 = new List<string>(list);
		for (int i = 0; i < num2; i++)
		{
			int index = Util.RandomInt(0, list3.Count);
			list2.Add(list3[index]);
			list3.RemoveAt(index);
		}
		float num3 = 1f;
		if (num2 != maxtoadd)
		{
			num3 = 200f;
		}
		foreach (XmlElement item3 in xmlNodeList)
		{
			if (!list2.Contains(item3.GetAttribute("Name")))
			{
				continue;
			}
			File.AppendAllText(Paths.DebugLog, "\n" + item3.GetAttribute("Name"));
			Vector3 val = WaitingArea.Around(4f);
			num3 += 3f;
			if (((Entity)Game.Player.Character).IsInRangeOf(WaitingArea, 200f))
			{
				if (ManualMeetSpawn & flag)
				{
					if (num3 > WaitingAreRange)
					{
						num3 += 50f;
						val = World.GetNextPositionOnStreet(WaitingArea.Around(num3));
					}
					else
					{
						val = WaitingArea.Around(num3);
					}
				}
				else
				{
					if (num3 < 500f)
					{
						num3 = 500f;
					}
					num3 += 50f;
					val = World.GetNextPositionOnStreet(WaitingArea.Around(num3));
				}
			}
			else
			{
				val = WaitingArea.Around(num3);
			}
			Vehicle val2 = null;
			int num4 = 0;
			int num5 = 1000;
			while (!CanWeUse((Entity)(object)val2) && num4 < num5)
			{
				num4++;
				if (num4 > num5 / 2 && DebugNotifications)
				{
					DisplayHelpTextThisFrame("Trying to spawn " + item3.SelectSingleNode("Vehicle/Model").InnerText + " for the Drag Meet...(" + num4 + "/" + num5 + ")");
				}
				val2 = ((!int.TryParse(item3.SelectSingleNode("Vehicle/Model").InnerText, out var result)) ? World.CreateVehicle(item3.SelectSingleNode("Vehicle/Model").InnerText, val) : World.CreateVehicle(result, val));
			}
			bool flag2 = false;
			if (num4 >= num5)
			{
				if (DebugNotifications)
				{
					WarnPlayer(ScriptName + " " + ScriptVer, "VEHICLE LOAD ERROR", "Error trying to load " + item3.SelectSingleNode("Vehicle/Model").InnerText + ". Skipping this car.");
				}
				File.AppendAllText(Paths.DebugLog, " -  ERROR vehicle model not found in game files, SKIPPED");
				flag2 = true;
			}
			if (flag2)
			{
				continue;
			}
			File.AppendAllText(Paths.DebugLog, " -  created");
			Ped val3 = null;
			if (item3.SelectSingleNode("PedModel") != null)
			{
				Vector3 position;
				int result2;
				if (item3.SelectSingleNode("PedModel").InnerText == "random")
				{
					Model val4 = RacerModel[Util.RandomInt(0, RacerModel.Count)];
					position = ((Entity)val2).Position;
					val3 = World.CreatePed(val4, position.Around(2f));
				}
				else if (int.TryParse(item3.SelectSingleNode("PedModel").InnerText, out result2))
				{
					Model val5 = result2;
					position = ((Entity)val2).Position;
					val3 = World.CreatePed(val5, position.Around(2f));
				}
				else
				{
					Model val6 = item3.SelectSingleNode("PedModel").InnerText;
					position = ((Entity)val2).Position;
					val3 = World.CreatePed(val6, position.Around(2f));
				}
			}
			if (!CanWeUse((Entity)(object)val3))
			{
				val3 = Function.Call<Ped>((Hash)unchecked((ulong)(-7250169592070323040L)), new InputArgument[2]
				{
					val2,
					true
				});
			}
			while ((Entity)(object)val2 == (Entity)null || (Entity)(object)val3 == (Entity)null)
			{
				Script.Wait(0);
			}
			File.AppendAllText(Paths.DebugLog, " -  ped created");
			if (RandomTuningAll)
			{
				Script.Wait(40);
				Util.RandomTuning(val2, neons: false, horn: false);
				File.AppendAllText(Paths.DebugLog, " - tuned (random)");
			}
			else if (UseDesigns)
			{
				Function.Call((Hash)2245783831530643834L, new InputArgument[2]
				{
					val2,
					0
				});
				if (item3.SelectSingleNode("Vehicle/PrimaryColor") != null)
				{
					if (item3.SelectSingleNode("Vehicle/WheelType") != null)
					{
						val2.WheelType = (VehicleWheelType)int.Parse(item3.SelectSingleNode("Vehicle/WheelType").InnerText, CultureInfo.InvariantCulture);
					}
					if (item3.SelectSingleNode("Vehicle/PrimaryColor") != null)
					{
						val2.PrimaryColor = (VehicleColor)int.Parse(item3.SelectSingleNode("Vehicle/PrimaryColor").InnerText, CultureInfo.InvariantCulture);
					}
					if (item3.SelectSingleNode("Vehicle/SecondaryColor") != null)
					{
						val2.SecondaryColor = (VehicleColor)int.Parse(item3.SelectSingleNode("Vehicle/SecondaryColor").InnerText, CultureInfo.InvariantCulture);
					}
					if (item3.SelectSingleNode("Vehicle/PearlescentColor") != null)
					{
						val2.PearlescentColor = (VehicleColor)int.Parse(item3.SelectSingleNode("Vehicle/PearlescentColor").InnerText, CultureInfo.InvariantCulture);
					}
					if (item3.SelectSingleNode("Vehicle/TrimColor") != null)
					{
						Function.Call((Hash)unchecked((ulong)(-860796651183309031L)), new InputArgument[2]
						{
							val2,
							int.Parse(item3.SelectSingleNode("Vehicle/TrimColor").InnerText, CultureInfo.InvariantCulture)
						});
					}
					if (item3.SelectSingleNode("Vehicle/DashColor") != null)
					{
						Function.Call((Hash)6956317558672667244L, new InputArgument[2]
						{
							val2,
							int.Parse(item3.SelectSingleNode("Vehicle/DashColor").InnerText, CultureInfo.InvariantCulture)
						});
					}
					if (item3.SelectSingleNode("Vehicle/RimColor") != null)
					{
						val2.RimColor = (VehicleColor)int.Parse(item3.SelectSingleNode("Vehicle/RimColor").InnerText, CultureInfo.InvariantCulture);
					}
					if (item3.SelectSingleNode("Vehicle/LicensePlateText") != null)
					{
						val2.NumberPlate = item3.SelectSingleNode("Vehicle/LicensePlateText").InnerText;
					}
					if (item3.SelectSingleNode("Vehicle/LicensePlate") != null)
					{
						Function.Call((Hash)unchecked((ulong)(-8031911162494865247L)), new InputArgument[2]
						{
							val2,
							int.Parse(item3.SelectSingleNode("Vehicle/LicensePlate").InnerText, CultureInfo.InvariantCulture)
						});
					}
					if (item3.SelectSingleNode("Vehicle/WindowsTint") != null)
					{
						val2.WindowTint = (VehicleWindowTint)int.Parse(item3.SelectSingleNode("Vehicle/WindowsTint").InnerText, CultureInfo.InvariantCulture);
					}
					if (item3.SelectSingleNode("Vehicle//SmokeColor") != null)
					{
						Color tireSmokeColor = Color.FromArgb(255, int.Parse(item3.SelectSingleNode("Vehicle//SmokeColor/Color/R").InnerText), int.Parse(item3.SelectSingleNode("Vehicle//SmokeColor/Color/G").InnerText), int.Parse(item3.SelectSingleNode("Vehicle//SmokeColor/Color/B").InnerText));
						val2.TireSmokeColor = tireSmokeColor;
					}
					if (item3.SelectSingleNode("Vehicle//NeonColor") != null)
					{
						if (item3.SelectSingleNode("Vehicle//NeonColor/Color") != null)
						{
							Color neonLightsColor = Color.FromArgb(255, int.Parse(item3.SelectSingleNode("Vehicle//NeonColor/Color/R").InnerText), int.Parse(item3.SelectSingleNode("Vehicle//NeonColor/Color/G").InnerText), int.Parse(item3.SelectSingleNode("//Vehicle/NeonColor/Color/B").InnerText));
							val2.NeonLightsColor = neonLightsColor;
						}
						else
						{
							Color neonLightsColor2 = Color.FromArgb(255, int.Parse(item3.SelectSingleNode("Vehicle//NeonColor/R").InnerText), int.Parse(item3.SelectSingleNode("Vehicle//NeonColor/G").InnerText), int.Parse(item3.SelectSingleNode("Vehicle//NeonColor/B").InnerText));
							val2.NeonLightsColor = neonLightsColor2;
						}
					}
					if (item3.SelectSingleNode("Vehicle/Neons/Back") != null)
					{
						val2.SetNeonLightsOn((VehicleNeonLight)3, bool.Parse(item3.SelectSingleNode("Vehicle/Neons/Back").InnerText));
					}
					if (item3.SelectSingleNode("Vehicle/Neons/Front") != null)
					{
						val2.SetNeonLightsOn((VehicleNeonLight)2, bool.Parse(item3.SelectSingleNode("Vehicle/Neons/Front").InnerText));
					}
					if (item3.SelectSingleNode("Vehicle/Neons/Left") != null)
					{
						val2.SetNeonLightsOn((VehicleNeonLight)0, bool.Parse(item3.SelectSingleNode("Vehicle/Neons/Left").InnerText));
					}
					if (item3.SelectSingleNode("Vehicle/Neons/Right") != null)
					{
						val2.SetNeonLightsOn((VehicleNeonLight)1, bool.Parse(item3.SelectSingleNode("Vehicle/Neons/Right").InnerText));
					}
					foreach (XmlElement item4 in item3.SelectNodes("Vehicle/Components/*"))
					{
						Function.Call((Hash)9143331738832080073L, new InputArgument[3]
						{
							val2,
							int.Parse(item4.GetAttribute("ComponentIndex")),
							int.Parse(item4.InnerText, CultureInfo.InvariantCulture)
						});
					}
					foreach (XmlElement item5 in item3.SelectNodes("Vehicle/ModToggles/*"))
					{
						Function.Call((Hash)3035231775696334088L, new InputArgument[3]
						{
							val2,
							int.Parse(item5.GetAttribute("ToggleIndex")),
							bool.Parse(item5.InnerText)
						});
					}
					foreach (XmlElement item6 in item3.SelectNodes("Vehicle/Mods/*"))
					{
						val2.SetMod((VehicleMod)int.Parse(item6.GetAttribute("ModIndex")), int.Parse(item6.InnerText, CultureInfo.InvariantCulture), bool.Parse(item3.SelectSingleNode("Vehicle/CustomTires").InnerText));
					}
					File.AppendAllText(Paths.DebugLog, " - tuned");
				}
				else if (RandomTuningGeneric)
				{
					Script.Wait(40);
					Util.RandomTuning(val2, neons: false, horn: false);
					File.AppendAllText(Paths.DebugLog, " - tuned (random)");
				}
			}
			int num6 = 1;
			if (item3.SelectSingleNode("CustomTurbo") != null)
			{
				num6 = int.Parse(item3.SelectSingleNode("CustomTurbo").InnerText, CultureInfo.InvariantCulture);
			}
			int num7 = 50;
			num7 = ((!(item3.GetAttribute("Shifting") != "") || !(item3.GetAttribute("Shifting") != "50")) ? Util.RandomInt(35, 80) : int.Parse(item3.GetAttribute("Shifting")));
			int num8 = 250;
			num8 = ((!(item3.GetAttribute("Reaction") != "") || !(item3.GetAttribute("Reaction") != "250")) ? Util.RandomInt(150, 850) : int.Parse(item3.GetAttribute("Reaction")));
			File.AppendAllText(Paths.DebugLog, " - skilled");
			string text = val2.FriendlyName;
			bool flag3 = true;
			if (UseDesigns)
			{
				if (item3.GetAttribute("Name") != "")
				{
					text = item3.GetAttribute("Name");
				}
				if (item3.GetAttribute("DriverName") != "")
				{
					text = item3.GetAttribute("Name");
				}
			}
			else
			{
				flag3 = false;
			}
			flag3 = PersistentRacers;
			if (num6 > 1 && DebugNotifications)
			{
				text = "(Supercharged) " + text;
			}
			((Entity)val2).Heading = Util.RandomInt(-180, 180);
			bool parachute = false;
			if (AllowParachutes)
			{
				if (item3.GetAttribute("Parachute") == "true")
				{
					parachute = true;
				}
				if (SupercarsUseParachutes && (int)val2.ClassType == 7)
				{
					parachute = true;
				}
			}
			int money = 0;
			if (item3.HasAttribute("Money"))
			{
				money = int.Parse(item3.GetAttribute("Money"));
			}
			int won = 0;
			if (item3.HasAttribute("RacesWon"))
			{
				won = int.Parse(item3.GetAttribute("RacesWon"));
			}
			int lost = 0;
			if (item3.HasAttribute("RacesLost"))
			{
				lost = int.Parse(item3.GetAttribute("RacesLost"));
			}
			Racers.Add(new Racer(text, val3, val2, num6, num8, num7, !flag, won, lost, money, parachute, flag3));
			File.AppendAllText(Paths.DebugLog, " - finished ");
		}
	}

	static DragMeets()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
	}
}

}