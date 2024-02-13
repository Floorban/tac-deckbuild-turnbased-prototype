using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAI : MonoBehaviour
{
    public GameObject unit;
    public float moveSpeed;
    public bool move;
    public GridManager gridManager;
    public List<GameObject> path;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    void Update()
    {
        if (gridManager && move)
        {
            path = gridManager.path;
            path.Reverse();
            MoveUnit();
            move = false;
        }
    }

    void MoveUnit()
    {
        StartCoroutine(MoveAlongPath());
    }

    IEnumerator MoveAlongPath()
    {
        foreach (GameObject gridCell in path)
        {
            Vector3 targetPosition = gridCell.transform.position;
            targetPosition.y = unit.transform.position.y;
            while (Vector3.Distance(unit.transform.position, targetPosition) > 0.1f)
            {
                unit.transform.position = Vector3.MoveTowards(unit.transform.position, targetPosition, Time.deltaTime * moveSpeed);
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
        path.Clear();
    }
}
