using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    GameObject mainMenu;
    GameObject levelMenu;
    GameObject optionsMenu;
    GameObject creditsMenu;
    GameObject helpMenu;
    GameObject creditAnimation;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu = GameObject.Find("MainMenu");
        levelMenu = GameObject.Find("LevelMenu");
        optionsMenu = GameObject.Find("OptionsMenu");
        creditsMenu = GameObject.Find("CreditsMenu");
        helpMenu = GameObject.Find("HelpMenu");
        creditAnimation = GameObject.Find("CreditAnimation");
        

        mainMenu.transform.localScale = new Vector3(1, 1, 1);
        levelMenu.transform.localScale = new Vector3(0, 0, 0);
        optionsMenu.transform.localScale = new Vector3(0, 0, 0);
        creditsMenu.transform.localScale = new Vector3(0, 0, 0);
        helpMenu.transform.localScale = new Vector3(0, 0, 0);
        creditAnimation.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ------------------------------------------------------

    public void LoadLevelOverview()
    {
        levelMenu.transform.localScale = new Vector3(1, 1, 1);
        mainMenu.transform.localScale = new Vector3(0, 0, 0);
    }

    public void LoadLevelOne()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevelTwo()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevelThree()
    {
        SceneManager.LoadScene(1);
    }

    // ------------------------------------------------------

    public void LoadOptionsOverview()
    {
        optionsMenu.transform.localScale = new Vector3(1, 1, 1);
        mainMenu.transform.localScale = new Vector3(0, 0, 0);
    }

    public void LoadCreditsOverview()
    {
        creditsMenu.transform.localScale = new Vector3(1, 1, 1);
        mainMenu.transform.localScale = new Vector3(0, 0, 0);
        creditAnimation.GetComponent<Animator>().enabled = true;
    }

    public void LoadHelpOverview()
    {
        helpMenu.transform.localScale = new Vector3(1, 1, 1);
        mainMenu.transform.localScale = new Vector3(0, 0, 0);
    }

    // ------------------------------------------------------

    public void backToMenu()
    {
        SceneManager.LoadScene(0);
        mainMenu.transform.localScale = new Vector3(1, 1, 1);
        levelMenu.transform.localScale = new Vector3(0, 0, 0);
        optionsMenu.transform.localScale = new Vector3(0, 0, 0);
        creditsMenu.transform.localScale = new Vector3(0, 0, 0);
        helpMenu.transform.localScale = new Vector3(0, 0, 0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
