using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public int rewardPoints;
    PropSpawner spawner;

    void Start()
    {
      spawner = FindObjectOfType<PropSpawner>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Unit>())
        {
            Unit unit = other.GetComponent<Unit>();
            unit.AddActionPoints(rewardPoints);
            gameObject.SetActive(false);
            spawner.canSpawn = true;
            spawner.canSpawnIndicator = true;
        }
    }
}
