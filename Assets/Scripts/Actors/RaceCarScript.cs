using System.Collections;
using UnityEngine;

public class RaceCarScript : MonoBehaviour
{
	public CameraManager cameraManager;
	public SpriteRenderer CarSpriteRenderer;
	public ParticleSystem Fire;
	public CarLapCounter CurrentCarLapCounter;

	public Color CarColor;
	public string CarColorName;
	public string PlayerName;
	public string SubstanceName;
	public string AdjectiveName;
	public int SubstanceIndex;
	public double SubstanceRank;
	public Transform MyPlayerFaceTransform;
	public double myTime;
	public int RcsID;
	public bool Focused;
	public Texture2D CarTexture;
	public Texture2D PlayerFace;
}
