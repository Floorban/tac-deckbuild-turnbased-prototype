using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /*public bool canFind;
    public int points;
    public Transform nextLocation;
    public Unit enemyUnit;
    public GridManager gridManager;
    void Start()
    {
        enemyUnit = GetComponent<Unit>();
        gridManager = FindObjectOfType<GridManager>();
    }
    void Update()
    {
        FindPlayer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enemyUnit.canAct)
            {
                gridManager.unit = this.gameObject;
                canFind = false;
                gridManager.endX = (int)nextLocation.position.x;
                gridManager.endY = (int)nextLocation.position.z;
                gridManager.TryFindPath();
                gridManager.TryMove();
                enemyUnit.canAct = false;
            }
        }
    }
    void FindPlayer()
    {
        if (!canFind) return;
        GameObject player = FindObjectOfType<PlayerController>().gameObject;
        nextLocation = player.transform;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject)
        {
            points = gameObject.GetComponent<GridInfo>().rewardPoints;
        }
    }*/
}
