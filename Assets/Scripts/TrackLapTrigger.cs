using UnityEngine;
using System.Collections;

public class TrackLapTrigger : MonoBehaviour
{
	[HideInInspector]
	public Vector3 NextPoint;
	public int CurrentRank;
	public TrackLapTrigger next;

	private CarLapCounter carLapCounter;
	private BoxCollider2D[] boxCollider2D;

	private void Awake()
	{
		CurrentRank = transform.GetSiblingIndex();
		boxCollider2D = gameObject.GetComponents<BoxCollider2D>();
	}

	public void GenerateNextPoint()
	{
		var randomIndex = Random.Range(0, boxCollider2D.Length);
		var collider = boxCollider2D[randomIndex];

		var px1 = Random.Range(collider.bounds.min.x, collider.bounds.max.x);
		var py1 = Random.Range(collider.bounds.min.y, collider.bounds.max.y);
		var pointPosition = new Vector3(px1, py1, transform.position.z);

		NextPoint = pointPosition;
	}

	//Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	//{
	//	var p1 = point;
	//	var dir = point - pivot; // get point direction relative to pivot

	//	dir = Quaternion.Euler(angles) * dir; // rotate it
	//	point = dir + pivot; // calculate rotated point

	//	Debug.Log(gameObject.name +  "---  Point: " + p1 + "  Pivot: " + pivot + "  Angles: " + angles + "  Dir: " + dir + "  ItogPoint: " + point);

	//	return point;
	//}

	void OnTriggerEnter2D(Collider2D other)
	{
		carLapCounter = other.gameObject.GetComponent<CarLapCounter>();

		if (carLapCounter) {
			carLapCounter.OnLapTrigger(this);
		}

		var rank = CurrentRank + (carLapCounter.CurrentLap * 100);

		carLapCounter.currentRaceCarScript.SubstanceRank = rank;
		ApplicationMain.Instance.CameraManager.CheckCurrentRankin();
	}
}
