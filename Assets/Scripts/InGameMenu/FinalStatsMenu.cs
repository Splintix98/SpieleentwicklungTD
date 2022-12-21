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


    public void Show(string title) {
        gameStatsMenu.SetActive(true);
        constructionMenu.SetActive(false);
        InGameStatsMenu.SetActive(false);
        GamePausedMenu.SetActive(false);
        Time.timeScale = 0;
        titleTextMesh.text = title;
        killedEnemiesTextMesh.text = "Killed enemies: " + PlayerStats.Instance.KilledEnemies;
        collectedCoinsTextMesh.text = "Collected coins: " + PlayerStats.Instance.AllCollectedCoins;
    }


    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
        gameStatsMenu.SetActive(false);
    }




}
