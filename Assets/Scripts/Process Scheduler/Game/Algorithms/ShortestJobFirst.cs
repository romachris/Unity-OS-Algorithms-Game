using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShortestJobFirst : LevelController
{
    Process RunningProcess;
    protected string[] sjfWrong = new string[9];


    private void Start()
    {
        Debug.Log("Shortest Job First");
        this.levelNumber = 1;
        this.algorithmName = "Shortest Job First";
        this.processes = new List<Process>();
        this.score = 0;
        choiceGiven = false;
        mistakeCounter = 0;
        initPositionArray();
        initStartPosition();
        RunningProcess = null;
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
        bool correctAnswer = true;
        int index = processSelected();

        if (index != -1)
        {
            feedbackText.gameObject.SetActive(false);
            var process = processes[index];
            float processSize = process.executionTime;
            //go through the processes
            for (int i = 0; i < processes.Count; i++)
            {
                //check if a process is smaller and wrong answer actions
                if (RunningProcess == null)
                {
                    if (processSize > processes[i].executionTime)
                    {
                        feedbackText.text = sjfWrong[Random.Range(0, 3)];
                        feedbackText.gameObject.SetActive(true);
                        //Wrong answer
                        bonusCounter = 1;
                        //reduce from score 
                        wrongChoiceGiven = true;
                        choiceGiven = true;
                        removeFromScore();
                        //highlight process and disable for a little
                        correctAnswer = false;
                        process.unSelectProcess();
                        process.removeSignal();
                    }
                    else if (processSize == processes[i].executionTime && process.arrivalTime > processes[i].arrivalTime)
                    {
                        feedbackText.text = sjfWrong[Random.Range(6, sjfWrong.Length - 1)];
                        feedbackText.gameObject.SetActive(true);
                        //Wrong answer
                        bonusCounter = 1;
                        //reduce from score 
                        wrongChoiceGiven = true;
                        choiceGiven = true;
                        removeFromScore();
                        //highlight process and disable for a little
                        correctAnswer = false;
                        process.unSelectProcess();
                        process.removeSignal();
                    }
                }
                else
                {
                    correctAnswer = true;
                }
            }
            if (correctAnswer)
            {
                if (RunningProcess == null || RunningProcess.Equals(process))
                {
                    lastSelectionIndex = index;
                    feedbackText.text = correctChoice[Random.Range(0, correctChoice.Length - 1)];
                    feedbackText.gameObject.SetActive(true);
                    //pointsText.text = 2
                    //pointsText.gameObject.SetActive(true);
                    RunningProcess = process;
                    //Correct answer
                    choiceGiven = true;
                    wrongChoiceGiven = false;
                    process.reduceFromProcess();
                    process.unSelectProcess();
                    process.removeSignal();
                    addOnScore();
                    bonusCounter++;
                    //run the part in the cpu
                    //win game
                    if (score > 500)
                    {
                        winHandler();
                    }
                }
                else
                {
                    feedbackText.text = sjfWrong[Random.Range(4, 5)];
                    feedbackText.gameObject.SetActive(true);
                    //Wrong answer
                    bonusCounter = 1;
                    //reduce from score 
                    wrongChoiceGiven = true;
                    choiceGiven = true;
                    removeFromScore();
                    //highlight process and disable for a little
                    correctAnswer = false;
                    process.unSelectProcess();
                    process.removeSignal();
                }
                //if process is finished removeit and refresh
                if (process.processIsFinished() && process.signal == true)
                {
                    hideAllLastSelection();
                    lastSelectionIndex = -1;
                    RunningProcess = null;
                    if (index == processes.Count - 1)
                    {
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
            if (lastSelectionIndex != -1)
            {
                hideAllLastSelection();
                displayLastSelection();
            }
        }
    }

    private void initFeedbackArray()
    {
        sjfWrong[0] = "Check the size again";
        sjfWrong[1] = "There is a shortest process";
        sjfWrong[2] = "Check again";
        sjfWrong[3] = "Check the width of the processes";
        sjfWrong[4] = "You have to finish the execution of the process";
        sjfWrong[5] = "Forgot to finish something";
        sjfWrong[6] = "First size then order";
        sjfWrong[7] = "Correct size but wrong order";
        sjfWrong[8] = "Correct size but pick the one that arrived first";
    }
}
