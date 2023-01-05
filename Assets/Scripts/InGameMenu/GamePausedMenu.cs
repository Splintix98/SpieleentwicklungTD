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
            Enemy.Clickable = false;
            TowerController.Clickable = false;
            ConstructButton.Clickable = false;
            Draggable.Enable = false;
        }
    }



    public void LeaveLevel() {
        AudioSource.PlayClipAtPoint(buttonClickAudioSource, Camera.main.transform.position);
        SceneManager.LoadScene("MenuScene");
        pauseMenu.SetActive(false);
        TowerController.Clickable = true;
        ConstructButton.Clickable = true;
        Enemy.Clickable = true;
        Draggable.Enable = true;
    }

    public void RestartLevel()
    {
        AudioSource.PlayClipAtPoint(buttonClickAudioSource, Camera.main.transform.position);
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        TowerController.Clickable = true;
        ConstructButton.Clickable = true;
        Enemy.Clickable = true;
        Draggable.Enable = true;
    }

    public void ContinueLevel()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Enemy.Clickable = true;
        TowerController.Clickable = ConstructionMenu.AllowNewTowerConstruction;
        ConstructButton.Clickable = true;
        Draggable.Enable = true;
    }



}
