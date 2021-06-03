using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.my_team
{
    public class Goalkeeper : TeamPlayer
    {
        private const float minimumDistanceToGoal = 5;

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