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
        transform.position = handTransform.position;
    }
}
