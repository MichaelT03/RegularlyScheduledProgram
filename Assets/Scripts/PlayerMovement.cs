using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Create a variable to hold external scripts
    TVInteraction tvi;

    [SerializeField] float moveSpeed = 5f;

    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCapsuleCollider;
    Vector2 moveInput;
    Animator myAnimator;

    // Raycast variables
    [SerializeField] float raycastDistance = 1.0f;
    [SerializeField] int raycastCount = 12;
    LayerMask raycastLayerMask;
    bool rayIsTouchingTv = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();

        raycastLayerMask = ~LayerMask.GetMask("Player");

        // Create a reference to the external gameObject and get it's sctript
        tvi = GameObject.FindGameObjectWithTag("TV").GetComponent<TVInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        HandleRaycasts();
    }

    void Run()
    {
        bool playerHasMovement = (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon) 
            || (Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon);

        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        myRigidbody.velocity = playerVelocity;

        FlipSprite();
        playerAnimation();
    }

    void playerAnimation()
    {
        
        bool playerHasHorizontalMovement = (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon);
        bool playerIsMovingUp = myRigidbody.velocity.y > Mathf.Epsilon;
        bool playerIsMovingDown = myRigidbody.velocity.y < 0f;

        if (playerHasHorizontalMovement)
        {
            myAnimator.SetBool("isMovingHoriz", true);
            myAnimator.SetBool("isMovingUp", false);
            myAnimator.SetBool("isMovingDown", false);
            return;
        }
        else if (playerIsMovingDown)
        {
            myAnimator.SetBool("isMovingHoriz", false);
            myAnimator.SetBool("isMovingUp", false);
            myAnimator.SetBool("isMovingDown", true);
            return;
        }
        else if (playerIsMovingUp)
        {
            myAnimator.SetBool("isMovingHoriz", false);
            myAnimator.SetBool("isMovingUp", true);
            myAnimator.SetBool("isMovingDown", false);
            return;
        }            
        else
        {
            myAnimator.SetBool("isMovingHoriz", false);
            myAnimator.SetBool("isMovingUp", false);
            myAnimator.SetBool("isMovingDown", false);
            return;
        }

    }

    void FlipSprite()
    {
        bool playerHasHorizontalMovement = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

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

    void HandleRaycasts()
    {
        // Evenly space out rays by degrees
        float angleInterval = 360f / raycastCount;

        for (int i = 0; i < raycastCount; i++)
        {
            float angle = i * angleInterval;
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * transform.up;

            PerformRaycast(direction);
        }
    }

    void PerformRaycast(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, raycastLayerMask);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("TV"))
            {
                rayIsTouchingTv = true;
                Debug.Log("Ray is hitting TV");
            }
            else if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Ray is hitting player");
            }
            else
            {
                rayIsTouchingTv = false;
                Debug.Log("Another object has been hit");
            }
        }
        else
        {
            rayIsTouchingTv = false;
            Debug.Log("Ray not currently touching anything");
        }

        Debug.DrawRay(transform.position, direction * raycastDistance, Color.red);
    }

    void OnInteract(InputValue input)
    {
        LayerMask TVLayer = LayerMask.GetMask("TV");

        if (rayIsTouchingTv)
            tvi.ToggleAnimation();
    }
}
