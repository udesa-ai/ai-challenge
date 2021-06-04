using Core.Games;
using Core.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Teams.ParticipantTeam
{
    [UsedImplicitly]
    public class ExampleTeam : Team
    {
        public TeamPlayer GetPlayerOne() => new Goalkeeper();

        public TeamPlayer GetPlayerTwo() => new Mid();

        public TeamPlayer GetPlayerThree() => new Offensive();
        
        public Color PrimaryColor => new Color(150f/255, 150f/255, 150f/255);

        public string GetName() => "Participant Team";

        public string TeamShield => "Black";


    }
}