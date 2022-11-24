using TMPro;
using UnityEngine;

public class TowerMenu : MonoBehaviour
{
    public GameObject towerPointer;
    public GameObject constructionMenu;
    private GameObject selectedTower;

    public TextMeshProUGUI towerName;
    public TextMeshProUGUI towerHealth;
    public TextMeshProUGUI towerDamage;
    public TextMeshProUGUI towerRange;

    public TextMeshProUGUI tomwerUpgradecosts;
    public TextMeshProUGUI tomwerDestroycosts;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void showTowerInformation(GameObject tower) {
        towerPointer.transform.position = new Vector3(tower.transform.position.x, tower.transform.position.y + 1.5f, tower.transform.position.z);
        towerPointer.transform.rotation = Camera.main.transform.rotation;
        towerPointer.transform.Rotate(0, 0, 180f);
        constructionMenu.SetActive(false);
        gameObject.SetActive(true);
        towerPointer.SetActive(true);
        selectedTower = tower;
        string name = selectedTower.name;
        TowerController towerController = selectedTower.GetComponent<TowerController>();
        name = name.Replace("_", " ");
        name = name.Replace("(", " ");
        name = name.Replace(")", " ");
        name = name.Replace("Clone", " ");

        towerName.text = "Name: " + name;
        towerHealth.text = "Health: " + towerController.towerHealth;
        towerDamage.text = "Damage: " + towerController.towerDamage.ToString();
        towerRange.text = "Range: " + towerController.towerRange;

        //tomwerUpgradecosts.text = "10" + " <sprite=4>";
        //tomwerDestroycosts.text = "0" + " <sprite=4>";
    }


    public void destroyTower()
    {
        towerPointer.SetActive(false);
        gameObject.SetActive(false);
        constructionMenu.SetActive(true);

        Destroy(selectedTower.gameObject);
        Destroy(selectedTower);
        selectedTower = null;
    }


    public void closeMenu() {
        towerPointer.SetActive(false);
        gameObject.SetActive(false);
        constructionMenu.SetActive(true);
        selectedTower = null;
    }


}
