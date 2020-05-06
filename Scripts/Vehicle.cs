using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vehicle : MonoBehaviour
{
	public float maxSpeed = 90f;
	private float minSpeed;
	private const float deacSpeed = 0.01f;
	private const float accIncrease = 1.5f;
	private const float brakeIncrease = 0.1f;
	private const float deacIncrease = 0.1f;

	private float speed = 0f;
	private float acceleration = 0f;
	private float deacceleration = deacSpeed;
	private float brake = 0f;

	private AudioSource loudAudio, quietAudio;
	private const float deltaSound = 0.08f;
	private const float maxLoudVolume = 0.8f;
	private const float maxQuietVolume = 0.8f;

	public float force = 800f;
	public float torque = 1000f;

	public float waterLevel = 40f;

	private bool alive;

	public float getSpeed()
	{
		return speed;
	}

	public void Start()
	{
		minSpeed = -maxSpeed;
		loudAudio = GameObject.Find("LoudSound").GetComponentInChildren<AudioSource>();
		quietAudio = GameObject.Find("QuietSound").GetComponentInChildren<AudioSource>();

		loudAudio.volume = 0;
		quietAudio.volume = 1;

		alive = true;
	}

	private void playLoud()
	{
		loudAudio.volume += deltaSound;
		if (loudAudio.volume >= 0.8f)
			loudAudio.volume = 0.8f;

		quietAudio.volume -= deltaSound;
		if (quietAudio.volume <= 0f)
			quietAudio.volume = 0f;
	}

	private void playQuiet()
	{
		quietAudio.volume += deltaSound;
		if (quietAudio.volume >= 0.8f)
			quietAudio.volume = 0.8f;

		loudAudio.volume -= deltaSound;
		if (loudAudio.volume <= 0f)
			loudAudio.volume = 0f;
	}

	public void Update()
	{

		float input_vert = Input.GetAxis("Vertical");
		float input_horz = Input.GetAxis("Horizontal");

		if (input_vert > 0 && input_vert <= 1)
		{
			brake = 0f;
			deacceleration = deacSpeed;
			acceleration += accIncrease;
			speed += acceleration;

			if (speed > maxSpeed)
				speed = maxSpeed;

			if (acceleration > 0.1f)
				acceleration = 0.1f;

			playLoud();

		}
		else if (input_vert < 0 && input_vert >= -1)
		{
			if (speed == 0.0f)
			{
				deacceleration = deacSpeed;
				acceleration += accIncrease;
				speed -= acceleration;

				if (speed < minSpeed)
					speed = minSpeed;

				if (acceleration < -0.1f)
					acceleration = -0.1f;
			}
			else
			{
				acceleration = 0f;
				brake += brakeIncrease;

				speed -= brake;

				if (speed < minSpeed)
					speed = minSpeed;

				if (acceleration < -0.1f)
					acceleration = -0.1f;
			}

			playLoud();
		}
		else
		{
			if (speed > 0f)
			{
				speed -= deacceleration;
				deacceleration += deacIncrease;

				if (speed < 0f)
					speed = 0f;
			}
			else if (speed < 0f)
			{
				speed += deacceleration;
				deacceleration += deacIncrease;

				if (speed > 0f)
					speed = 0f;
			}
			else
			{
				deacceleration = deacSpeed;
				speed = 0f;
				acceleration = 0f;
				brake = 0f;
			}

			playQuiet();

		}

		//rigidbody.AddForce(transform.rotation * new Vector3(0f, 0f, force * input_vert));

		//rigidbody.AddTorque(transform.rotation * new Vector3(0f, torque * input_horz, 0f));

		if (!GameGlobals.gameOver)
		{

			float angle = Mathf.Asin(Mathf.Sign(input_vert) * input_horz);

			transform.RotateAround(transform.position, Vector3.up, angle * 0.8f);

			if (angle >= 90 && angle <= 270)
				transform.Translate(0, 0, -speed * Time.deltaTime);
			else
				transform.Translate(0, 0, speed * Time.deltaTime);

			if (transform.rotation.x <= -45 || transform.rotation.x >= 45)
			{
				Quaternion targetRot = Quaternion.LookRotation(new Vector3(0, transform.rotation.y, transform.rotation.z));
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.1f);
			}

			if (transform.rotation.z <= -45 || transform.rotation.z >= 45)
			{
				Quaternion targetRot = Quaternion.LookRotation(new Vector3(transform.rotation.x, transform.rotation.y, 0));
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.1f);
			}

			if (Input.GetKey(KeyCode.R))
			{
				resetPosition();
				return;
			}

			//print("pos: " + transform.localPosition.y + " sampleHeight: " + GameObject.Find("Terrain").GetComponent<Terrain>().SampleHeight(transform.localPosition));

			float sampleHeight = GameObject.Find("Terrain").GetComponent<Terrain>().SampleHeight(transform.position);

			if (alive)
			{
				if ((transform.position.y < GameObject.Find("Terrain").GetComponent<Terrain>().SampleHeight(transform.position) + waterLevel) && sampleHeight < 10)
				{
					alive = false;
					Detonator det = this.transform.Find("DetonatorExplosion").gameObject.GetComponent<Detonator>();
					det.Explode();
					GameGlobals.pauseTime();
					Invoke("Respawn", 2f);
				}
			}

		}
	}
	
	private void resetPosition()
	{
		float yRot = closestAngle(transform.localRotation.eulerAngles.y);

		Quaternion targetRot = Quaternion.Euler(new Vector3(0, yRot, 0));
		transform.localRotation = targetRot;
	}

	private Vector3 closestRespawnPoint()
	{

		float min = 999999f;
		int index = 0;

		Vector3 playerPos = transform.position;

		GameObject game_obj = GameObject.Find("RespawnPoints");

		Transform tr_points = game_obj.transform;

		int child_cnt = tr_points.childCount;

		Transform[] points = new Transform[child_cnt];

		for (int i = 0; i < child_cnt; i++)
		{
			points[i] = tr_points.GetChild(i);
		}

		for (int i = 0; i < child_cnt; i++)
		{
			float dist = Vector3.Distance(playerPos, points[i].position);

			if (dist < min)
			{
				min = dist;
				index = i;
			}
		}

		return points[index].position;

	}

	private void Respawn()
	{
		transform.position = closestRespawnPoint();
		resetPosition();
		GameGlobals.startTime();
		alive = true;
	}

	private float closestAngle(float angle)
	{
		int index = 0;

		float[] angles = { 0, 90, 180, 270, 360};

		for (int i = 0; i < 4; i++)
		{
			if (angle >= angles[i] && angle <= angles[i + 1])
			{
				float dif1 = Mathf.Abs(angle - angles[i]);
				float dif2 = Mathf.Abs(angle - angles[i + 1]);

				if (dif1 < dif2)
					index = i;
				else
					index = i + 1;
			}
		}

		return angles[index];
	}
}