using System.Collections;
using UnityEngine;

public class AICarMovement : MonoBehaviour
{
	public bool Finish;
	public float acceleration = 0.3f;
	public float braking = 0.3f;
	public float steering = 4.0f;
	public float velocity;

	private float targetRot;
	private float rot;

	[HideInInspector] public Transform transform;

	private Rigidbody2D carRigidbody2D;
	private Vector2 towardNextTrigger;
	private Vector3 target;

	private void Awake()
	{
		transform = gameObject.transform;
		carRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		//StartCoroutine(GetSpeedPlayer());
	}

	public void OnNextTrigger(TrackLapTrigger next)
	{
		target = Vector3.Lerp(next.transform.position - next.transform.right,
							  next.transform.position + next.transform.right,
							  Random.value);
	}

	private void SteerTowardsTarget()
	{
		towardNextTrigger = target - transform.position;
		targetRot = Vector2.Angle(Vector2.right, towardNextTrigger);
		if (towardNextTrigger.y < 0.0f)
		{
			targetRot = -targetRot;
		}
		rot = Mathf.MoveTowardsAngle(transform.localEulerAngles.z, targetRot, steering);
		transform.eulerAngles = new Vector3(0.0f, 0.0f, rot);
	}

	// update for physics
	void FixedUpdate()
	{
		if(Finish)
			return;

		SteerTowardsTarget();

		velocity = carRigidbody2D.velocity.magnitude;
		velocity += acceleration;

		carRigidbody2D.velocity = transform.right * velocity;
		carRigidbody2D.angularVelocity = 0.0f;
	}

	IEnumerator GetSpeedPlayer()
	{
		while (true)
		{
			Debug.Log("velocity " + velocity);
			yield return new WaitForSeconds(2);
		}
	}
}
