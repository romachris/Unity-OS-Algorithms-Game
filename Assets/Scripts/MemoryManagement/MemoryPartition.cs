using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MemoryPartition : MonoBehaviour
{
    public float size;
    public bool isAvailable, selected;



    private void Start()
    {
        selected = false;
        isAvailable = true;
        size = GetComponent<RectTransform>().sizeDelta.y;
        GetComponent<ButtonController>().setSize(Convert.ToInt32(size));
    }

    public void partitionSelected()
    {
        //partition is selected
        selected = true;
    }
}
