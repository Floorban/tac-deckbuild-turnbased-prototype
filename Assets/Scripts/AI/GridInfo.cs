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
        gridFactor = 1;
    }
    void Update()
    {
        if (accessID <= 0)
            accessID = 1;

        rewardPoints = gridFactor * (5 / accessID);
        idText.text = rewardPoints.ToString();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            gridFactor = 50;
        }
        else if (other.gameObject.GetComponent<Prop>())
        {
            gridFactor = 40;
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
            gridFactor = 1;
        }
    }
}
