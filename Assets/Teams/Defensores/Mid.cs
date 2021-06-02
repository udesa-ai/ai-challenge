using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.NachoTeams
{
    public class Mid : TeamPlayer
    {
        private const float MIN_DISTANCE_TO_GOAL = 15;
        private const float MIN_DISTANCE_TO_GARRAFA = 5;



        public override void OnUpdate()
        {
            var ballPosition = GetBallPosition();


            //Debug.Log("rival positions");
            //Debug.Log(GetRivalsInformation().ElementAt(0).Position);
            //Debug.Log("Mates positions");
            //Debug.Log(GetTeamMatesInformation().ElementAt(2).Position);

            if (BallIsNearGoal(ballPosition))
            {
                MoveBy(GetDirectionTo(ballPosition));
            }
            else
            {
              GoTo(FieldPosition.C2);
//              GoTo(new FieldPosition(5.0f, 0.0f, 0.0f));
            }
        }

        private bool BallIsNearGoal(Vector3 ballPosition)
        {
            return Vector3.Distance(ballPosition, GetMyGoalPosition()) < MIN_DISTANCE_TO_GOAL;
        }

        private bool BallIsNearGarrafa(Vector3 ballPosition)
        {
            return Vector3.Distance(ballPosition, GetTeamMatesInformation()[2].Position) < MIN_DISTANCE_TO_GARRAFA;
        }

        public override void OnReachBall()
        {
            var ballPosition = GetBallPosition();

            if (BallIsNearGarrafa(ballPosition))
            {
                ShootBall(GetDirectionTo(GetRivalGoalPosition()*0.93f), ShootForce.High);
                Debug.Log("Tiro al arco");
            }
            else
            {
                ShootBall(GetDirectionTo(GetTeamMatesInformation()[2].Position), ShootForce.High);
                Debug.Log("Pase a Garrafa con fuerza:");
                //Debug.Log(ShootForce.Medium);

            }
            //ShootBall(GetDirectionTo(GetTeamMatesInformation().ElementAt(2).Position), ShootForce.High);
            //ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.Medium);
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
        }

        public override FieldPosition GetInitialPosition()
        {
            return FieldPosition.B2;
        }

        public override string GetPlayerDisplayName()
        {
            return "TORTUGA";
        }
    }
}
