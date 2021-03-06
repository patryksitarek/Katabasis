﻿using System.Collections;
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
    public HealthBar healthBar;

    public int dmgBoostValue = 2;
    public int hpBoostValue = 1000;
    private int coins = 0;
    private bool resistant = false;

    public Animator animator;
    public Transform attackPoint;
    public NavMeshAgent agent;
    public LayerMask enemyLayer;

    public GameFlowController gfc;

    private float currHealth;
    public bool isDead { private set; get; } = false;
    private bool attackOnCooldown = false;
    private SFXController sfxc;
    private LootDropper dropper;
    // Attack animation variation
    //[SerializeField]
    //private int numAttackAnims = 1;

    void Start()
    {
        sfxc = GetComponent<SFXController>();
        dropper = GetComponent<LootDropper>();
        healthBar.SetMaxHealth(maxHealth);
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

        // Play animation and sound
        // animator.SetInteger("useAttackAnim", 0);
        animator.SetTrigger("Attack");
        sfxc.playAttackSound();

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


        if (!resistant)
        {
            // Play animation
            animator.SetTrigger("Hurt");

            // Take damage
            currHealth -= damage;
            healthBar.SetHealth(currHealth);
        }
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
        sfxc.playDeathSound();

        isDead = true;

        if (gameObject.CompareTag("Player")) gfc.OnGameOver();
        else if (gameObject.CompareTag("Boss")) gfc.OnGameOver();
        else dropper.DropLoot();

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

    private void OnTriggerEnter(Collider collision)
    {
        

        if (gameObject.tag == "Player")
        {
            float duration = Random.Range(5.0f, 15.0f);

            switch (collision.gameObject.tag)
            {
                case "damageBoost":
                    StartCoroutine(damageBoost(duration));
                    Destroy(collision.gameObject);
                    break;
                case "healthBoost":
                    currHealth += hpBoostValue;
                    if (currHealth > maxHealth) 
                    {
                        currHealth = maxHealth;
                    }
                    healthBar.SetHealth(currHealth);
                    
                    Destroy(collision.gameObject);
                    break;
                case "coin":
                    coins++;
                    if (coins == 10)
                    {
                        attackDamage = (int) (attackDamage * 1.1f);
                        coins = 10;
                    }
                    Destroy(collision.gameObject);
                    break;
                case "resistBoost":
                    StartCoroutine(resistantBoost(duration));
                    Destroy(collision.gameObject);
                    break;
            }
            
            
            //StartCoroutine(damageBoost(15));
        }
    }

    private IEnumerator damageBoost(float time)
    {
        attackDamage *= dmgBoostValue;
        yield return new WaitForSeconds(time);
        attackDamage /= dmgBoostValue;
    }

    private IEnumerator resistantBoost(float time)
    {
        resistant = true;
        yield return new WaitForSeconds(time);
        resistant = false;
    }
}
