using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public int rewardPoints;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Unit>())
        {
            Unit unit = other.GetComponent<Unit>();
            unit.AddActionPoints(rewardPoints);
            gameObject.SetActive(false);
        }
    }
}
