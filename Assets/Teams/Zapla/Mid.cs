using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.Zapla
{
    public class Mid : TeamPlayer
    {
        private const float MIN_DISTANCE_TO_GOAL = 18;

        public override void OnUpdate()
        {
            var ballPosition = GetBallPosition();
            var directionToBall = GetDirectionTo(ballPosition);

            if (BallIsNearGoal(ballPosition))
            {
                MoveBy(GetDirectionTo(ballPosition));
            }
            else
            {
                GoTo(FieldPosition.C2);
            }


        }

        private bool BallIsNearGoal(Vector3 ballPosition)
        {
            return Vector3.Distance(ballPosition, GetMyGoalPosition()) < MIN_DISTANCE_TO_GOAL;
        }

        public override void OnReachBall()
        {
            var tiempo = GetTimeLeft();
            var rivalGoalPosition = GetRivalGoalPosition();
            var directionToRivaGoal = GetDirectionTo(rivalGoalPosition);
            var sortedTeam = GetTeamMatesInformation();

            if (tiempo >= 30)
            {
                GoTo(FieldPosition.E2);
                ShootBall(GetDirectionTo(sortedTeam[1].Position), ShootForce.High);//Le pasa la pelota a su compañero

            }
            else if(sortedTeam[1].Position == GetPositionFor(FieldPosition.D2))
            {
                ShootBall(GetDirectionTo(sortedTeam[1].Position), ShootForce.High);//Le pasa la pelota a su compañero

            }
            else
            {
                ShootBall(directionToRivaGoal, ShootForce.High); //Patea al arco
             
            }

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
            return "Gian";
        }
    }
}