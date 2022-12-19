using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAI : MonoBehaviour
{
    [Header("Statistique du robot")]
    private float maxHealth;
    public RobotScriptableObject config;
    private NavMeshAgent agent = null;
    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Cible du robot")]
    public Transform target;

    [Header("Animation du robot")]
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

    /// <summary>
    /// Contrôle les animations du robot pour si il bouge ou non
    /// </summary>
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
    }

    /// <summary>
    /// Contrôle l'animation du robot lorsqu'il est en contact avec le joueur
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("Contact");
        }
    }

    /// <summary>
    /// Contrôle la vitesse et la direction du robot
    /// </summary>
    void MoveToTarget()
    {
        agent.speed = config.speed;
        agent.SetDestination(target.transform.position);
    }

    /// <summary>
    /// Contrôle la vie du robot
    /// </summary>
    internal void TakeDamage(float damage)
    {
        if(maxHealth > 0){
            maxHealth -= damage;
        }
    }

    /// <summary>
    /// Vérifie si le robot est mort
    /// </summary>
    void HealthCheck()
    {
        if (maxHealth == 0)
        {
            EventManager.EnemyKilled();
            Destroy(gameObject);
        }
    }
}
