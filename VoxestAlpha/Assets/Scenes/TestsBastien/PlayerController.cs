using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    [SerializeField] private GameObject directionControllerObject;

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float distToGround;
    [SerializeField] private bool grounded = false;

    [SerializeField] private Vector3 vectorAvancer;
    [SerializeField] private Vector3 smoothMoveVelocity;

    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        distToGround = this.GetComponent<CapsuleCollider>().bounds.extents.y;
    }

    void Update()
    {
        Vector3 targetVectorAvancer = new Vector3(1,0,0) * speed;
        vectorAvancer = Vector3.SmoothDamp(vectorAvancer, targetVectorAvancer, ref smoothMoveVelocity, .15f);

    }

    void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(vectorAvancer) * Time.fixedDeltaTime);
    }
}
