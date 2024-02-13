using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canAct;
    public int actionPoints;
    public int maxActionPoints;
    public int minActionPoints = 0;

    public float movementSpeed = 5f;
    public float rotationSpeed = 180f;

    [SerializeField] bool isMoving = false;

    [SerializeField] GameObject playerDirector;
    void Start()
    {
        if (!canAct) return;
        actionPoints = maxActionPoints;
    }

    void Update()
    {
        if (canAct && !isMoving)
        {
            HandleMovement();
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
            StartCoroutine(RotatePlayer(-90f));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(RotatePlayer(90f));
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
    }
}
