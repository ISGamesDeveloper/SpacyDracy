using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlaneCamera : MonoBehaviour
{
    public Transform PlaneTransform;
#if UNITY_IOS && !UNITY_EDITOR
    private void Start()
    {
        PlaneTransform.localScale = new Vector3(PlaneTransform.localScale.x, PlaneTransform.localScale.y * -1, PlaneTransform.localScale.z);
    }
#endif
}
