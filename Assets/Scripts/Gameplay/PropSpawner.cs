using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    public bool canSpawn;
    public bool spawned;
    public GridManager gridManager;
    [SerializeField] GameObject particalPrefab;
    [SerializeField] GameObject propPrefab;
    [SerializeField] Transform spawnTrans;
    void Start()
    {
        gridManager = GetComponent<GridManager>();
    }
    public void SpawnIndicators()
    {
        int randomX = Random.Range(1, 9);
        int randomY = Random.Range(1, 9);
        GameObject gridCell = gridManager.gridArray[randomX,randomY];
        spawnTrans = gridCell.transform;
        GameObject indicator = Instantiate(particalPrefab, gridCell.transform);
        canSpawn = true;
        if (spawned)
        {
            Destroy(indicator);
        }
    }
    public void SpawnProps()
    {
        if (canSpawn && !spawned)
        {
            Instantiate(propPrefab, spawnTrans);
            spawned = true;
        }
    }
}
