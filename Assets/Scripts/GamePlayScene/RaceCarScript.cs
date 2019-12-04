using System.Collections;
using UnityEngine;

public class RaceCarScript : MonoBehaviour
{
	public CameraManager cameraManager;
	public SpriteRenderer CarSpriteRenderer;
	public ParticleSystem Fire;
	public string PlayerName;
	public Camera CurrentCamera;
	public CarLapCounter CurrentCarLapCounter;
	public Color CarColor;
}
