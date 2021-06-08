using Core.Games;
using Core.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Teams.ExampleTeam
{
    [UsedImplicitly]
    public class ExampleTeam : Team
    {
        public TeamPlayer GetPlayerOne() => new Goalkeeper();

        public TeamPlayer GetPlayerTwo() => new Mid();

        public TeamPlayer GetPlayerThree() => new Offensive();
        
        public Color PrimaryColor => new Color(0.6f, 0.6f, 1f);

        public string GetName() => "Example team";
        
        public string TeamShield => "Orange";

    }
}