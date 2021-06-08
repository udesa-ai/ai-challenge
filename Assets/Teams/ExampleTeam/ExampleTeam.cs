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
        
        public Color PrimaryColor => new Color(0.4f, 0.5f, 1.0f);

        public string GetName() => "Example team";
        
        public string TeamShield => "Blue";

    }
}