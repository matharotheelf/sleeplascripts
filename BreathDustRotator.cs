using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathDustRotator : MonoBehaviour
{
    [SerializeField] OVRCameraRig oVRCameraRig;

    void Update()
    {
        transform.position = oVRCameraRig.centerEyeAnchor.position - 0.1f*Vector3.up;
        transform.rotation = Quaternion.LookRotation(oVRCameraRig.centerEyeAnchor.forward);
    }
}
