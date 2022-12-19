using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Sol;
    private bool isGrounded;

    [Header("Orientation")]
    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    Vector3 moveDirection;
    Vector3 dashDirection;
    Rigidbody rb;

    [Header("UI")]
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI HpText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Ground Check
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

    void Move(){
        moveDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }

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

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void JumpReset()
    {
        readyToJump = true;
    }

    void Dash()
    {
        // Calcul de la direction du dash
        dashDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        // Ajout de la force
        rb.AddForce(dashDirection.normalized * dashForce * 10f, ForceMode.Impulse);
    }

    void DashControler()
    {
        if (maxDash > 3)
        {
            maxDash = 3;
        }

        dashText.text = maxDash.ToString();
    }

    void HealthCheck()
    {
        if (health <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("MainMenu");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health -= 20;
            HpText.text = health.ToString();
        }
    }

    IEnumerator WaitForDash(float time)
    {
        yield return new WaitForSeconds(time);
        maxDash++;
    }

    IEnumerator WaitForJump(float time)
    {
        yield return new WaitForSeconds(time);
        JumpReset();
    }
}
