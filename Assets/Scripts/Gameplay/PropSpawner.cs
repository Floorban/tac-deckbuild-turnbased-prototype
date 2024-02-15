using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PropSpawner : MonoBehaviour
{
    public bool canSpawnIndicator;
    public bool canSpawn;
    public bool spawned;
    public GridManager gridManager;
    [SerializeField] GameObject particalPrefab;
    [SerializeField] GameObject propPrefab;
    [SerializeField] GameObject indicator;
    [SerializeField] Transform spawnTrans;
    void Start()
    {
        gridManager = GetComponent<GridManager>();
        canSpawnIndicator = true;
    }
    public void SpawnIndicators()
    {
        int randomX = Random.Range(1, 9);
        int randomY = Random.Range(1, 9);
        GameObject gridCell = gridManager.gridArray[randomX, randomY];
        spawnTrans = gridCell.transform;
        if (canSpawnIndicator)
        {
            indicator = Instantiate(particalPrefab, gridCell.transform);
            canSpawn = true;
            canSpawnIndicator = false;
            spawned = false;
        }
    }
    public void SpawnProps()
    {
        if (canSpawn && !spawned)
        {
            Instantiate(propPrefab, spawnTrans);
            Destroy(indicator);
            spawned = true;
            canSpawn = false;
        }
    }
}
