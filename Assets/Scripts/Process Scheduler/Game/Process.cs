using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Process : MonoBehaviour
{
    public int positionIndex;
    public string processName;
    public int priority;
    public float executionTime;
    public GameObject slider;
    public Canvas canvas;
    public GameObject mySlider;
    public int arrivalTime;
    public bool signal;
    public int roundRobinCounter;
    public bool processTurn;
    public List<Color32> priorityColors;
    private void Awake()
    {
       priorityColors = new List<Color32> { new Color32(93, 188, 243, 255), new Color32(76, 156, 203, 255), 
       new Color32(59, 121, 157, 255), new Color32(36, 75, 97, 255), new Color32(20, 40, 51, 255) };
    }
    public void setupProcess(string processName, int priority, float executionTime, Vector3 Startposition, int positionIndex, int arrivalTime, bool showOrder, bool showSize, bool showPriority)
    {
        processTurn = false;
        roundRobinCounter = 0;
        this.arrivalTime = arrivalTime;
        this.processName = processName;
        this.priority = priority;
        this.positionIndex = positionIndex;
        this.executionTime = (float)System.Math.Round(executionTime * 100f) / 100f;
        
        mySlider = Instantiate(slider, Startposition, Quaternion.identity);
        mySlider.transform.SetParent(canvas.transform, false);
        //mySlider.GetComponent<RectTransform>().sizeDelta = new Vector2(executionTime * 100, 100);
        mySlider.GetComponent<SliderController>().setSize(this.executionTime);
        mySlider.GetComponent<SliderController>().setPriority(this.priority);
        mySlider.GetComponent<SliderController>().setOrderNum(this.arrivalTime);
        RectTransform HPSliderRect = mySlider.GetComponent<RectTransform>();
        HPSliderRect.sizeDelta = new Vector2(executionTime * 100, 100);
        if (showOrder)
        {
            //mySlider.GetComponent<SliderController>().showOrder();
        }
        if (showSize)
        {
            //mySlider.GetComponent<SliderController>().showSize();

        }
        if (showPriority)
        {
            //mySlider.GetComponent<SliderController>().showPriority();
            var fill = mySlider.GetComponentsInChildren<UnityEngine.UI.Image>().FirstOrDefault(t => t.name == "Fill");
            fill.color = priorityColors[this.priority];
        }
    }
    public void removeSignal()
    {
        signal=true;
    }
    public bool processIsFinished()
    {
        if (mySlider.GetComponent<Slider>().value < 0.1)
        {
            return true;
        }
        return false;
    }
    /*public void startProcessTimer()
    {
        cpuTimer.runTimer();
    }*/
    public void reduceFromProcess(int quantum =1)
    {
        mySlider.GetComponent<Slider>().value -=(quantum* 0.5f)/ executionTime;
        float dc = mySlider.GetComponent<Slider>().value;
        mySlider.GetComponent<SliderController>().setSize(dc);
        executionTime -= (quantum*0.5f);

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
}
