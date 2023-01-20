using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyMenu : MonoBehaviour
{


    #region Sigleton
    private static EnemyMenu instance;
    public static EnemyMenu Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<EnemyMenu>();
            return instance;
        }
    }
    #endregion

    public GameObject objectPointer;
    public GameObject constructionMenu;
    private GameObject selectedEnemy;
    public GameObject SelectedEnemy { get { return selectedEnemy; } }
    //private GameObject healthBarSelectedEnemy;
    public GameObject enemyMenu;


    [SerializeField] public TextMeshProUGUI enemyNameText;
    [SerializeField] public TextMeshProUGUI enemyHealthText;
    [SerializeField] public TextMeshProUGUI enemySpeedText;
    [SerializeField] public TextMeshProUGUI enemyResistanceText;
    [SerializeField] public TextMeshProUGUI enemyVulnerableText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (selectedEnemy == null) {
            return;
        }
        objectPointer.transform.SetPositionAndRotation(new Vector3(selectedEnemy.transform.position.x, selectedEnemy.transform.position.y + 1.2f, selectedEnemy.transform.position.z), Camera.main.transform.rotation);
        objectPointer.transform.Rotate(0, 0, 180f);

        Enemy enemy = selectedEnemy.GetComponent<Enemy>();

        string name = selectedEnemy.name;
        name = name.Replace("(Clone)", "");
        enemyNameText.text = "Name: " + name;
        enemyHealthText.text = "Health: " + Math.Round(enemy.getCurrentHealth(), 2) + " / " + enemy.getStartHealth();
        enemySpeedText.text = "Speed: " + Math.Round(enemy.Agent.speed, 2);
        enemyResistanceText.text = "Resistant to: " + enemy.immunityElement;
        enemyVulnerableText.text = "Vulnerable to: " + enemy.dangerousElement;


        if (Input.GetMouseButtonDown(1) && selectedEnemy)
        {
            CloseMenu();
            return;
        }
        if (Input.GetMouseButtonDown(0) && selectedEnemy && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject != selectedEnemy)
                {
                    Debug.Log(hit.collider.gameObject.name);
                    CloseMenu();
                }
            }
            else
            {
                CloseMenu();
            }
        }
    }



    public void ShowEnemyInformation(GameObject enemyGameObject, GameObject healthBar)
    {
        selectedEnemy = enemyGameObject;
        //healthBarSelectedEnemy = healthBar;
        enemyMenu.SetActive(true);
        objectPointer.SetActive(true);
        //healthBarSelectedEnemy.SetActive(true);

    }




    public void CloseMenu()
    {
        objectPointer.SetActive(false);
        enemyMenu.SetActive(false);
        constructionMenu.SetActive(true);
        selectedEnemy = null;
    }


}
