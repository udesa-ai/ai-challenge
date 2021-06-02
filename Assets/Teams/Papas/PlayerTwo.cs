using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.Las_Papas
{
	public class PlayerTwo : TeamPlayer
	{
		private float Area_Arco = 4;
		private float Velocidad_danger = 5;
		private float Borde_Arco = 2.5f;
		public override void OnUpdate()
		{
			var ballPosition = GetBallPosition();
			var ballVelocity = GetBallVelocity();
			var target = Goalkeeper_target(ballPosition, ballVelocity);
			MoveBy(GetDirectionTo(target));


		}

		private Vector3 Goalkeeper_target(Vector3 ballPosition, Vector3 ballVel)
		{
			var ballVelAbs = Mathf.Sqrt(Mathf.Pow(ballVel.x,2) + Mathf.Pow(ballVel.z, 2));
			var balldist = Mathf.Abs(Vector3.Distance(ballPosition, GetMyGoalPosition()));

			if ((ballVelAbs > Velocidad_danger) && (Mathf.Sign(ballVel.x) == Mathf.Sign(GetMyGoalPosition().x)) && balldist > 2)
            {
				bool move;
				Vector3 pos;
				
				CheckGol(ballPosition, ballVel, out move, out pos);
				
				if (move)
                {
					return pos;
				}
            }

			

			
			Vector3 Respuesta;

			if (balldist < Area_Arco)
            {
				Respuesta = ballPosition + ballVel * 0.1f * (Vector3.Distance(ballPosition, GetPosition()));
            }
			else
            {
				Vector3 origin = GetMyGoalPosition();
				origin.x *= 1.2f;
				var radius = Mathf.Sqrt(9);

				//var angulo = Mathf.Atan2(ballPosition.z - origin.z, ballPosition.x - origin.x);
				var alpha = Mathf.Atan2(ballPosition.z - GetMyGoalPosition().z, ballPosition.x - GetMyGoalPosition().x);
				float angulo;
				if (GetMyGoalPosition().x < 0)
				{
					var gamma = Mathf.Asin(Mathf.Sin(Mathf.PI - alpha) * 2 / radius);
					angulo = alpha - gamma;
				}
				else
                {
					var gamma = Mathf.Asin(Mathf.Sin(alpha) * 2 / radius);
					angulo = alpha + gamma;
				}
				



				var testx = Mathf.Cos(angulo) * radius;
				var testz = Mathf.Sin(angulo) * radius;

				Respuesta = new Vector3(testx, 0, testz) + origin;
				if (Mathf.Abs(Respuesta.x) > Mathf.Abs(GetMyGoalPosition().x))
				{
					Respuesta.x = GetMyGoalPosition().x;
				}

			}
			

			return  Respuesta;
		}

		private void CheckGol(Vector3 P, Vector3 V, out bool move, out Vector3 pos)
        {
			float Z = P.z + (V.z / V.x) * (Mathf.Abs(P.x - GetMyGoalPosition().x));
			if (Mathf.Abs(Z) <= Borde_Arco)
            {
				move = true;
				var Angulo_rot = Mathf.Atan2(V.z, V.x)*180/Mathf.PI;
				Vector3 diff_pos = Quaternion.Euler(0, Angulo_rot, 0) * (GetPosition() - P);
				diff_pos.z = 0;
				pos = Quaternion.Euler(0, -Angulo_rot, 0) * diff_pos + P;

				

				//pos = new Vector3(GetMyGoalPosition().x, 1, Z);
			}
            else
            {
				move = false;
				pos = GetMyGoalPosition();
            }
        }

		public override void OnReachBall()
		{
			var target1 = GetRivalGoalPosition() + new Vector3(0, 0, 1f);
			var target2 = GetRivalGoalPosition() + new Vector3(0, 0, -1f);
			var target3 = GetTeamMatesInformation()[0].Position;
			var target4 = GetTeamMatesInformation()[1].Position;

			var doI1 = IsClear(GetBallPosition(), target1, 1f);
			var doI2 = IsClear(GetBallPosition(), target2, 1f);
			var doI3 = IsClear(GetBallPosition(), target3, 1.5f);
			var doI4 = IsClear(GetBallPosition(), target3, 1.5f);

			Vector3 dir;

			if (doI1)
			{
				dir = GetDirectionTo(target1);
				ShootBall(dir, ShootForce.High);
			}
			else if (doI2)
			{
				dir = GetDirectionTo(target2);
				ShootBall(dir, ShootForce.High);
			}
			else if (doI3)
			{
				dir = GetDirectionTo(target3);
				ShootBall(dir, ShootForce.High);
			}
			else if (doI4)
            {
				dir = GetDirectionTo(target4);
				ShootBall(dir, ShootForce.Medium);
			}
			else
			{
				dir = GetDirectionTo(GetRivalGoalPosition());
				ShootBall(dir, ShootForce.High);
			}

			
			
		}

		private bool IsClear(Vector3 start, Vector3 finish, float changui)
		{
			var dir = finish - start;
			dir = dir.normalized;
			int i;
			int saltos = 20;
			float dist1, dist2, dist3;
			for (i = 1; i < (saltos + 1); i++)
			{
				var checkpos = dir * i + start;
				dist1 = Vector3.Distance(GetRivalsInformation()[0].Position, checkpos);
				dist2 = Vector3.Distance(GetRivalsInformation()[1].Position, checkpos);
				dist3 = Vector3.Distance(GetRivalsInformation()[2].Position, checkpos);
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
			return FieldPosition.A2;
		}

		public override string GetPlayerDisplayName()
		{
			return "Papas a la Crema";
		}
	}
}