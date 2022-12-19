using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAI : MonoBehaviour
{
    private float maxHealth;
    public RobotScriptableObject config;
    private NavMeshAgent agent = null;
    Vector3 moveDirection;
    Rigidbody rb;
    public Transform target;
    public Animator animator;

    void Start()
    {
        maxHealth = config.health;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        MoveToTarget();
        HealthCheck();
    }

    void FixedUpdate()
    {
        AnimationControler();
    }

    void AnimationControler()
    {
        moveDirection = agent.velocity;

        // Animation de marche
        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        // Animation de frappe
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("Contact");
        }
    }

    void MoveToTarget()
    {
        agent.speed = config.speed;
        agent.SetDestination(target.transform.position);
    }

    internal void TakeDamage(float damage)
    {
        if(maxHealth > 0){
            maxHealth -= damage;
        }
    }

    void HealthCheck()
    {
        if (maxHealth == 0)
        {
            EventManager.EnemyKilled();
            Destroy(gameObject);
        }
    }
}
