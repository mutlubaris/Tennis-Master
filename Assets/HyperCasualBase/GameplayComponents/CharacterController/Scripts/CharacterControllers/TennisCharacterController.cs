using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisCharacterController : CharacterControllerBase
{
    public float moveSpeed = 5f;
    private float currentSpeed;
    private Vector3 oldPosition;
    private Vector3 newPosition;

    private void Start()
    {
        oldPosition = transform.position;
    }

    public override float CurrentSpeed()
    {
        return currentSpeed;
    }

    public override bool IsGrounded() { return true; }

    public override void Jump() { }

    private void FixedUpdate()
    {
        newPosition = transform.position;
        currentSpeed = (newPosition - oldPosition).magnitude / Time.deltaTime;
        oldPosition = transform.position;
    }

    public override void Move(Vector3 Direction)
    {
        transform.position = Vector3.MoveTowards(transform.position, Direction, moveSpeed * Time.deltaTime);
    }
}
