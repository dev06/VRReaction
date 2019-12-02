using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderTarget : MonoBehaviour
{
    public Transform targetOne;
    public Transform targetTwo;
    private PlayerPackage _playerPackage;
    private float _targetTwoDistance;
    private bool _targetTwoHit;

    void Start()
    {
        _playerPackage = FindObjectOfType<PlayerPackage>();
    }

    void Update()
    {
        if (targetTwo == null) return;
        _targetTwoDistance = (transform.position - targetTwo.transform.position).magnitude;
        if (_targetTwoDistance < 5f && !_targetTwoHit)
        {
            _playerPackage.LoadNextScene();
            _targetTwoHit = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetOne.gameObject)
        {
            _playerPackage.LoadNextScene();
        }
    }
}
