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

    [SerializeField]
    private AudioSource createTowerAudioSource;
    [SerializeField]
    private AudioSource createTowerDeniedAudioSource;

    private int costs;

    public static bool Clickable { get; set; } = true;

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
        if (!Clickable) return;
        if (ConstructionMenu.AllowNewTowerConstruction && PlayerStats.Instance.Coins >= costs)
        {
            GameObject newTower = Instantiate(tower, new Vector3(0, 0, 0), Quaternion.identity);
            newTower.SetActive(true);
            Draggable draggable = newTower.AddComponent<Draggable>();
            draggable.buildArea = constructionMenu.BuildArea;
            draggable.buildAreaHalf = constructionMenu.BuildAreaHalf;
            draggable.constructionMenu = constructionMenu;
            draggable.createTowerAudioSource = createTowerAudioSource;
            ConstructionMenu.AllowNewTowerConstruction = false;
            TowerController towercontroller = newTower.GetComponent<TowerController>();

            // set projectile prefab based on towertype
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
        else {
            createTowerDeniedAudioSource.Play();
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
