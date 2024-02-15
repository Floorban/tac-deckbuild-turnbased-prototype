using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] bool allEnemiesFinished;

    public PropSpawner propSpawner;
    bool spawnEnemy = false;
    void OnEnable()
    {
        Actions.onGameOver += FinishTurn;
    }
    void OnDisable()
    {
        Actions.onGameOver -= FinishTurn;
    }
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        propSpawner = FindObjectOfType<PropSpawner>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerUnit.EndTurn();
            state = GameState.EnemyTurn;
            EnemyTurn();
        }
        HandleStateTransitions();
        HandleStates(state);
        if (state == GameState.Finish) return;
        StartBattle();
        EndTurnCondition(); 
    }
    void HandleStateTransitions()
    {

    }
    void HandleStates(GameState state)
    {
        switch(state)
        {
            case (GameState.Idle):
            //
            break;

            case (GameState.Start):
            //
            break;

            case (GameState.PlayerTurn):
                
            break;

            case (GameState.EnemyTurn):
      
            break;

            case (GameState.Finish):
                //
            break;
        }
    }
    void StartBattle()
    {
        if (start)
        {
            state = GameState.Start;
            propSpawner.SpawnProps();
            propSpawner.SpawnIndicators();
            StartCoroutine(SetUpState());
            start = false;
        }
    }
    void EndTurnCondition()
    {
        if (state == GameState.PlayerTurn && playerUnit.actionPoints <= 0)
        {
            playerUnit.EndTurn();
            state = GameState.EnemyTurn;
            EnemyTurn();
        }

        for (int i = 0; i < enemyUnits.Length; i++)
        {
            if (enemyUnits[i].canAct)
            {
                allEnemiesFinished = false;
            }
            else
            {
                allEnemiesFinished = true;
                break; // Exit the loop as soon as one enemy can still act
            }
        }

        if (allEnemiesFinished && state == GameState.EnemyTurn)
        {
            /*for (int i = 0; i < enemyUnits.Length; i++)
            {
                enemyUnits[i].EndTurn();
            }*/
            //PlayerTurn();
            start = true;
        }
    }
    IEnumerator SetUpState()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (!spawnEnemy)
            {
                playerUnit = player.GetComponent<Unit>();
                enemyUnits = new Unit[enemyPrefabs.Length];

                GameObject enemyGo = Instantiate(enemyPrefabs[i], enemySpawnTrans[i]);
                enemyUnits[i] = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Unit>();
            }
        }
        spawnEnemy = true;
        Debug.Log("Battel starts");
        yield return new WaitForSeconds(1f);
        gridManager.ClearGridFactors();
        PlayerTurn();
        state = GameState.PlayerTurn;
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
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyUnits[i].StartTurn();
            if (enemyUnits[i].canAct)
                StartCoroutine(gridManager.MoveEnemy());
        }
    }
    void FinishTurn()
    {
        state = GameState.Finish;
        start = false;
        playerUnit.actionPoints = 0;
        playerUnit.addedActionPoints = 0;
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyUnits[i].addedActionPoints = 0;
        }
        Debug.Log("Game Over");
    }
    public void StartGame()
    {
        state = GameState.Idle;
        start = true;
        spawnEnemy = false;
    }
}
