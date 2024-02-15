using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Machine : MonoBehaviour
{
    public bool isFixed;
    public bool canFixed;
    public Renderer render;

    private void Start()
    {
        render = GetComponent<Renderer>();
        render.material.color = Color.red;   
    }
    void Update()
    {
        if (isFixed)
        {
            render.material.color = Color.green;
        }
        else
        {
            render.material.color = Color.red;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Unit>()) 
        {
            canFixed = true;
        }else
        {
            canFixed &= false;
        }
    }

}
