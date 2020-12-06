using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatController : MonoBehaviour
{
    public enum Faction { Ally, Enemy }
    public Faction faction;
    public int attackDamage;
    public float maxHealth;
    public float attackRange;
    public float attackWidth;
    public float knockbackForce;
    public float hitDelay; // delay between attack and hit (for syncing animations)
    public float attackCooldown; // cooldown to prevent attack spam

    public Animator animator;
    public Transform attackPoint;
    public NavMeshAgent agent;
    public LayerMask enemyLayer;

    private float currHealth;
    public bool isDead { private set; get; } = false;
    private bool attackOnCooldown = false;
    // Attack animation variation
    //[SerializeField]
    //private int numAttackAnims = 1;

    void Start()
    {
        currHealth = maxHealth;
    }

    public Collider[] GetHitObjects()
    {
        Vector3 offset = new Vector3(0, 0, attackWidth);
        return Physics.OverlapCapsule(attackPoint.position - offset, attackPoint.position + offset, attackRange, enemyLayer);
    }

    public void Attack()
    {
        // Do not attack if dead or on cooldown
        if (isDead || attackOnCooldown) return;

        // Play animation
        // animator.SetInteger("useAttackAnim", 0);
        animator.SetTrigger("Attack");

        // Get targets and deal damage
        StartCoroutine(HandleAttacking());

        // Start attack cooldown
        attackOnCooldown = true;
    }

    private IEnumerator HandleAttacking()
    {
        yield return new WaitForSeconds(hitDelay);

        // Detect enemies
        Collider[] hitObjects = GetHitObjects();

        // Deal damage
        foreach (Collider obj in hitObjects)
        {
            if (obj.TryGetComponent<CombatController>(out CombatController target) && target.faction != faction)
            {
                target.TakeDamage(attackDamage, transform.position, knockbackForce);
            }
        }
        StartCoroutine(HandleAttackCooldown());
    }

    private IEnumerator HandleAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }

    public void TakeDamage(int damage, Vector3 enemyPosition, float knockbackForce)
    {
        if (isDead) return;

        // Play animation
        animator.SetTrigger("Hurt");

        // Take damage
        currHealth -= damage;

        // Apply knockback
        Vector3 direction = transform.position - enemyPosition;
        agent.velocity = direction * knockbackForce;

        // Check if dead
        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Death");

        isDead = true;

        StartCoroutine(DeleteObject(3));
    }
    public bool IsImmobilized()
    {
        return isDead;
    }

    private IEnumerator DeleteObject(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject.Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        // No function for wire capsule, so it's represented by a line for width
        // and a sphere for general size
        Vector3 offset = new Vector3(0, 0, attackWidth);

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Debug.DrawLine(attackPoint.position - offset, attackPoint.position + offset);
    }
}
