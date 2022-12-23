using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePausedMenu : MonoBehaviour
{


    public GameObject pauseMenu;
    [SerializeField] AudioClip buttonClickAudioSource;



    void Start()
    {
        pauseMenu.SetActive(false);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }



    public void LeaveLevel() {
        AudioSource.PlayClipAtPoint(buttonClickAudioSource, Camera.main.transform.position);
        SceneManager.LoadScene("MenuScene");
        pauseMenu.SetActive(false);
    }

    public void RestartLevel()
    {
        AudioSource.PlayClipAtPoint(buttonClickAudioSource, Camera.main.transform.position);
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void ContinueLevel()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }



}
