using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity3D : MonoBehaviour
{

    [SerializeField] private Orbitable planet;
    private Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

    }

    void FixedUpdate()
    {
        planet.Attract(transform);
    }
}
