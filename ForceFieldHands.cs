using Meta.XR.BuildingBlocks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldHands : MonoBehaviour
{
    [SerializeField] Transform handPosition;
    [SerializeField] ParticleSystemForceField forceField;
    [SerializeField] float fieldStrengthMultiplier;

    private Vector3 previousHandPosition;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = handPosition.position;

        float handSpeed = (handPosition.position - previousHandPosition).magnitude/Time.deltaTime;

        forceField.gravity = handSpeed * fieldStrengthMultiplier;

        previousHandPosition = handPosition.position;
    }
}