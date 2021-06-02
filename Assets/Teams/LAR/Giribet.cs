using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace LAR
{
    public class Giribet : TeamPlayer
    {
        private const float BALL_PREDICTION = 10.0f;
        private const float MIN_DISTANCE_TO_GOAL = 2.5f;
        private const float MIN_DISTANCE_TO_BALL = 1.0f;
        private const float PATH_WIDTH = 1.0f;

        private bool IsNearGoal(Vector3 ballPosition, float distance)
        {
            return Vector3.Distance(ballPosition, GetMyGoalPosition()) < distance;
        }

        private void MoveTowardsBall(Vector3 ballPosition,Vector3 ballPositionWithPrediction)
        {
            Vector3 ballVelocity = GetBallVelocity();
            Vector3 myGoalPosition = GetMyGoalPosition();

            if ((((myGoalPosition.x<0 && ballVelocity.x<0 && ((ballPosition.z>0 && ballVelocity.z<0)||(ballPosition.z<0 && ballVelocity.z>0))) || (myGoalPosition.x>0 && ballVelocity.x>0 && ((ballPosition.z>0 && ballVelocity.z<0)||(ballPosition.z<0 && ballVelocity.z>0)))) && IsNearGoal(ballPosition, MIN_DISTANCE_TO_GOAL*ballVelocity.magnitude*0.5f)) || IsNearGoal(ballPosition,MIN_DISTANCE_TO_GOAL*2.0f))
            {
                if (Vector3.Distance(ballPosition, GetPosition()) <= MIN_DISTANCE_TO_BALL)
                {
                    MoveBy(GetDirectionTo(ballPosition));
                }
                else
                {
                    MoveBy(GetDirectionTo(ballPositionWithPrediction));   
                }
            }
            else
            {
                if (!IsNearGoal(GetPosition(),MIN_DISTANCE_TO_GOAL*1.2f))
                {
                    MoveBy(GetDirectionTo(GetMyGoalPosition()));
                }
                else
                    //MoveBy(GetDirectionTo(ballPosition));
                    MoveOrthogonalTo(ballPosition,GetMyGoalPosition());
            }
        }

        private void MoveOrthogonalTo(Vector3 ballPos,Vector3 myGoalPos)
        {
            Vector3 goaliePos = GetPosition();
            Vector3 newDirection = Vector3.zero;
            Vector3 myGoalToBall = (ballPos-myGoalPos).normalized;
            var a = myGoalToBall.z/myGoalToBall.x;
            var b = myGoalPos.z-a*myGoalPos.x;
            if (myGoalPos.x<0)
            {
                if (goaliePos.z < a*goaliePos.x + b)
                {
                    newDirection.x = -myGoalToBall.z;
                    newDirection.z = +myGoalToBall.x;
                }else
                {
                    newDirection.x = +myGoalToBall.z;
                    newDirection.z = -myGoalToBall.x;
                }
            }else{
                if (goaliePos.z < a*goaliePos.x + b)
                {
                    newDirection.x = +myGoalToBall.z;
                    newDirection.z = -myGoalToBall.x;
                }else
                {
                    newDirection.x = -myGoalToBall.z;
                    newDirection.z = +myGoalToBall.x;
                }
            }
            MoveBy(newDirection);
        }

        
        private int GetRivalsOnPath(Vector3 target){
            var myPosition = GetPosition();
            var teamNoMates = GetRivalsInformation();

            var playerToTarget = (target-myPosition).normalized;
            var a = playerToTarget.z/playerToTarget.x;
            var b = myPosition.z-a*myPosition.x;
            var a2 = -1/a;
            var b2 = myPosition.z-a2*myPosition.x;
            var path_width_tilted = PATH_WIDTH * Mathf.Sqrt(1+a*a)/Mathf.Cos(Mathf.Atan(a));

            int rivalsOnPath = 0;

            if (a>0){a2=1/a;}

            for (int i = 0; i < teamNoMates.Count; i++)
            {
                Vector3 tNMPosition = teamNoMates[i].Position;
                if ((tNMPosition.z < a*tNMPosition.x + b + path_width_tilted) && (tNMPosition.z > a*tNMPosition.x + b - path_width_tilted) && (tNMPosition.z > a2*tNMPosition.x + b2))
                //if ((tNMPosition.z < a*tNMPosition.x + b + path_width_tilted) && (tNMPosition.z > a*tNMPosition.x + b - path_width_tilted))
                {
                    rivalsOnPath++;
                }
            }

            return rivalsOnPath;
        }
		
		//===================================================
        public override void OnUpdate()
        {
            CheckPosition();
            var ballPosition = GetBallPosition();
            var ballPositionWithPrediction = ballPosition + BALL_PREDICTION * Time.deltaTime * GetBallVelocity();

            
            MoveTowardsBall(ballPosition, ballPositionWithPrediction);
            //Vector3 pos = new Vector3(-9.2f, 0.0f, -9.2f);
            //MoveBy(GetDirectionTo(pos));
        }

        public override void OnReachBall()
        {
            var playerPosition = GetPosition();
            var teamMates = GetTeamMatesInformation();
            var teamNoMates = GetRivalsInformation();
            var rivalGoal = GetRivalGoalPosition(); rivalGoal.x=rivalGoal.x*0.93f;
            int indexSafestPath = -1;
            int numberTNM = 4;

            //enemies in path
            for (int i = 0; i < teamMates.Count; i++)
            {
                if (Vector3.Distance(teamMates[i].Position, playerPosition)>0.1f)
                {
                    int numberRivalsToTeammate = GetRivalsOnPath(teamMates[i].Position);
                    if (numberRivalsToTeammate <= numberTNM)
                    {
                        indexSafestPath = i;
                        numberTNM = numberRivalsToTeammate;
                    }
                }
            }
            int rivalsToGoal = GetRivalsOnPath(rivalGoal);
            if (rivalsToGoal <= numberTNM)
            {
                indexSafestPath = -1;
            }

            if (indexSafestPath==-1)
            {
                var direction = GetDirectionTo(rivalGoal);
                ShootBall(direction, ShootForce.High);
            }else
            {
                var direction = GetDirectionTo(teamMates[indexSafestPath].Position);
                ShootBall(direction, ShootForce.High);
            }

            /* Vector3 target = new Vector3(0.0f, 0.0f, +5.0f);
            if (GetPosition().z<0)
            {
                target.z=-target.z;
            }
            ShootBall(target - GetPosition(), ShootForce.High); */
        }

        //===================================================
        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
        }

        public override FieldPosition GetInitialPosition()
        {
            return FieldPosition.A2;
        }

        public override string GetPlayerDisplayName()
        {
            return "Giribet";
        }
    }
}