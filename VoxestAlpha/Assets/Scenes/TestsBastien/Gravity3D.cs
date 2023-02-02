using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity3D : MonoBehaviour
{

    [SerializeField] private GameObject planet;
    private Rigidbody myRigidbody;
    [SerializeField] private float distToGround;

    private Vector3 gravityVector;
    private Quaternion directionVector;

    void Start()
    {
        myRigidbody = this.GetComponent<Rigidbody>();
        distToGround = this.GetComponent<CapsuleCollider>().bounds.extents.y;
    }

    void Update()
    {
        //calculate gravity vector
        gravityVector = planet.transform.position - transform.position;
        gravityVector = gravityVector.normalized * Time.deltaTime;

        //apply gravity
        myRigidbody.AddForce(gravityVector * planet.GetComponent<Orbitable>().gravity);


        //calculate direction to look at
        directionVector = Quaternion.LookRotation(gravityVector, Vector3.up);
        directionVector *= Quaternion.LookRotation(Vector3.up);

        //apply direction to look at
        transform.localRotation = directionVector;
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.01f);
    }
}
