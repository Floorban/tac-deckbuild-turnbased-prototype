using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TurnBasedSystem : MonoBehaviour
{
    public bool start;
    public enum GameState
    {
        Idle,
        Start,
        PlayerTurn,
        EnemyTurn,
        Finish
    }
    public GameState state;
    public GameObject player;
    public GameObject[] enemyPrefabs;
    public Transform[] enemySpawnTrans;

    public Unit playerUnit;
    public Unit[] enemyUnits;

    public bool canFind;
    public int points;
    public Transform nextLocation;
    public GridManager gridManager;
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }
    void Update()
    {
        StartBattle();
        EndTurnCondition();
    }
    
    void StartBattle()
    {
        if (start)
        {
            state = GameState.Start;
            StartCoroutine(SetUpState());
            start = false;
        }
    }
    void EndTurnCondition()
    {
       if (state == GameState.PlayerTurn && playerUnit.actionPoints < 0)
        {
            playerUnit.EndTurn();
            state = GameState.EnemyTurn;
            EnemyTurn();
        }

        bool[] isFinished = new bool[enemyPrefabs.Length];

        for (int i = 0; i < enemyPrefabs.Length; i++) 
        {
            if (state == GameState.EnemyTurn && enemyUnits[i].actionPoints < 0)
            {
                isFinished[i] = true;
            }
        }
        bool allEnemiesFinished = true;

        for (int i = 0; i < isFinished.Length; i++)
        {
            if (!isFinished[i])
            {
                allEnemiesFinished = false;
                break;
            }
        }

        if (allEnemiesFinished)
        {
            for (int i = 0; i < enemyUnits.Length; i++)
            {
                enemyUnits[i].EndTurn();
            }
            FinishTurn();
            state = GameState.Finish;
        }
    }
    IEnumerator SetUpState()
    {
        playerUnit = player.GetComponent<Unit>();
        enemyUnits = new Unit[enemyPrefabs.Length];

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject enemyGo = Instantiate(enemyPrefabs[i], enemySpawnTrans[i]);
            enemyUnits[i] = enemyGo.GetComponent<Unit>();
        }
        Debug.Log("Battel starts");
        yield return new WaitForSeconds(1f);

        state = GameState.PlayerTurn;
        PlayerTurn();
        yield return null;
    }
    void PlayerTurn()
    {
        Debug.Log("Player turn");
        if (playerUnit)
        playerUnit.StartTurn();
    }
    void EnemyTurn()
    {
        Debug.Log("Enemy turn");
        FindPlayer();
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyUnits[i] = enemyPrefabs[i].GetComponent<Unit>();
            enemyUnits[i].StartTurn();

            if (enemyUnits[i].canAct)
            {
                /*gridManager.unit = FindObjectOfType<EnemyController>().gameObject;
                canFind = false;
                gridManager.endX = (int)nextLocation.position.x;
                gridManager.endY = (int)nextLocation.position.z;
                gridManager.TryFindPath();
                gridManager.MoveUnit();*/
                StartCoroutine(gridManager.MoveEnemy());
                Debug.Log("enemy moved");
                //enemyUnits[i].canAct = false;
            }
        }
    }
    void FindPlayer()
    {
        if (!canFind) return;
        GameObject player = FindObjectOfType<PlayerController>().gameObject;
        nextLocation = player.transform;
    }
    void FinishTurn()
    {
        Debug.Log("Next Round");
    }
}
