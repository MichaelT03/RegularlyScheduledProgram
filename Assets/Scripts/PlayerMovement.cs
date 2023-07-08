using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    Rigidbody2D myRigidbody;
    Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
    }

    void Run()
    {
        bool playerHasMovement = (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon) 
            || (Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon);

        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        myRigidbody.velocity = playerVelocity;

        
    }

    void FlipSprite()
    {
        bool playerHasHorizontalMovement = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        bool playerHasVerticalMovement = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;

        if (playerHasHorizontalMovement)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void OnMove(InputValue value)
    {
        // Gets an x and y value and assigns it to the moveInput variable
        moveInput = value.Get<Vector2>();
    }

    void OnInteract(InputValue input)
    {
        Debug.Log("Interact key pressed");
    }
}
