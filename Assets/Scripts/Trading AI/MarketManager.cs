using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public List<GameObject> goodsPrefab = new List<GameObject>();
    public List<int> g_Price = new List<int>();
    public List<int> g_Num = new List<int>();

    public void ConsumeGoods(GameObject targetGoods)
    {
            for (int i = 0; i < goodsPrefab.Count; i++)
            {
                 targetGoods.GetComponent<Goods>().s_Price--;
            }
    }
}
