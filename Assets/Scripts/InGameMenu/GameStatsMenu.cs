using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStatsMenu: MonoBehaviour
{

    public GameObject gameStatsMenu;
    public TextMeshProUGUI killedEnemiesText;

    // Start is called before the first frame update
    void Start()
    {
        //show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void show() {
        gameStatsMenu.SetActive(true);
        killedEnemiesText.text = "Killed enemies: " + 100;
    }


    public void gotoMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
        gameStatsMenu.SetActive(false);
    }




}
