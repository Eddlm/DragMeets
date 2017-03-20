using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

//public enum Reaction {Slowest=1000, Slow=500, Average=300, Fast=200, Fastest=50 }
//public enum Shifting { Worst=10, Bad=30, Average=50, Good=70, Best=90}


public enum RacerStatus { NotPicked,Picked,PickedNext,PickedThird}
public enum RacerExpectations { None, Win, Lose }

public enum DragRaceState { NotStarted, Idle, Setup, CountDown, RaceInProgress, RaceFinished }
public enum Subtask
{
    AIMED_SHOOTING_ON_FOOT = 4,
    GETTING_UP = 16,
    MOVING_ON_FOOT_NO_COMBAT = 35,
    MOVING_ON_FOOT_COMBAT = 38,
    USING_LADDER = 47,
    CLIMBING = 50,
    GETTING_OFF_SOMETHING = 51,
    SWAPPING_WEAPON = 56,
    REMOVING_HELMET = 92,
    DEAD = 97,
    MELEE_COMBAT = 130,
    HITTING_MELEE = 130,
    SITTING_IN_VEHICLE = 150,
    DRIVING_WANDERING = 151,
    EXITING_VEHICLE = 152,
    Drive= 157,
    ENTERING_VEHICLE_GENERAL = 160,
    ENTERING_VEHICLE_BREAKING_WINDOW = 161,
    ENTERING_VEHICLE_OPENING_DOOR = 162,
    ENTERING_VEHICLE_ENTERING = 163,
    ENTERING_VEHICLE_CLOSING_DOOR = 164,

    EXIING_VEHICLE_OPENING_DOOR_EXITING = 167,
    EXITING_VEHICLE_CLOSING_DOOR = 168,
    CTaskControlVehicle = 169,
    USING_MOUNTED_WEAPON = 199,
    AIMING_THROWABLE = 289,
    AIMING_GUN = 290,
    AIMING_PREVENTED_BY_OBSTACLE = 299,
    IN_COVER_GENERAL = 287,
    IN_COVER_FULLY_IN_COVER = 288,

    RELOADING = 298,

    RUNNING_TO_COVER = 300,
    IN_COVER_TRANSITION_TO_AIMING_FROM_COVER = 302,
    IN_COVER_TRANSITION_FROM_AIMING_FROM_COVER = 303,
    IN_COVER_BLIND_FIRE = 304,

    PARACHUTING = 334,
    PUTTING_OFF_PARACHUTE = 336,

    JUMPING_OR_CLIMBING_GENERAL = 420,
    JUMPING_AIR = 421,
    JUMPING_FINISHING_JUMP = 422,
    VehicleBurnout= 523,
    CTaskMotionInVehicle=173,
    CTaskMotionInAutomobile =170,
    CTaskVehicleGoForward=517,
    CTaskVehicleGoToAutomobileNew=453,
    CTaskVehicleTempAction=12,
}
public enum InstructionalButtonName
{
    INPUT_NEXT_CAMERA = 0,
    INPUT_LOOK_LR = 1,
    INPUT_LOOK_UD = 2,
    INPUT_LOOK_UP_ONLY = 3,
    INPUT_LOOK_DOWN_ONLY = 4,
    INPUT_LOOK_LEFT_ONLY = 5,
    INPUT_LOOK_RIGHT_ONLY = 6,
    INPUT_CINEMATIC_SLOWMO = 7,
    INPUT_SCRIPTED_FLY_UD = 8,
    INPUT_SCRIPTED_FLY_LR = 9,
    INPUT_SCRIPTED_FLY_ZUP = 10,
    INPUT_SCRIPTED_FLY_ZDOWN = 11,
    INPUT_WEAPON_WHEEL_UD = 12,
    INPUT_WEAPON_WHEEL_LR = 13,
    INPUT_WEAPON_WHEEL_NEXT = 14,
    INPUT_WEAPON_WHEEL_PREV = 15,
    INPUT_SELECT_NEXT_WEAPON = 16,
    INPUT_SELECT_PREV_WEAPON = 17,
    INPUT_SKIP_CUTSCENE = 18,
    INPUT_CHARACTER_WHEEL = 19,
    INPUT_MULTIPLAYER_INFO = 20,
    INPUT_SPRINT = 21,
    INPUT_JUMP = 22,
    INPUT_ENTER = 23,
    INPUT_ATTACK = 24,
    INPUT_AIM = 25,
    INPUT_LOOK_BEHIND = 26,
    INPUT_PHONE = 27,
    INPUT_SPECIAL_ABILITY = 28,
    INPUT_SPECIAL_ABILITY_SECONDARY = 29,
    INPUT_MOVE_LR = 30,
    INPUT_MOVE_UD = 31,
    INPUT_MOVE_UP_ONLY = 32,
    INPUT_MOVE_DOWN_ONLY = 33,
    INPUT_MOVE_LEFT_ONLY = 34,
    INPUT_MOVE_RIGHT_ONLY = 35,
    INPUT_DUCK = 36,
    INPUT_SELECT_WEAPON = 37,
    INPUT_PICKUP = 38,
    INPUT_SNIPER_ZOOM = 39,
    INPUT_SNIPER_ZOOM_IN_ONLY = 40,
    INPUT_SNIPER_ZOOM_OUT_ONLY = 41,
    INPUT_SNIPER_ZOOM_IN_SECONDARY = 42,
    INPUT_SNIPER_ZOOM_OUT_SECONDARY = 43,
    INPUT_COVER = 44,
    INPUT_RELOAD = 45,
    INPUT_TALK = 46,
    INPUT_DETONATE = 47,
    INPUT_HUD_SPECIAL = 48,
    INPUT_ARREST = 49,
    INPUT_ACCURATE_AIM = 50,
    INPUT_CONTEXT = 51,
    INPUT_CONTEXT_SECONDARY = 52,
    INPUT_WEAPON_SPECIAL = 53,
    INPUT_WEAPON_SPECIAL_TWO = 54,
    INPUT_DIVE = 55,
    INPUT_DROP_WEAPON = 56,
    INPUT_DROP_AMMO = 57,
    INPUT_THROW_GRENADE = 58,
    INPUT_VEH_MOVE_LR = 59,
    INPUT_VEH_MOVE_UD = 60,
    INPUT_VEH_MOVE_UP_ONLY = 61,
    INPUT_VEH_MOVE_DOWN_ONLY = 62,
    INPUT_VEH_MOVE_LEFT_ONLY = 63,
    INPUT_VEH_MOVE_RIGHT_ONLY = 64,
    INPUT_VEH_SPECIAL = 65,
    INPUT_VEH_GUN_LR = 66,
    INPUT_VEH_GUN_UD = 67,
    INPUT_VEH_AIM = 68,
    INPUT_VEH_ATTACK = 69,
    INPUT_VEH_ATTACK2 = 70,
    INPUT_VEH_ACCELERATE = 71,
    INPUT_VEH_BRAKE = 72,
    INPUT_VEH_DUCK = 73,
    INPUT_VEH_HEADLIGHT = 74,
    INPUT_VEH_EXIT = 75,
    INPUT_VEH_HANDBRAKE = 76,
    INPUT_VEH_HOTWIRE_LEFT = 77,
    INPUT_VEH_HOTWIRE_RIGHT = 78,
    INPUT_VEH_LOOK_BEHIND = 79,
    INPUT_VEH_CIN_CAM = 80,
    INPUT_VEH_NEXT_RADIO = 81,
    INPUT_VEH_PREV_RADIO = 82,
    INPUT_VEH_NEXT_RADIO_TRACK = 83,
    INPUT_VEH_PREV_RADIO_TRACK = 84,
    INPUT_VEH_RADIO_WHEEL = 85,
    INPUT_VEH_HORN = 86,
    INPUT_VEH_FLY_THROTTLE_UP = 87,
    INPUT_VEH_FLY_THROTTLE_DOWN = 88,
    INPUT_VEH_FLY_YAW_LEFT = 89,
    INPUT_VEH_FLY_YAW_RIGHT = 90,
    INPUT_VEH_PASSENGER_AIM = 91,
    INPUT_VEH_PASSENGER_ATTACK = 92,
    INPUT_VEH_SPECIAL_ABILITY_FRANKLIN = 93,
    INPUT_VEH_STUNT_UD = 94,
    INPUT_VEH_CINEMATIC_UD = 95,
    INPUT_VEH_CINEMATIC_UP_ONLY = 96,
    INPUT_VEH_CINEMATIC_DOWN_ONLY = 97,
    INPUT_VEH_CINEMATIC_LR = 98,
    INPUT_VEH_SELECT_NEXT_WEAPON = 99,
    INPUT_VEH_SELECT_PREV_WEAPON = 100,
    INPUT_VEH_ROOF = 101,
    INPUT_VEH_JUMP = 102,
    INPUT_VEH_GRAPPLING_HOOK = 103,
    INPUT_VEH_SHUFFLE = 104,
    INPUT_VEH_DROP_PROJECTILE = 105,
    INPUT_VEH_MOUSE_CONTROL_OVERRIDE = 106,
    INPUT_VEH_FLY_ROLL_LR = 107,
    INPUT_VEH_FLY_ROLL_LEFT_ONLY = 108,
    INPUT_VEH_FLY_ROLL_RIGHT_ONLY = 109,
    INPUT_VEH_FLY_PITCH_UD = 110,
    INPUT_VEH_FLY_PITCH_UP_ONLY = 111,
    INPUT_VEH_FLY_PITCH_DOWN_ONLY = 112,
    INPUT_VEH_FLY_UNDERCARRIAGE = 113,
    INPUT_VEH_FLY_ATTACK = 114,
    INPUT_VEH_FLY_SELECT_NEXT_WEAPON = 115,
    INPUT_VEH_FLY_SELECT_PREV_WEAPON = 116,
    INPUT_VEH_FLY_SELECT_TARGET_LEFT = 117,
    INPUT_VEH_FLY_SELECT_TARGET_RIGHT = 118,
    INPUT_VEH_FLY_VERTICAL_FLIGHT_MODE = 119,
    INPUT_VEH_FLY_DUCK = 120,
    INPUT_VEH_FLY_ATTACK_CAMERA = 121,
    INPUT_VEH_FLY_MOUSE_CONTROL_OVERRIDE = 122,
    INPUT_VEH_SUB_TURN_LR = 123,
    INPUT_VEH_SUB_TURN_LEFT_ONLY = 124,
    INPUT_VEH_SUB_TURN_RIGHT_ONLY = 125,
    INPUT_VEH_SUB_PITCH_UD = 126,
    INPUT_VEH_SUB_PITCH_UP_ONLY = 127,
    INPUT_VEH_SUB_PITCH_DOWN_ONLY = 128,
    INPUT_VEH_SUB_THROTTLE_UP = 129,
    INPUT_VEH_SUB_THROTTLE_DOWN = 130,
    INPUT_VEH_SUB_ASCEND = 131,
    INPUT_VEH_SUB_DESCEND = 132,
    INPUT_VEH_SUB_TURN_HARD_LEFT = 133,
    INPUT_VEH_SUB_TURN_HARD_RIGHT = 134,
    INPUT_VEH_SUB_MOUSE_CONTROL_OVERRIDE = 135,
    INPUT_VEH_PUSHBIKE_PEDAL = 136,
    INPUT_VEH_PUSHBIKE_SPRINT = 137,
    INPUT_VEH_PUSHBIKE_FRONT_BRAKE = 138,
    INPUT_VEH_PUSHBIKE_REAR_BRAKE = 139,
    INPUT_MELEE_ATTACK_LIGHT = 140,
    INPUT_MELEE_ATTACK_HEAVY = 141,
    INPUT_MELEE_ATTACK_ALTERNATE = 142,
    INPUT_MELEE_BLOCK = 143,
    INPUT_PARACHUTE_DEPLOY = 144,
    INPUT_PARACHUTE_DETACH = 145,
    INPUT_PARACHUTE_TURN_LR = 146,
    INPUT_PARACHUTE_TURN_LEFT_ONLY = 147,
    INPUT_PARACHUTE_TURN_RIGHT_ONLY = 148,
    INPUT_PARACHUTE_PITCH_UD = 149,
    INPUT_PARACHUTE_PITCH_UP_ONLY = 150,
    INPUT_PARACHUTE_PITCH_DOWN_ONLY = 151,
    INPUT_PARACHUTE_BRAKE_LEFT = 152,
    INPUT_PARACHUTE_BRAKE_RIGHT = 153,
    INPUT_PARACHUTE_SMOKE = 154,
    INPUT_PARACHUTE_PRECISION_LANDING = 155,
    INPUT_MAP = 156,
    INPUT_SELECT_WEAPON_UNARMED = 157,
    INPUT_SELECT_WEAPON_MELEE = 158,
    INPUT_SELECT_WEAPON_HANDGUN = 159,
    INPUT_SELECT_WEAPON_SHOTGUN = 160,
    INPUT_SELECT_WEAPON_SMG = 161,
    INPUT_SELECT_WEAPON_AUTO_RIFLE = 162,
    INPUT_SELECT_WEAPON_SNIPER = 163,
    INPUT_SELECT_WEAPON_HEAVY = 164,
    INPUT_SELECT_WEAPON_SPECIAL = 165,
    INPUT_SELECT_CHARACTER_MICHAEL = 166,
    INPUT_SELECT_CHARACTER_FRANKLIN = 167,
    INPUT_SELECT_CHARACTER_TREVOR = 168,
    INPUT_SELECT_CHARACTER_MULTIPLAYER = 169,
    INPUT_SAVE_REPLAY_CLIP = 170,
    INPUT_SPECIAL_ABILITY_PC = 171,
    INPUT_CELLPHONE_UP = 172,
    INPUT_CELLPHONE_DOWN = 173,
    INPUT_CELLPHONE_LEFT = 174,
    INPUT_CELLPHONE_RIGHT = 175,
    INPUT_CELLPHONE_SELECT = 176,
    INPUT_CELLPHONE_CANCEL = 177,
    INPUT_CELLPHONE_OPTION = 178,
    INPUT_CELLPHONE_EXTRA_OPTION = 179,
    INPUT_CELLPHONE_SCROLL_FORWARD = 180,
    INPUT_CELLPHONE_SCROLL_BACKWARD = 181,
    INPUT_CELLPHONE_CAMERA_FOCUS_LOCK = 182,
    INPUT_CELLPHONE_CAMERA_GRID = 183,
    INPUT_CELLPHONE_CAMERA_SELFIE = 184,
    INPUT_CELLPHONE_CAMERA_DOF = 185,
    INPUT_CELLPHONE_CAMERA_EXPRESSION = 186,
    INPUT_FRONTEND_DOWN = 187,
    INPUT_FRONTEND_UP = 188,
    INPUT_FRONTEND_LEFT = 189,
    INPUT_FRONTEND_RIGHT = 190,
    INPUT_FRONTEND_RDOWN = 191,
    INPUT_FRONTEND_RUP = 192,
    INPUT_FRONTEND_RLEFT = 193,
    INPUT_FRONTEND_RRIGHT = 194,
    INPUT_FRONTEND_AXIS_X = 195,
    INPUT_FRONTEND_AXIS_Y = 196,
    INPUT_FRONTEND_RIGHT_AXIS_X = 197,
    INPUT_FRONTEND_RIGHT_AXIS_Y = 198,
    INPUT_FRONTEND_PAUSE = 199,
    INPUT_FRONTEND_PAUSE_ALTERNATE = 200,
    INPUT_FRONTEND_ACCEPT = 201,
    INPUT_FRONTEND_CANCEL = 202,
    INPUT_FRONTEND_X = 203,
    INPUT_FRONTEND_Y = 204,
    INPUT_FRONTEND_LB = 205,
    INPUT_FRONTEND_RB = 206,
    INPUT_FRONTEND_LT = 207,
    INPUT_FRONTEND_RT = 208,
    INPUT_FRONTEND_LS = 209,
    INPUT_FRONTEND_RS = 210,
    INPUT_FRONTEND_LEADERBOARD = 211,
    INPUT_FRONTEND_SOCIAL_CLUB = 212,
    INPUT_FRONTEND_SOCIAL_CLUB_SECONDARY = 213,
    INPUT_FRONTEND_DELETE = 214,
    INPUT_FRONTEND_ENDSCREEN_ACCEPT = 215,
    INPUT_FRONTEND_ENDSCREEN_EXPAND = 216,
    INPUT_FRONTEND_SELECT = 217,
    INPUT_SCRIPT_LEFT_AXIS_X = 218,
    INPUT_SCRIPT_LEFT_AXIS_Y = 219,
    INPUT_SCRIPT_RIGHT_AXIS_X = 220,
    INPUT_SCRIPT_RIGHT_AXIS_Y = 221,
    INPUT_SCRIPT_RUP = 222,
    INPUT_SCRIPT_RDOWN = 223,
    INPUT_SCRIPT_RLEFT = 224,
    INPUT_SCRIPT_RRIGHT = 225,
    INPUT_SCRIPT_LB = 226,
    INPUT_SCRIPT_RB = 227,
    INPUT_SCRIPT_LT = 228,
    INPUT_SCRIPT_RT = 229,
    INPUT_SCRIPT_LS = 230,
    INPUT_SCRIPT_RS = 231,
    INPUT_SCRIPT_PAD_UP = 232,
    INPUT_SCRIPT_PAD_DOWN = 233,
    INPUT_SCRIPT_PAD_LEFT = 234,
    INPUT_SCRIPT_PAD_RIGHT = 235,
    INPUT_SCRIPT_SELECT = 236,
    INPUT_CURSOR_ACCEPT = 237,
    INPUT_CURSOR_CANCEL = 238,
    INPUT_CURSOR_X = 239,
    INPUT_CURSOR_Y = 240,
    INPUT_CURSOR_SCROLL_UP = 241,
    INPUT_CURSOR_SCROLL_DOWN = 242,
    INPUT_ENTER_CHEAT_CODE = 243,
    INPUT_INTERACTION_MENU = 244,
    INPUT_MP_TEXT_CHAT_ALL = 245,
    INPUT_MP_TEXT_CHAT_TEAM = 246,
    INPUT_MP_TEXT_CHAT_FRIENDS = 247,
    INPUT_MP_TEXT_CHAT_CREW = 248,
    INPUT_PUSH_TO_TALK = 249,
    INPUT_CREATOR_LS = 250,
    INPUT_CREATOR_RS = 251,
    INPUT_CREATOR_LT = 252,
    INPUT_CREATOR_RT = 253,
    INPUT_CREATOR_MENU_TOGGLE = 254,
    INPUT_CREATOR_ACCEPT = 255,
    INPUT_CREATOR_DELETE = 256,
    INPUT_ATTACK2 = 257,
    INPUT_RAPPEL_JUMP = 258,
    INPUT_RAPPEL_LONG_JUMP = 259,
    INPUT_RAPPEL_SMASH_WINDOW = 260,
    INPUT_PREV_WEAPON = 261,
    INPUT_NEXT_WEAPON = 262,
    INPUT_MELEE_ATTACK1 = 263,
    INPUT_MELEE_ATTACK2 = 264,
    INPUT_WHISTLE = 265,
    INPUT_MOVE_LEFT = 266,
    INPUT_MOVE_RIGHT = 267,
    INPUT_MOVE_UP = 268,
    INPUT_MOVE_DOWN = 269,
    INPUT_LOOK_LEFT = 270,
    INPUT_LOOK_RIGHT = 271,
    INPUT_LOOK_UP = 272,
    INPUT_LOOK_DOWN = 273,
    INPUT_SNIPER_ZOOM_IN = 274,
    INPUT_SNIPER_ZOOM_OUT = 275,
    INPUT_SNIPER_ZOOM_IN_ALTERNATE = 276,
    INPUT_SNIPER_ZOOM_OUT_ALTERNATE = 277,
    INPUT_VEH_MOVE_LEFT = 278,
    INPUT_VEH_MOVE_RIGHT = 279,
    INPUT_VEH_MOVE_UP = 280,
    INPUT_VEH_MOVE_DOWN = 281,
    INPUT_VEH_GUN_LEFT = 282,
    INPUT_VEH_GUN_RIGHT = 283,
    INPUT_VEH_GUN_UP = 284,
    INPUT_VEH_GUN_DOWN = 285,
    INPUT_VEH_LOOK_LEFT = 286,
    INPUT_VEH_LOOK_RIGHT = 287,
    INPUT_REPLAY_START_STOP_RECORDING = 288,
    INPUT_REPLAY_START_STOP_RECORDING_SECONDARY = 289,
    INPUT_SCALED_LOOK_LR = 290,
    INPUT_SCALED_LOOK_UD = 291,
    INPUT_SCALED_LOOK_UP_ONLY = 292,
    INPUT_SCALED_LOOK_DOWN_ONLY = 293,
    INPUT_SCALED_LOOK_LEFT_ONLY = 294,
    INPUT_SCALED_LOOK_RIGHT_ONLY = 295,
    INPUT_REPLAY_MARKER_DELETE = 296,
    INPUT_REPLAY_CLIP_DELETE = 297,
    INPUT_REPLAY_PAUSE = 298,
    INPUT_REPLAY_REWIND = 299,
    INPUT_REPLAY_FFWD = 300,
    INPUT_REPLAY_NEWMARKER = 301,
    INPUT_REPLAY_RECORD = 302,
    INPUT_REPLAY_SCREENSHOT = 303,
    INPUT_REPLAY_HIDEHUD = 304,
    INPUT_REPLAY_STARTPOINT = 305,
    INPUT_REPLAY_ENDPOINT = 306,
    INPUT_REPLAY_ADVANCE = 307,
    INPUT_REPLAY_BACK = 308,
    INPUT_REPLAY_TOOLS = 309,
    INPUT_REPLAY_RESTART = 310,
    INPUT_REPLAY_SHOWHOTKEY = 311,
    INPUT_REPLAY_CYCLEMARKERLEFT = 312,
    INPUT_REPLAY_CYCLEMARKERRIGHT = 313,
    INPUT_REPLAY_FOVINCREASE = 314,
    INPUT_REPLAY_FOVDECREASE = 315,
    INPUT_REPLAY_CAMERAUP = 316,
    INPUT_REPLAY_CAMERADOWN = 317,
    INPUT_REPLAY_SAVE = 318,
    INPUT_REPLAY_TOGGLETIME = 319,
    INPUT_REPLAY_TOGGLETIPS = 320,
    INPUT_REPLAY_PREVIEW = 321,
    INPUT_REPLAY_TOGGLE_TIMELINE = 322,
    INPUT_REPLAY_TIMELINE_PICKUP_CLIP = 323,
    INPUT_REPLAY_TIMELINE_DUPLICATE_CLIP = 324,
    INPUT_REPLAY_TIMELINE_PLACE_CLIP = 325,
    INPUT_REPLAY_CTRL = 326,
    INPUT_REPLAY_TIMELINE_SAVE = 327,
    INPUT_REPLAY_PREVIEW_AUDIO = 328,
    INPUT_VEH_DRIVE_LOOK = 329,
    INPUT_VEH_DRIVE_LOOK2 = 330,
    INPUT_VEH_FLY_ATTACK2 = 331,
    INPUT_RADIO_WHEEL_UD = 332,
    INPUT_RADIO_WHEEL_LR = 333,
    INPUT_VEH_SLOWMO_UD = 334,
    INPUT_VEH_SLOWMO_UP_ONLY = 335,
    INPUT_VEH_SLOWMO_DOWN_ONLY = 336,
    INPUT_MAP_POI = 337,
}

namespace Drag_Meets_Reborn
{
   
   public class Util
    {

        public static List<Model> CannotWheelie = new List<Model> { "PRAIRIE", "PENUMBRA", "BLISTA", "BLISTA2","FUTO","ASEA" ,"SURGE","PREMIER","INGOT","ISSI","DILETTANTE","PIGALLE"};

        public static List<string> ChatCurse = new List<string> { "GENERIC_CURSE_MED", "GENERIC_CURSE_HIGH", "GENERIC_FUCK_YOU" };
        public static List<string> ChatTaunt = new List<string> { "KILLED_ALL", "KILLED_ALL", "KILLED_ALL" }; //GENERIC_WAR_CRY
        public static List<string> ChatThanks = new List<string> { "GENERIC_THANKS", "GENERIC_THANKS", "GENERIC_THANKS" };
        public static List<string> ChatNiceCar = new List<string> { "NICE_CAR_RANDOM", "NICE_CAR_01", "NICE_CAR_SHOUT_RANDOM", "NICE_CAR_SHOUT_01", "SEE_WEIRDO_RANDOM " };

        public static void ChangePedMoney(Ped ped, int money)
        {
            if (ped.IsPlayer) Game.Player.Money += money;
            else ped.Money += money;

            if (ped.IsPlayer) if (Game.Player.Money < 0) Game.Player.Money = 0;
            else if (ped.Money < 0) ped.Money = 0;

        }

        public static int PedMoney(Ped ped)
        {
            if (ped.IsPlayer) return Game.Player.Money;
            else return ped.Money;
        }
        Util()
        {

        }


        public static int RotationalVelocity(Entity ent)
        {
            return Function.Call<int>(Hash.GET_ENTITY_ROTATION_VELOCITY, ent);
        }
        public static void PlayAmbientSpeech(Ped ped, List<string> speechlist)
        {
            Function.Call(GTA.Native.Hash._PLAY_AMBIENT_SPEECH1, ped.Handle, speechlist[RandomInt(0,speechlist.Count)], "SPEECH_PARAMS_FORCE");
        }
        public static void SetRadioLoud(Vehicle veh, string radio)
        {
            Function.Call(GTA.Native.Hash.SET_VEHICLE_ENGINE_ON, veh, true, true);
            Function.Call(GTA.Native.Hash.SET_VEHICLE_RADIO_ENABLED, veh, true);
            Function.Call(GTA.Native.Hash.SET_VEHICLE_RADIO_LOUD, veh, true);
            Function.Call(GTA.Native.Hash.SET_VEH_RADIO_STATION, veh, radio);
        }

        public static void SetRadioQuiet(Vehicle veh)
        {
            Function.Call(GTA.Native.Hash.SET_VEHICLE_ENGINE_ON, veh, true, true);
            Function.Call(GTA.Native.Hash.SET_VEHICLE_RADIO_LOUD, veh, false);
            Function.Call(GTA.Native.Hash.SET_VEHICLE_RADIO_ENABLED, veh, true);

        }
        public static Entity EntitySeenByPlayer()
        {
            //DrawLine(GameplayCamera.Position+GameplayCamera.Direction, Game.Player.Character.Position);
            RaycastResult ent = World.Raycast(GameplayCamera.Position + (GameplayCamera.Direction*10), Game.Player.Character.Position, IntersectOptions.Everything);
            if (CanWeUse(ent.HitEntity)) return ent.HitEntity; else return null;
        }
        public static bool CanWeUse(Entity entity)
        {
            return entity != null && entity.Exists();
        }

        public static bool IsSubttaskActive(Ped ped, Subtask task)
        {
            return Function.Call<bool>(Hash.GET_IS_TASK_ACTIVE, ped, (int)task);
        }
        public static bool CarCanSeePos(Vehicle veh, Vector3 pos)
        {
            //if (veh.Position.DistanceTo(pos) < 50f) return true;
            if (veh.Position.DistanceTo(pos) < 100f)
            {
                RaycastResult raycast = World.Raycast(veh.Position + new Vector3(0, 0, 2), pos + new Vector3(0, 0, 2), IntersectOptions.Map);
                if (!raycast.DitHitAnything) return true;
            }
            return false;
        }
        public static bool IsPointOnRoad(Vector3 pos, Entity ent)
        {
            return Function.Call<bool>(Hash.IS_POINT_ON_ROAD, pos.X, pos.Y, pos.Z, ent);
        }
        public static bool IsDriving(Ped ped)
        {
            if (!ped.IsOnFoot && ped.TaskSequenceProgress != -1) return true;
            if (ped.IsPlayer) return false;
            List<Subtask> subtasks = new List<Subtask> {Subtask.CTaskControlVehicle };

            foreach(int i in subtasks)
            {
                if (Function.Call<bool>(Hash.GET_IS_TASK_ACTIVE, ped, i)) return true;
            }
            return false;

        }
        public static Vector3 GetPosAheadOf(Entity ent, int distance)
        {
            return ent.Position + (ent.ForwardVector * distance);
        }

        public static Vector3 GetPosRightOf(Entity ent, int distance)
        {
            return ent.Position + (ent.RightVector * distance);
        }

        public static Vector3 GetEntityOffset(Entity ent, float ahead, float right)
        {
            return ent.Position + (ent.ForwardVector * ahead) + (ent.RightVector*right);
        }

        public static void DrawLine(Vector3 from, Vector3 to)
        {
            Function.Call(Hash.DRAW_LINE, from.X, from.Y, from.Z, to.X, to.Y, to.Z, 255, 255, 0, 255);
        }
        public static Vector3 ToGround(Vector3 pos, float zCheck)
        {
            Vector3 newpos= World.Raycast(pos + new Vector3(0, 0, zCheck), pos + new Vector3(0, 0, -zCheck), IntersectOptions.Map).HitCoords;
            if (newpos == Vector3.Zero) return pos; else return newpos;
        }
        public static void WarnPlayer(string script_name, string title, string message)
        {
            Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "STRING");
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, message);
            Function.Call(Hash._SET_NOTIFICATION_MESSAGE, "CHAR_SOCIAL_CLUB", "CHAR_SOCIAL_CLUB", true, 0, title, "~b~" + script_name);
        }

        public static void HaltVehicle(Ped ped)
        {
            Vehicle veh = ped.CurrentVehicle;

            if (CanWeUse(veh))
            {
                Vector3 pos = ToGround(veh.ForwardVector * (veh.Speed*5), 5);
                if (pos == Vector3.Zero) pos = (veh.ForwardVector * (veh.Speed * 5));
                ped.Task.DriveTo(veh, pos, 10f, 2f, 16777216);
            }
        }
        public static double AngleBetween(Vector3 u, Vector3 v, bool returndegrees)
        {
            double toppart = 0;
            for (int d = 0; d < 3; d++) toppart += u[d] * v[d];

            double u2 = 0; //u squared
            double v2 = 0; //v squared
            for (int d = 0; d < 3; d++)
            {
                u2 += u[d] * u[d];
                v2 += v[d] * v[d];
            }

            double bottompart = 0;
            bottompart = Math.Sqrt(u2 * v2);


            double rtnval = Math.Acos(toppart / bottompart);
            if (returndegrees) rtnval *= 360.0 / (2 * Math.PI);
            return rtnval;
        }
        public static double AngleBetween(Vector3 vector1, Vector3 vector2)
        {
            double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

            return Math.Atan2(sin, cos) * (180 / Math.PI);
        }
        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
        public static double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
        public static bool IsPosAheadentity(Entity ent, Vector3 pos, float threshold)
        {
            return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, ent, pos.X, pos.Y, pos.Z).Y > (0+threshold);
        }
        public static bool IsPosRightOf(Entity ent, Vector3 pos, float threshold)
        {
            return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, ent, pos.X, pos.Y, pos.Z).X > (0 + threshold);
        }

        public static void CalcReputation(int wins, int loses)
        {

        }
        public static void TempAction(Ped ped, Vehicle veh, int action, int time)
        {
            Vector3 pos = veh.Position;


            //Util.DisplayHelpTextThisFrame("attempting burnout");


            //ped.Task.DriveTo(veh, pos, 10, 30f);

            TaskSequence TempSequence = new TaskSequence();

            Function.Call(Hash.TASK_VEHICLE_TEMP_ACTION, 0, veh, action, time);
            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, 0, veh, pos.X, pos.Y, pos.Z, 200f, 4194304, 250f);

            TempSequence.Close();            
            ped.Task.PerformSequence(TempSequence);
            TempSequence.Dispose();
            /*
*/

        }

        public static void DrawTextHere(string text, float scale,Vector3 pos, Color color)
        {
            Vector2 screeninfo = World3DToScreen2d(pos + new Vector3(0, 0, 1.5f));
            Function.Call(Hash._SET_TEXT_ENTRY, "STRING");
            Function.Call(Hash.SET_TEXT_CENTRE, true);
            Function.Call(Hash.SET_TEXT_COLOUR, color.R, color.G, color.B, 255);
            Function.Call(Hash.SET_TEXT_SCALE, scale, 0.2f);
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, text);
            Function.Call(Hash._DRAW_TEXT, screeninfo.X, screeninfo.Y);
        }
        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public static bool InTaskSequence(Ped ped)
        {
            return Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS,ped) != -1;

        }
        public static Vector2 World3DToScreen2d(Vector3 pos)
        {
            var x2dp = new OutputArgument();
            var y2dp = new OutputArgument();

            Function.Call<bool>(Hash._WORLD3D_TO_SCREEN2D, pos.X, pos.Y, pos.Z, x2dp, y2dp);
            return new Vector2(x2dp.GetResult<float>(), y2dp.GetResult<float>());
        }

        public static void DisplayHelpTextThisFrame(string text)
        {
            Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, "STRING");
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, text);
            Function.Call(Hash._DISPLAY_HELP_TEXT_FROM_STRING_LABEL, 0, false, false, -1);
        }

        public static void DisplayHelpTextThisFramed(char text)
        {
            Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, "STRING");
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, text);
            Function.Call(Hash._DISPLAY_HELP_TEXT_FROM_STRING_LABEL, 0, false, false, -1);
        }

        public static bool ThrowDice(int minPercent)
        {
            return RandomInt(0, 100) < minPercent;
        }
        public static double DiffBetweenAngles(double firstAngle, double secondAngle)
        {
            double difference = secondAngle - firstAngle;
            while (difference < -180) difference += 360;
            while (difference > 180) difference -= 360;
            return difference;
        }
        /*
        public static int RandomInt(int min, int max)
        {
            max++;
            return Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, min, max);
        }
        */
        public static Random rnd = new Random();
        public static int RandomInt(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public static Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
        {
            Vector3 P = x * Vector3.Normalize(B - A) + A;
            return P;
        }

        public static int GetBoneIndex(Entity entity, string value)
        {
            return GTA.Native.Function.Call<int>(Hash._0xFB71170B7E76ACBA, entity, value);
        }
        public static Vector3 GetBoneCoords(Entity entity, int boneIndex)
        {
            return GTA.Native.Function.Call<Vector3>(Hash._0x44A8FCB8ED227738, entity, boneIndex);
        }
        public static List<string> Exhausts = new List<string> { "exhaust","exhaust_2","exhaust_3","exhaust_4","exhaust_5","exhaust_6","exhaust_7"   };

        public static float ForwardSpeed(Vehicle veh)
        {
            if (CanWeUse(veh)) return Function.Call<Vector3>(Hash.GET_ENTITY_SPEED_VECTOR, veh, true).Y; else return 0;
        }

        public static void ForceNitro(Vehicle veh)
        {
            if (CanWeUse(veh))
            {
                Function.Call(Hash._SET_VEHICLE_ENGINE_TORQUE_MULTIPLIER, veh, 30f);

                if (Function.Call<bool>(Hash._0x8702416E512EC454, "scr_carsteal4"))
                {
                    float direction = veh.Heading;
                    float pitch = Function.Call<float>(Hash.GET_ENTITY_PITCH, veh);

                    foreach (string exhaust in Exhausts)
                    {
                        Vector3 offset = GetBoneCoords(veh, GetBoneIndex(veh, exhaust));
                        Function.Call(Hash._0x6C38AF3693A69A91, "scr_carsteal4");
                        Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_carsteal5_car_muzzle_flash", offset.X, offset.Y, offset.Z, 0f, pitch, direction - 90f, 1.0f, false, false, false);
                    }
                }
                else
                {
                    Function.Call(Hash._0xB80D8756B4668AB6, "scr_carsteal4");
                }
            }
        }

        public static bool IsNightTime()
        {
            return (World.CurrentDayTime.Hours > 20 || World.CurrentDayTime.Hours < 7);
        }

        public static string GetInstructionalButton(GTA.Control key)
        {//*GET_CONTROL_INSTRUCTIONAL_BUTTON(int inputGroup, int control, BOOL p2) 
         // return Function.Call<string>(Hash._GET_CONTROL_ACTION_NAME, 2, (int)key, true);
            return "~"+((InstructionalButtonName)key).ToString()+"~";
        }
        public static void RandomTuning(Vehicle veh, bool neons, bool horn)
        {
            Function.Call(Hash.SET_VEHICLE_MOD_KIT, veh, 0);

            //Change color
            var color = Enum.GetValues(typeof(VehicleColor));
            Random random = new Random();
            veh.PrimaryColor = (VehicleColor)color.GetValue(random.Next(color.Length));

            Random random2 = new Random();
            veh.SecondaryColor = (VehicleColor)color.GetValue(random2.Next(color.Length));

            if (veh.LiveryCount > 0) veh.Livery = RandomInt(0, veh.LiveryCount);

            //Change tuning parts
            foreach (int mod in Enum.GetValues(typeof(VehicleMod)).Cast<VehicleMod>())
            {

                if (mod == (int)VehicleMod.Horns && !horn) continue;
                veh.SetMod((VehicleMod)mod, RandomInt(0, veh.GetModCount((VehicleMod)mod)), false);
            }

            if (neons)
            {

                //Color neoncolor = Color.FromArgb(0, Util.GetRandomInt(0, 255), Util.GetRandomInt(0, 255), Util.GetRandomInt(0, 255));

                Color neoncolor = Color.FromKnownColor((KnownColor)RandomInt(0, Enum.GetValues(typeof(KnownColor)).Cast<KnownColor>().Count()));
                veh.NeonLightsColor = neoncolor;

                veh.SetNeonLightsOn(VehicleNeonLight.Front, true);
                veh.SetNeonLightsOn(VehicleNeonLight.Back, true);
                veh.SetNeonLightsOn(VehicleNeonLight.Left, true);
                veh.SetNeonLightsOn(VehicleNeonLight.Right, true);

            }
        }
        public static float Clearance(Vehicle veh,bool back)
        {
            Vector3 ground;
            RaycastResult clearance;
            if (back)
            {
                Vector3 offset = Util.GetEntityOffset(veh, -(int)veh.Model.GetDimensions().Y/2, 0);
                ground = Util.ToGround(offset, 5f);
                clearance = World.Raycast(ground, offset + new Vector3(0, 0, 5), IntersectOptions.Everything);

            }
            else
            {
                 ground = Util.ToGround(veh.Position + new Vector3(0, 0, 1), 5f);
                clearance = World.Raycast(ground, veh.Position + new Vector3(0, 0, 5), IntersectOptions.Everything);

            }
            //Height


            DrawLine(ground, clearance.HitCoords);
            float realheight = clearance.HitCoords.DistanceTo(ground);

            if (realheight > 5) realheight = 0;
            return realheight;
        }

        public static bool IsSliding(Vehicle veh, float threshold)
        {
            return Math.Abs(Function.Call<Vector3>(Hash.GET_ENTITY_SPEED_VECTOR, veh, true).X) > threshold;
        }
        public static string SaveDriverToFile(string name, Ped ped, Vehicle veh, string filePath, bool SavePerfMods, bool SaveVisualMods, string kind, int avgShiftingSkill, int avgReactionTime)
        {
            UI.Notify(filePath);
            string error = "";
            if (!File.Exists(filePath))
            {
                //File.Create(@"scripts\\" + filePath);
                File.WriteAllText(filePath, "<Racers></Racers>");
            }


            XmlDocument originalXml = new XmlDocument();
            originalXml.Load(filePath);

            XmlNode changes = originalXml.SelectSingleNode("//Racers");

            if(changes==null)  changes = originalXml.SelectSingleNode("//Drivers");

            XmlNode Driver = originalXml.CreateNode(XmlNodeType.Element, "Driver", null);

            XmlAttribute DriverName = originalXml.CreateAttribute("Name");
            DriverName.InnerText = name;
            Driver.Attributes.Append(DriverName);


            XmlAttribute Shifting = originalXml.CreateAttribute("Shifting");
            Shifting.InnerText = avgShiftingSkill.ToString();
            Driver.Attributes.Append(Shifting);

            XmlAttribute Reaction = originalXml.CreateAttribute("Reaction");
            Reaction.InnerText = avgReactionTime.ToString();
            Driver.Attributes.Append(Reaction);

            if (ped == null)
            {
                XmlElement PedModel = originalXml.CreateElement("PedModel");
                PedModel.InnerText = "random";
                Driver.AppendChild(PedModel);
            }
            else
            {
                XmlElement PedModel = originalXml.CreateElement("PedModel");
                PedModel.InnerText = ped.Model.Hash.ToString();
                Driver.AppendChild(PedModel);
            }


            //Kind of vehicle, gotten from the Meet itself
            XmlElement RacesAllowed = originalXml.CreateElement("RacesAllowed");
            XmlElement Kind;
            Kind = originalXml.CreateElement("Kind");
            Kind.InnerText = kind;
            RacesAllowed.AppendChild(Kind);

            Driver.AppendChild(RacesAllowed);
            /*
            XmlAttribute Class = originalXml.CreateAttribute("Class");

            if (ClassList[CarClass.Index] == "Defined by player")
            {
                UI.Notify("~b~[Oponent Creator]~w~: Set the vehicle's racing Class.~n~~r~Case sensitive.");
                Class.InnerText = Game.GetUserInput(veh.ClassType.ToString(), 40);
            }
            else
            {
                Class.InnerText = ClassList[CarClass.Index];
            }

            Driver.Attributes.Append(Class);
            */


            XmlNode Data = originalXml.CreateNode(XmlNodeType.Element, "Vehicle", null);

            XmlElement Model = originalXml.CreateElement("Model");
            Model.InnerText = veh.Model.Hash.ToString();


            XmlAttribute Name = originalXml.CreateAttribute("Name");
            if (veh.FriendlyName != "") Name.InnerText = veh.FriendlyName; else Name.InnerText = veh.DisplayName;
            Model.Attributes.Append(Name);

            Data.AppendChild(Model);
            if (SaveVisualMods)
            {

                XmlElement Wheeltype = originalXml.CreateElement("WheelType");
                Wheeltype.InnerText = ((int)veh.WheelType).ToString();
                Data.AppendChild(Wheeltype);


                XmlElement TrimColor = originalXml.CreateElement("TrimColor");
                TrimColor.InnerText = ((int)veh.TrimColor).ToString();
                Data.AppendChild(TrimColor);

                XmlElement DashColor = originalXml.CreateElement("DashColor");
                DashColor.InnerText = ((int)veh.DashboardColor).ToString();
                Data.AppendChild(DashColor);

                XmlElement PrimaryColor = originalXml.CreateElement("PrimaryColor");
                PrimaryColor.InnerText = ((int)veh.PrimaryColor).ToString();
                Data.AppendChild(PrimaryColor);

                XmlElement SecondaryColor = originalXml.CreateElement("SecondaryColor");
                SecondaryColor.InnerText = ((int)veh.SecondaryColor).ToString();
                Data.AppendChild(SecondaryColor);

                XmlElement PearlescentColor = originalXml.CreateElement("PearlescentColor");
                PearlescentColor.InnerText = ((int)veh.PearlescentColor).ToString();
                Data.AppendChild(PearlescentColor);


                XmlElement RimColor = originalXml.CreateElement("RimColor");
                RimColor.InnerText = ((int)veh.RimColor).ToString();
                Data.AppendChild(RimColor);


                XmlElement LicensePlate = originalXml.CreateElement("LicensePlate");
                LicensePlate.InnerText = ((int)veh.NumberPlateType).ToString();
                Data.AppendChild(LicensePlate);

                XmlElement LicensePlateText = originalXml.CreateElement("LicensePlateText");
                LicensePlateText.InnerText = veh.NumberPlate;
                Data.AppendChild(LicensePlateText);


                XmlElement WindowsTint = originalXml.CreateElement("WindowsTint");
                WindowsTint.InnerText = ((int)veh.WindowTint).ToString();
                Data.AppendChild(WindowsTint);

                XmlElement Livery = originalXml.CreateElement("Livery");
                Livery.InnerText = ((int)veh.Livery).ToString();
                Data.AppendChild(Livery);


                XmlElement Components = originalXml.CreateElement("Components");

                for (int i = 0; i <= 25; i++)
                {
                    XmlElement Component = originalXml.CreateElement("Component");

                    XmlAttribute Attribute = originalXml.CreateAttribute("ComponentIndex");
                    Attribute.InnerText = i.ToString();
                    Component.Attributes.Append(Attribute);

                    if (Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, veh, i))
                    {
                        Component.InnerText = "1";
                    }
                    else
                    {
                        Component.InnerText = "0";
                    }
                    Components.AppendChild(Component);
                }
                Data.AppendChild(Components);

                //SMOKE COLOR GOES HERE

                //CUSTOM TYRES GO HERE
                XmlElement CustomTires = originalXml.CreateElement("CustomTires");

                CustomTires.InnerText = Util.HasCustomTires(veh).ToString();
                Data.AppendChild(CustomTires);


                //NEON COLORS GO HERE
                XmlElement Neons = originalXml.CreateElement("Neons");

                XmlElement NeonInfo = originalXml.CreateElement("Left");
                NeonInfo.InnerText = ((bool)veh.IsNeonLightsOn(VehicleNeonLight.Left)).ToString();
                Neons.AppendChild(NeonInfo);

                NeonInfo = originalXml.CreateElement("Right");
                NeonInfo.InnerText = ((bool)veh.IsNeonLightsOn(VehicleNeonLight.Right)).ToString();
                Neons.AppendChild(NeonInfo);

                NeonInfo = originalXml.CreateElement("Front");
                NeonInfo.InnerText = ((bool)veh.IsNeonLightsOn(VehicleNeonLight.Front)).ToString();
                Neons.AppendChild(NeonInfo);

                NeonInfo = originalXml.CreateElement("Back");
                NeonInfo.InnerText = ((bool)veh.IsNeonLightsOn(VehicleNeonLight.Back)).ToString();
                Neons.AppendChild(NeonInfo);

                Data.AppendChild(Neons);


                XmlElement NeonColor = originalXml.CreateElement("NeonColor");
                XmlElement Color = originalXml.CreateElement("R");
                Color.InnerText = veh.NeonLightsColor.R.ToString();
                NeonColor.AppendChild(Color);

                Color = originalXml.CreateElement("G");
                Color.InnerText = veh.NeonLightsColor.G.ToString();
                NeonColor.AppendChild(Color);

                Color = originalXml.CreateElement("B");
                Color.InnerText = veh.NeonLightsColor.B.ToString();
                NeonColor.AppendChild(Color);

                Data.AppendChild(NeonColor);

            }
            if (SavePerfMods)
            {
                XmlElement ModToggles = originalXml.CreateElement("ModToggles");

                for (int i = 0; i <= 25; i++)
                {
                    XmlElement Component = originalXml.CreateElement("Toggle");

                    XmlAttribute Attribute = originalXml.CreateAttribute("ToggleIndex");
                    Attribute.InnerText = i.ToString();
                    Component.Attributes.Append(Attribute);

                    if (Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, veh, i))
                    {
                        Component.InnerText = "true";
                        ModToggles.AppendChild(Component);
                    }
                }
                Data.AppendChild(ModToggles);

                XmlElement Mods = originalXml.CreateElement("Mods");
                for (int i = 0; i <= 500; i++)
                {
                    XmlElement Component = originalXml.CreateElement("Mod");

                    XmlAttribute Attribute = originalXml.CreateAttribute("ModIndex");
                    Attribute.InnerText = i.ToString();
                    Component.Attributes.Append(Attribute);

                    if (Function.Call<int>(Hash.GET_VEHICLE_MOD, veh, i) != -1)
                    {
                        Component.InnerText = Function.Call<int>(Hash.GET_VEHICLE_MOD, veh, i).ToString();
                        Mods.AppendChild(Component);
                    }
                }
                Data.AppendChild(Mods);
            }

            Driver.AppendChild(Data);

            changes.AppendChild(Driver);

            originalXml.Save(filePath);

            return error;
        }


        public static bool SimilarPerformance(Racer first, Racer second, float threshold)
        {            
            return Math.Abs(first.PerformancePoints - second.PerformancePoints) < threshold;
        }

        public static bool SimilarPerformance(Racer first, Racer second, float threshold, float cheatfirst,float cheatsecond)
        {
            return Math.Abs((first.PerformancePoints+cheatfirst) - (second.PerformancePoints+cheatsecond)) < threshold;
        }
        public static bool SimilarRep(Racer first, Racer second, float threshold)
        {
            return Math.Abs(first.ReputationBalance() - second.ReputationBalance()) < threshold;
        }

        public static bool WasCheatStringJustEntered(string cheat)
        {
            return Function.Call<bool>(Hash._0x557E43C447E700A8, Game.GenerateHash(cheat));
        }
        public static bool HasCustomTires(Vehicle veh)
        {
            return Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, veh, 23);
        }
        public static float CalculatePerformancePoints(Vehicle veh)
        {
            float Points = 0;
            float maxspeed = (float)Math.Round(Function.Call<float>(Hash._GET_VEHICLE_MAX_SPEED, veh.Model.Hash)) * 2.2f; //converted to MPH
            float acceleration = (float)Math.Round(Function.Call<float>(Hash.GET_VEHICLE_MODEL_ACCELERATION, veh.Model.Hash));
            int EMSUpgrades = veh.GetMod(VehicleMod.Engine);
            int Transmission = veh.GetMod(VehicleMod.Transmission);
            if (veh.IsToggleModOn(VehicleToggleMod.Turbo)) Points += 3;

            Points += maxspeed*0.5f;
            Points += acceleration*10;
            Points += EMSUpgrades*2;
            Points += Transmission*2;

            return Points;
        }

        public static float CalculateAcc(Vehicle veh)
        {
            float Points = 0;
            float acceleration = (float)Math.Round(Function.Call<float>(Hash.GET_VEHICLE_MODEL_ACCELERATION, veh.Model.Hash));
            Points += acceleration * 10;
            return Points;
        }

        public static float CalculateTopSpeed(Vehicle veh)
        {
            float Points = 0;
            float maxspeed = (float)Math.Round(Function.Call<float>(Hash._GET_VEHICLE_MAX_SPEED, veh.Model.Hash)) * 2.2f; //converted to MPH
            float acceleration = (float)Math.Round(Function.Call<float>(Hash.GET_VEHICLE_MODEL_ACCELERATION, veh.Model.Hash));
            int EMSUpgrades = veh.GetMod(VehicleMod.Engine);
            int Transmission = veh.GetMod(VehicleMod.Transmission);
            if (veh.IsToggleModOn(VehicleToggleMod.Turbo)) Points += 3;

            Points += maxspeed * 0.5f;
            Points += acceleration * 10;
            Points += EMSUpgrades * 2;
            Points += Transmission * 2;

            return Points;
        }

        //Notifications

        public static List<String> MessageQueue = new List<String>();
        public static int MessageQueueInterval = 8000;
        public static int MessageQueueReferenceTime = 0;
        public static void HandleMessages()
        {
            if (MessageQueue.Count > 0)
            {
                DisplayHelpTextThisFrame(MessageQueue[0]);
            }
            else
            {
                MessageQueueReferenceTime = Game.GameTime;
            }

            if (Game.GameTime > MessageQueueReferenceTime + MessageQueueInterval)
            {
                if (MessageQueue.Count > 0)
                {
                    MessageQueue.RemoveAt(0);
                }
                MessageQueueReferenceTime = Game.GameTime;
            }
        }
        public static void AddQueuedHelpText(string text)
        {
            if (!MessageQueue.Contains(text)) MessageQueue.Add(text);
        }

        public static void ClearAllHelpText(string text)
        {
            MessageQueue.Clear();
        }


        public static List<String> NotificationQueueText = new List<String>();
        public static List<String> NotificationQueueAvatar = new List<String>();
        public static List<String> NotificationQueueAuthor = new List<String>();
        public static List<String> NotificationQueueTitle = new List<String>();

        public static int NotificationQueueInterval = 8000;
        public static int NotificationQueueReferenceTime = 0;
        public static void HandleNotifications()
        {
            if (Game.GameTime > NotificationQueueReferenceTime)
            {
                if (NotificationQueueAvatar.Count > 0 && NotificationQueueText.Count > 0 && NotificationQueueAuthor.Count > 0 && NotificationQueueTitle.Count > 0)
                {

                    int Moretime = ((int)(NotificationQueueText[0].Length * 0.1f) * 1000);
                    if (Moretime > 5000) Moretime = 5000;
                    NotificationQueueReferenceTime = Game.GameTime + Moretime;
                    Notify(NotificationQueueAvatar[0], NotificationQueueAuthor[0], NotificationQueueTitle[0], NotificationQueueText[0]);
                    NotificationQueueText.RemoveAt(0);
                    NotificationQueueAvatar.RemoveAt(0);
                    NotificationQueueAuthor.RemoveAt(0);
                    NotificationQueueTitle.RemoveAt(0);
                }
            }
        }

        public static void AddNotification(string avatar, string author, string title, string text)
        {
            NotificationQueueText.Add(text);
            NotificationQueueAvatar.Add(avatar);
            NotificationQueueAuthor.Add(author);
            NotificationQueueTitle.Add(title);
        }
        public static void CleanNotifications()
        {
            NotificationQueueText.Clear();
            NotificationQueueAvatar.Clear();
            NotificationQueueAuthor.Clear();
            NotificationQueueTitle.Clear();
            NotificationQueueReferenceTime = Game.GameTime;
            Function.Call(Hash._REMOVE_NOTIFICATION, CurrentNotification);
        }

        public static int CurrentNotification;
        public static void Notify(string avatar, string author, string title, string message)
        {
            if (avatar != "" && author != "" && title != "")
            {
                Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "STRING");
                Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, message);
                CurrentNotification = Function.Call<int>(Hash._SET_NOTIFICATION_MESSAGE, avatar, avatar, true, 0, title, author);
            }
            else
            {
                UI.Notify(message);
            }
        }
    }
}
