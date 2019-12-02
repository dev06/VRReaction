using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcerciseController : MonoBehaviour
{
    public JumpingJackColliderContainer jumpingJackHandler;
    void Start()
    {
        if (jumpingJackHandler.transform.gameObject.activeSelf)
        {
            jumpingJackHandler.StartJumpingjacks(15);
        }
    }
}