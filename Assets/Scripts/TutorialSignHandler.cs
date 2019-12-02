using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSignHandler : MonoBehaviour
{
    public float targetDistance = 5;
    public Transform targetOne, targetTwo;

    private PlayerPackage _playerPackage;
    private bool _hasReached;

    //void Update()
    //{
    //    if (hasReached() && !_hasReached)
    //    {
    //        if (_playerPackage == null)
    //        {
    //            _playerPackage = FindObjectOfType<PlayerPackage>();
    //        }

    //        _playerPackage.LoadNextScene();
    //        _hasReached = true;
    //    }
    //}

    //private bool hasReached()
    //{
    //    float _distanceOne = (transform.position - targetOne.position).sqrMagnitude;
    //    float _distanceTwo = (transform.position - targetTwo.position).sqrMagnitude;
    //    return _distanceOne < targetDistance || _distanceOne < targetDistance;
    //}
}