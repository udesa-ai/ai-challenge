using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ParticipantTeam
{
    public class PlayerOne : TeamPlayer
    {
        public override void OnUpdate()
        {
            
        }

        public override void OnReachBall()
        {
            
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public override FieldPosition GetInitialPosition() => FieldPosition.A2;

        public override string GetPlayerDisplayName() => "Player 1";
    }
}