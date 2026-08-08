using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HealthBar healthBar;
    public CrystalIntegrityBar crystalBar;

    private int currentLevel = 1;
    private int waveNumber = 0;

    void Start()
    {
        healthBar.InitHealthBar();
        crystalBar.EnableBar(false);        currentLevel = 1;
        waveNumber = 0;

        healthBar.SetHealth(15);
    }

    public void OnNextLevel()
    {
        currentLevel = 2;
        waveNumber = 0;

        healthBar.SetHealth(5 + waveNumber);
        crystalBar.EnableBar(true);        crystalBar.SetHealth(100);    }

    public void OnNextWave()
    {
        waveNumber++;
        if (currentLevel == 2)
        {
            healthBar.SetHealth(5 + waveNumber);
        }
    }

    public void OnTakeDamage()
    {
        healthBar.TakeDamage();
    }

    public void OnHealthIncreased()
    {
        healthBar.OnHealthIncreased();
    }

    public void OnCrystalDamage()
    {
        crystalBar.TakeDamage(10);    }

    public void OnCrystalRepair()
    {
        crystalBar.Repair(5);    }
}
