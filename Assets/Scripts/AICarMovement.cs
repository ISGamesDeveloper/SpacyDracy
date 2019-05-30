using UnityEngine;
using System.Collections;

public class AICarMovement : MonoBehaviour {

	public float acceleration = 0.3f;
	public float braking = 0.3f;
	public float steering = 4.0f;

	private float distance = 0;
	private float velocity;
	private float targetRot;
	private float rot;

	private Rigidbody2D carRigidbody2D;
	private Vector2 towardNextTrigger;
	private Vector3 target;

	private void Start()
	{
		carRigidbody2D = GetComponent<Rigidbody2D>();
		StartCoroutine(UpdateInfo());
	}

	public void OnNextTrigger(TrackLapTrigger next) {

		// choose a target to drive towards
		target = Vector3.Lerp(next.transform.position - next.transform.right, 
		                      next.transform.position + next.transform.right, 
		                      Random.value);
	}

	private void SteerTowardsTarget ()
	{
		towardNextTrigger = target - transform.position;
		targetRot = Vector2.Angle (Vector2.right, towardNextTrigger);
		if (towardNextTrigger.y < 0.0f) {
			targetRot = -targetRot;
		}
		rot = Mathf.MoveTowardsAngle (transform.localEulerAngles.z, targetRot, steering);
		transform.eulerAngles = new Vector3 (0.0f, 0.0f, rot);
	}

	// update for physics
	void FixedUpdate() {

		SteerTowardsTarget();

		// always accelerate
		velocity = carRigidbody2D.velocity.magnitude;
		velocity += acceleration;

		// apply car movement
		carRigidbody2D.velocity = transform.right * velocity;
		carRigidbody2D.angularVelocity = 0.0f;
	}

	private IEnumerator UpdateInfo()
	{
		while(true)
		{
			var p1 = gameObject.transform.position;
			yield return new WaitForSeconds(1f);
			var p2 = gameObject.transform.position;

			var td = (p2 - p1).magnitude;


			//var preDis = (td.x * td.y) * Time.deltaTime;
			 
			distance += td;
			//Debug.Log(gameObject.name + "  velocity:" + velocity + "  rot:" + rot + "  targetRot:" + targetRot);
			Debug.Log(gameObject.name + "  distance:" + distance);
			//yield return new WaitForSeconds(3f);
		}
	}
}
