using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Unit playerUnit;
    public float movementSpeed = 5f;
    public float rotationSpeed = 180f;
    public bool canFix;
    public GameObject machine;

    [SerializeField] bool isMoving = false;

    [SerializeField] GameObject playerDirector;
    TurnBasedSystem system;
    void Start()
    {
        playerUnit = GetComponent<Unit>();
        system = FindObjectOfType<TurnBasedSystem>();
    }

    void Update()
    {
        if (playerUnit.canAct && !isMoving && playerUnit.actionPoints > 0 && system.state == TurnBasedSystem.GameState.PlayerTurn)
        {
            HandleMovement();
        }

        if (Input.GetKeyDown(KeyCode.Space) && canFix)
        {
            machine.GetComponent<Machine>().isFixed = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Machine>())
        {
            machine = other.gameObject;
            canFix = true;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Actions.onGameOver();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Machine>())
        {
            machine = null;
            canFix = false;
        }
    }
    void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(MovePlayer(playerDirector.transform.forward));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(MovePlayer(-playerDirector.transform.forward));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(MovePlayer(-playerDirector.transform.right));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(MovePlayer(playerDirector.transform.right));
        }
    }

    IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        Vector3 targetPosition = transform.position + direction;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * movementSpeed;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
        playerUnit.actionPoints--;
    }

    IEnumerator RotatePlayer(float angle)
    {
        isMoving = true;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0) * transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        transform.rotation = targetRotation;
        isMoving = false;
        playerUnit.actionPoints--;
    }
}
