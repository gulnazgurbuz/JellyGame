using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JellyController : MonoBehaviour
{

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _PathPoints;
    [HideInInspector] public RaycastHit Hit;
    [HideInInspector] public Transform JellyCopyCreateTransform;
    
    private Vector3 _directionToObstacle;
    private GameController _gameController;
    private int _pathPoint = 0;
    private bool _raycastControl = false;
    private bool _firstTimeJellyPos = true;

    private void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        RaycastToNextObstacle();
    }

    private void FixedUpdate()
    {   
        RaycastToNextObstacle();
        JellyMovement();
        UpdatePathPoint();
        PositionForJellyCopyObj();
    }

    private void RaycastToNextObstacle()
    {
        if (Physics.Raycast(transform.position, transform.forward, out Hit, 3000, _layerMask))
        {
            _raycastControl = true;
            Debug.DrawLine(transform.position, Hit.point, Color.red);
            if (_firstTimeJellyPos)
            {
                PositionForJellyCopyObj();
                _firstTimeJellyPos = false;
            }
        }
        else
        {
            _raycastControl = false;
            _firstTimeJellyPos = true;
        }
    }
    private void PositionForJellyCopyObj()
    {
        if (_raycastControl)
        {
            JellyCopyCreateTransform = Hit.collider.transform.parent.Find("JellySampleCreatePoint");
            _gameController.jellyCopy.transform.position = new Vector3(JellyCopyCreateTransform.position.x, _gameController.jellyCopy.transform.position.y,
                                                        JellyCopyCreateTransform.position.z);
        }
    }
    private void JellyMovement()
    {
        if (_gameController.GameStatusEnum == GameStatus.CRASH)
        {
            _gameController.GameStatusEnum = GameStatus.EMPTY;
            transform.DOJump(transform.position - transform.forward, 0.2f, 1, 0.3f).OnComplete(() =>
             {
                 _gameController.GameStatusEnum = GameStatus.STAY;
                 _gameController.ObstacleCounter = 0;
             });
        }
        else if (_gameController.GameStatusEnum == GameStatus.STAY)
        {
            if (_PathPoints.childCount > _pathPoint)
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(_PathPoints.GetChild(_pathPoint).transform.position.x, transform.position.y, _PathPoints.GetChild(_pathPoint).transform.position.z), 0.08f);
        }
        else if (_gameController.GameStatusEnum == GameStatus.STARSTAY)
        {
            if (_PathPoints.childCount > _pathPoint)
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(_PathPoints.GetChild(_pathPoint).transform.position.x, transform.position.y, _PathPoints.GetChild(_pathPoint).transform.position.z), 0.15f);
        }
    }
    private void JellyObjectDİrection(GameObject _myObj)
    {
        if (_PathPoints.childCount > _pathPoint)
        {
            _myObj.transform.DOLookAt(_PathPoints.GetChild(_pathPoint).transform.position, 0.5f).OnComplete(() =>
            {
                _gameController.jellyCopy.transform.localRotation = transform.localRotation;
                _gameController.jellyWay.transform.localRotation = transform.localRotation;
            });
        }
    }
    private void UpdatePathPoint()
    {
        if (_pathPoint < _PathPoints.childCount && Math.Abs(Vector3.Distance(transform.position, _PathPoints.GetChild(_pathPoint).transform.position)) < 0.5f)
        {
            _pathPoint++;
            JellyObjectDİrection(this.gameObject);
        }
    }
    private void HittedObstacle(GameObject obstacle, float forceIntensity)
    {
        _directionToObstacle = (obstacle.transform.position - transform.position).normalized;
        obstacle.GetComponent<Rigidbody>().AddForce(_directionToObstacle * forceIntensity);
        obstacle.GetComponent<Rigidbody>().useGravity = true;
        obstacle.GetComponent<BoxCollider>().isTrigger = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "JellyCopyTag")
        {
            PositionForJellyCopyObj();
            if (_gameController.GameStatusEnum == GameStatus.STAY)
            {
                _gameController.ObstacleCounter++;
                if (_gameController.ObstacleCounter == 3)
                {
                    _gameController.ObstacleCounter = 0;
                    _gameController.GameStatusEnum = GameStatus.STARINIT;
                }
            }
        }
        else if (other.tag == "FinalTag")
        {
            Debug.Log("Level has been complete with success!");
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (_gameController.GameStatusEnum == GameStatus.STARSTAY && other.tag == "JellyCopyTag")
        {
            for (int i = 2; i < other.gameObject.transform.parent.gameObject.transform.childCount; i++)
            {
                if (other.gameObject.transform.parent.gameObject.transform.GetChild(i).tag == "ObstacleTag")
                {
                    //Debug.Log(other.gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject);
                    HittedObstacle(other.gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject, 50);
                }
            }
        }
        else if (other.tag == "ObstacleTag")
        {
            other.tag = "Untagged";
            if (_gameController.GameStatusEnum == GameStatus.STAY)
                _gameController.GameStatusEnum = GameStatus.CRASH;
            HittedObstacle(other.gameObject, 20);
        }
    }
}
