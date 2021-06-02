using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;
using System.Linq;

namespace Teams.NachoTeams
{
    public class Goalkeeper : TeamPlayer
    {
        private const float MIN_DISTANCE_TO_GOAL = 6;
        private const float MIN_DISTANCE_TO_GARRAFA = 8;
        //Vector3 alArco;


        public override void OnUpdate()
        {
          var ballPosition = GetBallPosition();
          var MyGoalPosition = GetMyGoalPosition();

            if (BallIsNearGoal(ballPosition))
            {
                MoveBy(GetDirectionTo(ballPosition));
            }
            else
            {
                //GoTo(FieldPosition.A2);
          //      alArco = new Vector3(10.0f, 0.0f, 0.0f);
                MyGoalPosition = MyGoalPosition *0.93f;
                var dirToBall = GetDirectionTo(ballPosition-MyGoalPosition);
                var dirToBallUnit = dirToBall.normalized;
                var dirToBallYBig = Vector3.Scale(dirToBallUnit, new Vector3(1, 1, 5));
                var dirToBallShort =dirToBallYBig*2.0f;
                //var golliePosition = FieldPositionAdapter.ConvertTo(dirToBall);
//                MoveBy(GetDirectionTo(dirToBallShort+alArco));
                MoveBy(GetDirectionTo(dirToBallShort+MyGoalPosition));
                //MoveBy(dirToBall);
                //Debug.Log("dirToBallShort");
                //Debug.Log(dirToBallYBig);
                //Debug.Log(dirToBallShort+MyGoalPosition);
                //Debug.Log("MyGoalPosition");
                //Debug.Log(MyGoalPosition[0]);
            }
        }

        private bool BallIsNearGoal(Vector3 ballPosition)
        {
            return Vector3.Distance(ballPosition, GetMyGoalPosition()) < MIN_DISTANCE_TO_GOAL;
        }

        private bool BallIsNearGarrafa(Vector3 ballPosition)
        {
            return Vector3.Distance(ballPosition, GetTeamMatesInformation().ElementAt(2).Position) < MIN_DISTANCE_TO_GARRAFA;
        }

        public override void OnReachBall()
        {
            //ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.Medium);
            var ballPosition = GetBallPosition();

            if (BallIsNearGarrafa(ballPosition))
            {
                ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
                //Debug.Log("Tiro al arco");
            }
            else
            {
               ShootBall(GetDirectionTo(GetTeamMatesInformation().ElementAt(2).Position), ShootForce.High);
                //ShootBall(GetDirectionTo(FieldPosition.C2), ShootForce.High);

                Debug.Log("Pase a Garrafa");
            }
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
            if (scoreBoard.My < scoreBoard.Rival)
            {
                // losing
            }
            else if (scoreBoard.My == scoreBoard.Rival)
            {
                // drawing
            }
            else
            {
                // winning
            }
        }

        public override FieldPosition GetInitialPosition()
        {
            return FieldPosition.A2;
        }

        public override string GetPlayerDisplayName()
        {
            return "AngelDavid";
        }
    }
}
