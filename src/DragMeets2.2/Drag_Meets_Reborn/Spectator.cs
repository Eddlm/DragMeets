using GTA;
using GTA.Math;
using GTA.Native;

namespace Drag_Meets_Reborn
{
public class Spectator
{
	public Ped SpectatorPed;

	public Vehicle Car;

	private Spectatus State = Spectatus.Idle;

	public Vector3 DesiredPos = DragMeets.WaitingArea;

	private int GameTimeRef = Game.GameTime + 2000;

	private bool RemoveMe;

	public Spectator(Ped ped, Vehicle veh)
	{
		SpectatorPed = ped;
		if (Util.CanWeUse((Entity)(object)veh))
		{
			Car = veh;
		}
		if (!((Entity)SpectatorPed).CurrentBlip.Exists() && DragMeets.RacerBlips)
		{
			((Entity)SpectatorPed).AddBlip();
			((Entity)SpectatorPed).CurrentBlip.Color = (BlipColor)2;
			((Entity)SpectatorPed).CurrentBlip.Scale = 0.3f;
			((Entity)SpectatorPed).CurrentBlip.IsShortRange = true;
		}
		SpectatorPed.RelationshipGroup = DragMeets.RacerRLGroup;
		SpectatorPed.AlwaysKeepTask = true;
		SpectatorPed.Task.TurnTo(DragMeets.StartingLine);
		DesiredPos = ((Entity)SpectatorPed).Position;
	}

	public bool Ok()
	{
		if (!Util.CanWeUse((Entity)(object)SpectatorPed))
		{
			return false;
		}
		if (!((Entity)SpectatorPed).IsAlive)
		{
			return false;
		}
		return true;
	}

	public void HandleAI()
	{
		//IL_027f: Expected Obj, but got Unknown
		//IL_007f: Expected Obj, but got Unknown
		if (State == Spectatus.RidingCar && Util.CanWeUse((Entity)(object)SpectatorPed.CurrentVehicle) && ((Entity)SpectatorPed).IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange * 3f) && SpectatorPed.TaskSequenceProgress == -1 && !Function.Call<bool>((Hash)6317224474499895619L, new InputArgument[1] { SpectatorPed }))
		{
			TaskSequence val = new TaskSequence();
			Vector3 val2 = DesiredPos.Around(3f);
			Function.Call((Hash)unchecked((ulong)(-1785072049739624382L)), new InputArgument[2]
			{
				0,
				Util.RandomInt(1, 3) * 1000
			});
			Function.Call((Hash)1572784988947265811L, new InputArgument[9]
			{
				0,
				val2.X,
				val2.Y,
				val2.Z,
				2f,
				30000,
				0f,
				0,
				0f
			});
			Function.Call((Hash)2151193443362375025L, new InputArgument[5]
			{
				0,
				DragMeets.StartingLine.X,
				DragMeets.StartingLine.Y,
				DragMeets.StartingLine.Z,
				3000
			});
			Function.Call((Hash)1452976313881078745L, new InputArgument[4]
			{
				0,
				"WORLD_HUMAN_CHEERING",
				5000,
				true
			});
			val.Close();
			SpectatorPed.Task.PerformSequence(val);
			State = Spectatus.Idle;
			val.Dispose();
		}
		if (GameTimeRef < Game.GameTime && DragMeets.MeetState != DragRaceState.CountDown)
		{
			GameTimeRef = Game.GameTime + 20000;
			if (Function.Call<bool>((Hash)6317224474499895619L, new InputArgument[1] { SpectatorPed }) && !((Entity)SpectatorPed).IsInRangeOf(DesiredPos, 4f))
			{
				TaskSequence val3 = new TaskSequence();
				Vector3 val4 = DesiredPos.Around(3f);
				Function.Call((Hash)unchecked((ulong)(-1785072049739624382L)), new InputArgument[2]
				{
					0,
					Util.RandomInt(1, 3) * 1000
				});
				Function.Call((Hash)1572784988947265811L, new InputArgument[9]
				{
					0,
					val4.X,
					val4.Y,
					val4.Z,
					2f,
					30000,
					0f,
					0,
					0f
				});
				Function.Call((Hash)2151193443362375025L, new InputArgument[5]
				{
					0,
					DragMeets.StartingLine.X,
					DragMeets.StartingLine.Y,
					DragMeets.StartingLine.Z,
					3000
				});
				Function.Call((Hash)1452976313881078745L, new InputArgument[4]
				{
					0,
					"WORLD_HUMAN_CHEERING",
					5000,
					true
				});
				val3.Close();
				SpectatorPed.Task.PerformSequence(val3);
				val3.Dispose();
			}
		}
	}

	public bool Idle()
	{
		if (SpectatorPed.TaskSequenceProgress != -1)
		{
			return Function.Call<bool>((Hash)6317224474499895619L, new InputArgument[1] { SpectatorPed });
		}
		return true;
	}

	public void Clear(bool HardDeletion)
	{
		if (!SpectatorPed.IsPlayer)
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
			if (Util.CanWeUse((Entity)(object)SpectatorPed))
			{
				if (HardDeletion)
				{
					((Entity)SpectatorPed).Delete();
				}
				else
				{
					((Entity)SpectatorPed).MarkAsNoLongerNeeded();
				}
			}
		}
		else if (Util.CanWeUse((Entity)(object)Car) && ((Entity)Car).IsPersistent)
		{
			((Entity)Car).IsPersistent = false;
		}
		RemoveMe = true;
	}

	public void TaskCheckCarOut(Vehicle veh)
	{
		//IL_000d: Expected Obj, but got Unknown
		State = Spectatus.CheckingCarOut;
		TaskSequence val = new TaskSequence();
		Vector3 position = ((Entity)veh).Position;
		Vector3 val2 = position.Around(3f);
		Function.Call((Hash)unchecked((ulong)(-1785072049739624382L)), new InputArgument[2]
		{
			0,
			Util.RandomInt(1, 3) * 1000
		});
		Function.Call((Hash)1572784988947265811L, new InputArgument[9]
		{
			0,
			val2.X,
			val2.Y,
			val2.Z,
			1.2f,
			30000,
			0f,
			0,
			0f
		});
		Function.Call((Hash)2151193443362375025L, new InputArgument[5]
		{
			0,
			((Entity)veh).Position.X,
			((Entity)veh).Position.Y,
			((Entity)veh).Position.Z,
			3000
		});
		Function.Call((Hash)unchecked((ulong)(-1785072049739624382L)), new InputArgument[2]
		{
			0,
			10000
		});
		Function.Call((Hash)1452976313881078745L, new InputArgument[4]
		{
			0,
			"WORLD_HUMAN_SMOKING",
			5000,
			true
		});
		val.Close();
		SpectatorPed.Task.PerformSequence(val);
		val.Dispose();
	}

	public void TaskRideCar(Racer racer)
	{
		State = Spectatus.RidingCar;
		SpectatorPed.Task.EnterVehicle(racer.Car, (VehicleSeat)0);
	}
}

}