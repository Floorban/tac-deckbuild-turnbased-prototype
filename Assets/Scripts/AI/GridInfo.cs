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
    }
    void Update()
    {
        idText.text = accessID.ToString();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            rewardPoints = 10;
        }else if (other.gameObject.GetComponent<Prop>())
        {
            rewardPoints = 100;
        }else
        {
            rewardPoints = 1;
        }
    }
}
