using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRigidbody;

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float distToGround;
    [SerializeField] private bool grounded = false;

    [SerializeField] private Vector3 vectorAvancer;

    void Start()
    {
        myRigidbody = this.GetComponent<Rigidbody>();
        distToGround = this.GetComponent<CapsuleCollider>().bounds.extents.y;
    }

    void Update()
    {
        vectorAvancer = transform.rotation * Vector3.forward * speed * Time.deltaTime + transform.position;
        if(grounded = isGrounded())
            myRigidbody.MovePosition(vectorAvancer);
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, transform.rotation * Vector3.down, distToGround + 0.1f);
    }
}
