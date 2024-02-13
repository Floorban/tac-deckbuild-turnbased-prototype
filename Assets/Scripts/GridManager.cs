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
    void Update()
    {
        if (findDistance && !move)
        {
            SetDistance();
            SetPath();
            findDistance = false;
        }

        if (move && path.Count > 0)
        {
            MoveUnit();
            move = false;
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
                GameObject obj = Instantiate(gridPrefab, new Vector3(leftButtonPos.x + scale * i, leftButtonPos.y, leftButtonPos.z + scale * j), Quaternion.identity);
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
            SetVisited(x, y + 1, step);

        if (TestDirection(x, y, -1, 2))
            SetVisited(x + 1, y, step);

        if (TestDirection(x, y, -1, 3))
            SetVisited(x, y - 1, step);

        if (TestDirection(x, y, -1, 4))
            SetVisited(x - 1, y, step);
    }
    void SetVisited(int x, int y, int step)
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
    void MoveUnit()
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
    IEnumerator MoveAlongPath()
    {

        foreach (GameObject gridCell in path)
        {
            Vector3 targetPosition = gridCell.transform.position;
            targetPosition.y = unit.transform.position.y;
            unit.transform.position = targetPosition;

            yield return new WaitForSeconds(0.1f);
        }
        startX = (int)unit.transform.position.x;
        startY = (int)unit.transform.position.z;
        path.Clear();
    }
}
