using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderType
{
    TOP,
    BOTTOM,
}
public class JumpingJackCollider : MonoBehaviour
{
    public ColliderType type;

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.tag == "Objects/Lefthand" || col.gameObject.tag == "Objects/Righthand")
        {
            if (JumpingJackColliderContainer.OnJumpingJackColliderHit != null)
            {
                JumpingJackColliderContainer.OnJumpingJackColliderHit (type);
            }
        }
    }
}