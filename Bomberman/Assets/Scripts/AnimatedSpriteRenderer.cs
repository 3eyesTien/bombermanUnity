using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite idleSprite;
    public Sprite[] animationSprites;

    public float animationtime = 0.25f; // every four frames is one second
    private int animationFrame; // defaults to zero by itself. Don't need to set it

    public bool loop = true;
    public bool idle = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), animationtime, animationtime);
    }

    private void NextFrame() 
    {
        animationFrame++;

        // Check if animation is supposed to loop back to the first frame of the animation
        if(loop && animationFrame >= animationSprites.Length) 
        {
            animationFrame = 0;
        }

        if (idle)
        {
            spriteRenderer.sprite = idleSprite;
        }
        else if(animationFrame >= 0 && animationFrame < animationSprites.Length) // won't check out of array bounds for next animation frame
        {
            spriteRenderer.sprite = animationSprites[animationFrame];
        }
    }
}
