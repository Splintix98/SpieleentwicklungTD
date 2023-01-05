using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    public delegate void UpdateGUIDelegate();
    public UpdateGUIDelegate updateGUICallback;

    #region Sigleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerStats>();
            return instance;
        }
    }
    #endregion

    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxTotalHealth;
    [SerializeField] private float coins;
    [SerializeField] private float killedEnemies;
    [SerializeField] private AudioSource playerDamageAudioSource;
    private float allCollectedCoins;

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }

    public float Coins { get { return coins; } }
    public float KilledEnemies { get { return killedEnemies; } }

    public float AllCollectedCoins { get { return allCollectedCoins; } }




    public void CollectLoot(float loot) { 
        coins += loot;
        allCollectedCoins += loot;
        killedEnemies++;
        updateGUICallback();
    }

    public void SpendCoins(float costs)
    {
        coins -= costs;
        updateGUICallback();
    }

    public void Heal(float health)
    {
        this.health += health;
        ClampHealth();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        playerDamageAudioSource.Play();
        ClampHealth();
        if (health <= 0)
        {
            FinalStatsMenu.Show(false);
        }
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }   
    }

    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }
}
