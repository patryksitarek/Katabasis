using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 7;
    public Animator animator;
    public NavMeshAgent agent;
    public CombatController combatController;
    private bool facingRight;
    public float speedBoostValue = 1.4f;

    void Start()
    {
        agent.updateRotation = false;
        facingRight = transform.localScale.x > 0;
    }
    
    void FixedUpdate()
    {
        if (combatController.IsImmobilized()) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        MoveBy(h, v);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            combatController.Attack();
        }
    }

    void MoveBy(float horizontal, float vertical)
    {
        // Animations
        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        Vector3 movement = new Vector3(horizontal, 0, vertical) * movementSpeed * Time.deltaTime;
        agent.Move(-movement);

        // Adjust character orientation
        if (horizontal < 0 && facingRight) Flip();
        else if (horizontal > 0 && !facingRight) Flip();
    }

    private void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;

        facingRight = !facingRight;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (gameObject.tag == "Player" && collision.gameObject.tag == "speedBoost")
        {
            StartCoroutine(speedBoost(Random.Range(3.0f, 8.0f)));
            Destroy(collision.gameObject);
        }
        
    }

    private IEnumerator speedBoost(float time)
    {
        movementSpeed *= speedBoostValue;
        yield return new WaitForSeconds(time);
        movementSpeed /= speedBoostValue;
    }



}
