using TMPro;
using UnityEngine;

public class TopStatsBar : MonoBehaviour
{

    [SerializeField] GameObject inGameStatsMenu;
    [SerializeField] TextMeshProUGUI textMeshProMoney;
    [SerializeField] TextMeshProUGUI textMeshProKilledEnemies;
    [SerializeField] TextMeshProUGUI textMeshProWaveCounter;



    void Start()
    {
        PlayerStats.Instance.updateGUICallback += UpdateGUI;
        textMeshProMoney.text = PlayerStats.Instance.Coins.ToString(); // + "  <sprite=1>";
        textMeshProKilledEnemies.text = PlayerStats.Instance.KilledEnemies.ToString();

        EnemySpawner.Instance.updateGUICallback += UpdateGUI;
        textMeshProWaveCounter.text = EnemySpawner.Instance.CurrentWave.ToString() + "/" + EnemySpawner.Instance.MaxWaves.ToString();
    }

    void Update()
    {
        
    }

    public void UpdateGUI() {
        textMeshProMoney.text = PlayerStats.Instance.Coins.ToString(); // + "  <sprite=1>";
        textMeshProKilledEnemies.text = PlayerStats.Instance.KilledEnemies.ToString();
        textMeshProWaveCounter.text = EnemySpawner.Instance.CurrentWave.ToString() + "/" + EnemySpawner.Instance.MaxWaves.ToString();
    }


}
