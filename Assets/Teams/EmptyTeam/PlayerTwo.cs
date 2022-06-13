using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ParticipantTeam
{
    public class PlayerTwo : TeamPlayer
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

        public override FieldPosition GetInitialPosition() => FieldPosition.B2;

        public override string GetPlayerDisplayName() => "Player 2";
    }
}