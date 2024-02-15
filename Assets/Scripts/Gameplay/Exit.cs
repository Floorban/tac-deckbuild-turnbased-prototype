using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public Machine[] machines;

    private void Start()
    {
        Machine[] allMachines = FindObjectsOfType<Machine>();
        int numMachines = 4;
        if (allMachines.Length >= numMachines)
        {
            machines = new Machine[numMachines];
            for (int i = 0; i < numMachines; i++)
            {
                machines[i] = allMachines[i];
            }
        }
    }
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
