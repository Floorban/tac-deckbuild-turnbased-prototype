using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public Machine[] machines = new Machine[4];
    void Update()
    {
        bool allMachinesFixed = true;

        for (int i = 0; i < machines.Length; i++)
        {
            if (!machines[i].isFixed)
            {
                allMachinesFixed = false;
                break;
            }
        }

        if (allMachinesFixed)
        {
            Actions.onGameOver();
        }
    }
}
