using UnityEngine;
using System.Collections;

public class PatrollerAI : EnemyAI 
{
	public enum Direction { forward = 1, right=2, backward=3, left=4 };

	private enum State { moving, turning, firing, continuing, chasing, standing };

	private State state;
	private State lastState;

	public Direction direction;

	private Transform trans;
	public float speed; 

	private Vector3 enemyPos;

	//initial rotation for the patroller if it does not look forward initially
	private bool initialRotation;

	//coordinates of the patrolling rectangle
	private Vector3 upperLeft;
	private Vector3 upperRight;
	private Vector3 lowerRight;
	private Vector3 lowerLeft;

	//distances that the patroller will walk, along x and z axis
	public float distanceZ, distanceX;

	private bool stunned;

	float startTime = -10;
	float rotTime = 1;

	Quaternion targetRotation = Quaternion.identity;
	Quaternion initRotation = Quaternion.identity;

	private void createPatrolRectangle()
	{
		Vector3 vectorZ = new Vector3(0, 0, distanceZ);
		Vector3 vectorX = new Vector3(distanceX, 0, 0);

		switch (direction)
		{
			case Direction.forward:

				lowerLeft = transform.position;
				upperLeft = lowerLeft + vectorZ;
				upperRight = upperLeft + vectorX;
				lowerRight = upperRight - vectorZ;

				break;

			case Direction.right:

				upperLeft = transform.position;
				upperRight = upperLeft + vectorX;
				lowerRight = upperRight - vectorZ;
				lowerLeft = lowerRight - vectorX;

				break;

			case Direction.backward:

				upperRight = transform.position;
				lowerRight = upperRight - vectorZ;
				lowerLeft = lowerRight - vectorX;
				upperLeft = lowerLeft + vectorZ;

				break;

			case Direction.left:

				lowerRight = transform.position;
				lowerLeft = lowerRight - vectorX;
				upperLeft = lowerLeft + vectorZ;
				upperRight = upperLeft + vectorX;

				break;
		}

		initialRotation = true;
	}

	private void drawRect()
	{
		Debug.DrawLine(lowerRight, lowerLeft);
		Debug.DrawLine(lowerLeft, upperLeft);
		Debug.DrawLine(upperLeft, upperRight);
		Debug.DrawLine(upperRight, lowerRight);
	}

	// "Stunner" methods

	public void Stun()
	{
		stunned = true;

		Invoke("cancelStun", GameGlobals.stunTime);
	}

	private void cancelStun()
	{
		stunned = false;
		//if(playerDetected)
		//    playerDetected = false;
	}

	public bool isStunned()
	{
		return stunned;
	}

	new void Start () 
    {
		base.Start();

		targetRotation = transform.rotation;

		state = State.moving;

		createPatrolRectangle();
	}

	private float setDirectionAngle()
	{
		float angle = 0.0f;

		switch (direction)
		{
			case Direction.forward:
				angle = 0.0f;
				break;

			case Direction.right:
				angle = 90.0f;
				break;

			case Direction.backward:
				angle = 180.0f;
				break;

			case Direction.left:
				angle = 270.0f;
				break;
		}

		return angle;
	}

	public void patrol()
	{
		drawRect();

		print(state);

		if (stunned)
		{
			print("stun on");
			state = State.standing;
		}

		switch (state)
		{
			case State.standing:
				if (!stunned)
				{
					print("stun off");
					state = lastState;
				}
				break;

			case State.chasing:
				break;

			case State.firing:

				if (playerDetected)
				{
					Transform target = GameObject.Find("Junkman").transform;

					//Vector3 difference = (target.transform.position - transform.position);

					//difference.Normalize();
					//difference.Scale(new Vector3(speed, speed, speed));

					//transform.position += difference;

					float tx = target.transform.position.x;
					float tz = target.transform.position.z;
					float x = transform.position.x;
					float z = transform.position.z;
					float tangent;
					float rotation;

					float pi = Mathf.PI;
					float angle;

					tangent = Mathf.Atan((tx - x) / (tz - z));

					angle = ((tangent * 360) / pi);

					if (tz < z)
						angle += 360;

					rotation = (angle / 2);

					transform.eulerAngles = new Vector3(0, rotation, 0);

					lastState = state = State.firing;
				}
				else
				{
					Quaternion rotDirection = Quaternion.AngleAxis(setDirectionAngle(),Vector3.up);

					//print("Angle: " + setDirectionAngle());

					targetRotation = rotDirection;
					initRotation = transform.rotation;

					//print(initRotation + "   " + targetRotation);

					startTime = Time.time;

					lastState = state = State.continuing;
				}
				break;

			case State.continuing:

				if (playerDetected)
				{
					lastState = state = State.firing; 
					break;
				}

				if (Time.time <= (startTime + rotTime) && Time.time > (startTime))
				{
					transform.rotation = Quaternion.Slerp(initRotation, targetRotation, (Time.time - startTime) / rotTime);
					lastState = state = State.continuing;
				}
				else
				{
					transform.rotation = targetRotation;
					lastState = state = State.moving;
				}

				break;

			case State.turning:

				if (playerDetected)
				{
					lastState = state = State.firing;
					break;
				}

				if (Time.time < (startTime + rotTime) && Time.time > (startTime))
				{
					transform.rotation = Quaternion.Slerp(initRotation, targetRotation, (Time.time - startTime) / rotTime);
				}
				else
				{
					transform.rotation = targetRotation;
					lastState = state = State.moving;
				}

				break;

			case State.moving:

				if (playerDetected)
				{
					lastState = state = State.firing;
					break;
				}

				enemyPos = transform.position;

				float moveSpeed = speed * Time.deltaTime;

				float enemyPosZ = enemyPos.z;
				float enemyPosX = enemyPos.x;


				switch (direction)
				{
					case Direction.forward:

						if (initialRotation)
							initialRotation = false;

						transform.Translate(0, 0, moveSpeed);

						if (enemyPosZ >= upperLeft.z)
						{
							Quaternion rot = transform.rotation;
							Quaternion rot90 = Quaternion.AngleAxis(90.0f, Vector3.up);

							//print("rot90: " + rot90);

							//deltaAngle += 1f;

							targetRotation = rot * rot90;
							initRotation = transform.rotation;

							//print(initRotation + "   " + targetRotation);

							startTime = Time.time;

							direction = Direction.right;

							lastState = state = State.turning;
						}
						break;

					case Direction.right:

						if (initialRotation)
						{
							transform.RotateAround(transform.position, Vector3.up, 90.0f);
							initialRotation = false;
						}

						transform.Translate(0, 0, moveSpeed);

						if (enemyPosX >= upperRight.x)
						{
							Quaternion rot = transform.rotation;
							Quaternion rot90 = Quaternion.AngleAxis(90.0f, Vector3.up);

							//print("rot90: " + rot90);

							//deltaAngle += 1f;

							targetRotation = rot * rot90;
							initRotation = transform.rotation;

							//print(initRotation + "   " + targetRotation);

							startTime = Time.time;

							direction = Direction.backward;

							lastState = state = State.turning;
						}
						break;

					case Direction.backward:

						if (initialRotation)
						{
							transform.RotateAround(transform.position, Vector3.up, 180.0f);
							initialRotation = false;
						}

						transform.Translate(0, 0, moveSpeed);

						if (enemyPosZ <= lowerRight.z)
						{
							Quaternion rot = transform.rotation;
							Quaternion rot90 = Quaternion.AngleAxis(90.0f, Vector3.up);

							//print("rot90: " + rot90);

							//deltaAngle += 1f;

							targetRotation = rot * rot90;
							initRotation = transform.rotation;

							//print(initRotation + "   " + targetRotation);

							startTime = Time.time;

							direction = Direction.left;

							lastState = state = State.turning;
						}
						break;

					case Direction.left:

						if (initialRotation)
						{
							transform.RotateAround(transform.position, Vector3.up, 270.0f);
							initialRotation = false;
						}

						transform.Translate(0, 0, moveSpeed);

						if (enemyPosX <= lowerLeft.x)
						{
							Quaternion rot = transform.rotation;
							Quaternion rot90 = Quaternion.AngleAxis(90.0f, Vector3.up);

							//print("rot90: " + rot90);

							//deltaAngle += 1f;

							targetRotation = rot * rot90;
							initRotation = transform.rotation;

							//print(initRotation + "   " + targetRotation);

							startTime = Time.time;

							direction = Direction.forward;

							lastState = state = State.turning;
						}
						break;
				}

				
				break;
		}
		RaycastHit hit;

		if (Physics.Raycast(transform.position + Vector3.up * 5, -Vector3.up, out hit))
		{
			Vector3 new_rot = hit.normal;
			
			Vector3 baktigimnokta = this.transform.position + this.transform.TransformDirection(Vector3.forward);
			baktigimnokta.y = GameObject.Find("Terrain").GetComponent<Terrain>().SampleHeight(baktigimnokta) + GameObject.Find("Terrain").GetComponent<Terrain>().transform.position.y;

			Quaternion q = Quaternion.LookRotation(baktigimnokta - transform.position, new_rot);
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, q, 0.1f);
			//this.transform.rotation = q;

			Debug.DrawLine(this.transform.position, this.transform.position + this.transform.TransformDirection(Vector3.forward) * 5, Color.yellow);
			Debug.DrawLine(this.transform.position, this.transform.position + new_rot * 5);
		}

		Vector3 vector = this.transform.position;
		vector.y = GameObject.Find("Terrain").GetComponent<Terrain>().SampleHeight(vector) + GameObject.Find("Terrain").GetComponent<Terrain>().transform.position.y;
		this.transform.position = vector;

		turnRangeCircle();
	}

	public void changeDetected(bool detected)
	{
		playerDetected = detected;
	}

	private void turnRangeCircle()
	{
		//transform.Find("RangeCircle").RotateAround(transform.Find("RangeCircle").position, Vector3.up, 60 * Time.deltaTime);
		Quaternion q = Quaternion.identity;
		q *= Quaternion.Euler(0, 60 * Time.time, 0);
		q *= Quaternion.Euler(90, 0, 0);
		transform.Find("RangeCircle").transform.rotation = q;
	}

	public void Update()
	{
		patrol();
	}
}
