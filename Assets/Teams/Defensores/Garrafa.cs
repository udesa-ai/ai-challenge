using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.NachoTeams
{
    public class Garrafa : TeamPlayer
    {
        private const float MIN_DISTANCE_TO_ATTACK = 15;

        public override void OnUpdate()
        {
            var ballPosition = GetBallPosition();
            var directionToBall = GetDirectionTo(ballPosition);
            MoveBy(directionToBall);

            //Debug.Log(ToString()); // Debugs position, direction and speed of the player
            //Debug.Log("ballPosition");
            //Debug.Log(ballPosition);
            //Debug.Log(FieldPosition.E2);

            var rivalGoalPosition = GetRivalGoalPosition();
            var directionToRivaGoal = GetDirectionTo(rivalGoalPosition);
            //Debug.Log(rivalGoalPosition);
            //Debug.Log(directionToRivaGoal);


            //posPescador = new Vector3(-4.0f, 0.5f, 5.0f);
            var atacar = Vector3.Distance(ballPosition, rivalGoalPosition) < MIN_DISTANCE_TO_ATTACK;

            if (atacar)
            {
                MoveBy(GetDirectionTo(ballPosition));
            }
            else
            {
                GoTo(FieldPosition.E1);
            }


        }

        public override void OnReachBall()
        {
            var rivalGoalPosition = GetRivalGoalPosition();
            rivalGoalPosition = rivalGoalPosition *0.94f;
            var directionToRivaGoal = GetDirectionTo(rivalGoalPosition);
            ShootBall(directionToRivaGoal, ShootForce.High);
            //Debug.Log("rivalGoalPosition");
            //Debug.Log(rivalGoalPosition);
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
        }

        public override FieldPosition GetInitialPosition()
        {
            return FieldPosition.C2;
        }

        public override string GetPlayerDisplayName()
        {
            return "GARRAFA";
        }
    }
}
