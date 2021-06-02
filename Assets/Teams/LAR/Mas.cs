using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace LAR
{
    public class Mas : TeamPlayer
    {
        private const float BALL_PREDICTION = 20.0f;    //at 60fps, half a second prediction
        private const float BALL_NEAR = 2.0f;
        private const float PATH_WIDTH = 1.0f;
        private const float SHOOT_BALL_HIGH_DISTANCE = 6.0f;
        private const float SHOOT_BALL_MED_DISTANCE = 1.0f;
        private Vector3 retreat = new Vector3(-1.0f, 0.0f, 1.0f);

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

        private void ShootBallDistance(Vector3 target){
            var distanceToTarget = Vector3.Distance(target,GetPosition());
            var directionToTarget = GetDirectionTo(target);
            if (distanceToTarget>SHOOT_BALL_HIGH_DISTANCE){ShootBall(directionToTarget, ShootForce.High);}
            else if (distanceToTarget>SHOOT_BALL_MED_DISTANCE){ShootBall(directionToTarget, ShootForce.Medium);}
            else {ShootBall(directionToTarget, ShootForce.Low);}
        }

        public override void OnUpdate()
        {
            //area allowed (top player z>SLOPE*x+BIAS)
            // mirror top bottom
            float SLOPE=+1.0f;
            float BIAS=-2.0f;

            var ballPosition = GetBallPosition();
            var ballPositionWithPrediction = ballPosition + BALL_PREDICTION * Time.deltaTime * GetBallVelocity();

            var playerPosition = GetPosition();
            var teamMates = GetTeamMatesInformation();
            var teamNoMates = GetRivalsInformation();

            if (GetMyGoalPosition().x>0)
            {
                SLOPE = -SLOPE;
                retreat.x = -retreat.x;
            }

            if (Vector3.Distance(playerPosition, ballPosition)<BALL_NEAR)
            {
                var direc = GetDirectionTo(ballPosition);
                MoveBy(direc);
            }
            else
            {
                if (playerPosition.z<SLOPE*playerPosition.x+BIAS)
                {
                    MoveBy(retreat);
                }else
                {
                    var direc = GetDirectionTo(ballPositionWithPrediction);
                    MoveBy(direc);
                }
            }
        }

        public override void OnReachBall()
        {
            var ballPosition = GetBallPosition();
            var ballPositionWithPrediction = ballPosition + BALL_PREDICTION * Time.deltaTime * GetBallVelocity();

            var playerPosition = GetPosition();
            var teamMates = GetTeamMatesInformation();
            var teamNoMates = GetRivalsInformation();
            var rivalGoal = GetRivalGoalPosition(); rivalGoal.x=rivalGoal.x*0.92f;
            int indexNearestToGoal = -1;
            int indexSafestPath = -1;
            float mindist = 100.0f;
            int numberTNM = 4;
            
            //dist of teammates to goal and enemies in path
            for (int i = 0; i < teamMates.Count; i++)
            {
                if (Vector3.Distance(teamMates[i].Position, playerPosition)>0.1f)
                {
                    float distTeammateToGoal = Vector3.Distance(teamMates[i].Position, rivalGoal);
                    if ( distTeammateToGoal <= mindist)
                    {
                        indexNearestToGoal = i;  
                        mindist = distTeammateToGoal;
                    }
                    int numberRivalsToTeammate = GetRivalsOnPath(teamMates[i].Position);
                    if (numberRivalsToTeammate <= numberTNM)
                    {
                        indexSafestPath = i;
                        numberTNM = numberRivalsToTeammate;
                    }
                }
            }

            //dist player to goal
            float distPlayerToGoal = Vector3.Distance(playerPosition, rivalGoal);
            if ( distPlayerToGoal <= mindist)
            { 
                indexNearestToGoal = -1; 
            }
            int rivalsToGoal = GetRivalsOnPath(rivalGoal);
            if (rivalsToGoal <= numberTNM)
            {
                indexSafestPath = -1;
            }

            //Debug.Log("Safest path:" + indexSafestPath.ToString() + " // Nearest to goal: " + indexNearestToGoal.ToString());

            //shoot
            var directionToGoal = GetDirectionTo(rivalGoal);
            if (indexSafestPath==-1)
            {
                if ((rivalsToGoal == 1) && Vector3.Distance(playerPosition,rivalGoal)>7.0f)
                {
                    MoveBy(directionToGoal);
                }else
                {
                    ShootBall(directionToGoal, ShootForce.High);
                }
            }else
            {
                if (indexSafestPath==indexNearestToGoal)
                {
                    ShootBallDistance(teamMates[indexSafestPath].Position);
                }else
                {
                    if (indexNearestToGoal==-1){ShootBall(directionToGoal, ShootForce.High);}
                    else{ShootBallDistance(teamMates[indexNearestToGoal].Position);}
                }
            }
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
            return "Mas";
        }
    }
}