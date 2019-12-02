using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    private DataRecorder _dataRecorder;
    private float _lookingTimer;

    void Start()
    {
        _dataRecorder = FindObjectOfType<DataRecorder>();
    }

    public void LookingAt()
    {
        _lookingTimer += Time.deltaTime;
#if UNITY_EDITOR
        Debug.Log("Recording Timer -> " + _lookingTimer);
#endif
    }

    public float LookingTimer
    {
        get { return _lookingTimer; }
    }
}
