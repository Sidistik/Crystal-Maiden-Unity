using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    private Vector2 attackDirection;
    private PlayerMovement playerMovement;
    private bool isAttacking;
    private float lastAttackTime;
    public float attackCooldown = 0.5f;
    public int attackDamage = 10;
    public float attackRange = 1.5f; 
    public float knockbackForce = 20f; 
    public Transform attackPoint; 

    public LayerMask enemyLayer; 

    private Collider2D attackCollider; 

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        isAttacking = false;
        lastAttackTime = 0f;

        
        attackCollider = attackPoint.GetComponent<Collider2D>();
        if (attackCollider == null)
        {
           
            attackCollider = attackPoint.gameObject.AddComponent<CircleCollider2D>();
            attackCollider.isTrigger = true; 
        }
        attackCollider.enabled = false; 
    }

    void Update()
    {
        if (isAttacking || Time.time < lastAttackTime + attackCooldown) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartAttack();
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        animator.SetBool("isAttacking", true);

        attackDirection = playerMovement.LastMoveDir;

      
        if (attackDirection == Vector2.zero)
            attackDirection = Vector2.down;

      
        animator.SetFloat("AttackX", attackDirection.x);
        animator.SetFloat("AttackY", attackDirection.y);

     
        attackCollider.enabled = true;

        
        attackCollider.transform.position = attackPoint.position;

        StartCoroutine(PerformAttackDamage());
    }

    IEnumerator PerformAttackDamage()
    {
        yield return new WaitForSeconds(0.1f); 

        
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (var enemy in enemiesHit)
        {
            if (enemy.CompareTag("Enemy"))
            {
               
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.ChangeHealth(-attackDamage);
                }

                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                    enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }

        attackCollider.enabled = false;
    }

    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }
}
