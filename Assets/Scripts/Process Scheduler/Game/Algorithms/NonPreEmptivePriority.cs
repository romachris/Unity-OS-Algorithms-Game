using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class NonPreEmptivePriority : LevelController
{
    Process RunningProcess;
    protected string[] npepWrong = new string[9];


    private void Start()
    {
        Debug.Log("Non Preemptive Priority Scheduling");
        this.levelNumber = 1;
        this.algorithmName = "Non Preemptive Priority Scheduling";
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
            int processPriority = process.priority;
            //go through the processes
            for (int i = 0; i < processes.Count; i++)
            {
                //check if a process is smaller and wrong answer actions
                if(RunningProcess==null)
                {
                    if (processPriority > processes[i].priority)
                    {
                        feedbackText.text = npepWrong[Random.Range(0, 3)];
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
                    else if (processPriority == processes[i].priority && process.executionTime > processes[i].executionTime)
                    {
                        feedbackText.text = npepWrong[Random.Range(4, 6)];
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
                lastSelectionIndex = index;
                if (RunningProcess==null || RunningProcess.Equals(process))
                {
                    feedbackText.text = correctChoice[Random.Range(0, correctChoice.Length - 1)];
                    feedbackText.gameObject.SetActive(true);
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
                    feedbackText.text = npepWrong[Random.Range(7, npepWrong.Length-1)];
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
        npepWrong[0] = "Check the priority again";
        npepWrong[1] = "The lighter the colour, the higher the priority";
        npepWrong[2] = "Check again";
        npepWrong[3] = "Check the color of the process again";
        npepWrong[4] = "After the priority check the size";
        npepWrong[5] = "Correct priority but wrong size";
        npepWrong[6] = "Choose the shortest if the priority is the same";
        npepWrong[7] = "You have to finish the process you executed previously";
        npepWrong[8] = "You left a process unfinished";
    }
}
