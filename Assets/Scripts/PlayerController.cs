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

     Animator animator;

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
    public float interval = 10f;
    public Transform groundCheck;
    public Transform projectileSpawn;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public int keys = 0;
    public GameObject shield;
    public GameObject keyring1;
    public GameObject keyring2;

    public int ScoreOne = 0;
    public int ScoreTwo = 0;
    public int ScoreThree = 0;

    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip projectileSound;

    Vector3 velocity;
    bool isGrounded;
    bool isDashing;
    private bool canDoubleJump;
    float lastTapTime;
    float lastProjectileTime;

    public GameObject spaceWalk;
    private float lastActivatedTime;
    private const float cooldownTime = 10f;

    private bool hasDoubleDashed;


    private void Start()
    {
        lastProjectileTime = Time.time - projectileCooldown;
        health = maxHealth;

       animator=this.GetComponent<Animator>();

       spaceWalk.SetActive(false);

       if (PlayerPrefs.HasKey("ScoreThree"))
        {
        ScoreThree = PlayerPrefs.GetInt("ScoreThree");
        }
        if (PlayerPrefs.HasKey("ScoreTwo"))
        {
        ScoreTwo = PlayerPrefs.GetInt("ScoreTwo");
        }
        if (PlayerPrefs.HasKey("ScoreOne"))
        {
        ScoreOne = PlayerPrefs.GetInt("ScoreOne");
        }
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
        transform.rotation = Quaternion.Euler(0f, angle, 0f).normalized;

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDirection.normalized * speed * Time.deltaTime);

        animator.SetInteger("walk",1);
    }

    if (Input.GetKeyDown(KeyCode.E) && Time.time > lastActivatedTime + cooldownTime)
            {
                Debug.Log("Pressed E!");
                    spaceWalk.SetActive(true);
                    lastActivatedTime = Time.time;
                     StartCoroutine(DeactivateAfterDelay(3f));

            }


        if (!isPaused && !isDashing)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

             if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            canDoubleJump = true;
            hasDoubleDashed = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                audioSource.PlayOneShot(jumpSound);
            }
            else if (canDoubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                audioSource.PlayOneShot(jumpSound);
                canDoubleJump = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isGrounded && !hasDoubleDashed && Time.time - lastTapTime < doubleTapTime)
            {
                StartCoroutine(Dash(direction));
                hasDoubleDashed = true;
            }
            lastTapTime = Time.time;
        }

            if (Input.GetButtonDown("Fire1") && Time.time - lastProjectileTime > projectileCooldown)
            {
                FireProjectile();
                lastProjectileTime = Time.time;
                audioSource.PlayOneShot(projectileSound);
                animator.SetInteger("projectile",1);

            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.P))
            {
                isPaused = !isPaused;
            }
        }
        if (keys == 2)
        {
            Destroy(shield);
        }

        if (ScoreOne >= 1 && ScoreTwo >= 1 && ScoreThree >= 1)
        {
            SceneManager.LoadScene("Win Menu");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


   IEnumerator Dash(Vector3 direction)
{
    isDashing = true;
    float startTime = Time.time;

    // Get the camera's forward direction, ignoring the y-axis
    Vector3 cameraForward = new Vector3(cam.forward.x, 0f, cam.forward.z).normalized;

    while (Time.time - startTime < dashDuration)
    {
        controller.Move(cameraForward * dashSpeed * Time.deltaTime);
        yield return null;
    }

    isDashing = false;
}

IEnumerator DeactivateAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    spaceWalk.SetActive(false);
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
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
       // Debug.Log("damage taken");
        // Handle player death, e.g., restart the level or load a game over scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Example: Restart the level
        
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


    if (hit.collider.CompareTag("WinPlatform"))
    {
        ScoreOne++;
        // Handle the win condition, e.g., load a win menu scene or display a message
        SceneManager.LoadScene("Hub Level"); // Example: Load a win menu scene
       PlayerPrefs.SetInt("ScoreOne", ScoreOne);
        Debug.Log("You win!");
         Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    if (hit.collider.CompareTag("WinPlatform2"))
    {
         ScoreTwo++;
        // Handle the win condition, e.g., load a win menu scene or display a message
        SceneManager.LoadScene("Hub Level"); // Example: Load a win menu scene
       PlayerPrefs.SetInt("ScoreTwo", ScoreOne);
        Debug.Log("You win!");
         Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
     if (hit.collider.CompareTag("WinPlatform3"))
    {
        ScoreThree++;
        // Handle the win condition, e.g., load a win menu scene or display a message
        SceneManager.LoadScene("Hub Level"); // Example: Load a win menu scene
       PlayerPrefs.SetInt("ScoreThree", ScoreOne);
        Debug.Log("You win!");
         Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }
      if (hit.collider.CompareTag("Level1"))
    {
        // Handle the win condition, e.g., load a win menu scene or display a message
         SceneManager.LoadScene("Level 1"); // Example: Load a win menu scene
    }
      if (hit.collider.CompareTag("Level2"))
    {
        // Handle the win condition, e.g., load a win menu scene or display a message
         SceneManager.LoadScene("Level 2"); // Example: Load a win menu scene
    }
     if (hit.collider.CompareTag("Level3"))
    {
        // Handle the win condition, e.g., load a win menu scene or display a message
         SceneManager.LoadScene("Level 3"); // Example: Load a win menu scene
    }
    if (hit.collider.CompareTag("Key"))
    {
        keys++;
        Destroy(keyring1);
    }
    if (hit.collider.CompareTag("Keys"))
    {
        keys++;
        Destroy(keyring2);
    }
    
}

 }
