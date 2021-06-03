using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ParticipantTeam
{
    public class Mid : TeamPlayer
    {
        private const float minimumDistanceToGoal = 10;

        public override void OnUpdate()
        {

        }

        private bool BallIsNearGoal(Vector3 ballPosition) => 
            Vector3.Distance(ballPosition, GetMyGoalPosition()) < minimumDistanceToGoal;

        public override void OnReachBall()
        {
            
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
        }

        public override FieldPosition GetInitialPosition() => FieldPosition.B2;

        public override string GetPlayerDisplayName() => "Mid";
    }
}