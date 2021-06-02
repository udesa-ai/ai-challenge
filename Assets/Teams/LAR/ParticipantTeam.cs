using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;
using JetBrains.Annotations;

namespace LAR
{
    [UsedImplicitly]
    public class ParticipantTeam : Team
    {
        public string TeamEmblem => throw new System.NotImplementedException();

        public Color PrimaryColor => Color.green;

        public TeamPlayer GetPlayerOne()
        {
            return new Giribet();
        }

        public TeamPlayer GetPlayerTwo()
        {
            return new Ghersin();
        }
        
        public TeamPlayer GetPlayerThree()
        {
            return new Mas();
        }
        
        
        public string TeamShield => "cheat_shield";


        public string GetName()
        {
            return "LAR team";
        }
    }
}