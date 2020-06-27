using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PhysicsObject))]
public class Alien : MonoBehaviour
{
   [SerializeField] private float maxSpeed;

   private PhysicsObject physicsObject;
   private SpriteRenderer spriteRenderer;
   private Animator animator;

   private Transform player;
   private int movementDirection;
   
   private void Awake()
   {
      physicsObject = GetComponent<PhysicsObject>();
      spriteRenderer = GetComponent<SpriteRenderer>();
      animator = GetComponent<Animator>();
      
      player = FindObjectOfType<PlayerPlatformerController>().transform;
   }

   private void OnEnable()
   {
      isHuntingPlayer = false;
   }

   private bool isHuntingPlayer;

   private void Update()
   {
      if (physicsObject.IsGrounded)
      {
         if (!isHuntingPlayer)
         {
            movementDirection = player.position.x > transform.position.x ? 1 : -1;
            isHuntingPlayer = true;
         }
         
         physicsObject.SetVelocity(movementDirection * maxSpeed);
         animator.SetFloat("hSpeed",maxSpeed);
      }

      bool flipSprite = spriteRenderer.flipX ? (movementDirection > 0) : (movementDirection< 0);
      if (flipSprite)
         spriteRenderer.flipX = !spriteRenderer.flipX;
      
   }
}
