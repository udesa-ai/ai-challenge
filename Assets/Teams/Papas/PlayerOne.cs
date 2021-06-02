using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.Las_Papas
{
	public class PlayerOne : TeamPlayer
	{
		
		public override void OnReachBall()
		{

			var target1 = GetRivalGoalPosition() + new Vector3(0, 0, 1f);
			var target2 = GetRivalGoalPosition() + new Vector3(0, 0, -1f);
			var target3 = GetTeamMatesInformation()[1].Position;

			var doI1 = IsClear(GetBallPosition(), target1, 0.5f, 7);
			var doI2 = IsClear(GetBallPosition(), target2, 0.5f, 7);
			var doI3 = IsClear(GetBallPosition(), target3, 1f, 7);

			Vector3 dir;
			int angle_check;

			if (IsClear(GetBallPosition(), target1, 0.5f, 1) && (Vector3.Distance(target1, GetBallPosition()) < 4))
            {
				ShootBall(GetDirectionTo(target1), ShootForce.High);
				return;
            }

			if (IsClear(GetBallPosition(), target2, 0.5f, 1) && (Vector3.Distance(target1, GetBallPosition()) < 4))
			{
				ShootBall(GetDirectionTo(target1), ShootForce.High);
				return;
			}


			if (doI1)
			{
				dir = GetDirectionTo(target1);
				angle_check = 30;
			}
			else if (doI2)
			{
				dir = GetDirectionTo(target2);
				angle_check = 30;
			}
			else if (doI3)
			{
				dir = GetDirectionTo(target3);
				angle_check = 20;
			}
			else
			{
				var test = GetRivalGoalPosition();
				test.z = -10;
				dir = GetDirectionTo(test);
				angle_check = -1;
			}

			var mydir = GetTeamMatesInformation()[0].Direction;
			var angle = Mathf.Abs(Vector3.Angle(mydir, dir));

			if (angle > angle_check)
			{
				ShootBall(dir, ShootForce.Low);
			}
			else
			{
				ShootBall(dir, ShootForce.High);
			}

		}

		private bool IsClear(Vector3 start, Vector3 finish, float changui, int saltos)
		{
			var dir = finish - start;
			dir = dir.normalized;
			int i;
			float dist1 = 10, dist2 = 10, dist3 = 10;
			for (i = 1; i < (saltos + 1); i++)
			{
				var checkpos = dir * i + start;
				var rival1 = GetRivalsInformation()[0].Position - finish;
				var rival2 = GetRivalsInformation()[1].Position - finish;
				var rival3 = GetRivalsInformation()[2].Position - finish;

				if (Vector3.Dot(rival1, start - finish) < Mathf.Abs(Vector3.Distance(start, finish)))
                {
					dist1 = Vector3.Distance(GetRivalsInformation()[0].Position, checkpos);
                }

				if (Vector3.Dot(rival2, start - finish) < Mathf.Abs(Vector3.Distance(start, finish)))
				{
					dist2 = Vector3.Distance(GetRivalsInformation()[1].Position, checkpos);
				}

				if (Vector3.Dot(rival3, start - finish) < Mathf.Abs(Vector3.Distance(start, finish)))
				{
					dist3 = Vector3.Distance(GetRivalsInformation()[2].Position, checkpos);
				}

				if ((dist1 < changui) || (dist2 < changui) || (dist3 < changui))
				{
					return false;
				}
			}
			return true;
		}

		public override void OnUpdate()
		{
			var Target = GetTarget();
			MoveBy(GetDirectionTo(Target));


		}

		private Vector3 GetTarget()
        {
			var ballPos = GetBallPosition();
			var ballVel = GetBallVelocity();
			var myPos = GetPosition();
			var distToBall = Vector3.Distance(myPos, ballPos);
			var myGoal = GetMyGoalPosition();

			Vector3 target;

			var Angulo_rot = Mathf.Atan2(ballVel.z, ballVel.x) * 180 / Mathf.PI;
			Vector3 diff_pos = Quaternion.Euler(0, Angulo_rot, 0) * (myPos - ballPos);

			float Z = ballPos.z + (ballVel.z / ballVel.x) * (Mathf.Abs(ballPos.x - GetRivalGoalPosition().x));

			/*
			if (Vector3.Distance(ballPos, myGoal) < 9.5f)
			{
				//idle
				target = new Vector3( - myGoal.x * 0.1f, 1, ballPos.z * 0.9f);
				return target;
			}
			else if ((Mathf.Sign(ballVel.x) != Mathf.Sign(myGoal.x)) && (Mathf.Abs(diff_pos.z) < 3) && (Mathf.Abs(Z) < 10) && (Mathf.Sign(diff_pos.x) > 0) && (Mathf.Abs(ballVel.x) > 5))
			{
				diff_pos.z = 0;
				target = Quaternion.Euler(0, -Angulo_rot, 0) * diff_pos + myPos;
				return target;
			}
			else
			{
				//not idle
				target = ballPos;
				return target;		
			}
			*/

			if ((Mathf.Sign(ballVel.x) != Mathf.Sign(myGoal.x)) && (Mathf.Abs(diff_pos.z) < 3) && (Mathf.Abs(Z) < 20) && (Mathf.Sign(diff_pos.x) > 0) && (Mathf.Abs(ballVel.x) > 4))
			{
				diff_pos.z = 0;
				target = Quaternion.Euler(0, -Angulo_rot, 0) * diff_pos + ballPos;
				return target;
			}
			else if (Vector3.Distance(ballPos, myGoal) < 9.5f)
			{
				//idle
				target = new Vector3(-myGoal.x * 0.1f, 1, ballPos.z * 0.9f);
				return target;
			}
			else
			{
				//not idle
				target = ballPos;
				return target;
			}



		}

		public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
		{
		}

		public override FieldPosition GetInitialPosition()
		{
			return FieldPosition.C2 ;
		}

		public override string GetPlayerDisplayName()
		{
			return "Papas Fritas";
		}
	}
}