using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    public GameObject home;
    public GameObject destination;
    public bool canTrade;
    public bool canTravel;

    public enum states
    {
        Trading,
        Traveling
    };

    void Update()
    {
        if (transform.position == destination.transform.position)
        {

        }
    }

    public void HandleStates(states s)
    {
        switch (s)
        {
            case (states.Trading):
                canTrade = true;
                Trade();
            break;
            case (states.Traveling):
                canTravel = true;
                Travel();
            break; 
        }
    }
    public void EnterMarket()
    {
        canTrade = true;
    }
    public void Trade()
    {
        if (!canTrade) return;


        canTrade = false;
    }
    public void Travel()
    {
        if (!canTravel) return;

        canTravel = false;
    }
}
