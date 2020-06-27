using System;
using UnityEngine;

[RequireComponent(typeof(PhysicsObject))]
[RequireComponent(typeof(Animator))]
public class PlayerPlatformerController : MonoBehaviour
{
   [SerializeField] private float maxSpeed;
   [SerializeField] private float jumpHeight;

   private PhysicsObject physicsObject;
   private float jumpSpeed;

   private Animator animator;
   private SpriteRenderer spriteRenderer;

   private void Start()
   {
      animator = GetComponent<Animator>();
      spriteRenderer = GetComponent<SpriteRenderer>();

      physicsObject = GetComponent<PhysicsObject>();
      jumpSpeed = Mathf.Sqrt(-2 * physicsObject.GravityModifier * Physics2D.gravity.y * jumpHeight);
   }

   private void Update()
   {
      Vector2 velocity = Vector2.zero;
      velocity.x = Input.GetAxisRaw("Horizontal") * maxSpeed;

      if (Input.GetButtonDown("Jump") && physicsObject.IsGrounded)
      {
         velocity.y = jumpSpeed;
         physicsObject.SetVelocity(velocity.x, velocity.y);
      }
      else
      {
         physicsObject.SetVelocity(velocity.x);
      }

      bool flipSprite = spriteRenderer.flipX ? (velocity.x > 0) : (velocity.x < 0);
      if (flipSprite)
         spriteRenderer.flipX = !spriteRenderer.flipX;
      
      animator.SetFloat("hSpeed", Mathf.Abs(velocity.x));
      animator.SetBool("grounded",physicsObject.IsGrounded);
   }
}
