using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.my_team
{
    public class Mid : TeamPlayer
    {
        private const float minimumDistanceToGoal = 10;

        public override void OnUpdate()
        {
            GoTo(FieldPosition.D1);
        }

        private bool BallIsNearGoal(Vector3 ballPosition) => 
            Vector3.Distance(ballPosition, GetMyGoalPosition()) < minimumDistanceToGoal;

        public override void OnReachBall()
        {
            ShootBall(GetDirectionTo(GetRivalGoalPosition()),ShootForce.High);
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
        }

        public override FieldPosition GetInitialPosition() => FieldPosition.C2;

        public override string GetPlayerDisplayName() => "Nash";
    }
}