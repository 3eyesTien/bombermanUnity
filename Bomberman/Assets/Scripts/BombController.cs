using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject bombPrefab;
    public KeyCode inputKey = KeyCode.Space;
    public float bombFuseTime = 3f; // Time until bomb explodes. Default at 3 seconds
    public int bombAmount = 1; // How many bombs player can have on the field simultaneously
    private int bombsRemaining;

    private void OnEnable()
    {
        // Set bombsRemaining to however many you start with. Otherwise it would default to 0 and you can't place bombs at round start
        bombsRemaining = bombAmount; 
    }

    private void Update()
    {
        // Input.GetKeyDown specifically because you only want to register when the button is pressed, not held
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey)) {
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        // Round the x,y values in position so that the bombs are aligned in the grid of the stage
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        // Suspend execution of this function for the length of bombFuseTime
        yield return new WaitForSeconds(bombFuseTime);

        Destroy(bomb);
        bombsRemaining++;
    }
    
    // Make it so that the player can lay a bomb at the current position, then remove the trigger to be able to interact with/push it
    private void OnTriggerExit2D(Collider2D other)
    {
        // If the layer we just walked off of is "Bomb"
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb")) {
            other.isTrigger = false; 
        }
    }
}
