using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
   public abstract bool Pickup(ItemPicker itemPicker);
   public abstract void Use(ItemPicker itemPicker);

   protected virtual void Awake()
   {
      gameObject.layer = LayerMask.NameToLayer("Items");
   }
}
