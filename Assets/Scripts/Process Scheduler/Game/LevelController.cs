using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
public class LevelController : MonoBehaviour
{

    public string algorithmName;
    protected List<Process> processes;
    public int bonusCounter;
    //protected Process runningProcess;
    public int levelNumber;
    public int score;
    public float timeLeft;
    public Image life1;
    public Image life2;
    public Image life3;
    public int mistakeCounter;
    public Timer cpuFiller;
    public Timer timer;
    public Process pr;
    public int counter = 0;
    public Canvas canvas;
    protected ProcessPosition[] positionArray;
    protected ProcessPosition[] startPosArray;
    protected bool choiceGiven,wrongChoiceGiven;
    protected int orderNum;
    //texts
    public TMPro.TextMeshProUGUI algorithmText;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI feedbackText;
    public TMPro.TextMeshProUGUI pointsText;
    protected string[] correctChoice = new string[4];
    protected int lastSelectionIndex;



    public void setup()
    {
        lastSelectionIndex = -1;
        initCorrectChoiceArray();
        score = 0;
        bonusCounter = 1;
        wrongChoiceGiven =  false;
        //initialize text
        algorithmText.text = algorithmName;
        scoreText.text = score.ToString();
    }
    private void initCorrectChoiceArray()
    {
        correctChoice[0] = "Good Job";
        correctChoice[1] = "Excelent";
        correctChoice[2] = "Well Done";
        correctChoice[3] = "Nice One";
    }
    public virtual void processClick()
    {

    }
    public float generateSize()
    {
        List<float> sizes = new List<float>
        {
            0.25f,0.5f,1f,1.25f,1.5f,2f,2.25f,2.5f
        };

        return sizes[Random.Range(0,7)];
    }
    public int generatePriority()
    {
        return Random.Range(0, 5);
    }
    public void winHandler()
    {
        SceneManager.LoadScene("WinGameScene");
    }
    public void loseHandler()
    {
        SceneManager.LoadScene("LoseGameScene");
    }
    public void initPositionArray()
    {
        positionArray = new ProcessPosition[7];

        float xpos = 550;
        float ypos = -155;
        float zpos = 0f;
        for (int i = 0; i < positionArray.Length; i++)
        {
            positionArray[i] = new ProcessPosition(new Vector3(xpos, ypos, zpos), true);

            ypos += 70f;
        }
    }
    public void addOnScore()
    {
        if(bonusCounter>=5)
        {
            score += 3 * 5;
            //int sum = 3 * 5;
            //pointsText.text = "+ "+sum.ToString();
            //pointsText.gameObject.SetActive(true);
            scoreText.text = score.ToString();
        }
        else
        {
            score += 3 * bonusCounter;
            //int sum = 3 * bonusCounter;
            //pointsText.text = "+ " + sum.ToString();
            //pointsText.gameObject.SetActive(true);
            scoreText.text = score.ToString();
        }    
    }
    public void removeFromScore()
    {
        
        if (score>1)
        {
            score -= 2;
            
            scoreText.text = score.ToString();
        }
        else if(score ==1)
        {
            score -= 1;
            scoreText.text = score.ToString();
        }
        else
        {
            score = 0;
        }
        //pointsText.text = "- 2";
        //pointsText.gameObject.SetActive(true);

    }
    public void initStartPosition()
    {
        startPosArray = new ProcessPosition[4];
        startPosArray[0] = new ProcessPosition(positionArray[6].position, true);
        startPosArray[1] = new ProcessPosition(positionArray[5].position, true);
        startPosArray[2] = new ProcessPosition(positionArray[4].position, true);
        startPosArray[3] = new ProcessPosition(positionArray[3].position, true);
    }
    public void refreshProcessList(int index)
    {

        var process = processes[index];

        positionArray[processes[index].positionIndex].isAvailable = true;

        for (int i = 0; i < processes.Count; i++)
        {
            positionArray[processes[i].positionIndex].isAvailable = true;
            processes[i].positionIndex = findAvailablePosition();
            positionArray[processes[i].positionIndex].isAvailable = false;
        }
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
    public void moveJobQueueProcesses()
    {
        float speed = 10f;
        for (int i = 0; i < processes.Count; i++)
        {
            var repos = processes[i].mySlider.GetComponent<RectTransform>().transform.position;
            RectTransform rt = processes[i].mySlider.GetComponent<RectTransform>();
            if (rt.localPosition.y > positionArray[processes[i].positionIndex].position.y)
            {
                rt.localPosition += Vector3.down * speed;
            }
        }
        initStartPosition();
    }
    public void disableProcesses()
    {
        Color32 col;
        if (wrongChoiceGiven)
        {
            col = new Color32(154, 28, 24, 255);
        }
        else
        {
            col = new Color32(35, 159, 17, 255);
        }

        for (int i = 0; i < processes.Count; i++)
        {
            var fill = processes[i].mySlider.GetComponentsInChildren<UnityEngine.UI.Image>().FirstOrDefault(t => t.name == "Fill");
            fill.color = col;
            processes[i].mySlider.GetComponent<SliderController>().allowClick = false;
        }
    }
    public void enableProcesses()
    {
        Color32 orangeColor = new Color32(159, 60, 17, 255);

        for (int i = 0; i < processes.Count; i++)
        {
            var fill = processes[i].mySlider.GetComponentsInChildren<UnityEngine.UI.Image>().FirstOrDefault(t => t.name == "Fill");
            fill.color = processes[i].priorityColors[processes[i].priority];
            processes[i].mySlider.GetComponent<SliderController>().allowClick = true;

        }
    }
    public int processSelected()
    {
        for (int i = 0; i < processes.Count; i++)
        {
            if (processes[i].processIsSelected())
            {
                return i;
            }
        }
        return -1;
    }
    public void changeTimerSpeed()
    {
        if (score >= 10 && score <= 59)
        {
            timer.runTime = 1;
        }
        else if (score >= 60 && score <= 299)
        {
            timer.runTime = 0.75f;
        }
        else if (score >= 300)
        {
            timer.runTime = 0.5f;

        }
    }
    public void runCPU()
    {
        if (timer.runTime >= 1)
        {
            cpuFiller.timer.fillAmount += 5 * Time.fixedDeltaTime;
        }
        else if (timer.runTime == 0.5f)
        {
            cpuFiller.timer.fillAmount += 10 * Time.fixedDeltaTime;
        }
        else
        {
            cpuFiller.timer.fillAmount += 15 * Time.fixedDeltaTime;
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
    public void addProcess(int positionIndex)
    {
        orderNum++;
        positionArray[positionIndex].isAvailable = false;
        startPosArray[0].isAvailable = false;

        Process process = (Instantiate(pr) as Process);
        process.setupProcess("p" + counter.ToString(), generatePriority(), generateSize(), startPosArray[0].position, positionIndex, orderNum, true, true, true);
        processes.Add(process);

        counter++;
    }
    public void generateNextJob()
    {
        int total = processes.Count;
        if (total != 7)
        {
            if (total == 1 || total == 0)
            {
                addProcess(findAvailablePosition());
                addProcess(findAvailablePosition());
                addProcess(findAvailablePosition());
            }
            else if (total == 2)
            {
                addProcess(findAvailablePosition());
                addProcess(findAvailablePosition());
            }
            else
            {
                addProcess(findAvailablePosition());
            }
        }
        else
        {
            //loseHandler();
        }
    }
    public void displayLastSelection()
    {
        processes[lastSelectionIndex].mySlider.GetComponent<SliderController>().displayLastSelected();
    }
    public void hideLastSelection()
    {
        processes[lastSelectionIndex].mySlider.GetComponent<SliderController>().hideLastSelected();
    }
    public void hideAllLastSelection()
    {
        for(int i=0;i<processes.Count;i++)
        {
            processes[i].mySlider.GetComponent<SliderController>().hideLastSelected();
        }
    }


    /* public bool escPressed()
     {
         if (Input.GetButtonUp("Escape"))
         {
             return true;
         }
         return false;
     }*/

}
