using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
	public Button ChangeCameraButton;
	public Camera MainCamera;
	public Camera AutoCamera;
	public Transform Target;
	public bool MainCameraView;

    void Start()
    {
		ChangeCameraButton.onClick.AddListener(ChangeStateCameras);
		ChangeStateCameras();
	}

    // Update is called once per frame
    void Update()
    {
		if (MainCameraView || Target == null)
			return;

		AutoCamera.transform.position = new Vector3(Target.position.x, Target.position.y, Target.position.z - 10);
	}

	public void ChangeStateCameras()
	{
		MainCameraView = MainCameraView ? false : true;

		MainCamera.enabled = MainCameraView;
		AutoCamera.enabled = !MainCameraView;
	}

	public void ChangeTarget(Transform target)
	{
		Target = target;//создать синглтон, что бы передавать из AICarMovement объектов сюда их таргет!!!!!
	}
}
