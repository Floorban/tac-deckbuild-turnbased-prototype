using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public bool findDistance;
    public int rows;
    public int columns;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftButtonPos = Vector3.zero;
    public GameObject[,] gridArray;
    public GameObject[,] rewardArray;
    public int startX;
    public int startY;
    public int endX;
    public int endY;
    public List<GameObject> path = new List<GameObject>();

    void Awake()
    {
        gridArray = new GameObject[rows, columns];
        if (gridPrefab)
        {
            GenerateGrid();
        }
        else
        {
            Debug.Log("generate failed");
        }
    }
    public void SetNeighborGridFactors()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GridInfo gridInfo = gridArray[i, j].GetComponent<GridInfo>();

                if (gridInfo.gridFactor > 0)
                {
                    UpdateNeighborGridFactors(gridInfo);
                }
            }
        }
    }
    void UpdateNeighborGridFactors(GridInfo gridInfo)
    {
        Vector2Int gridPosition = gridInfo.GetGridPosition();

        // Define a range for neighboring grids
        int range = 6;

        for (int a = Mathf.Max(0, gridPosition.x - range); a <= Mathf.Min(columns - 1, gridPosition.x + range); a++)
        {
            for (int b = Mathf.Max(0, gridPosition.y - range); b <= Mathf.Min(rows - 1, gridPosition.y + range); b++)
            {
                if (gridArray[a, b] && (a != gridPosition.x || b != gridPosition.y)) // Skip the center grid
                {
                    GridInfo neighborGrid = gridArray[a, b].GetComponent<GridInfo>();
                    int distance = Mathf.Max(Mathf.Abs(a - gridPosition.x), Mathf.Abs(b - gridPosition.y));
                    int newFactor = Mathf.Max(0, gridInfo.gridFactor - distance);
                    neighborGrid.gridFactor = Mathf.Max(neighborGrid.gridFactor, newFactor);
                }
            }
        }
    }
    public void ClearGridFactors()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GridInfo gridInfo = gridArray[i, j].GetComponent<GridInfo>();
                gridInfo.gridFactor = 0;
            }
        }
    }
    public void TryFindPath()
    {
        findDistance = true;
    }
    public void TryMove()
    {
        move = true;
    }
    void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(gridPrefab, 
                    new Vector3(leftButtonPos.x + scale * i, leftButtonPos.y, leftButtonPos.z + scale * j), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridInfo>().x = i;
                obj.GetComponent<GridInfo>().y = j;
                gridArray[i, j] = obj;
            }
        }
    }
    void SetDistance()
    {
        Init();
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];
        for (int step = 1; step < rows * columns; step++)
        {
            foreach (GameObject obj in gridArray)
            {
                if (obj && obj.GetComponent<GridInfo>().accessID == step - 1)
                {
                    TestFourDirections(obj.GetComponent<GridInfo>().x, obj.GetComponent<GridInfo>().y, step);
                }
            }
        }
    }
    void SetPath()
    {
        SetNeighborGridFactors();
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();
        if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridInfo>().accessID > 0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridInfo>().accessID - 1;
        }
        else
        {
            Actions.onEnemyFinish();
            Debug.Log("can't reach the endPos");
            return;
        }
        for (int i = step; step > -1; step--)
        {
            if (TestDirection(x, y, step, 1))
            {
                tempList.Add(gridArray[x, y + 1]);
            }

            if (TestDirection(x, y, step, 2))
            {
                tempList.Add(gridArray[x + 1, y]);
            }

            if (TestDirection(x, y, step, 3))
            {
                tempList.Add(gridArray[x, y - 1]);
            }

            if (TestDirection(x, y, step, 4))
            {
                tempList.Add(gridArray[x - 1, y]);
            }
            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridInfo>().x;
            y = tempObj.GetComponent<GridInfo>().y;
            tempList.Clear();
        }
    }
    void Init()
    {
        foreach (GameObject obj in gridArray)
        {
            obj.GetComponent<GridInfo>().accessID = -1;
        }
        gridArray[startX, startY].GetComponent<GridInfo>().accessID = 0;
    }
    bool TestDirection(int x, int y, int step, int direction)
    {
        switch (direction)
        {
            case 4:
                if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridInfo>().accessID == step)
                    return true;
                else
                    return false;
            case 3:
                if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridInfo>().accessID == step)
                    return true;
                else
                    return false;
            case 2:
                if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridInfo>().accessID == step)
                    return true;
                else
                    return false;
            case 1:
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridInfo>().accessID == step)
                    return true;
                else
                    return false;

        }
        return false;
    }
    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))
            SetAccessId(x, y + 1, step);

        if (TestDirection(x, y, -1, 2))
            SetAccessId(x + 1, y, step);

        if (TestDirection(x, y, -1, 3))
            SetAccessId(x, y - 1, step);

        if (TestDirection(x, y, -1, 4))
            SetAccessId(x - 1, y, step);
    }
    void SetAccessId(int x, int y, int step)
    {
        if (gridArray[x, y])
            gridArray[x, y].GetComponent<GridInfo>().accessID = step;
    }
    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * rows * columns;
        int indexNum = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNum = i;
            }
        }
        return list[indexNum];
    }

    public GameObject unit;
    public float moveSpeed;
    public bool move;
    public void MoveUnit()
    {
        if (move && path.Count > 0)
        {
            if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridInfo>().accessID > 0)
            {
                path.Reverse();
                StartCoroutine(MoveAlongPath());
                //Vector3 targetPos = path[1].transform.position;
                //unit.transform.position = targetPos;
            }
            else
            {
                Debug.Log("im gonna sleep");
            }
            //startX = (int)unit.transform.position.x;
            //startY = (int)unit.transform.position.z;
            //path.Clear();
        }
        move = false;
        //ClearGridFactors();
    }
    IEnumerator MoveAlongPath()
    {
        Unit unitComponent = unit.GetComponent<Unit>();

        while (unitComponent.actionPoints > 0 && path.Count > 1)
        {
            GameObject gridCell = path[1];
            Vector3 targetPosition = gridCell.transform.position;
            targetPosition.y = unit.transform.position.y;
            unit.transform.position = targetPosition;
            yield return new WaitForSeconds(0.5f);
            path.RemoveAt(0);
            unitComponent.actionPoints--;
        }

        if (unit.transform == nextLocation.transform)
        {
            unitComponent.actionPoints = 0;
            unitComponent.canAct = false;
        }
        startX = (int)unit.transform.position.x;
        startY = (int)unit.transform.position.z;
        path.Clear();
    }
    public bool canFind;
    public int points;
    public Transform nextLocation;
    public IEnumerator MoveEnemy()
    {
        FindPlayer();
        unit = FindObjectOfType<EnemyController>().gameObject;
        //canFind = false;
        endX = (int)nextLocation.position.x;
        endY = (int)nextLocation.position.z;
        //TryFindPath();
        if (findDistance && !move)
        {
            SetDistance();
            SetPath();
            //findDistance = false;
            move = true;
        }

        yield return new WaitForSeconds(1f);
        MoveUnit();

    }
    void FindPlayer()
    {
        if (!canFind) return;
        /*GameObject player = FindObjectOfType<PlayerController>().gameObject;
        nextLocation = player.transform;*/
        nextLocation = FindGridWithHighestFactor().transform;
    }
    public GameObject FindGridWithHighestFactor()
    {
        GridInfo highestFactorGrid = null;
        int highestFactor = 0;

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GridInfo gridInfo = gridArray[i, j].GetComponent<GridInfo>();

                if (gridInfo.rewardPoints > highestFactor)
                {
                    highestFactor = gridInfo.rewardPoints;
                    highestFactorGrid = gridInfo;
                }
            }
        }

        return highestFactorGrid?.gameObject;
    }
}
