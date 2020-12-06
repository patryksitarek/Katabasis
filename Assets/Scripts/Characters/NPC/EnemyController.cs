using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    public TargettingHandler targettingHandler;
    public NavMeshAgent agent;
    public CombatController combatController;
    public Animator animator;

    private bool facingRight;
    private bool startingOrientation;
    private Vector3 startingPosition;

    void Start()
    {
        agent.updateRotation = false;

        facingRight = transform.localScale.x > 0;
        startingOrientation = facingRight;
        startingPosition = transform.position;
    }

    void Update()
    {
        if (combatController.IsImmobilized()) return;

        if (targettingHandler.target) HandleTarget();
        else HandleNoTarget();

        HandleFlipping();
        animator.SetBool("isRunning", agent.velocity != Vector3.zero);
    }

    void HandleNoTarget()
    {
        animator.SetBool("inCombat", false);

        agent.SetDestination(startingPosition);
        if (agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            if (startingOrientation != facingRight) Flip();
        }
    }

    void HandleTarget()
    {
        animator.SetBool("inCombat", true);

        agent.SetDestination(targettingHandler.target.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            HandleAttack();
        }
    }

    void HandleAttack()
    {
        foreach (Collider hitObject in combatController.GetHitObjects())
        {
            if (hitObject.TryGetComponent<CombatController>(out CombatController targetCC)
                && targetCC.faction != combatController.faction)
            {
                combatController.Attack();
                return;
            }
        }
    }

    void HandleFlipping()
    {
        if (agent.desiredVelocity.x > 0 && facingRight) Flip();
        else if (agent.desiredVelocity.x < 0 && !facingRight) Flip();
    }

    void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;

        facingRight = !facingRight;
    }
}
