using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool canAct;
    public int actionPoints;
    public int maxActionPoints;
    public int health;
    public int maxHealth;
    public void StartTurn()
    {
        actionPoints = maxActionPoints;
        canAct = true;
    }
    public void EndTurn()
    {
        if (actionPoints < 0)
        {
            canAct = false;
        }
    }
}
