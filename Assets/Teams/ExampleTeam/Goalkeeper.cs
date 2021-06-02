using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ExampleTeam
{
    public class Goalkeeper : TeamPlayer
    {
        private const float minimumDistanceToGoal = 5;

        public override void OnUpdate()
        {
            var ballPosition = GetBallPosition();

            if (BallIsNearGoal(ballPosition))
                MoveBy(GetDirectionTo(ballPosition));
            else
                GoTo(FieldPosition.A2);
        }

        private bool BallIsNearGoal(Vector3 ballPosition) => 
            Vector3.Distance(ballPosition, GetMyGoalPosition()) < minimumDistanceToGoal;

        public override void OnReachBall() => 
            ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.Medium);

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

        public override FieldPosition GetInitialPosition() => FieldPosition.A2;

        public override string GetPlayerDisplayName() => "Goalkeeper";
    }
}