using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    public CharacterController controller;
    public float speed = 12f;
    public float interval = 0.5f;
    bool Boosting;
   
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Boosting = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool groundedPlayer = controller.isGrounded;
 
        // slam into the ground

        float x = Input.GetAxis ("Horizontal");
        float z = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    
        if (movement.magnitude >= 0.1f) 
        {     
            controller.Move(movement * speed * Time.deltaTime); 
        }

        if (Boosting == false)
        {
            speed = 12f;
        }

        if (Input.GetKeyDown("space"))
        {
            Boosting = true;
            StartCoroutine(Boost());
            
        }
// "speed" is a private float variable that is used to control the speed of the player
    }
    IEnumerator Boost()
    {
            if(true)
            {
                speed = 30f;
                yield return new WaitForSeconds(interval);
                if (interval == 0.5f)
                {
                   Boosting = false;
                }
                
            }
    }

}
