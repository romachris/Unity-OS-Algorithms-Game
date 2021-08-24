using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MemoryProcess : MonoBehaviour
{
    public string processName;
    public int positionIndex;
    public float size;
    public int partitionIndex;
    public GameObject slider;
    public Canvas canvas;
    public GameObject mySlider;
    public int arrivalTime;
    public bool signal,selected;
    //public int roundRobinCounter;
    public bool processTurn;
    //public List<Color32> priorityColors;

    private void Update()
    {
        onProcessClick();
    }
    public void setupProcess(string processName, int size, Vector3 Startposition, int positionIndex, int arrivalTime)
    {
        processTurn = false;
        this.arrivalTime = arrivalTime;
        this.processName = processName;
        this.positionIndex = positionIndex;
        this.size = size;


        mySlider = Instantiate(slider, Startposition, Quaternion.identity);
        mySlider.transform.SetParent(canvas.transform, false);
        RectTransform HPSliderRect = mySlider.GetComponent<RectTransform>();
        HPSliderRect.sizeDelta = new Vector2(210, size);
        mySlider.GetComponent<ButtonController>().setSize(size);

    }
    public void removeSignal()
    {
        signal = true;
    }
    public void destroyProcess()
    {
        Destroy(mySlider);
    }
    public void unSelectProcess()
    {
        mySlider.GetComponent<SliderController>().unSelectProcess();
    }
    public bool processIsSelected()
    {
        return mySlider.GetComponent<SliderController>().getSelected();
    }
    public void onProcessClick()
    {
        if (mySlider.GetComponent<ButtonController>().selected)
        {
            selected=true;
            Debug.Log(processName + " is selected");
        }
        mySlider.GetComponent<ButtonController>().selected = false;
    }



}
