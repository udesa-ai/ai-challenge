using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.NachoTeams
{
    public class ParticipantTeam : Team
    {
        public string TeamEmblem => throw new System.NotImplementedException();

        public Color PrimaryColor => Color.blue;

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
            return new Garrafa();
        }

        public string TeamShield { get; }

        public string GetName()
        {
            return "Defensores de Nacho";
        }
    }
}
