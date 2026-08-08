using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private AudioClip damageSoundClip;

    public int currentHealth = 3;
    public int maxHealth = 3;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (SoundFXManager.instance != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 1f);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject); 
    }
}
