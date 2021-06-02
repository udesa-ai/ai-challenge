using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.Las_Papas
{
	public class PlayerThree : TeamPlayer
	{
		private float Area = 12;
		private float midPos = 7;
		private float velocidadDanger = 10;
		private float Borde_Arco = 5;
		public override void OnUpdate()
		{
			var Target = getTarget();
			MoveBy(GetDirectionTo(Target));
		}

		private Vector3 getTarget()
        {
			var BallVel = GetBallVelocity();
			var BallPos = GetBallPosition();
			var MyGoal = GetMyGoalPosition();
			Vector3 target;

			var dangerDir = Mathf.Sign(BallVel.x) == Mathf.Sign(MyGoal.x);
			if (dangerDir && (Vector3.Magnitude(BallVel) > velocidadDanger))
			{
				bool move;
				SaveIt(BallPos, BallVel, out move, out target);

				var DistBall = BallPos - MyGoal;
				var DistPlayer = GetPosition() - MyGoal;

				if (move && (Mathf.Abs(DistBall.x) > Mathf.Abs(DistPlayer.x)))
                {
					return target;
                }
			}


			var distToGoal = Vector3.Distance(BallPos, MyGoal);
			if (distToGoal < Area)
			{
				target = BallPos + 0.1f * BallVel * Vector3.Distance(BallPos, GetPosition());
				return target;
			}
			else
			{
				var ballFromGoal = BallPos - MyGoal;
				target = ballFromGoal.normalized * midPos + MyGoal;
				
			}
			
			return target;
        }

		private void SaveIt(Vector3 P, Vector3 V, out bool move, out Vector3 Target)
        {
			float Z = P.z + (V.z / V.x) * (Mathf.Abs(P.x - GetMyGoalPosition().x));
			if (Mathf.Abs(Z) <= Borde_Arco)
			{
				move = true;
				var Angulo_rot = Mathf.Atan2(V.z, V.x) * 180 / Mathf.PI;
				Vector3 diff_pos = Quaternion.Euler(0, Angulo_rot, 0) * (GetPosition() - P);
				diff_pos.z = 0;
				Target = Quaternion.Euler(0, -Angulo_rot, 0) * diff_pos + P;
			}
			else
			{
				move = false;
				Target = GetPosition();
			}

		}

		public override void OnReachBall()
		{
			var target1 = GetRivalGoalPosition() + new Vector3(0, 0, 1f);
			var target2 = GetRivalGoalPosition() + new Vector3(0, 0, -1f);
			var target3 = GetTeamMatesInformation()[0].Position;
			var target4 = GetTeamMatesInformation()[1].Position;

			var doI1 = IsClear(GetBallPosition(), target1, 0.5f);
			var doI2 = IsClear(GetBallPosition(), target2, 0.5f);
			var doI3 = IsClear(GetBallPosition(), target3, 0.5f);
			var doI4 = IsClear(GetBallPosition(), target3, 1.5f);

			Vector3 dir;
			int angle_check, strength;

			if (doI3)
			{
				dir = GetDirectionTo(target3);
				angle_check = 45;
				strength = 2;
			}
			else if (doI2)
			{
				dir = GetDirectionTo(target2);
				angle_check = 45;
				strength = 3;
			}
			else if (doI1)
			{
				dir = GetDirectionTo(target1);
				angle_check = 45;
				strength = 3;
			}
			else if (doI4)
			{
				dir = GetDirectionTo(target4);
				angle_check = 10;
				strength = 2;
			}
			else
			{
				dir = GetDirectionTo(GetRivalGoalPosition());
				angle_check = 360;
				strength = 3;
			}

			var mydir = this.Direction;
			var angle = Mathf.Abs(Vector3.Angle(mydir, dir));

			if (angle > angle_check)
			{
				ShootBall(dir, ShootForce.Low);
			}
			else
			{
				switch (strength)
                {
					case 1:
						ShootBall(dir, ShootForce.Low);
						break;
					case 2:
						ShootBall(dir, ShootForce.Medium);
						break;
					case 3:
						ShootBall(dir, ShootForce.High);
						break;
				}
					
				ShootBall(dir, ShootForce.High);
			}
			


		}

		private bool IsClear(Vector3 start, Vector3 finish, float changui)
        {
			var dir = finish - start;
			dir = dir.normalized;
			int i;
			int saltos = 20;
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

		public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
		{
		}

		public override FieldPosition GetInitialPosition()
		{
			return FieldPosition.B2;
		}

		public override string GetPlayerDisplayName()
		{
			return "Pure de Papas";
		}
	}
}