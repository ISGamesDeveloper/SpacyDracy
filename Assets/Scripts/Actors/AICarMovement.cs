using System.Collections;
using UnityEngine;

public class AICarMovement : MonoBehaviour
{
    public bool Finish;
    public bool Start;
    public float acceleration = 0.3f;
    public float braking = 0.3f;
    public float steering = 4.0f;
    public float velocity;

    private float targetRot;
    private float rot;

    [HideInInspector]
    public Transform transform;

    private Rigidbody2D carRigidbody2D;
    private Vector2 towardNextTrigger;
    public Vector3 target;

    private void Awake()
    {
        transform = gameObject.transform;
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }

    //private GameObject PointObject;

    public void OnNextTrigger(TrackLapTrigger next)
    {
        if (!gameObject.activeInHierarchy)
            return;

        next.GenerateNextPoint();

        target = next.NextPoint;

        //if(PointObject != null)
        //{
        //    Destroy(PointObject);
        //}

        //PointObject = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), target, new Quaternion(0,0,0,0));
        //PointObject.name = gameObject.name + " Point";
        //PointObject.GetComponent<MeshRenderer>().material.color = Color.red;
        //PointObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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

    void FixedUpdate()
    {
        if (Finish || !Start)
            return;

        SteerTowardsTarget();

        velocity = carRigidbody2D.velocity.magnitude;
        velocity += acceleration;

        carRigidbody2D.velocity = transform.right * velocity;
        carRigidbody2D.angularVelocity = 0.0f;
    }
}
