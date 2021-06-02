using Core.Games;
using Core.Player;
using UnityEngine;

namespace Teams.TeamCheat
{
    public class TeamCheat : Team
    {
        public TeamPlayer GetPlayerOne() => new PlayerCheatOne();

        public TeamPlayer GetPlayerTwo() => new PlayerCheatTwo();

        public TeamPlayer GetPlayerThree() => new PlayerCheatThree();

        public string GetName() => "Team Cheaters";
        
        public string TeamShield => "cheat_shield";

        public string TeamEmblem => "cheat_shield";

        public Color PrimaryColor => Color.red;
    }
}