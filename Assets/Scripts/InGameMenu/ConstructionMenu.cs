using TMPro;
using UnityEngine;

public class ConstructionMenu : MonoBehaviour
{

    [SerializeField]
    private GameObject constructionMenu;
    [SerializeField]
    private Transform buildArea;
    [SerializeField]
    private Transform buildAreaHalf;


    [SerializeField]
    private GameObject towerAttributesContainer;
    [SerializeField]
    private TextMeshProUGUI towerFireRate;
    [SerializeField]
    private TextMeshProUGUI towerDamage;
    [SerializeField]
    private TextMeshProUGUI towerRange;

    [SerializeField]
    private AudioSource buildTowerAudioSource;

    public static bool AllowNewTowerConstruction { get; set; } = true;
    public Transform BuildArea { get { return buildArea; } }
    public Transform BuildAreaHalf { get { return buildAreaHalf; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ShowTowerAttributes(GameObject tower)
    {
        TowerController towerController = tower.GetComponent<TowerController>();
        towerDamage.text = "Damage: " + towerController.towerDamage;
        towerFireRate.text = "Fire rate: " + towerController.fireRate + " shots/sec"; ;
        towerRange.text = "Range: " + towerController.towerRange + " fields";
        towerAttributesContainer.SetActive(true);
        EnemyMenu.Instance.CloseMenu();
    }

    public void HideTowerAttributes()
    {
        towerAttributesContainer.SetActive(false);
    }




}
