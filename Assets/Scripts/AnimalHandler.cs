using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AnimalHandler : MonoBehaviour
{
    public bool shouldFollow;
    [HideInInspector] public bool followPlayer;
    private Vector3 _targetLocation;
    public float speed = 1f;
    public float distanceToPlayer = 10;
    public float rotationLerp = 5f;

    public Transform ViveCamera, RiftCamera;
    private bool _chosen;
    private NavMeshAgent _agent;
    private bool _isWalking;
    private Animator _animator;
    private float _followTimer;
    private bool _startFollowTimer;
    private float _followPlayerTime;
    private bool _init2, _chosen2;
    private bool _init1, _chosen1;
    private Vector3 _targetCameraPosition;

    private PlayerPackage _playerPackage;

    void Start()
    {
        _playerPackage = FindObjectOfType<PlayerPackage>();
        _targetLocation = chooseNextLocation();
        transform.position = chooseNextLocation();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.updateRotation = false;
        _followPlayerTime = Random.Range(5f, 15f);
        followPlayer = Random.value < .2f;
    }


    void Update()
    {
        if (!shouldFollow)
        {
            followPlayer = false;
        }

        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     followPlayer = !followPlayer;
        //     if (followPlayer)
        //     {
        //         _targetLocation = chooseNextLocation(RiftCamera.transform.position);
        //         _agent.SetDestination(_targetLocation);
        //     }
        //     else
        //     {
        //         _targetLocation = chooseNextLocation();
        //         _agent.SetDestination(_targetLocation);
        //     }
        // }



        if (followPlayer)
        {
            Vector3 _camera = getTargetCameraTransform();
            _init2 = false;
            if (!_init1)
            {
                _targetLocation = chooseNextLocation(_camera);
                _agent.SetDestination(_targetLocation);
                _init1 = true;
            }

            if (_agent.remainingDistance < .5f)
            {
                if (_chosen1 == false)
                {
                    _targetLocation = chooseNextLocation(_camera);
                    _agent.SetDestination(_targetLocation);
                    _chosen1 = true;
                }
            }
            else
            {
                _chosen1 = false;
            }


            Vector3 v = _camera - transform.position;
            if (v.sqrMagnitude < 2f)
            {
                _startFollowTimer = true;
            }

            if (_startFollowTimer)
            {
                _followTimer += Time.deltaTime;
                if (_followTimer > _followPlayerTime)
                {
                    _followTimer = 0;
                    _startFollowTimer = false;
                    followPlayer = false;
                }
            }
        }
        else
        {
            _init1 = false;
            if (!_init2)
            {
                _targetLocation = chooseNextLocation();
                _agent.SetDestination(_targetLocation);
                _init2 = true;
            }

            if (_agent.remainingDistance < .5f)
            {
                if (_chosen2 == false)
                {
                    _targetLocation = chooseNextLocation();
                    _agent.SetDestination(_targetLocation);
                    _chosen2 = true;
                }
            }
            else
            {
                _chosen2 = false;
            }

            _followTimer += Time.deltaTime;
            if (_followTimer > 60f)
            {
                if (Random.value < .5f)
                {
                    followPlayer = true;
                    _followTimer = 0;
                }
            }
        }

        transform.forward = Vector3.Lerp(transform.forward, _agent.velocity, Time.deltaTime * rotationLerp);
        // if (followPlayer)
        // {
        //     _init2 = false;
        //     if (!_chosen)
        //     {
        //         _targetLocation = chooseNextLocation(RiftCamera.transform.position);
        //         _chosen = true;
        //     }

        //     if (_agent.remainingDistance < 1f)
        //     {
        //         _chosen = false;
        //     }
        //     transform.forward = Vector3.Lerp(transform.forward, _agent.velocity, Time.deltaTime * rotationLerp);
        //     _agent.SetDestination(_targetLocation);
        // }
        // else
        // {
        //     _chosen = false;
        //     if (!_init2)
        //     {
        //         _targetLocation = chooseNextLocation();
        //         _agent.SetDestination(_targetLocation);
        //         _init2 = true;
        //     }

        //     if (_agent.remainingDistance < 1f)
        //     {
        //         _targetLocation = chooseNextLocation();
        //         _agent.SetDestination(_targetLocation);
        //     }


        //     transform.forward = Vector3.Lerp(transform.forward, _agent.velocity, Time.deltaTime * rotationLerp);

        // }


        // Vector3 _directionToPlayer = (RiftCamera.transform.position - transform.position);
        // float _distanceToPlayer = _directionToPlayer.sqrMagnitude;

        // transform.forward = Vector3.Lerp(transform.forward, _agent.velocity, Time.deltaTime * rotationLerp);
        // if (_distanceToPlayer < distanceToPlayer && _followPlayer)
        // {
        //     if (!_chosen)
        //     {
        //         _targetLocation = chooseNextLocation(RiftCamera.transform.position);
        //         _agent.SetDestination(_targetLocation);
        //         _chosen = true;
        //     }

        //     Debug.Log(_agent.remainingDistance);
        //     if (_agent.remainingDistance < 1f)
        //     {
        //         _startFollowTimer = true;
        //         _chosen = false;
        //     }

        //     if (_startFollowTimer)
        //     {
        //         _followTimer += Time.deltaTime;
        //         if (_followTimer > _followPlayerTime)
        //         {
        //             _followPlayer = false;
        //             _followTimer = 0;
        //             _startFollowTimer = false;
        //             _targetLocation = chooseNextLocation();
        //         }
        //     }
        // }

        // _agent.SetDestination(_targetLocation);
        // if (_agent.remainingDistance < 1f && _distanceToPlayer > distanceToPlayer)
        // {
        //     _targetLocation = chooseNextLocation();
        // }


        // Vector3 direction = _targetLocation - transform.position;
        // float distance = direction.sqrMagnitude;
        // transform.forward = direction;
        // transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        // if (distance < 1f)
        // {
        //     _targetLocation = chooseNextLocation();
        // }

    }

    private Vector3 getTargetCameraTransform()
    {
        return _playerPackage.appSettings.deviceType == DeviceType.Oculus ? RiftCamera.transform.position : ViveCamera.transform.position;
    }

    private Vector3 getPositionAroundTarget(Vector3 _target)
    {
        Vector2 circle = Random.insideUnitCircle * 3f;
        return _target + new Vector3(circle.x, _target.y, circle.y);
    }

    private Vector3 chooseNextLocation()
    {
        float range = 40f;
        return new Vector3(Random.Range(-range, range), transform.position.y, Random.Range(90f, 150f));
    }

    private Vector3 chooseNextLocation(Vector3 _base)
    {
        float range = 2f;
        return _base + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
    }

    private Vector3 getLocation(Vector3 _base)
    {
        Vector3 _location = new Vector3(_base.x + Random.Range(-2f, 2f), _base.y, _base.z + Random.Range(-2f, 2f));

        do
        {
            _location = new Vector3(_base.x + Random.Range(-2f, 2f), _base.y, _base.z + Random.Range(-2f, 2f));

        } while ((_location - _base).sqrMagnitude < 1f);

        return _location;
    }
}
