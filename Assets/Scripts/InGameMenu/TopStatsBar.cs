using TMPro;
using UnityEngine;

public class TopStatsBar : MonoBehaviour
{

    [SerializeField] GameObject inGameStatsMenu;
    [SerializeField] TextMeshProUGUI textMeshProMoney;
    [SerializeField] TextMeshProUGUI textMeshProKilledEnemies;

    

    void Start()
    {
        PlayerStats.Instance.updateGUICallback += UpdateGUI;
        textMeshProMoney.text = PlayerStats.Instance.Coins.ToString() + "  <sprite=1>";
        textMeshProKilledEnemies.text = PlayerStats.Instance.KilledEnemies.ToString();
    }

    void Update()
    {
        
    }

    public void UpdateGUI() {
        textMeshProMoney.text = PlayerStats.Instance.Coins.ToString()+ "  <sprite=1>";
        textMeshProKilledEnemies.text = PlayerStats.Instance.KilledEnemies.ToString();
    }


}
