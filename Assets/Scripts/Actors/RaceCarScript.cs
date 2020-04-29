using System.Collections;
using UnityEngine;

public class RaceCarScript : MonoBehaviour
{
	public CameraManager cameraManager;
	public SpriteRenderer CarSpriteRenderer;
	public ParticleSystem Fire;
	//public SpriteOutline spriteOutline;
	public Camera CurrentCamera;
	public CarLapCounter CurrentCarLapCounter;

	public Color CarColor;
	public string CarColorName;
	public string PlayerName;
	public string SubstanceName;
	public string AdjectiveName;
	public int SubstanceIndex;
	public float SubstanceRank;

	public Texture2D CarTexture;
	public Texture2D PlayerFace;
}
