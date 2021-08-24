using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextFit : MemoryAlgorithm
{
    protected string[] memoryWrong = new string[6];
    private bool choiceGiven;
    private int lastChoiceIndex;

    private void Start()
    {
        choiceGiven = false;
        lastChoiceIndex = -1;
        Debug.Log("Next Fit");
        this.algorithmName = "Next Fit";
        this.memoryProcesses = new List<MemoryProcess>();
        this.onMemoryProcesses = new List<MemoryProcess>();
        this.score = 0;
        initPositionArray();
        initStartPosition();
        setup();
        timer.setup(true);
        timer.runTime = 2;
        Time.timeScale = 1;
        initFeedbackArray();
        initRemainingSpaceText();

        for (int i = 0; i < 1; i++)
        {
            addProcess(findAvailablePosition());

        }
        initStartPosition();
    }
    private void FixedUpdate()
    {
        moveJobQueueProcesses();
        selectProcess();
        selectPartition();
        if (memoryProcesses.Count == 0 || Random.Range(0, 4) == 2)
        {
            generateNextJob();
        }
        timer.runTimer();
        if (timer.timerFinished() && onMemoryProcesses.Count >= 1)
        {
            if(onMemoryProcesses.Count>=6)
            {
                removeProcessesFromMemory(Random.Range(0, onMemoryProcesses.Count));
                removeProcessesFromMemory(Random.Range(0, onMemoryProcesses.Count));
            }
            else
            {
                removeProcessesFromMemory(Random.Range(0, onMemoryProcesses.Count));
            }
        }
        if (wrongChoiceGiven == true)
        {
            loseLife();
        }
        changeTimerSpeed();
    }
    public void selectProcess()
    {
        for (int i = 0; i < memoryProcesses.Count; i++)
        {
            if (memoryProcesses[i].selected)
            {
                if (selectedProcess == null)
                {
                    selectedProcess = memoryProcesses[i];
                    clearPartitionSelections();
                    return;
                }
                else
                {
                    selectedProcess.selected = false;
                    selectedProcess = null;
                    selectedProcess = memoryProcesses[i];
                    clearPartitionSelections();
                    return;
                }
            }
        }
    }
    public void selectPartition()
    {
        if (selectedProcess != null && selectedPartition == null)
        {

            for (int i = 0; i < memoryPartitions.Length; i++)
            {
                if (memoryPartitions[i].selected && memoryPartitions[i].isAvailable)
                {
                    selectedPartition = memoryPartitions[i];
                    feedbackText.gameObject.SetActive(false);

                    if (checkAnswer() && checkProcess())
                    {

                        wrongChoiceGiven = false;
                        bonusCounter++;
                        Debug.Log("Correct Answer");
                        //add score
                        addOnScore();
                        //make partition unavailable
                        selectedPartition.isAvailable = false;
                        selectedPartition.GetComponent<Button>().enabled = false;
                        selectedProcess.enabled = false;
                        moveProcessToPartition();
                        if (score > 500)
                        {
                            winHandler();
                        }
                    }
                    else
                    {

                        bonusCounter = 1;
                        mistakeCounter++;
                        wrongChoiceGiven = true;
                        Debug.Log("Wrong Answer");
                        //remove from score
                        removeFromScore();
                        //unselect partition
                        selectedPartition.selected = false;
                        selectedPartition = null;
                    }
                }
            }
        }
    }
    public void clearPartitionSelections()
    {
        for (int i = 0; i < memoryPartitions.Length; i++)
        {
            memoryPartitions[i].selected = false;
        }
    }
    public bool checkAnswer()
    {
        if (selectedPartition.size >= selectedProcess.size)
        {
            Debug.Log("First Criteria Met");

            if(choiceGiven== true)
            {
                if(0==lastChoiceIndex)
                {
                    Debug.Log("Last Choice Index : "+lastChoiceIndex);
                    for (int i = memoryPartitions.Length-1; i >= 0; i--)
                    {
                        if (memoryPartitions[i].isAvailable && memoryPartitions[i].size >= selectedProcess.size)
                        {
                            if (selectedPartition == memoryPartitions[i])
                            {
                                Debug.Log("Second Criteria Met");
                                feedbackText.text = correctChoice[Random.Range(0, correctChoice.Length - 1)];
                                feedbackText.gameObject.SetActive(true);
                                choiceGiven = true;
                                lastChoiceIndex = i;
                                selectedPartition.GetComponent<ButtonController>().setRemainingSize(selectedPartition.size - selectedProcess.size);
                                removeAllSelectedImages();
                                selectedPartition.GetComponent<ButtonController>().lastSelected(true);
                                return true;
                            }
                            else
                            {
                                feedbackText.text = memoryWrong[Random.Range(3, 5)];
                                feedbackText.gameObject.SetActive(true);
                                return false;
                            }
                        }
                    }

                }
                else 
                {
                    Debug.Log("Last Choice Index : " + lastChoiceIndex);
                    for (int i = lastChoiceIndex-1; i >= 0; i--)
                    {
                        if (memoryPartitions[i].isAvailable && memoryPartitions[i].size >= selectedProcess.size)
                        {
                            if (selectedPartition == memoryPartitions[i])
                            {
                                Debug.Log("Second Criteria Met");
                                feedbackText.text = correctChoice[Random.Range(0, correctChoice.Length - 1)];
                                feedbackText.gameObject.SetActive(true);
                                choiceGiven = true;
                                lastChoiceIndex = i;
                                selectedPartition.GetComponent<ButtonController>().setRemainingSize(selectedPartition.size-selectedProcess.size);
                                removeAllSelectedImages();
                                selectedPartition.GetComponent<ButtonController>().lastSelected(true);
                                return true;
                            }
                            else
                            {
                                feedbackText.text = memoryWrong[Random.Range(3, 5)];
                                feedbackText.gameObject.SetActive(true);
                                return false;
                            }
                        }
                    }
                    for (int i = memoryPartitions.Length - 1; i >= 0; i--)
                    {
                        if (memoryPartitions[i].isAvailable && memoryPartitions[i].size >= selectedProcess.size)
                        {
                            if (selectedPartition == memoryPartitions[i])
                            {
                                Debug.Log("Second Criteria Met");
                                feedbackText.text = correctChoice[Random.Range(0, correctChoice.Length - 1)];
                                feedbackText.gameObject.SetActive(true);
                                choiceGiven = true;
                                lastChoiceIndex = i;
                                removeAllSelectedImages();
                                selectedPartition.GetComponent<ButtonController>().setRemainingSize(selectedPartition.size - selectedProcess.size);
                                selectedPartition.GetComponent<ButtonController>().lastSelected(true);
                                return true;
                            }
                            else
                            {
                                feedbackText.text = memoryWrong[Random.Range(3, 5)];
                                feedbackText.gameObject.SetActive(true);
                                return false;
                            }
                        }
                    }

                }
               


            }
            else
            {
                for (int i = memoryPartitions.Length - 1; i >= 0; i--)
                {
                    if (memoryPartitions[i].isAvailable && memoryPartitions[i].size >= selectedProcess.size)
                    {
                        if (selectedPartition == memoryPartitions[i])
                        {
                            Debug.Log("Second Criteria Met");
                            feedbackText.text = correctChoice[Random.Range(0, correctChoice.Length - 1)];
                            feedbackText.gameObject.SetActive(true);
                            choiceGiven = true;
                            lastChoiceIndex = i;
                            selectedPartition.GetComponent<ButtonController>().setRemainingSize(selectedPartition.size - selectedProcess.size);
                            removeAllSelectedImages();
                            selectedPartition.GetComponent<ButtonController>().lastSelected(true);
                            return true;
                        }
                        else
                        {
                            feedbackText.text = memoryWrong[Random.Range(3, 5)];
                            feedbackText.gameObject.SetActive(true);
                            return false;
                        }
                    }
                }
            }
           
        }
        feedbackText.text = memoryWrong[Random.Range(0, 2)];
        feedbackText.gameObject.SetActive(true);
        return false;
    }
    public bool checkProcess()
    {
        if (selectedProcess.positionIndex == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void removeProcessesFromMemory(int index)
    {
        int partitionIndex = onMemoryProcesses[index].partitionIndex;
        memoryPartitions[partitionIndex].isAvailable = true;
        memoryPartitions[partitionIndex].GetComponent<Button>().enabled = true;
        onMemoryProcesses[index].destroyProcess();
        onMemoryProcesses.RemoveAt(index);
        //make partitions available
        //enable click on partitions
        //move processes away and destroy them
    }
    private void initFeedbackArray()
    {
        memoryWrong[0] = "Check the size again";
        memoryWrong[1] = "Too big for this partition";
        memoryWrong[2] = "Doesn't fit";
        memoryWrong[3] = "This is not the next available";
        memoryWrong[4] = "Find the next available after your last choice";
        memoryWrong[5] = "Check again the last selected partition";
    }
}
