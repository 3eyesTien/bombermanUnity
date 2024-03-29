using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimatedSpriteRenderer start;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer end;

    public void SetActiveRenderer(AnimatedSpriteRenderer renderer)
    {
        //Game will render which ever part of the animation that was passed into the function if true
        //ex. if middle was passed through, then the middle sprite animation will be enabled and seen by the player
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    public void SetDirection(Vector2 direction) 
    {
        float angle = Mathf.Atan2(direction.y, direction.x); // This returns angle in radians
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward); //use Mathf.Rad2Deg to get proper values. Rotate explosion to face direction we tell it to
    }

    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}
