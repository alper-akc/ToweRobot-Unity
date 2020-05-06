using UnityEngine;
using System.Collections;

public class GameTimer
{
	private float elapsedTime;
	private float timeAmount;
	private bool enabled;

	public GameTimer(float time)
	{
		timeAmount = time;
	}

	public void setEnabled(bool _enabled)
	{
		enabled = _enabled;
	}

	public bool isEnabled()
	{
		return enabled;
	}

	void Start()
	{

	}

	void Update()
	{

	}
}
