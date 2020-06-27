using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private float pickUpRadius;
    [SerializeField] private LayerMask pickUpLayer;

    private Item currentItem;
    
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(currentItem==null)
                TryToPickUpItem();
            else
            {
                UseItem();
            }
        }
    }

    private void TryToPickUpItem()
    {
       Collider2D collider2D = Physics2D.OverlapCircle(transform.position, pickUpRadius, pickUpLayer);
       if (collider2D != null && collider2D.TryGetComponent(out Item item))
       {
           if (item.Pickup(this))
           {
               currentItem = item;
               animator.SetTrigger("pickup");
               animator.SetLayerWeight(1,1);
           }
       }
    }

    private void UseItem()
    {
        currentItem.Use(this);
        currentItem = null;
        animator.SetLayerWeight(1,0);
    }
}
