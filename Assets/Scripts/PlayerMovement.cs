using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Joueur")]
    public float health;

    [Header("Movement")]
    public float speed;
    public float dashForce;
    public float dashCooldown;
    public int maxDash = 3;
    public float drag;
    public float jumpForce;
    public float jumpCooldown;
    bool readyToJump = true;

    [Header("SiLeJoueurEstAuSol")]
    public float playerHeight;
    public LayerMask Sol;
    private bool isGrounded;

    [Header("Orientation")]
    public Transform orientation;
    private float horizontalInput;
    private float verticalInput;

    [Header("Dash")]
    Vector3 moveDirection;
    Vector3 dashDirection;
    Rigidbody rb;

    [Header("UI")]
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI HpText;

    [Header("Audio")]
    public AudioSource dashSound;
    public AudioSource jumpSound;
    public AudioSource hurtSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Regarde si le joueur est au sol
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Sol);

        GetInput();
        speedControler();
        DashControler();
        HealthCheck();

        // Ajout du drag si le joueur est au sol
        if(isGrounded){
            rb.drag = drag;
        } else {
            rb.drag = 0;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Gère les inputs du joueur
    /// </summary>
    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && readyToJump)
        {
            readyToJump = false;

            Jump();

            //attendre avant de pouvoir sauter à nouveau
            StartCoroutine(WaitForJump(jumpCooldown));

            JumpReset();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            if(horizontalInput != 0 || verticalInput != 0){
                if(maxDash > 0){
                    maxDash--;

                    Dash();

                    //attendre avant de pouvoir ajouter un dash à nouveau
                    StartCoroutine(WaitForDash(dashCooldown));
                }
            }
        }
    }

    /// <summary>
    /// Gère le déplacement du joueur
    /// </summary>
    void Move(){
        moveDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }

    /// <summary>
    /// Gère la vitesse du joueur
    /// </summary>
    void speedControler()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        //Limites la vitesse du joueur
        if (flatVelocity.magnitude > speed)
        {
            Vector3 limite = flatVelocity.normalized * speed;
            rb.velocity = new Vector3(limite.x, rb.velocity.y, limite.z);
        }
    }

    /// <summary>
    /// Gère le saut du joueur
    /// </summary>
    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;

        jumpSound.Play();
    }

    /// <summary>
    /// Réinitialise le temps d'attente du saut
    void JumpReset()
    {
        readyToJump = true;
    }

    /// <summary>
    /// Gêre le dash du joueur
    /// </summary>
    void Dash()
    {
        // Calcul de la direction du dash
        dashDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        // Ajout de la force
        rb.AddForce(dashDirection.normalized * dashForce * 10f, ForceMode.Impulse);

        // Joue le son du dash
        dashSound.Play();
    }

    /// <summary>
    /// Gère le nombre de dash du joueur
    /// </summary>
    void DashControler()
    {
        if (maxDash > 3)
        {
            maxDash = 3;
        }

        dashText.text = maxDash.ToString();
    }

    /// <summary>
    /// Gère la vie du joueur
    /// </summary>
    void HealthCheck()
    {
        if (health <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("MainMenu");
        }
    }

    /// <summary>
    /// Gère les dégats du joueur
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            hurtSound.Play();
            health -= 20;
            HpText.text = health.ToString();
        }
    }

    /// <summary>
    /// Fait attendre le joueur avant de pouvoir ajouter un dash
    /// </summary>
    IEnumerator WaitForDash(float time)
    {
        yield return new WaitForSeconds(time);
        maxDash++;
    }

    /// <summary>
    /// Fait attendre le joueur avant de pouvoir sauter à nouveau
    /// </summary>
    IEnumerator WaitForJump(float time)
    {
        yield return new WaitForSeconds(time);
        JumpReset();
    }
}
