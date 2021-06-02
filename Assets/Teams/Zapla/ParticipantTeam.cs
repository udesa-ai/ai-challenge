using Core.Games;
using Core.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Teams.Zapla
{
    [UsedImplicitly]
    public class Zapla : Team
    {
        public string TeamEmblem => throw new System.NotImplementedException();

        public Color PrimaryColor => Color.red;

        public TeamPlayer GetPlayerOne()
        {
            return new Goalkeeper();
        }

        public TeamPlayer GetPlayerTwo()
        {
            return new Mid();
        }

        public TeamPlayer GetPlayerThree()
        {
            return new Offensive();
        }
        
        
        public string TeamShield { get; }


        public string GetName()
        {
            return "Zapla";
        }
    }
}