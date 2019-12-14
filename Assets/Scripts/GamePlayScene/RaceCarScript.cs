using System.Collections;
using UnityEngine;

public class RaceCarScript : MonoBehaviour
{
	public CameraManager cameraManager;
	public SpriteRenderer CarSpriteRenderer;
	public ParticleSystem Fire;
	public Camera CurrentCamera;
	public CarLapCounter CurrentCarLapCounter;
	public Texture2D CarTexture;
	public Color CarColor;
	public string CarName;
}
