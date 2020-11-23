using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInputHandler : MonoBehaviour
{

    public float movementSpeed = 7;
    public Animator animator;
    public NavMeshAgent agent;

    void Start()
    {
        agent.updateRotation = false;    
    }
    
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        MoveBy(h, v);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            // attack
        }
    }

    void MoveBy(float horizontal, float vertical)
    {
        if (horizontal != 0 || vertical != 0)
        {
            // animator.SetBool("isRunning", true);

            // if (!AudioManager.Instance.isSFXPlaying()) AudioManager.Instance.PlaySFX(walkSound);
        }
        else
        {
            // animator.SetBool("isRunning", false);
        }
        Vector3 movement = new Vector3(horizontal, 0, vertical) * movementSpeed * Time.deltaTime;
        agent.Move(-movement);
    }
}
