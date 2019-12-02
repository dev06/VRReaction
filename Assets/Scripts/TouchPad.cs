using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; 
public class TouchPad : MonoBehaviour
{
    private float movementSpeed; 
    public Transform cameraRigTransform;
    public Transform head;
    public SteamVR_Action_Vector2 touchpadAction; 
    // Start is called before the first frame update
    void Start()
    {
        _movement = Vector3.zero;
        cameraRigTransform.position = Vector3.zero; 
        movementSpeed = 5; 
    }

    Vector3 _movement; 
    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = touchpadAction.GetAxis(SteamVR_Input_Sources.Any);
        Vector3 newV = new Vector3(velocity.x, 0, velocity.y);
        newV = head.rotation * newV;
        newV.y = 0; 
        _movement += new Vector3(newV.x, 0f, newV.z); 
        cameraRigTransform.Translate(newV * Time.deltaTime * movementSpeed);
    }

}
