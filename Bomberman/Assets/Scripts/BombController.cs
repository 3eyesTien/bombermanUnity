using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Destructible destructiblePrefab;
    public Tilemap destructibleTiles;

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
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombsRemaining++;
    }

    // Make the explosion expand from the center of the bomb
    private void Explode(Vector2 position, Vector2 direction, int length) 
    { 
        if(length <= 0)
        {
            return;
        }

        position += direction; // Get new position of explosion

        // Create a box and  check if a collider is overlapping that box
        // Layermask checks only certain layers for objects. In this case, the stage objects only
        if(Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return; // Doesn't instantiate an explosion because it returned a collider in the OverlapBox
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        // Depending on the length of the explosion, it'll render the middle or end sprites of the full explosion animation
        // If there are multiple "lengths" left, it'll render the middle, otherwise it'll render the tip of the explosion
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction); // Rotate the explosion up, down, left, or right
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1); // Recursively call the function until length <= 0;
    }

    private void ClearDestructible(Vector2 position) 
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position); // Grab a cell to identify the position of the destructible tile from world coordinates
        TileBase tile = destructibleTiles.GetTile(cell);

        if(tile != null)
        {
            // Instantiate a wall prefab over the tile that got exploded
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
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
