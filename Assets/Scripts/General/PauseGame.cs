using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{

    public GameObject FirstCanvas;
    public GameObject SecondCanvas;
    public GameObject stage;


    public void disableCanvas()
    {
        if (FirstCanvas.gameObject.activeInHierarchy == false)
        {
            FirstCanvas.gameObject.SetActive(true);
            SecondCanvas.gameObject.SetActive(false);
            stage.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            FirstCanvas.gameObject.SetActive(false);
            SecondCanvas.gameObject.SetActive(true);
            stage.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}