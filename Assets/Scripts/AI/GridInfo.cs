using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridInfo : MonoBehaviour
{
    public int accessID;
    public int rewardPoints;
    public int x;
    public int y;

    [SerializeField] TMP_Text idText;
    void Start()
    {
        idText = GetComponentInChildren<TMP_Text>();
        rewardPoints = 1;
    }
    void Update()
    {
        idText.text = accessID.ToString();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            rewardPoints = 5;
        }
        else if (other.gameObject.GetComponent<Prop>())
        {
            rewardPoints = 4;
        }
        else if (other.gameObject.CompareTag("Indicator"))
        {
            rewardPoints = 3;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() || other.gameObject.GetComponent<Prop>() || other.gameObject.CompareTag("Indicator"))
        {
            rewardPoints = 1;
        }
    }
}
