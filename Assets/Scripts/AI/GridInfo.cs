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
}
