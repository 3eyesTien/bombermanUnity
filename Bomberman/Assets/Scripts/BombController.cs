using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [Header ("Bomb")] // Headers create headers for organization of groups of vars in Unity's inspector
    public GameObject bombPrefab;
    public KeyCode inputKey = KeyCode.Space;
    public float bombFuseTime = 3f; // Time until bomb explodes. Default at 3 seconds
    public int bombAmount = 1; // How many bombs player can have on the field simultaneously
    private int bombsRemaining;

    [Header ("Explosion")]
    public Explosion explosionPrefab; //When assigning prefab in editor, it has to have the Explosion script attached
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

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

        // Round off bomb position so that it's aligned with the grid tiles instead of between tiles
        position = bomb.transform.position; 
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // Create the explosion
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        Destroy(explosion.gameObject, explosionDuration);

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
