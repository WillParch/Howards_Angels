using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    //This code is making me VIOLENT I am going to drop out of game design I swear

    public CharacterController controller;
    public Transform cam;
    public GameObject projectilePrefab;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
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

    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip projectileSound;

    Vector3 velocity;
    bool isGrounded;
    bool isDashing;
    private bool canDoubleJump;
    float lastTapTime;
    float lastProjectileTime;

    private void Start()
    {
        lastProjectileTime = Time.time - projectileCooldown;
        health = maxHealth;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        if (!isPaused && !isDashing)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                audioSource.PlayOneShot(jumpSound);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isGrounded && Time.time - lastTapTime < doubleTapTime)
                {
                    StartCoroutine(Dash(direction));
                }
                lastTapTime = Time.time;
            }

            if (Input.GetButtonDown("Fire1") && Time.time - lastProjectileTime > projectileCooldown)
            {
                FireProjectile();
                lastProjectileTime = Time.time;
                audioSource.PlayOneShot(projectileSound);
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
    Vector3 fireDir = Camera.main.transform.forward;

    GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
    projectile.transform.forward = fireDir;
    projectile.GetComponent<Rigidbody>().velocity = fireDir * projectileSpeed;
    lastProjectileTime = Time.time;
    audioSource.PlayOneShot(projectileSound);
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
        if (lastPlatformHitTime == 0f)
        {
            lastPlatformHitTime = Time.time;
        }
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
    Debug.Log("Fading platform");
    platform.SetActive(false);
    yield return new WaitForSeconds(interval);
    platform.SetActive(true);
}

 }