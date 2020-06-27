using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class AlienSpaceshipMotor : MonoBehaviour
{
    [SerializeField] private float heightLevelRange=1;
    [SerializeField] private float heightLevelStep=0.25f;

    [SerializeField] private float horizontalTravelDistance=5;
    [SerializeField] private float maxSpeed=3;
    
    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 destinationPoint;

    private float[] possibleHeightLevels;
    
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        startPoint = rigidbody2D.position - Vector2.right * horizontalTravelDistance / 2;
        endPoint = rigidbody2D.position + Vector2.right * horizontalTravelDistance / 2;
        rigidbody2D.position = startPoint;
        destinationPoint = endPoint;
        
        GeneratePossibleHeightLevels();
    }

    private void GeneratePossibleHeightLevels()
    {
        float minimumHeight = rigidbody2D.position.y - heightLevelRange / 2;
        float maximumHeight = rigidbody2D.position.y + heightLevelRange / 2;
        float heightLevel = minimumHeight;

        int levelsCount = (int)(heightLevelRange / heightLevelStep)+1;
        possibleHeightLevels = new float[levelsCount];

        possibleHeightLevels[0] = minimumHeight;
        possibleHeightLevels[levelsCount - 1] = maximumHeight;
        for (int i = 1; i < levelsCount-1; i++)
        {
            heightLevel += heightLevelStep;
            possibleHeightLevels[i] = heightLevel;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = Vector2.MoveTowards(rigidbody2D.position, destinationPoint, maxSpeed * Time.fixedDeltaTime);
        rigidbody2D.MovePosition(position);

        if ((position - destinationPoint).magnitude < 0.1f)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            float heightLevel = possibleHeightLevels[Random.Range(0, possibleHeightLevels.Length)];

            Vector2 currentPoint = destinationPoint;
            currentPoint.y = heightLevel;
            rigidbody2D.position = currentPoint;
            
            destinationPoint = destinationPoint == startPoint ? endPoint : startPoint;
            destinationPoint.y = startPoint.y = endPoint.y = heightLevel;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 centerPosition = transform.position;
        Vector3 halfDistance = Vector3.right * horizontalTravelDistance / 2;
        Gizmos.DrawLine(centerPosition,centerPosition+halfDistance);
        Gizmos.DrawLine(centerPosition,centerPosition-halfDistance);
        
        Gizmos.DrawSphere(centerPosition+halfDistance,0.1f);
        Gizmos.DrawSphere(centerPosition-halfDistance,0.1f);
    }
}
