using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePausedMenu : MonoBehaviour
{
   
    public GameObject pauseMenu;


    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }



    public void leaveLevel() {
        SceneManager.LoadScene("MenuScene");
        pauseMenu.SetActive(false);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void continueLevel()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }



}
