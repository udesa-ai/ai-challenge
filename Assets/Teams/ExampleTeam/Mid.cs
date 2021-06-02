using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ExampleTeam
{
    public class Mid : TeamPlayer
    {
        private const float minimumDistanceToGoal = 10;

        public override void OnUpdate()
        {
            var ballPosition = GetBallPosition();

            if (BallIsNearGoal(ballPosition))
                MoveBy(GetDirectionTo(ballPosition));
            else
                GoTo(FieldPosition.C2);
        }

        private bool BallIsNearGoal(Vector3 ballPosition) => 
            Vector3.Distance(ballPosition, GetMyGoalPosition()) < minimumDistanceToGoal;

        public override void OnReachBall() => 
            ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.Medium);

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
        }

        public override FieldPosition GetInitialPosition() => FieldPosition.B2;

        public override string GetPlayerDisplayName() => "Mid";
    }
}