using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalStatsMenu : MonoBehaviour
{
    #region Sigleton
    private static FinalStatsMenu instance;
    public static FinalStatsMenu Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<FinalStatsMenu>();
            return instance;
        }
    }
    #endregion

    [SerializeField]
    private GameObject gameStatsMenu;
    [SerializeField]
    private GameObject constructionMenu;
    [SerializeField]
    private GameObject InGameStatsMenu;
    [SerializeField]
    private GameObject GamePausedMenu;
    [SerializeField]
    private GameObject towerMenu;
    [SerializeField]
    private TextMeshProUGUI titleTextMesh;
    [SerializeField]
    private TextMeshProUGUI killedEnemiesTextMesh;
    [SerializeField]
    private TextMeshProUGUI collectedCoinsTextMesh;
    public GameObject pauseMenu;
    [SerializeField] AudioClip buttonClickAudioSource;
    [SerializeField] AudioSource playerHasLostAudioSource;

    public static void Show(bool won) { 
        Instance.show(won);
    }
    private void show(bool won) {
        gameStatsMenu.SetActive(true);
        constructionMenu.SetActive(false);
        InGameStatsMenu.SetActive(false);
        GamePausedMenu.SetActive(false);
        Enemy.Clickable = false;
        TowerController.Clickable = false;
        ConstructButton.Clickable = false;
        Draggable.Enable = false;
        ConstructionMenu.AllowNewTowerConstruction = true;
        Time.timeScale = 0;
        titleTextMesh.text = won ? "Victory" : "Game over";
        if (!won) {
            playerHasLostAudioSource.Play();
        }
        killedEnemiesTextMesh.text = "Killed enemies: " + PlayerStats.Instance.KilledEnemies;
        collectedCoinsTextMesh.text = "Collected coins: " + PlayerStats.Instance.AllCollectedCoins;
    }


    public void GotoMainMenu()
    {
        AudioSource.PlayClipAtPoint(buttonClickAudioSource, Camera.main.transform.position);
        SceneManager.LoadScene("MenuScene");
        gameStatsMenu.SetActive(false);
        TowerController.Clickable = true;
        ConstructButton.Clickable = true;
        Enemy.Clickable = true;
        Draggable.Enable = true;
        ConstructionMenu.AllowNewTowerConstruction = true;
    }




}
