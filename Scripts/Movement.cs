using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* a class that is used to move the player character in the kitchen (StartScene)*/
//The code has been referred from this : https://github.com/Brackeys/2D-Character-Controller
public class Movement : MonoBehaviour
{
    private bool isRight = false; //indicates if the player is facing right
    private float moveSpeed = 10f; //speed of the player while moving
    private Rigidbody2D rb; //Rigidbody component to facilitate the movement
    private Animator playerAnim; //Animator component to play the moving animation
    float horizontalMove = 0f; //represents the horizontal displacement
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed; //get the horizontal displacement of the player based on the movement keys and the player speed

        rb.velocity = new Vector2(horizontalMove, rb.velocity.y); //update the position of the player based on the horizontal displacement

        playerAnim.SetFloat("Speed", Mathf.Abs(horizontalMove)); //play the walking animation

        //if the player is moving to the right but is faced to the left
        if (horizontalMove > 0f && !isRight)
        {
            Flip(); //flip him

        }
        //if the player is moving to the left but is faced to the right
        else if (horizontalMove < 0f && isRight)
        {
            Flip(); //flip him
        }

    }
    /* function to flip the player to the opposite direction */
    void Flip()
    {
        isRight = !isRight;

        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
