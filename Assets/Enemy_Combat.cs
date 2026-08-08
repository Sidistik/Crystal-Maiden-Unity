using System.Collections;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public float knockbackForce = 30f;
    public Transform attackPoint; 
    public LayerMask playerLayer;

    private Animator animator;
    private bool canAttack = true;

    
    private float inputX, inputY;

    private void Start()
    {
        animator = GetComponent<Animator>();
       

    }

    private void Update()
    {
        Collider2D playerHit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if (playerHit != null && canAttack)
        {
            StartCoroutine(Attack(playerHit));
        }

        inputX = animator.GetFloat("LastInputX");
        inputY = animator.GetFloat("LastInputY");
    }

    private IEnumerator Attack(Collider2D player)
    {
        GetComponent<AttackSoundPlayer>()?.PlayAttackSound();
        Vector2 direction = (player.transform.position - transform.position).normalized;

        animator.SetFloat("AttackX", direction.x);
        animator.SetFloat("AttackY", direction.y);

        animator.SetFloat("inputX", direction.x);
        animator.SetFloat("inputY", direction.y);

        if (direction.magnitude > 0.01f)
        {
            animator.SetFloat("LastInputX", direction.x);
            animator.SetFloat("LastInputY", direction.y);
        }

      
        canAttack = false;
        animator.SetTrigger("Attack");

        
        yield return new WaitForSeconds(0.2f);

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ChangeHealth(-damage);
        }

       
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 knockbackDir = direction;
            playerRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        }

      
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

   
    public void OnAttackAnimationEnd()
    {
        canAttack = true;
    }
   

}

