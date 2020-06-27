using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
   private GameObject[] pool;

   public GameObjectPool(int size, GameObject objeectToPool)
   {
      pool = new GameObject[size];

      for (int i = 0; i < size; i++)
      {
         pool[i] = GameObject.Instantiate(objeectToPool);
         pool[i].SetActive(false);
      }
   }

   public GameObject GetPooledObject()
   {
      for (int i = 0; i < pool.Length; i++)
      {
         if (!pool[i].activeSelf)
         {
            pool[i].SetActive(true);
            return pool[i];
         }
      }

      return null;
   }
}
