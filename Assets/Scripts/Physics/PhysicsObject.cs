using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsObject : MonoBehaviour
{
    [SerializeField] private float gravityModifier;
    [SerializeField] private float minimumMovementDistance=0.001f;
    [SerializeField] private float shellRadius = 0.01f;
    [SerializeField] private float minGroundYNormal; //min ground/slope supported angle
    
    private new Rigidbody2D rigidbody2D;
    private Vector2 velocity;
    
    private ContactFilter2D contactFilter;
    private readonly RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    private readonly List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>();

    public bool IsGrounded { get; private set; }
    private Vector2 groundNormal;
    
    public float GravityModifier => gravityModifier;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.isKinematic = true;
        rigidbody2D.useFullKinematicContacts = true;
        rigidbody2D.simulated = true;

        //setting layer collision mask for custom collision detection based on project physics settings
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    private void OnEnable()
    {
        velocity = Vector2.zero;
        IsGrounded = false;
    }

    public void SetVelocity(float horizontal)
    {
        velocity.x = horizontal;
    }

    public void SetVelocity(float horizontal, float vertical)
    {
        velocity.x = horizontal;
        velocity.y = vertical;
    }

    private void FixedUpdate()
    {
        IsGrounded = false;

        velocity += Physics2D.gravity * (gravityModifier * Time.fixedDeltaTime); //applying gravity acceleration (v = v0 + at)
        Vector2 deltaPosition = velocity * Time.fixedDeltaTime; //delta position = v0t+at^2/2 = (v0 +at/2)*t = vt; - removing division by 2
        
        //horizontal movement
        Vector2 moveAlongGround = new Vector2(groundNormal.y,-groundNormal.x); //vector perpendicular to ground normal
        Vector2 movement = moveAlongGround * deltaPosition.x;
        Move(movement,false);
        
        //vertical movement
        movement = Vector2.up*deltaPosition.y;
        //todo: do not apply gravity if grounded???
        Move(movement,true);
    }

    private void Move(Vector2 movement, bool isMovingVertically)
    {
        float movementDistance = movement.magnitude;
        if (movementDistance > minimumMovementDistance)
        {
            
            int hitCount = rigidbody2D.Cast(movement, contactFilter, hitBuffer, movementDistance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < hitCount; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundYNormal) //if current object has ground angle
                {
                    IsGrounded = true;
                    if (isMovingVertically)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;//if falls to ground move object only up to ground collision normal
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                    velocity -= projection * currentNormal;

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                movementDistance = modifiedDistance < movementDistance ? modifiedDistance : movementDistance; //preventing going beyond shell distance
            }
        }

        rigidbody2D.position += movement.normalized * movementDistance;
        //rigidbody2D.MovePosition(rigidbody2D.position+movement.normalized*movementDistance);
    }
}
