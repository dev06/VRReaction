using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AppType
{
    Neutral,
    Negative,
    Postive,
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AppSettings")]
public class AppSettings : ScriptableObject
{
    public DeviceType deviceType;
    public AppType appType;
}
