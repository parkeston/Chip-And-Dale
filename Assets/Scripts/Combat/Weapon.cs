using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private string targetLayer;

    private int layer;
    private void Awake()
    {
        layer = LayerMask.NameToLayer(targetLayer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer==layer && other.TryGetComponent(out Health health))
            health.TakeDamage(damage);
    }
}
