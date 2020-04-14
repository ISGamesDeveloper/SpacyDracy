using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {

	public float acceleration = 0.3f;
	public float braking = 0.3f;
	public float steering = 4.0f;

	private PlayerInputs _inputs;
	private Rigidbody2D _rigidbody2D;

	void Start ()
	{
		_inputs = GetComponent<PlayerInputs>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		_rigidbody2D.AddTorque(_inputs.x * steering * -0.2f);
		_rigidbody2D.AddForce(transform.right * _inputs.y * acceleration * 50.0f);
	}
}
