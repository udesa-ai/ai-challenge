using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.Zapla
{
    public class Offensive : TeamPlayer
    {
        private const float MIN_DISTANCE_TO_GOAL = 7;
        public override void OnUpdate()
        {
            var ballPosition = GetBallPosition();
            var directionToBall = GetDirectionTo(ballPosition);
            var tiempo = GetTimeLeft();
            var sortedTeam = GetTeamMatesInformation();


            if (tiempo >= 36)
            {
                GoTo(FieldPosition.E3);     
            }

            else if (BallIsNearGoal(ballPosition))
            {
                GoTo(FieldPosition.D2);
            }
            else
            {
                MoveBy(GetDirectionTo(GetBallPosition()));
            }
        }
        private bool BallIsNearGoal(Vector3 ballPosition)
        {
            return Vector3.Distance(ballPosition, GetMyGoalPosition()) < MIN_DISTANCE_TO_GOAL;
        }
        public override void OnReachBall()
        {
            var rivalGoalPosition = GetRivalGoalPosition();
            var directionToRivaGoal = GetDirectionTo(rivalGoalPosition);
            ShootBall(directionToRivaGoal, ShootForce.High);
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
            if (GetMyScore() > GetRivalScore())
            {
                MoveBy(GetBallPosition()-GetMyGoalPosition());
            }
        }
        
        public override FieldPosition GetInitialPosition()
        {
            return FieldPosition.C2;
        }

        public override string GetPlayerDisplayName()
        {
            return "Jose";
        }
    }
}