using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MemoryAlgorithm : MonoBehaviour
{
    public  MemoryPartition[] memoryPartitions = new MemoryPartition[7];
    protected List<MemoryProcess> memoryProcesses;
    protected List<MemoryProcess> onMemoryProcesses;
    public MemoryProcess memoryProcess;
    protected MemoryProcess selectedProcess;
    protected MemoryPartition selectedPartition;
    public Image life1;
    public Image life2;
    public Image life3;
    public int mistakeCounter, orderNum, score, bonusCounter, counter;
    public MemoryTimer timer;
    public Canvas canvas;
    public ProcessPosition[] positionArray;
    public ProcessPosition[] startPosArray;
    protected bool? wrongChoiceGiven;
    protected string algorithmName;
    //texts
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI algorithmText;
    public TMPro.TextMeshProUGUI feedbackText;
    protected string[] correctChoice = new string[4];



    public void setup()
    {
        initCorrectChoiceArray();
        wrongChoiceGiven = null;
        selectedPartition = null;
        selectedProcess = null;
        score = 0;
        bonusCounter = 1;
        //initialize text
        algorithmText.text = algorithmName;
        scoreText.text = score.ToString();
    }
    public int generateSize()
    {
        List<int> sizes = new List<int>
        {
            47,56,100,34,50,65,80
        };

        return sizes[Random.Range(0, sizes.Count-1)];
    }
    public void winHandler()
    {
        SceneManager.LoadScene("WinMM");
    }
    public void loseHandler()
    {
        SceneManager.LoadScene("LoseMM");
    }
    public void addOnScore()
    {
        score += 3 * bonusCounter;
        scoreText.text = score.ToString();
    }
    public void removeFromScore()
    {
        if (score > 1)
        {
            score -= 2;
            scoreText.text = score.ToString();
        }
        else if (score == 1)
        {
            score -= 1;
            scoreText.text = score.ToString();
        }
        else
        {
            score = 0;
        }

    }
    public void initPositionArray()
    {
        positionArray = new ProcessPosition[7];

        float xpos = 170;
        float ypos = -320;
        float zpos = 0f;
        for (int i = 0; i < positionArray.Length; i++)
        {
            positionArray[i] = new ProcessPosition(new Vector3(xpos, ypos, zpos), true);
            ypos += 150f;
        }
    }
    public void initStartPosition()
    {
        startPosArray = new ProcessPosition[4];
        startPosArray[0] = new ProcessPosition(positionArray[6].position, true);
        startPosArray[1] = new ProcessPosition(positionArray[5].position, true);
        startPosArray[2] = new ProcessPosition(positionArray[4].position, true);
        startPosArray[3] = new ProcessPosition(positionArray[3].position, true);
    }
    public int findAvailablePosition()
    {
        for (int i = 0; i < positionArray.Length; i++)
        {
            if (positionArray[i].isAvailable)
            {
                return i;
            }
        }
        return -1;
    }
    public void addProcess(int positionIndex)
    {
        orderNum++;
        positionArray[positionIndex].isAvailable = false;

        MemoryProcess process = (Instantiate(memoryProcess) as MemoryProcess);
        process.setupProcess("p" + counter.ToString(), generateSize(), startPosArray[0].position, positionIndex, orderNum);
        memoryProcesses.Add(process);
        counter++;
        selectedProcess = process;
        selectedProcess.selected = true;
        selectedProcess.enabled = false;
    }
    public void moveJobQueueProcesses()
    {
        float speed = 10f;
        for (int i = 0; i < memoryProcesses.Count; i++)
        {
            var repos = memoryProcesses[i].mySlider.GetComponent<RectTransform>().transform.position;
            RectTransform rt = memoryProcesses[i].mySlider.GetComponent<RectTransform>();
            if (rt.localPosition.y > positionArray[memoryProcesses[i].positionIndex].position.y)
            {
                rt.localPosition += Vector3.down * speed;
            }
        }
        initStartPosition();
    }
    public void refreshProcessList(int index)
    {

        var process = memoryProcesses[index];

        positionArray[memoryProcesses[index].positionIndex].isAvailable = true;

        for (int i = 0; i < memoryProcesses.Count; i++)
        {
            positionArray[memoryProcesses[i].positionIndex].isAvailable = true;
            memoryProcesses[i].positionIndex = findAvailablePosition();
            positionArray[memoryProcesses[i].positionIndex].isAvailable = false;
        }
    }
    public void generateNextJob()
    {

        int total = memoryProcesses.Count;
        if (total == 0 )
        {
            addProcess(findAvailablePosition());
        }
    }
    public void changeTimerSpeed()
    {
        if (onMemoryProcesses.Count >= 5)
        {
            timer.runTime = 2;
        }
        else if (onMemoryProcesses.Count ==7)
        {
            timer.runTime = 1f;
        }
        else
        {
            timer.runTime = 3;

        }

    }
    public void loseLife()
    {

        if (mistakeCounter <= 3)
        {

            if (mistakeCounter == 1)
            {
                life1.fillAmount -= 3 * Time.fixedDeltaTime;
            }
            else if (mistakeCounter == 2)
            {
                life2.fillAmount -= 3 * Time.fixedDeltaTime;
            }
            else if (mistakeCounter == 3)
            {
                life3.fillAmount -= 3 * Time.fixedDeltaTime;
            }
        }
        else if (mistakeCounter >= 4)
        {
            loseHandler();
        }

    }
    public void moveProcessToPartition()
    {
        if (selectedPartition != null && selectedProcess != null)
        {
            RectTransform processRT = selectedProcess.mySlider.GetComponent<RectTransform>();
            RectTransform partitionRT = selectedPartition.GetComponent<RectTransform>();
            processRT.localPosition = new Vector3(partitionRT.localPosition.x -24f, partitionRT.localPosition.y - 6.5f, 0);

            int index = selectedProcess.positionIndex;
            for (int i = 0; i < memoryPartitions.Length; i++)
            {
                if (selectedPartition.Equals(memoryPartitions[i]))
                {
                    selectedProcess.partitionIndex = i;
                }
            }
            //make process position array available and refresh list
            if (index == memoryProcesses.Count - 1)
            {
                positionArray[memoryProcess.positionIndex].isAvailable = true;
                memoryProcesses.RemoveAt(index);
                onMemoryProcesses.Add(selectedProcess);
                //in case this is the last process
                if (index == 0)
                {

                    positionArray[0].isAvailable = true;

                }
                else
                {

                    refreshProcessList(selectedProcess.positionIndex - 1);
                }
            }
            else
            {
                positionArray[memoryProcess.positionIndex].isAvailable = true;
                memoryProcesses.RemoveAt(index);
                onMemoryProcesses.Add(selectedProcess);
                Debug.Log(onMemoryProcesses.Count);
                refreshProcessList(index);
            }
            //remove from process list
            memoryProcesses.Remove(selectedProcess);
            selectedProcess = null;
            selectedPartition = null;
            //start memory timer
            //make both buttons unvailable
        }
    }
    private void initCorrectChoiceArray()
    {
        correctChoice[0] = "Good Job";
        correctChoice[1] = "Excelent";
        correctChoice[2] = "Well Done";
        correctChoice[3] = "Nice One";
    }

    public void initRemainingSpaceText()
    {
        for(int i=0;i<memoryPartitions.Length;i++)
        {
            memoryPartitions[i].GetComponent<ButtonController>().setRemainingSize(memoryPartitions[i].size);
        }
    }

    public void removeAllSelectedImages()
    {
        for(int i=0;i<memoryPartitions.Length;i++)
        {
            memoryPartitions[i].GetComponent<ButtonController>().lastSelected(false);
        }
    }

}
