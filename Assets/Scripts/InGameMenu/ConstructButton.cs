using TMPro;
using UnityEngine;

public class ConstructButton : MonoBehaviour
{
    [SerializeField]
    private ConstructionMenu constructionMenu;
    [SerializeField]
    private TowerMenu towerMenu;
    [SerializeField]
    private TextMeshProUGUI towerConstructionCostText;
    [SerializeField]
    private GameObject projectilePreset;
    [SerializeField]
    private GameObject tower;

    private int costs;

    // Start is called before the first frame update
    void Start()
    {
        TowerController towercontroller = tower.GetComponent<TowerController>();
        towerConstructionCostText.text = towercontroller.ConstructionCosts.ToString() + "  <sprite=1>";
        costs = towercontroller.ConstructionCosts;
        PlayerStats.Instance.updateGUICallback += UpdateGUI;
        UpdateGUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTower()
    {
        if (constructionMenu.AllowNewTowerConstruction && PlayerStats.Instance.Coins >= costs)
        {
            GameObject newTower = Instantiate(tower, new Vector3(0, 0, 0), Quaternion.identity);
            newTower.SetActive(true);
            Draggable draggable = newTower.AddComponent<Draggable>();
            draggable.buildArea = constructionMenu.BuildArea;
            draggable.buildAreaHalf = constructionMenu.BuildAreaHalf;
            draggable.constructionMenu = constructionMenu;
            constructionMenu.AllowNewTowerConstruction = false;
            TowerController towercontroller = newTower.GetComponent<TowerController>();

            if (newTower.name == "Tower_Fire(Clone)")
            {
                towercontroller.setProjectilePreset(Resources.Load("Prefabs/fireProjectile/fireProjectile") as GameObject);
            }
            if (newTower.name == "Tower_Earth(Clone)")
            {
                towercontroller.setProjectilePreset(Resources.Load("Prefabs/stoneProjectile/stoneProjectile") as GameObject);
            }
            if (newTower.name == "Tower_Air(Clone)")
            {
                towercontroller.setProjectilePreset(Resources.Load("Prefabs/airProjectile/airProjectile") as GameObject);
            }
            if (newTower.name == "Tower_Water(Clone)")
            {
                towercontroller.setProjectilePreset(Resources.Load("Prefabs/waterProjectile/waterProjectile") as GameObject);
            }

        }
    }




    public void UpdateGUI()
    {
        if (costs > PlayerStats.Instance.Coins)
        {
            towerConstructionCostText.color = Color.red;
        }
        else {
            towerConstructionCostText.color = Color.white;
        }
    }


}
