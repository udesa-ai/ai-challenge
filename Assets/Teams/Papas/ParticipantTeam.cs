using Core.Games;
using Core.Player;
using UnityEngine;

namespace Teams.Las_Papas
{
	public class ParticipantTeam : Team
	{
        public string TeamEmblem => throw new System.NotImplementedException();

        public Color PrimaryColor => Color.yellow;

        public string GetName()
		{
			return "Las Papas";
		}

		public TeamPlayer GetPlayerOne()
		{
			return new PlayerOne();
		}

		public TeamPlayer GetPlayerTwo()
		{
			return new PlayerTwo();
		}
		
		public string TeamShield { get; }


		public TeamPlayer GetPlayerThree()
		{
			return new PlayerThree();
		}
	}
}