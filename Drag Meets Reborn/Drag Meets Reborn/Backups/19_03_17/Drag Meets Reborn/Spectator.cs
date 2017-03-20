using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


enum Spectatus {CheeringInCrowdPoint,CheckingCarOut,RidingCar,Idle }
namespace Drag_Meets_Reborn
{

   public class Spectator
    {
        public Ped SpectatorPed;
        public Vehicle Car;
        Spectatus State = Spectatus.Idle;
       public  Vector3 DesiredPos = DragMeets.WaitingArea;

        int GameTimeRef = Game.GameTime+2000;
        bool RemoveMe = false;

        public Spectator(Ped ped, Vehicle veh)
        {
            SpectatorPed = ped;
            if (Util.CanWeUse(veh)) Car = veh;


            if (!SpectatorPed.CurrentBlip.Exists())
            {
                SpectatorPed.AddBlip();
            }
            SpectatorPed.CurrentBlip.Color = BlipColor.Green;
            SpectatorPed.CurrentBlip.Scale = 0.3f;
            SpectatorPed.CurrentBlip.IsShortRange = true;

            SpectatorPed.RelationshipGroup = DragMeets.RacerRLGroup;
            SpectatorPed.AlwaysKeepTask = true;

            SpectatorPed.Task.TurnTo(DragMeets.StartingLine);
            //Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, 0, "WORLD_HUMAN_CHEERING", 5000, true);
            DesiredPos = SpectatorPed.Position;

            //SpectatorPed.Task.WanderAround(SpectatorPed.Position, 5f);
        }
        public bool Ok()
        {

            if (!Util.CanWeUse(SpectatorPed)) {  return false; }
            if (!SpectatorPed.IsAlive) {return false; }

            return true;
        }
        
        public void HandleAI()
        {//Ontick


            /*
            if (Game.Player.Character.IsInRangeOf(SpectatorPed.Position,3f))
            {
                Util.DisplayHelpTextThisFrame("InScenario: "+Function.Call<bool>(Hash.IS_PED_USING_ANY_SCENARIO, SpectatorPed).ToString()+"~n~InSequence: "+(SpectatorPed.TaskSequenceProgress != -1).ToString());
            }*/


            if(State == Spectatus.RidingCar)
            {
                if (Util.CanWeUse(SpectatorPed.CurrentVehicle))
                {
                    if (SpectatorPed.IsInRangeOf(DragMeets.WaitingArea, DragMeets.WaitingAreRange * 3) && SpectatorPed.TaskSequenceProgress == -1 && !Function.Call<bool>(Hash.IS_PED_USING_ANY_SCENARIO, SpectatorPed))
                    {
                        TaskSequence IdlSeq = new TaskSequence();
                        Vector3 pos = DesiredPos.Around(3);
                        Function.Call(Hash.TASK_PAUSE, 0, Util.RandomInt(1, 3) * 1000);
                        Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, 0, pos.X, pos.Y, pos.Z, 2f, 30000, 0f, 0, 0f);
                        Function.Call(Hash.TASK_TURN_PED_TO_FACE_COORD, 0, DragMeets.StartingLine.X, DragMeets.StartingLine.Y, DragMeets.StartingLine.Z, 3000);
                        Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, 0, "WORLD_HUMAN_CHEERING", 5000, true);
                        IdlSeq.Close();
                        SpectatorPed.Task.PerformSequence(IdlSeq);
                        State = Spectatus.Idle;
                        IdlSeq.Dispose();
                    }
                }
            }
            //Every 20 secs
            if (GameTimeRef < Game.GameTime && DragMeets.MeetState != DragRaceState.CountDown)
            {
                GameTimeRef = Game.GameTime + 20000;
                //if (SpectatorPed.TaskSequenceProgress == -1 && !Function.Call<bool>(Hash.IS_PED_USING_ANY_SCENARIO, SpectatorPed)) Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, SpectatorPed, "WORLD_HUMAN_SMOKING", 5000, true);
                if (Function.Call<bool>(Hash.IS_PED_USING_ANY_SCENARIO, SpectatorPed))
                {
                    if (!SpectatorPed.IsInRangeOf(DesiredPos, 4))
                    {
                        TaskSequence IdlSeq = new TaskSequence();
                        Vector3 pos = DesiredPos.Around(3);
                        Function.Call(Hash.TASK_PAUSE, 0, Util.RandomInt(1, 3) * 1000);

                        Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, 0, pos.X, pos.Y, pos.Z, 2f, 30000, 0f, 0, 0f);
                        Function.Call(Hash.TASK_TURN_PED_TO_FACE_COORD, 0, DragMeets.StartingLine.X, DragMeets.StartingLine.Y, DragMeets.StartingLine.Z, 3000);
                        Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, 0, "WORLD_HUMAN_CHEERING", 5000, true);
                        IdlSeq.Close();
                        SpectatorPed.Task.PerformSequence(IdlSeq);
                        IdlSeq.Dispose();
                    }
                }
            }
        }

        public bool Idle()
        {
            return SpectatorPed.TaskSequenceProgress == -1 || Function.Call<bool>(Hash.IS_PED_USING_ANY_SCENARIO, SpectatorPed);
        }

        public void Clear(bool HardDeletion)
        {
                if (!SpectatorPed.IsPlayer)
                {

                    if (Util.CanWeUse(Car))
                    {
                        //UI.Notify(Car.FriendlyName + " has been cleared.");
                        if (HardDeletion) Car.Delete(); else Car.MarkAsNoLongerNeeded();
                        //Car.IsPersistent = false;
                        //Car.MarkAsNoLongerNeeded();

                    }

                    if (Util.CanWeUse(SpectatorPed))
                    {

                        if (HardDeletion) SpectatorPed.Delete(); else SpectatorPed.MarkAsNoLongerNeeded();
                        //Driver.IsPersistent = false;
                        //Driver.MarkAsNoLongerNeeded();
                    }

                }
                else
                {
                    if (Util.CanWeUse(Car) && Car.IsPersistent) Car.IsPersistent = false;
                }
                RemoveMe = true;
        }

        public void TaskCheckCarOut(Vehicle veh)
        {

            State = Spectatus.CheckingCarOut;
            TaskSequence IdlSeq = new TaskSequence();

            Vector3 pos = veh.Position.Around(3); // Util.GetEntityOffset(checkout, 4, 0).Around(3);
            Function.Call(Hash.TASK_PAUSE, 0, Util.RandomInt(1,3)*1000);

            Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, 0, pos.X, pos.Y, pos.Z, 1.2f, 30000, 0f, 0, 0f);
            Function.Call(Hash.TASK_TURN_PED_TO_FACE_COORD, 0, veh.Position.X, veh.Position.Y, veh.Position.Z, 3000);
            Function.Call(Hash.TASK_PAUSE, 0, 10000);
            Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, 0, "WORLD_HUMAN_SMOKING", 5000, true);
            IdlSeq.Close();
            SpectatorPed.Task.PerformSequence(IdlSeq);
            IdlSeq.Dispose();
        }
        public void TaskRideCar(Racer racer)
        {

            State = Spectatus.RidingCar;
            SpectatorPed.Task.EnterVehicle(racer.Car, VehicleSeat.Passenger);
        }
    }
}
