using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    [SerializeField] private float minimumTimeRangeForSpawn;
    [SerializeField] private float maximumTimeRangeForSpawn;

    private GameObjectPool gameObjectPool;

    private float spawnTime;
    
    private void Awake()
    {
        gameObjectPool = new GameObjectPool(10,objectToSpawn);

        StartCoroutine(WaitForSpawn(Random.Range(minimumTimeRangeForSpawn, maximumTimeRangeForSpawn)));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator WaitForSpawn(float time)
    {
        yield return new WaitForSeconds(time);

        Spawn();
    }

    private void Spawn()
    {
        GameObject pooledObject = gameObjectPool.GetPooledObject();
        pooledObject.transform.position = transform.position;
        
        float spawnTime = Random.Range(minimumTimeRangeForSpawn, maximumTimeRangeForSpawn);
        StartCoroutine(WaitForSpawn(spawnTime));
    }
}
