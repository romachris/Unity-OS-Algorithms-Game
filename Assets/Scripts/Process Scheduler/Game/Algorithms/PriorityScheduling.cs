using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PriorityScheduling : LevelController
{
    protected string[] psWrong = new string[7];


    private void Start()
    {
        Debug.Log("Preemptive Priority Scheduling");
        this.levelNumber = 1;
        this.algorithmName = "Preemptive Priority Scheduling";
        this.processes = new List<Process>();
        this.score = 0;
        choiceGiven = false;
        mistakeCounter = 0;
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
                if (processPriority > processes[i].priority)
                {
                    feedbackText.text = psWrong[Random.Range(0, 3)];
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
                else if(processPriority == processes[i].priority && process.executionTime > processes[i].executionTime)
                {
                    feedbackText.text = psWrong[Random.Range(4, psWrong.Length-1)];
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
            if (correctAnswer)
            {
                lastSelectionIndex = index;
                feedbackText.text = correctChoice[Random.Range(0, correctChoice.Length - 1)];
                feedbackText.gameObject.SetActive(true);
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
                //if process is finished removeit and refresh
                if (process.processIsFinished() && process.signal == true)
                {
                    hideAllLastSelection();
                    lastSelectionIndex = -1;
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

            if(wrongChoiceGiven)
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
        psWrong[0] = "Check the priority again";
        psWrong[1] = "The lighter the colour, the higher the priority";
        psWrong[2] = "Check again";
        psWrong[3] = "Check the color of the process again";
        psWrong[4] = "After the priority check the size";
        psWrong[5] = "Correct priority but wrong size";
        psWrong[6] = "Choose the shortest if the priority is the same";
    }
}
