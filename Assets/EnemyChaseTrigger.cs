using UnityEngine;

public class EnemyChaseTrigger : MonoBehaviour
{
    private Enemy_Movement enemyMovement;
    public MusicManager musicManager;

    
    public bool isBossEnemy = false; 

    private void Start()
    {
        enemyMovement = GetComponentInParent<Enemy_Movement>();

        if (musicManager == null)
        {
            musicManager = FindObjectOfType<MusicManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyMovement.StartChase(collision.transform);

            if (isBossEnemy) 
            {
                musicManager.PlayBossMusic(); 
            }
            else
            {
                musicManager.PlayFightMusic(); 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyMovement.StopChase();
            musicManager.PlayNormalMusic(); 
        }
    }
}
