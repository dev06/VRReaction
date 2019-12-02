using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    void Update()
    {
        RaycastHit hit;
        Ray r = new Ray(transform.position, transform.forward);
        int layer = 1 << 8;
        Debug.DrawRay(r.origin, r.direction, Color.blue);


        layer = ~layer;
        if (Physics.Raycast(r, out hit, 10, layer))
        {
            Sign s = hit.transform.GetComponent<Sign>();
            if (s != null)
            {
                s.LookingAt();
            }
        }
    }
}
