using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class masterVolume : MonoBehaviour {

	public GameObject sourceObject;
	public Slider mySlider;

	[HideInInspector]
	public AudioSource masterAudioSource;

	void Start () {
		this.masterAudioSource = sourceObject.GetComponent<AudioSource>();
	}
	
	public void myVolume(float vol){
		this.masterAudioSource.volume = mySlider.value;
	}
}
