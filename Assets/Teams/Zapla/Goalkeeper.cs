using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.Zapla
{
    public class Goalkeeper : TeamPlayer
    {
        private const float BALL_PREDICTION = 16.0f;

        private const float MIN_DISTANCE_TO_GOAL = 5.0f;

        private bool BallIsNearGoal(Vector3 ballPosition, float distance)
        {
            return Vector3.Distance(ballPosition, GetMyGoalPosition()) < distance;

        }

        private void MoveNearToBall(Vector3 position)
        {
            if (BallIsNearGoal(position, MIN_DISTANCE_TO_GOAL))
            {
                MoveBy(GetDirectionTo(position));
            }
            else
            {
                if (BallIsNearGoal(position, MIN_DISTANCE_TO_GOAL * 2.2f))
                {
                    MoveSideOrStop(position);
                }
                else
                    GotoInitialPosition();
            }
        }

        private void MoveSideOrStop(Vector3 position)
        {
            if (GetSizeDistance() && GetInitialBallPosition(position)) MoveSideBy(position);
            else Stop();
        }

        private void GotoInitialPosition()
        {
            GoTo(FieldPosition.A2);
        }

        private bool GetInitialBallPosition(Vector3 position)
        {
            return Mathf.Abs(GetPosition().z - position.z) > 0.25f;
        }

        private bool GetSizeDistance()
        {
            return Mathf.Abs(GetPosition().z) < 2.5f;
        }

        private void MoveSideBy(Vector3 position)
        {
            Vector3 newDirection = position.z > 0 ? Vector3.forward : Vector3.back;
            MoveBy(newDirection);
        }

        public override void OnUpdate()
        {
            var ballPositionWithPrediction = GetBallPosition();


            MoveNearToBall(ballPositionWithPrediction);
        }

        public override void OnReachBall()
        {
            var rivalGoalPosition = GetRivalGoalPosition();
            var directionToRivaGoal = GetDirectionTo(rivalGoalPosition);
            ShootBall(directionToRivaGoal, ShootForce.High);
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
        }

        public override FieldPosition GetInitialPosition()
        {
            return FieldPosition.A2;
        }

        public override string GetPlayerDisplayName()
        {
            return "FerK";
        }
    }
}