using System;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
   [SerializeField] private float maxSpeed;
   [SerializeField] private float jumpHeight;
   //get rid of inheritance, make use of physics object like character controller

   private float jumpSpeed;
   private void Start()
   {
      jumpSpeed = Mathf.Sqrt(-2 * GravityModifier * Physics2D.gravity.y*jumpHeight);
   }

   protected override void ComputeVelocity()
   {
      Vector2 move = Vector2.zero;
      move.x = Input.GetAxisRaw("Horizontal");

      if (Input.GetButtonDown("Jump") && isGrounded)
      {
         velocity.y = jumpSpeed;
      }

      targetVelocity = move.normalized * maxSpeed;
   }
}
