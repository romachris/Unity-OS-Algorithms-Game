using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneController : MonoBehaviour
{
    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<SettingController>().PlayMusic();
    }
    // Start is called before the first frame update
    public void loadProcessScheduler()
    {
        SceneManager.LoadScene("PS");
    }
    public void loadMemoryManagement()
    {
        SceneManager.LoadScene("MM");
    }
    public void loadMPSMenu2()
    {
        SceneManager.LoadScene("PSMenu");
    }
    public void loadMMMMenu2()
    {
        SceneManager.LoadScene("MMMenu");
    }
    public void loadPSMEnu()
    {
        SceneManager.LoadScene("PSMenu");
    }
    public void loadMMMenu()
    {
        SceneManager.LoadScene("MMMenu");
    }
    public void chooseGame()
    {
        SceneManager.LoadScene("chooseGame");
    }   
    public void loadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
    {
        Debug.Log("Goodbye");
        Application.Quit();
    }
}
