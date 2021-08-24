using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RoundRobin : LevelController
{
    protected string[] rrWrong = new string[4];
    protected int quantum;
    public TMPro.TextMeshProUGUI quantumText;
    public GameObject quantumObject;
    public List<Process> processesTemp;


    private void Start()
    {
        Debug.Log("RoundRobin Awake");
        quantumObject.SetActive(true);
        generateQuantum();
        this.levelNumber = 1;
        this.algorithmName = "Round Robin";
        this.processes = new List<Process>();
        this.processesTemp = new List<Process>();
        this.score = 0;
        choiceGiven = false;
        quantumText.text = quantum.ToString();
        initPositionArray();
        initStartPosition();
        setup();
        timer.setup(true);
        timer.runTime = 2;
        cpuFiller.setup(true);
        cpuFiller.runTime = 0.5f;
        cpuFiller.timer.fillAmount = 0f;
        Time.timeScale = 1;
        initFeedbackArray();
        for (int i = 0; i < 4; i++)
        {
            addProcess(findAvailablePosition());
        }
        initStartPosition();
        processes[0].processTurn = true;
        processes[0].roundRobinCounter = quantum;
    }
    private void FixedUpdate()
    {
        changeTimerSpeed();
        moveJobQueueProcesses();
        processClick();
        timer.runTimer();

        if (choiceGiven && !wrongChoiceGiven)
        {
            runCPU();
        }
        else if (wrongChoiceGiven)
        {
            loseLife();
        }
        if (timer.timerFinished())
        {
            if (!choiceGiven)
            {
                removeFromScore();
                bonusCounter = 1;
            }
            if (Random.Range(0, 5) == 3 || processes.Count == 1)
            {
                generateNextJob();
            }
            choiceGiven = false;
            enableProcesses();
            if (cpuFiller.timer.fillAmount == 1)
            {
                cpuFiller.timer.fillAmount = 0;
            }
        }

        if (choiceGiven)
        {
            disableProcesses();
        }
    }
    public override void processClick()
    {
        int index = processSelected();

        if (index != -1)
        {

            feedbackText.gameObject.SetActive(false);
            var process = processes[index];
            float processSize = process.executionTime;

            //go through the processes
            if(!process.processTurn)
            {
                feedbackText.text = rrWrong[Random.Range(0, rrWrong.Length - 1)];
                feedbackText.gameObject.SetActive(true);
                //Wrong answer
                bonusCounter = 1;
                wrongChoiceGiven = true;
                //reduce from score 
                choiceGiven = true;
                removeFromScore();
                //highlight process and disable for a little
                process.unSelectProcess();
                process.removeSignal();
            }
            else
            {
                lastSelectionIndex = index;
                feedbackText.text = correctChoice[Random.Range(0, correctChoice.Length - 1)];
                feedbackText.gameObject.SetActive(true);
                process.roundRobinCounter --;
                choiceGiven = true;
                wrongChoiceGiven = false;
                process.reduceFromProcess();
                process.unSelectProcess();
                process.removeSignal();
                addOnScore();
                bonusCounter++;
                if (score > 500)
                {
                    winHandler();
                }
                if (process.roundRobinCounter == 0)
                {
                    if (index < processes.Count - 1)
                    {
                        process.processTurn = false;
                        process.roundRobinCounter = quantum;
                        processes[index + 1].processTurn = true;
                        processes[index + 1].roundRobinCounter = quantum;
                    }
                    else
                    {
                        process.processTurn = false;
                        process.roundRobinCounter = quantum;
                        processes[0].processTurn = true;
                        processes[0].roundRobinCounter = quantum;
                    }
                }

                if (process.processIsFinished() && process.signal == true)
                {
                    hideAllLastSelection();
                    lastSelectionIndex = -1;
                    if (index == processes.Count - 1)
                    {
                        processes[0].roundRobinCounter = quantum;
                        processes[0].processTurn = true;

                        //moveToReadyQueue(process);
                        positionArray[process.positionIndex].isAvailable = true;
                        process.destroyProcess();
                        processes.RemoveAt(index);
                        //in case this is the last process
                        if (index == 0)
                        {

                            positionArray[0].isAvailable = true;

                        }
                        else
                        {

                            refreshProcessList(index - 1);
                        }
                    }
                    else
                    {
                        processes[index + 1].roundRobinCounter = quantum;
                        processes[index + 1].processTurn = true;
                        //moveToReadyQueue(process);
                        positionArray[process.positionIndex].isAvailable = true;
                        process.destroyProcess();
                        processes.RemoveAt(index);
                        refreshProcessList(index);
                    }
                }
            }
            if (wrongChoiceGiven)
            {
                mistakeCounter++;
            }

            if(lastSelectionIndex!=-1)
            {
                hideAllLastSelection();
                displayLastSelection();
            }

        }
    }
    public void generateQuantum()
    {
        quantum = Random.Range(1, 3);
    }

    private void initFeedbackArray()
    {
        rrWrong[0] = "Check the quantum";
        rrWrong[1] = "You forgot something";
        rrWrong[2] = "Wrong Process";
        rrWrong[3] = "Use the quantum";
    }

}
