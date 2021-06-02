using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.TeamCheat
{
    public class PlayerCheatThree : TeamPlayer
    {
        private const float MinimumDistanceToGoal = 5;

        public override void OnUpdate()
        {
            var ballPosition = GetBallPosition();

            if (BallIsNearGoal(ballPosition))
                MoveBy(GetDirectionTo(ballPosition));
            else
                GoTo(FieldPosition.A2);
        }

        private bool BallIsNearGoal(Vector3 ballPosition) => 
            Vector3.Distance(ballPosition, GetMyGoalPosition()) < MinimumDistanceToGoal;

        public override void OnReachBall() => 
            ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.Medium);

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
        }

        public override FieldPosition GetInitialPosition() => FieldPosition.A2;

        public override string GetPlayerDisplayName() => "Goalkeeper cheat";
    }
}