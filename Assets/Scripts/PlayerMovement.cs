using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public int health = 3;
    public bool isPaused = false;
    public float winTime = 90f;
    public float dashSpeed = 50f;
    public float dashDuration = 0.2f;
    public float doubleTapTime = 0.2f;
    public GameObject platform;
    public float interval = 10f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isDashing;
    float lastTapTime;
    private float lastPlatformHitTime = 0f;
    private float platformHitCooldown = 0.5f;

    void Start()
    {
        platform.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isPaused)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            if (!isDashing)
            {
                controller.Move(move * speed * Time.deltaTime);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isGrounded && Time.time - lastTapTime < doubleTapTime)
                {
                    StartCoroutine(Dash(move));
                }
                lastTapTime = Time.time;
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;
            }
        }
    }

    IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time - startTime < dashDuration)
        {
            controller.Move(direction * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            // Handle player death, e.g., restart the level or load a game over scene
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Example: Restart the level
        }
    }

      void OnControllerColliderHit(ControllerColliderHit hit)
    {
        EnemyController enemy = hit.collider.GetComponentInParent<EnemyController>();

        if (enemy != null)
        {
            // Check if the player collided with the enemy
            if (hit.collider.CompareTag("Enemy"))
            {
                TakeDamage(enemy.playerDamage);
            }
        }

        // Check if the player collided with a platform
        if (hit.collider.CompareTag("Platform") && hit.normal.y > 0.5f && Time.time - lastPlatformHitTime > platformHitCooldown)
        {
            lastPlatformHitTime = Time.time;
            StartCoroutine(Fade(hit.collider.gameObject));
        }
    }

    IEnumerator Fade(GameObject platform)
    {
        platform.gameObject.SetActive(false);
        yield return new WaitForSeconds(interval);
        platform.gameObject.SetActive(true);
    }
}



