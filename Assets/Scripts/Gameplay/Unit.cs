using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool canAct;
    public int actionPoints;
    public int maxActionPoints;
    public int addedActionPoints;
    public int health;
    public int maxHealth;

    void Update()
    {
        EndTurn();
    }
    public void AddActionPoints(int nextTurnAddedPoints)
    {
        addedActionPoints = nextTurnAddedPoints;
    }
    public void StartTurn()
    {
        actionPoints = maxActionPoints + addedActionPoints;
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
