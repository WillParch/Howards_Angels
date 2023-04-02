using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public GameObject projectilePrefab;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public int health = 3;
    public int maxHealth = 3;
    public bool isPaused = false;
    public float winTime = 90f;
    public float dashSpeed = 50f;
    public float dashDuration = 0.2f;
    public float doubleTapTime = 0.2f;
    public float projectileSpeed = 20f;
    public float projectileCooldown = 0.5f;
    public GameObject platform;
    private float lastPlatformHitTime = 5f;
    private float platformHitCooldown = 0.5f;
    public float interval = 10f;
    public Transform groundCheck;
    public Transform projectileSpawn;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isDashing;
    float lastTapTime;
    float lastProjectileTime;

    private void Start()
    {
        lastProjectileTime = Time.time - projectileCooldown;
        health = maxHealth;
    }

    private void Update()
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

            if (Input.GetButtonDown("Fire1") && Time.time - lastProjectileTime > projectileCooldown)
            {
                FireProjectile();
                lastProjectileTime = Time.time;
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
    if (health <= 0)
    {
        
        return;
    }
    
    health -= damage;

    if (health <= 0)
    {
        SceneManager.LoadScene("Lose Menu");
       // Debug.Log("damage taken");
        // Handle player death, e.g., restart the level or load a game over scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Example: Restart the level
        
    }
    HealthBar.instance.SetValue(health / (float)maxHealth);
}

    void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
    }

  void OnControllerColliderHit(ControllerColliderHit hit)
{
    EnemyController enemy = hit.collider.GetComponentInParent<EnemyController>();

    if (enemy != null && hit.collider.CompareTag("Enemy"))
    {
        TakeDamage(enemy.playerDamage);
    }

    if (hit.collider.CompareTag("Platform") && hit.normal.y > 0.5f && Time.time - lastPlatformHitTime > platformHitCooldown)
    {
        lastPlatformHitTime = Time.time;
        StartCoroutine(Fade(hit.collider.gameObject));
    }

    if (hit.collider.CompareTag("WinPlatform"))
    {
        // Handle the win condition, e.g., load a win menu scene or display a message
         SceneManager.LoadScene("Win Menu"); // Example: Load a win menu scene
        Debug.Log("You win!");
    }
}


    IEnumerator Fade(GameObject platform)
    {
        platform.SetActive(false);
        yield return new WaitForSeconds(20f);
        platform.SetActive(true);
    }
 }
