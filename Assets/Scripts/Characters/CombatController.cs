using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatController : MonoBehaviour
{
    public enum Faction { Ally, Enemy }
    public Faction faction;
    public float maxHealth;
    public int attackDamage;
    public float attackRange;
    public float attackWidth;
    public float knockbackForce;
    public float hitDelay; // delay between attack and hit (for syncing animations)
    public float attackCooldown; // cooldown to prevent attack spam

    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public NavMeshAgent navMeshAgent;

    private float currHealth;
    private bool isDead = false;
    private bool attackOnCooldown = false;

    // Start is called before the first frame update
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
        animator.SetTrigger("Attack");

        // Get targets and deal damage
        StartCoroutine(HandleAttacking());

        // Start attack cooldown
        attackOnCooldown = true;
        StartCoroutine(HandleAttackCooldown());
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
    }

    private IEnumerator HandleAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }

    public void TakeDamage(int damage, Vector3 enemyPosition, float knockback)
    {
        if (isDead) return;

        // Play animation
        animator.SetTrigger("Hurt");

        // Take damage
        currHealth -= damage;

        // TODO: Apply knockback

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
    }

    public bool isImmobilized()
    {
        return isDead;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null || attackRange == null)
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
