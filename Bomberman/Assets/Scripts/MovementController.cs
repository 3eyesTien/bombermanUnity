using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    private Vector2 direction = Vector2.down;
    public float speed = 5f;

    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    private AnimatedSpriteRenderer activeSpriteRenderer;

    private void Awake() // Called when script is initialized
    {
        rb = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
    }

    private void Update()
    {
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if(Input.GetKey(inputDown)) 
        {
            SetDirection(Vector2.down, spriteRendererDown);
        } 
        else if(Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        else 
        { 
            SetDirection(Vector2.zero, activeSpriteRenderer); 
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rb.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer sRenderer)
    {
        direction = newDirection;

        // essentially if(sRenderer == spriteRendererup){spriteRendererUp.enabled;} and disables the rest so the animations aren't overlapping 
        spriteRendererUp.enabled = sRenderer == spriteRendererUp;
        spriteRendererDown.enabled = sRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = sRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = sRenderer == spriteRendererRight;

        activeSpriteRenderer = sRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

}
