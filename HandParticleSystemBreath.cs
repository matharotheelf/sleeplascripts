using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Input;

public class HandParticleSystemBreath : MonoBehaviour
{
    [SerializeField] Transform handTransform;

    // Update is called once per frame
    void Update()
    {
        // move particle system to the centre of hands
        transform.position = handTransform.position;
    }
}
