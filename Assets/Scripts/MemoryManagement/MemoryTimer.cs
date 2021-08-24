using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryTimer : MonoBehaviour
{
    bool startSignal;
    private bool finished;
    public Image timer;
    public float runTime = 100.0f;

    private void Start()
    {
        //timeElapsed = 0f;
    }
    public void setup(bool startSignal)
    {
        this.startSignal = startSignal;
        //this.maxTime = maxTime;
        finished = false;
        timer.fillAmount = 1;
    }
    public void pauseTimer()
    {
        //float temp = cpuTimer.fillAmount;
        startSignal = false;
        //return temp;
    }
    public void resumeTimer()
    {
        //cpuTimer.fillAmount = pauseTimer();
        startSignal = true;

    }
    public void runTimer()
    {
        if (startSignal == true)
        {
            finished = false;
            //Reduce fill amount 
            timer.fillAmount -= 0.5f / runTime * Time.fixedDeltaTime;
            if (timer.fillAmount == 0.0f)
            {
                finished = true;
                timer.fillAmount = 1;
            }
        }
    }
    public void resetTimer()
    {
        timer.fillAmount = 1;
    }
    public bool timerFinished()
    {
        return finished;
    }
}
