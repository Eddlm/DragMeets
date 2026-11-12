using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Drag_Meets_Reborn
{
public class Util
{
	public static List<Model> CannotWheelie = new List<Model>
	{
		"PRAIRIE",
		"PENUMBRA",
		"BLISTA",
		"BLISTA2",
		"FUTO",
		"ASEA",
		"SURGE",
		"PREMIER",
		"INGOT",
		"ISSI",
		"DILETTANTE",
		"PIGALLE"
	};

	public static List<string> ChatCurse = new List<string> { "GENERIC_CURSE_MED", "GENERIC_CURSE_HIGH", "GENERIC_FUCK_YOU" };

	public static List<string> ChatTaunt = new List<string> { "KILLED_ALL", "KILLED_ALL", "KILLED_ALL" };

	public static List<string> ChatThanks = new List<string> { "GENERIC_THANKS", "GENERIC_THANKS", "GENERIC_THANKS" };

	public static List<string> ChatNiceCar = new List<string> { "NICE_CAR_RANDOM", "NICE_CAR_01", "NICE_CAR_SHOUT_RANDOM", "NICE_CAR_SHOUT_01", "SEE_WEIRDO_RANDOM " };

	public static Random rnd = new Random();

	public static List<string> Exhausts = new List<string> { "exhaust", "exhaust_2", "exhaust_3", "exhaust_4", "exhaust_5", "exhaust_6", "exhaust_7" };

	public static List<string> MessageQueue = new List<string>();

	public static int MessageQueueInterval = 8000;

	public static int MessageQueueReferenceTime = 0;

	public static List<string> NotificationQueueText = new List<string>();

	public static List<string> NotificationQueueAvatar = new List<string>();

	public static List<string> NotificationQueueAuthor = new List<string>();

	public static List<string> NotificationQueueTitle = new List<string>();

	public static int NotificationQueueInterval = 8000;

	public static int NotificationQueueReferenceTime = 0;

	public static int CurrentNotification;

	public static void ChangePedMoney(Ped ped, int money)
	{
		if (ped.IsPlayer)
		{
			Player player = Game.Player;
			player.Money += money;
		}
		else
		{
			ped.Money += money;
		}
		if (ped.IsPlayer)
		{
			if (Game.Player.Money < 0)
			{
				Game.Player.Money = 0;
			}
			else if (ped.Money < 0)
			{
				ped.Money = 0;
			}
		}
	}

	public static int PedMoney(Ped ped)
	{
		if (ped.IsPlayer)
		{
			return Game.Player.Money;
		}
		return ped.Money;
	}

	private Util()
	{
	}

	public static int RotationalVelocity(Entity ent)
	{
		return Function.Call<int>((Hash)2394667074804365699L, new InputArgument[1] { ent });
	}

	public static void PlayAmbientSpeech(Ped ped, List<string> speechlist)
	{
		Function.Call((Hash)unchecked((ulong)(-8213159594590722974L)), new InputArgument[3]
		{
			((Entity)ped).Handle,
			speechlist[RandomInt(0, speechlist.Count)],
			"SPEECH_PARAMS_FORCE"
		});
	}

	public static void SetRadioLoud(Vehicle veh, string radio)
	{
		Function.Call((Hash)2636792098548582430L, new InputArgument[3]
		{
			veh,
			true,
			true
		});
		Function.Call((Hash)4294324703405435915L, new InputArgument[2]
		{
			veh,
			true
		});
		Function.Call((Hash)unchecked((ulong)(-4940698729221256242L)), new InputArgument[2]
		{
			veh,
			true
		});
		Function.Call((Hash)1989465795936398022L, new InputArgument[2]
		{
			veh,
			radio
		});
	}

	public static void SetRadioQuiet(Vehicle veh)
	{
		Function.Call((Hash)2636792098548582430L, new InputArgument[3]
		{
			veh,
			true,
			true
		});
		Function.Call((Hash)unchecked((ulong)(-4940698729221256242L)), new InputArgument[2]
		{
			veh,
			false
		});
		Function.Call((Hash)4294324703405435915L, new InputArgument[2]
		{
			veh,
			true
		});
	}

	public static Entity EntitySeenByPlayer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		RaycastResult val = World.Raycast(GameplayCamera.Position + GameplayCamera.Direction * 10f, ((Entity)Game.Player.Character).Position, (IntersectOptions)(-1));
		if (CanWeUse(val.HitEntity))
		{
			return val.HitEntity;
		}
		return null;
	}

	public static bool CanWeUse(Entity entity)
	{
		if (entity != (Entity)null)
		{
			return entity.Exists();
		}
		return false;
	}

	public static bool IsSubttaskActive(Ped ped, Subtask task)
	{
		return Function.Call<bool>((Hash)unchecked((ulong)(-5731389963444272811L)), new InputArgument[2]
		{
			ped,
			(int)task
		});
	}

	public static bool CarCanSeePos(Vehicle veh, Vector3 pos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Entity)veh).Position;
		if (position.DistanceTo(pos) < 100f)
		{
			RaycastResult val = World.Raycast(((Entity)veh).Position + new Vector3(0f, 0f, 2f), pos + new Vector3(0f, 0f, 2f), (IntersectOptions)1);
			if (!val.DitHitAnything)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsPointOnRoad(Vector3 pos, Entity ent)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		return Function.Call<bool>((Hash)1322919935073282825L, new InputArgument[4]
		{
			pos.X,
			pos.Y,
			pos.Z,
			ent
		});
	}

	public static bool IsDriving(Ped ped)
	{
		if (!ped.IsOnFoot && ped.TaskSequenceProgress != -1)
		{
			return true;
		}
		if (ped.IsPlayer)
		{
			return false;
		}
		foreach (Subtask item in new List<Subtask> { Subtask.CTaskControlVehicle })
		{
			if (Function.Call<bool>((Hash)unchecked((ulong)(-5731389963444272811L)), new InputArgument[2]
			{
				ped,
				(int)item
			}))
			{
				return true;
			}
		}
		return false;
	}

	public static Vector3 GetPosAheadOf(Entity ent, int distance)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return ent.Position + ent.ForwardVector * (float)distance;
	}

	public static Vector3 GetPosRightOf(Entity ent, int distance)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return ent.Position + ent.RightVector * (float)distance;
	}

	public static Vector3 GetEntityOffset(Entity ent, float ahead, float right)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return ent.Position + ent.ForwardVector * ahead + ent.RightVector * right;
	}

	public static void DrawLine(Vector3 from, Vector3 to)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		Function.Call((Hash)7742345298724472448L, new InputArgument[10]
		{
			from.X,
			from.Y,
			from.Z,
			to.X,
			to.Y,
			to.Z,
			255,
			255,
			0,
			255
		});
	}

	public static Vector3 ToGround(Vector3 pos, float zCheck)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		RaycastResult val = World.Raycast(pos + new Vector3(0f, 0f, zCheck), pos + new Vector3(0f, 0f, 0f - zCheck), (IntersectOptions)1);
		Vector3 hitCoords = val.HitCoords;
		if (hitCoords == Vector3.Zero)
		{
			return pos;
		}
		return hitCoords;
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

	public static void HaltVehicle(Ped ped)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		Vehicle currentVehicle = ped.CurrentVehicle;
		if (CanWeUse((Entity)(object)currentVehicle))
		{
			Vector3 val = ToGround(((Entity)currentVehicle).ForwardVector * (currentVehicle.Speed * 5f), 5f);
			if (val == Vector3.Zero)
			{
				val = ((Entity)currentVehicle).ForwardVector * (currentVehicle.Speed * 5f);
			}
			ped.Task.DriveTo(currentVehicle, val, 10f, 2f, 16777216);
		}
	}

	public static double AngleBetween(Vector3 u, Vector3 v, bool returndegrees)
	{
		double num = 0.0;
		for (int i = 0; i < 3; i++)
		{
			num += (double)(u[i] * v[i]);
		}
		double num2 = 0.0;
		double num3 = 0.0;
		for (int j = 0; j < 3; j++)
		{
			num2 += (double)(u[j] * u[j]);
			num3 += (double)(v[j] * v[j]);
		}
		double num4 = 0.0;
		num4 = Math.Sqrt(num2 * num3);
		double num5 = Math.Acos(num / num4);
		if (returndegrees)
		{
			num5 *= 180.0 / Math.PI;
		}
		return num5;
	}

	public static double AngleBetween(Vector3 vector1, Vector3 vector2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		double y = vector1.X * vector2.Y - vector2.X * vector1.Y;
		double x = vector1.X * vector2.X + vector1.Y * vector2.Y;
		return Math.Atan2(y, x) * (180.0 / Math.PI);
	}

	public static double RadianToDegree(double angle)
	{
		return angle * (180.0 / Math.PI);
	}

	public static double ConvertToRadians(double angle)
	{
		return Math.PI / 180.0 * angle;
	}

	public static bool IsPosAheadentity(Entity ent, Vector3 pos, float threshold)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		return Function.Call<Vector3>((Hash)2482816124249826099L, new InputArgument[4]
		{
			ent,
			pos.X,
			pos.Y,
			pos.Z
		}).Y > 0f + threshold;
	}

	public static bool IsPosRightOf(Entity ent, Vector3 pos, float threshold)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		return Function.Call<Vector3>((Hash)2482816124249826099L, new InputArgument[4]
		{
			ent,
			pos.X,
			pos.Y,
			pos.Z
		}).X > 0f + threshold;
	}

	public static void CalcReputation(int wins, int loses)
	{
	}

	public static void TempAction(Ped ped, Vehicle veh, int action, int time)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected Obj, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Entity)veh).Position;
		TaskSequence val = new TaskSequence();
		Function.Call((Hash)unchecked((ulong)(-4311672250463297239L)), new InputArgument[4]
		{
			0,
			veh,
			action,
			time
		});
		Function.Call((Hash)1552531582173918732L, new InputArgument[8]
		{
			0,
			veh,
			position.X,
			position.Y,
			position.Z,
			200f,
			4194304,
			250f
		});
		val.Close();
		ped.Task.PerformSequence(val);
		val.Dispose();
	}

	public static void DrawTextHere(string text, float scale, Vector3 pos, Color color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = World3DToScreen2d(pos + new Vector3(0f, 0f, 1.5f));
		Function.Call((Hash)2736978246810207435L, new InputArgument[1] { "STRING" });
		Function.Call((Hash)unchecked((ulong)(-4598371208749279093L)), new InputArgument[1] { true });
		Function.Call((Hash)unchecked((ulong)(-4725643803099155390L)), new InputArgument[4]
		{
			color.R,
			color.G,
			color.B,
			255
		});
		Function.Call((Hash)560759698880214217L, new InputArgument[2]
		{
			scale,
			0.2f
		});
		Function.Call((Hash)7789129354908300458L, new InputArgument[1] { text });
		Function.Call((Hash)unchecked((ulong)(-3674552073055540649L)), new InputArgument[2]
		{
			val.X,
			val.Y
		});
	}

	public static bool IsOdd(int value)
	{
		return value % 2 != 0;
	}

	public static bool InTaskSequence(Ped ped)
	{
		return Function.Call<int>((Hash)47570426378728755L, new InputArgument[1] { ped }) != -1;
	}

	public static Vector2 World3DToScreen2d(Vector3 pos)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected Obj, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		OutputArgument val = new OutputArgument();
		OutputArgument val2 = new OutputArgument();
		Function.Call<bool>((Hash)3812348786834502901L, new InputArgument[5]
		{
			pos.X,
			pos.Y,
			pos.Z,
			(InputArgument)val,
			(InputArgument)val2
		});
		return new Vector2(val.GetResult<float>(), val2.GetResult<float>());
	}

	public static void DisplayHelpTextThisFrame(string text)
	{
		Function.Call((Hash)unchecked((ulong)(-8860350453193909743L)), new InputArgument[1] { "STRING" });
		Function.Call((Hash)7789129354908300458L, new InputArgument[1] { text });
		Function.Call((Hash)2562546386151446694L, new InputArgument[4]
		{
			0,
			false,
			false,
			-1
		});
	}

	public static void DisplayHelpTextThisFramed(char text)
	{
		Function.Call((Hash)unchecked((ulong)(-8860350453193909743L)), new InputArgument[1] { "STRING" });
		Function.Call((Hash)7789129354908300458L, new InputArgument[1] { (ushort)text });
		Function.Call((Hash)2562546386151446694L, new InputArgument[4]
		{
			0,
			false,
			false,
			-1
		});
	}

	public static bool ThrowDice(int minPercent)
	{
		return RandomInt(0, 100) < minPercent;
	}

	public static double DiffBetweenAngles(double firstAngle, double secondAngle)
	{
		double num;
		for (num = secondAngle - firstAngle; num < -180.0; num += 360.0)
		{
		}
		while (num > 180.0)
		{
			num -= 360.0;
		}
		return num;
	}

	public static int RandomInt(int min, int max)
	{
		return rnd.Next(min, max);
	}

	public static Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return x * Vector3.Normalize(B - A) + A;
	}

	public static int GetBoneIndex(Entity entity, string value)
	{
		return Function.Call<int>((Hash)unchecked((ulong)(-328455959687549766L)), new InputArgument[2]
		{
			entity,
			value
		});
	}

	public static Vector3 GetBoneCoords(Entity entity, int boneIndex)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		return Function.Call<Vector3>((Hash)4947482061849130808L, new InputArgument[2]
		{
			entity,
			boneIndex
		});
	}

	public static float ForwardSpeed(Vehicle veh)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (CanWeUse((Entity)(object)veh))
		{
			return Function.Call<Vector3>((Hash)unchecked((ulong)(-7310063430528173299L)), new InputArgument[2]
			{
				veh,
				true
			}).Y;
		}
		return 0f;
	}

	public static void ForceNitro(Vehicle veh)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		if (!CanWeUse((Entity)(object)veh))
		{
			return;
		}
		Function.Call((Hash)unchecked((ulong)(-5359763134852459813L)), new InputArgument[2]
		{
			veh,
			30f
		});
		if (Function.Call<bool>((Hash)unchecked((ulong)(-8718333986571631532L)), new InputArgument[1] { "scr_carsteal4" }))
		{
			float heading = ((Entity)veh).Heading;
			float num = Function.Call<float>((Hash)unchecked((ulong)(-3144143070260239874L)), new InputArgument[1] { veh });
			{
				foreach (string exhaust in Exhausts)
				{
					Vector3 boneCoords = GetBoneCoords((Entity)(object)veh, GetBoneIndex((Entity)(object)veh, exhaust));
					Function.Call((Hash)7798175403732277905L, new InputArgument[1] { "scr_carsteal4" });
					Function.Call<int>((Hash)2671361570822135507L, new InputArgument[11]
					{
						"scr_carsteal5_car_muzzle_flash",
						boneCoords.X,
						boneCoords.Y,
						boneCoords.Z,
						0f,
						num,
						heading - 90f,
						1f,
						false,
						false,
						false
					});
				}
				return;
			}
		}
		Function.Call((Hash)unchecked((ulong)(-5184338789570016586L)), new InputArgument[1] { "scr_carsteal4" });
	}

	public static bool LastDriverIsPed(Vehicle veh, Ped p)
	{
		if (!CanWeUse((Entity)(object)veh))
		{
			return false;
		}
		Ped val = Function.Call<Ped>((Hash)unchecked((ulong)(-8936995806156511644L)), new InputArgument[2]
		{
			veh,
			-1
		});
		if (CanWeUse((Entity)(object)val) && CanWeUse((Entity)(object)p) && ((Entity)val).Handle == ((Entity)p).Handle)
		{
			return true;
		}
		return false;
	}

	public static bool NeverBeenDriven(Vehicle veh)
	{
		if (!CanWeUse((Entity)(object)Function.Call<Ped>((Hash)unchecked((ulong)(-8936995806156511644L)), new InputArgument[2]
		{
			veh,
			-1
		})))
		{
			return true;
		}
		return false;
	}

	public static bool IsNightTime()
	{
		if (World.CurrentDayTime.Hours <= 20)
		{
			return World.CurrentDayTime.Hours < 7;
		}
		return true;
	}

	public static string GetInstructionalButton(Control key)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected I4, but got Unknown
		return "~" + ((InstructionalButtonName)key/*cast due to constrained. prefix*/).ToString() + "~";
	}

	public static void RandomTuning(Vehicle veh, bool neons, bool horn)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected I4, but got Unknown
		Function.Call((Hash)2245783831530643834L, new InputArgument[2]
		{
			veh,
			0
		});
		Array values = Enum.GetValues(typeof(VehicleColor));
		Random random = new Random();
		veh.PrimaryColor = (VehicleColor)values.GetValue(random.Next(values.Length));
		Random random2 = new Random();
		veh.SecondaryColor = (VehicleColor)values.GetValue(random2.Next(values.Length));
		if (veh.LiveryCount > 0)
		{
			veh.Livery = RandomInt(0, veh.LiveryCount);
		}
		foreach (VehicleMod item in Enum.GetValues(typeof(VehicleMod)).Cast<VehicleMod>())
		{
			int num = (int)item;
			if (num != 14 || horn)
			{
				veh.SetMod((VehicleMod)num, RandomInt(0, veh.GetModCount((VehicleMod)num)), false);
			}
		}
		if (neons)
		{
			Color neonLightsColor = Color.FromKnownColor((KnownColor)RandomInt(0, Enum.GetValues(typeof(KnownColor)).Cast<KnownColor>().Count()));
			veh.NeonLightsColor = neonLightsColor;
			veh.SetNeonLightsOn((VehicleNeonLight)2, true);
			veh.SetNeonLightsOn((VehicleNeonLight)3, true);
			veh.SetNeonLightsOn((VehicleNeonLight)0, true);
			veh.SetNeonLightsOn((VehicleNeonLight)1, true);
		}
	}

	public static float Clearance(Vehicle veh, bool back)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val;
		RaycastResult val2;
		if (back)
		{
			Model model = ((Entity)veh).Model;
			Vector3 entityOffset = GetEntityOffset((Entity)(object)veh, -(int)model.GetDimensions().Y / 2, 0f);
			val = ToGround(entityOffset, 5f);
			val2 = World.Raycast(val, entityOffset + new Vector3(0f, 0f, 5f), (IntersectOptions)(-1));
		}
		else
		{
			val = ToGround(((Entity)veh).Position + new Vector3(0f, 0f, 1f), 5f);
			val2 = World.Raycast(val, ((Entity)veh).Position + new Vector3(0f, 0f, 5f), (IntersectOptions)(-1));
		}
		DrawLine(val, val2.HitCoords);
		Vector3 hitCoords = val2.HitCoords;
		float num = hitCoords.DistanceTo(val);
		if (num > 5f)
		{
			num = 0f;
		}
		return num;
	}

	public static bool IsSliding(Vehicle veh, float threshold)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		return Math.Abs(Function.Call<Vector3>((Hash)unchecked((ulong)(-7310063430528173299L)), new InputArgument[2]
		{
			veh,
			true
		}).X) > threshold;
	}

	public static string SaveDriverToFile(string name, Ped ped, Vehicle veh, string filePath, bool SavePerfMods, bool SaveVisualMods, string kind, int avgShiftingSkill, int avgReactionTime)
	{
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected I4, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected I4, but got Unknown
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Expected I4, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Expected I4, but got Unknown
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected I4, but got Unknown
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Expected I4, but got Unknown
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected I4, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected I4, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected I4, but got Unknown
		UI.Notify(filePath);
		string result = "";
		if (!File.Exists(filePath))
		{
			File.WriteAllText(filePath, "<Racers></Racers>");
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(filePath);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("//Racers");
		if (xmlNode == null)
		{
			xmlNode = xmlDocument.SelectSingleNode("//Drivers");
		}
		XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, "Driver", null);
		XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("Name");
		xmlAttribute.InnerText = name;
		xmlNode2.Attributes.Append(xmlAttribute);
		XmlAttribute xmlAttribute2 = xmlDocument.CreateAttribute("Shifting");
		xmlAttribute2.InnerText = avgShiftingSkill.ToString();
		xmlNode2.Attributes.Append(xmlAttribute2);
		XmlAttribute xmlAttribute3 = xmlDocument.CreateAttribute("Reaction");
		xmlAttribute3.InnerText = avgReactionTime.ToString();
		xmlNode2.Attributes.Append(xmlAttribute3);
		Model model;
		if ((Entity)(object)ped == (Entity)null)
		{
			XmlElement xmlElement = xmlDocument.CreateElement("PedModel");
			xmlElement.InnerText = "random";
			xmlNode2.AppendChild(xmlElement);
		}
		else
		{
			XmlElement xmlElement2 = xmlDocument.CreateElement("PedModel");
			model = ((Entity)ped).Model;
			xmlElement2.InnerText = model.Hash.ToString();
			xmlNode2.AppendChild(xmlElement2);
		}
		XmlElement xmlElement3 = xmlDocument.CreateElement("RacesAllowed");
		XmlElement xmlElement4 = xmlDocument.CreateElement("Kind");
		xmlElement4.InnerText = kind;
		xmlElement3.AppendChild(xmlElement4);
		xmlNode2.AppendChild(xmlElement3);
		XmlNode xmlNode3 = xmlDocument.CreateNode(XmlNodeType.Element, "Vehicle", null);
		XmlElement xmlElement5 = xmlDocument.CreateElement("Model");
		model = ((Entity)veh).Model;
		xmlElement5.InnerText = model.Hash.ToString();
		XmlAttribute xmlAttribute4 = xmlDocument.CreateAttribute("Name");
		if (veh.FriendlyName != "")
		{
			xmlAttribute4.InnerText = veh.FriendlyName;
		}
		else
		{
			xmlAttribute4.InnerText = veh.DisplayName;
		}
		xmlElement5.Attributes.Append(xmlAttribute4);
		xmlNode3.AppendChild(xmlElement5);
		if (SaveVisualMods)
		{
			XmlElement xmlElement6 = xmlDocument.CreateElement("WheelType");
			xmlElement6.InnerText = ((int)veh.WheelType).ToString();
			xmlNode3.AppendChild(xmlElement6);
			XmlElement xmlElement7 = xmlDocument.CreateElement("TrimColor");
			xmlElement7.InnerText = ((int)veh.TrimColor).ToString();
			xmlNode3.AppendChild(xmlElement7);
			XmlElement xmlElement8 = xmlDocument.CreateElement("DashColor");
			xmlElement8.InnerText = ((int)veh.DashboardColor).ToString();
			xmlNode3.AppendChild(xmlElement8);
			XmlElement xmlElement9 = xmlDocument.CreateElement("PrimaryColor");
			xmlElement9.InnerText = ((int)veh.PrimaryColor).ToString();
			xmlNode3.AppendChild(xmlElement9);
			XmlElement xmlElement10 = xmlDocument.CreateElement("SecondaryColor");
			xmlElement10.InnerText = ((int)veh.SecondaryColor).ToString();
			xmlNode3.AppendChild(xmlElement10);
			XmlElement xmlElement11 = xmlDocument.CreateElement("PearlescentColor");
			xmlElement11.InnerText = ((int)veh.PearlescentColor).ToString();
			xmlNode3.AppendChild(xmlElement11);
			XmlElement xmlElement12 = xmlDocument.CreateElement("RimColor");
			xmlElement12.InnerText = ((int)veh.RimColor).ToString();
			xmlNode3.AppendChild(xmlElement12);
			XmlElement xmlElement13 = xmlDocument.CreateElement("LicensePlate");
			xmlElement13.InnerText = ((int)veh.NumberPlateType).ToString();
			xmlNode3.AppendChild(xmlElement13);
			XmlElement xmlElement14 = xmlDocument.CreateElement("LicensePlateText");
			xmlElement14.InnerText = veh.NumberPlate;
			xmlNode3.AppendChild(xmlElement14);
			XmlElement xmlElement15 = xmlDocument.CreateElement("WindowsTint");
			xmlElement15.InnerText = ((int)veh.WindowTint).ToString();
			xmlNode3.AppendChild(xmlElement15);
			XmlElement xmlElement16 = xmlDocument.CreateElement("Livery");
			xmlElement16.InnerText = veh.Livery.ToString();
			xmlNode3.AppendChild(xmlElement16);
			XmlElement xmlElement17 = xmlDocument.CreateElement("Components");
			for (int i = 0; i <= 25; i++)
			{
				XmlElement xmlElement18 = xmlDocument.CreateElement("Component");
				XmlAttribute xmlAttribute5 = xmlDocument.CreateAttribute("ComponentIndex");
				xmlAttribute5.InnerText = i.ToString();
				xmlElement18.Attributes.Append(xmlAttribute5);
				if (Function.Call<bool>((Hash)unchecked((ulong)(-3249766948097570627L)), new InputArgument[2]
				{
					veh,
					i
				}))
				{
					xmlElement18.InnerText = "1";
				}
				else
				{
					xmlElement18.InnerText = "0";
				}
				xmlElement17.AppendChild(xmlElement18);
			}
			xmlNode3.AppendChild(xmlElement17);
			XmlElement xmlElement19 = xmlDocument.CreateElement("CustomTires");
			xmlElement19.InnerText = HasCustomTires(veh).ToString();
			xmlNode3.AppendChild(xmlElement19);
			XmlElement xmlElement20 = xmlDocument.CreateElement("Neons");
			XmlElement xmlElement21 = xmlDocument.CreateElement("Left");
			xmlElement21.InnerText = veh.IsNeonLightsOn((VehicleNeonLight)0).ToString();
			xmlElement20.AppendChild(xmlElement21);
			xmlElement21 = xmlDocument.CreateElement("Right");
			xmlElement21.InnerText = veh.IsNeonLightsOn((VehicleNeonLight)1).ToString();
			xmlElement20.AppendChild(xmlElement21);
			xmlElement21 = xmlDocument.CreateElement("Front");
			xmlElement21.InnerText = veh.IsNeonLightsOn((VehicleNeonLight)2).ToString();
			xmlElement20.AppendChild(xmlElement21);
			xmlElement21 = xmlDocument.CreateElement("Back");
			xmlElement21.InnerText = veh.IsNeonLightsOn((VehicleNeonLight)3).ToString();
			xmlElement20.AppendChild(xmlElement21);
			xmlNode3.AppendChild(xmlElement20);
			XmlElement xmlElement22 = xmlDocument.CreateElement("NeonColor");
			XmlElement xmlElement23 = xmlDocument.CreateElement("R");
			xmlElement23.InnerText = veh.NeonLightsColor.R.ToString();
			xmlElement22.AppendChild(xmlElement23);
			xmlElement23 = xmlDocument.CreateElement("G");
			xmlElement23.InnerText = veh.NeonLightsColor.G.ToString();
			xmlElement22.AppendChild(xmlElement23);
			xmlElement23 = xmlDocument.CreateElement("B");
			xmlElement23.InnerText = veh.NeonLightsColor.B.ToString();
			xmlElement22.AppendChild(xmlElement23);
			xmlNode3.AppendChild(xmlElement22);
		}
		if (SavePerfMods)
		{
			XmlElement xmlElement24 = xmlDocument.CreateElement("ModToggles");
			for (int j = 0; j <= 25; j++)
			{
				XmlElement xmlElement25 = xmlDocument.CreateElement("Toggle");
				XmlAttribute xmlAttribute6 = xmlDocument.CreateAttribute("ToggleIndex");
				xmlAttribute6.InnerText = j.ToString();
				xmlElement25.Attributes.Append(xmlAttribute6);
				if (Function.Call<bool>((Hash)unchecked((ulong)(-8884982314829051161L)), new InputArgument[2]
				{
					veh,
					j
				}))
				{
					xmlElement25.InnerText = "true";
					xmlElement24.AppendChild(xmlElement25);
				}
			}
			xmlNode3.AppendChild(xmlElement24);
			XmlElement xmlElement26 = xmlDocument.CreateElement("Mods");
			for (int k = 0; k <= 500; k++)
			{
				XmlElement xmlElement27 = xmlDocument.CreateElement("Mod");
				XmlAttribute xmlAttribute7 = xmlDocument.CreateAttribute("ModIndex");
				xmlAttribute7.InnerText = k.ToString();
				xmlElement27.Attributes.Append(xmlAttribute7);
				if (Function.Call<int>((Hash)8586499896144719835L, new InputArgument[2]
				{
					veh,
					k
				}) != -1)
				{
					xmlElement27.InnerText = Function.Call<int>((Hash)8586499896144719835L, new InputArgument[2]
					{
						veh,
						k
					}).ToString();
					xmlElement26.AppendChild(xmlElement27);
				}
			}
			xmlNode3.AppendChild(xmlElement26);
		}
		xmlNode2.AppendChild(xmlNode3);
		xmlNode.AppendChild(xmlNode2);
		xmlDocument.Save(filePath);
		return result;
	}

	public static bool SimilarPerformance(Racer first, Racer second, float threshold)
	{
		return Math.Abs(first.PerformancePoints - second.PerformancePoints) < threshold;
	}

	public static bool SimilarPerformance(Racer first, Racer second, float threshold, float cheatfirst, float cheatsecond)
	{
		return Math.Abs(first.PerformancePoints + cheatfirst - (second.PerformancePoints + cheatsecond)) < threshold;
	}

	public static bool SimilarRep(Racer first, Racer second, float threshold)
	{
		return Math.Abs(first.ReputationBalance() - second.ReputationBalance()) < threshold;
	}

	public static bool WasCheatStringJustEntered(string cheat)
	{
		return Function.Call<bool>((Hash)6160435850588389544L, new InputArgument[1] { Game.GenerateHash(cheat) });
	}

	public unsafe static int CurrentGear(Vehicle v)
	{
		ulong num = (ulong)((Entity)v).MemoryAddress;
		ulong num2 = 2032uL;
		return *(int*)(num + num2);
	}

	public static bool HasCustomTires(Vehicle veh)
	{
		return Function.Call<bool>((Hash)unchecked((ulong)(-5507252750051666468L)), new InputArgument[2]
		{
			veh,
			23
		});
	}

	public static float CalculatePerformancePoints(Vehicle veh)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		InputArgument[] array = new InputArgument[1];
		Model model = ((Entity)veh).Model;
		array[0] = model.Hash;
		float num2 = (float)Math.Round(Function.Call<float>((Hash)unchecked((ulong)(-858003554332316349L)), array)) * 2.2f;
		InputArgument[] array2 = new InputArgument[1];
		model = ((Entity)veh).Model;
		array2[0] = model.Hash;
		float num3 = (float)Math.Round(Function.Call<float>((Hash)unchecked((ulong)(-8357471048252236950L)), array2));
		int mod = veh.GetMod((VehicleMod)11);
		int mod2 = veh.GetMod((VehicleMod)13);
		if (veh.IsToggleModOn((VehicleToggleMod)18))
		{
			num += 3f;
		}
		num += num2 * 0.5f;
		num += num3 * 10f;
		num += (float)(mod * 2);
		return num + (float)(mod2 * 2);
	}

	public static float CalculateAcc(Vehicle veh)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		InputArgument[] array = new InputArgument[1];
		Model model = ((Entity)veh).Model;
		array[0] = model.Hash;
		float num2 = (float)Math.Round(Function.Call<float>((Hash)unchecked((ulong)(-8357471048252236950L)), array));
		return num + num2 * 10f;
	}

	public static float CalculateTopSpeed(Vehicle veh)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		InputArgument[] array = new InputArgument[1];
		Model model = ((Entity)veh).Model;
		array[0] = model.Hash;
		float num2 = (float)Math.Round(Function.Call<float>((Hash)unchecked((ulong)(-858003554332316349L)), array)) * 2.2f;
		InputArgument[] array2 = new InputArgument[1];
		model = ((Entity)veh).Model;
		array2[0] = model.Hash;
		float num3 = (float)Math.Round(Function.Call<float>((Hash)unchecked((ulong)(-8357471048252236950L)), array2));
		int mod = veh.GetMod((VehicleMod)11);
		int mod2 = veh.GetMod((VehicleMod)13);
		if (veh.IsToggleModOn((VehicleToggleMod)18))
		{
			num += 3f;
		}
		num += num2 * 0.5f;
		num += num3 * 10f;
		num += (float)(mod * 2);
		return num + (float)(mod2 * 2);
	}

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
		if (!MessageQueue.Contains(text))
		{
			MessageQueue.Add(text);
		}
	}

	public static void ClearAllHelpText(string text)
	{
		MessageQueue.Clear();
	}

	public static void HandleNotifications()
	{
		if (Game.GameTime > NotificationQueueReferenceTime && NotificationQueueAvatar.Count > 0 && NotificationQueueText.Count > 0 && NotificationQueueAuthor.Count > 0 && NotificationQueueTitle.Count > 0)
		{
			int num = (int)((float)NotificationQueueText[0].Length * 0.1f) * 1000;
			if (num > 5000)
			{
				num = 5000;
			}
			NotificationQueueReferenceTime = Game.GameTime + num;
			Notify(NotificationQueueAvatar[0], NotificationQueueAuthor[0], NotificationQueueTitle[0], NotificationQueueText[0]);
			NotificationQueueText.RemoveAt(0);
			NotificationQueueAvatar.RemoveAt(0);
			NotificationQueueAuthor.RemoveAt(0);
			NotificationQueueTitle.RemoveAt(0);
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
		Function.Call((Hash)unchecked((ulong)(-4736783180425337305L)), new InputArgument[1] { CurrentNotification });
	}

	public static void Notify(string avatar, string author, string title, string message)
	{
		if (avatar != "" && author != "" && title != "")
		{
			Function.Call((Hash)2316831480196236324L, new InputArgument[1] { "STRING" });
			Function.Call((Hash)7789129354908300458L, new InputArgument[1] { message });
			CurrentNotification = Function.Call<int>((Hash)2075484565200204495L, new InputArgument[6]
			{
				avatar,
				avatar,
				true,
				0,
				title,
				author
			});
		}
		else
		{
			UI.Notify(message);
		}
	}

	static Util()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
	}
}

}