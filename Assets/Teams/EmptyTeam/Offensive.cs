using Core.Games;
using Core.Player;
using Core.Utils;

namespace Teams.ParticipantTeam
{
    public class Offensive : TeamPlayer
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

        public override FieldPosition GetInitialPosition() => FieldPosition.C2;

        public override string GetPlayerDisplayName() => "Offensive";
    }
}