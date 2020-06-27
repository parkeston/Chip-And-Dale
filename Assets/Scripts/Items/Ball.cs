using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsObject))]
[RequireComponent(typeof(Collider2D))]
public class Ball : Item
{
    [SerializeField] private Vector3 relativePickupPosition;
    [SerializeField] private float throwHeight;
    
    private PhysicsObject physicsObject;
    private new Rigidbody2D rigidbody2D;

    private float throwForce;
    
    protected override void Awake()
    {
        base.Awake();

        physicsObject = GetComponent<PhysicsObject>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        throwForce = Mathf.Sqrt(-2 * Physics.gravity.y * physicsObject.GravityModifier * throwHeight);
    }

    public override bool Pickup(ItemPicker itemPicker)
    {
        if(!physicsObject.IsGrounded)
            return false;
        
        rigidbody2D.simulated = false;
        transform.position = itemPicker.transform.position + relativePickupPosition;
        transform.SetParent(itemPicker.transform);

        return true;
    }

    public override void Use(ItemPicker itemPicker)
    {
        transform.SetParent(null);
        
        rigidbody2D.simulated = true;
        physicsObject.SetVelocity(0,throwForce);
    }
}
