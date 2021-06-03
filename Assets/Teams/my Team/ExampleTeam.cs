using Core.Games;
using Core.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Teams.my_team
{
    [UsedImplicitly]
    public class ExampleTeam : Team
    {
        public TeamPlayer GetPlayerOne() => new Goalkeeper();

        public TeamPlayer GetPlayerTwo() => new Mid();

        public TeamPlayer GetPlayerThree() => new Offensive();

        public string TeamEmblem => "red_shield";

        public Color PrimaryColor => new Color(236f/255, 119f/255, 96f/255); 

        public string GetName() => "My Team";
        
        
        public string TeamShield => "blue_shield";

    }
}