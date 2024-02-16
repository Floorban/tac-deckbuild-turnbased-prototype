using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridInfo : MonoBehaviour
{
    public int accessID;
    public int rewardPoints;
    public int gridFactor;
    public int x;
    public int y;

    [SerializeField] TMP_Text idText;
    void Start()
    {
        idText = GetComponentInChildren<TMP_Text>();
    }
    void Update()
    {
        rewardPoints = 1 - (accessID / 3) + gridFactor;
        idText.text = rewardPoints.ToString();

        if (accessID <= 0)
            accessID = 1;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            gridFactor = 5;
        }
        else if (other.gameObject.GetComponent<Prop>())
        {
            gridFactor = 4;
        }
        else if (other.gameObject.CompareTag("Indicator"))
        {
            gridFactor = 3;
        }  
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() || other.gameObject.GetComponent<Prop>() || other.gameObject.CompareTag("Indicator"))
        {
            gridFactor = 0;
        }
    }
    public Vector2Int GetGridPosition()
    {
        return new Vector2Int(x, y);
    }
}
