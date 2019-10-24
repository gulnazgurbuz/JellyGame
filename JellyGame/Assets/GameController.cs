using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    [HideInInspector] public Transform jelly;
    [HideInInspector] public Transform jellyCopy;
    [HideInInspector] public Transform jellyWay;
    [HideInInspector] public GameStatus GameStatusEnum;
    [HideInInspector] public int ObstacleCounter = 0;

    private JellyController _jellyController;

    private Vector3 jellyScale;
    private Vector3 jellyCopyScale;
    private Vector3 jellyWayScale;
    private Vector3 _beginPos;
    private Vector3 _touchStart;
    private Vector3 _direction;

    private float _excessWidth;
    private float _timeCounterForStarEnum = 0;

    void Start()
    {
        GameStatusEnum = GameStatus.STAY;

        jelly = GameObject.FindGameObjectWithTag("Player").transform;
        _jellyController = jelly.GetComponent<JellyController>();
        jellyWay = jelly.Find("JellyWay");
        jellyCopy = jelly.Find("JellyCopy");
        jellyCopy.transform.SetParent(transform.parent);
        jellyWay.transform.SetParent(transform.parent);

        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerSet += OnFingerSet;
        
        _excessWidth = (jellyCopy.transform.localScale.z + jelly.transform.localScale.z) / 2;      
    }

    private void FixedUpdate()
    {
        JellyWayPropertyZ();
        switch (GameStatusEnum)
        {
            case GameStatus.START:
                break;
            case GameStatus.STAY:
                if (_jellyController.Hit.distance > 0.2f)
                    jellyCopy.gameObject.SetActive(true);
                else
                    jellyCopy.gameObject.SetActive(false);
                break;
            case GameStatus.CRASH:
                break;
            case GameStatus.BREAKOBSTACLE:
                break;
            case GameStatus.STARINIT:
                ChangeObstacleMat("green");
                jellyCopy.gameObject.SetActive(false);
                jellyWay.gameObject.SetActive(false);
                GameStatusEnum = GameStatus.STARSTAY;
                break;
            case GameStatus.STARSTAY:
                _timeCounterForStarEnum += Time.deltaTime;
                if (_timeCounterForStarEnum > 3)
                {
                    GameStatusEnum = GameStatus.STAY;
                    ChangeObstacleMat("gray");
                    ObstacleCounter = 0;
                }
                break;
            case GameStatus.EMPTY:
                break;
        }

    }

    void ResizeWithTouch(GameObject myObj, int condition)
    {
        if (condition == 1)
        {
            myObj.transform.localScale -= new Vector3(-0.1f, 0.1f, 0);
            myObj.transform.position -= new Vector3(0, 0.1f, 0) / 2;
        }
        else
        {
            myObj.transform.localScale += new Vector3(-0.1f, 0.1f, 0);
            myObj.transform.position += new Vector3(0, 0.1f, 0) / 2;
        }
    }
    void JellyWayPropertyZ()
    {
        if (GameStatusEnum == GameStatus.STAY)
        {
            if (_jellyController.Hit.distance > 0.2f)
            {
                jellyWay.gameObject.SetActive(true);
                jellyWay.transform.localScale = new Vector3(jellyWay.transform.localScale.x, jellyWay.transform.localScale.y, (_jellyController.Hit.distance - _excessWidth) + jellyCopy.transform.localScale.z);
                jellyWay.transform.position = jelly.transform.position + jelly.transform.forward * (_jellyController.Hit.distance + (jelly.transform.localScale.z - jellyCopy.transform.localScale.z * 2)) / 2;
            }
            else
            {
                jellyWay.gameObject.SetActive(false);
            }
        }

    }
    private void ChangeObstacleMat(string _color)
    {
        if (_color == "green")
        {
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("ObstacleTag").Length; i++)
            {
                GameObject.FindGameObjectsWithTag("ObstacleTag")[i].GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }
        else
        {
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("ObstacleTag").Length; i++)
            {
                GameObject.FindGameObjectsWithTag("ObstacleTag")[i].GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }


    }

    private void OnFingerDown(LeanFinger obj)
    {
        _beginPos = obj.ScreenPosition;
    }
    private void OnFingerSet(LeanFinger obj)
    {
        if (GameStatusEnum == GameStatus.STAY || GameStatusEnum == GameStatus.STARSTAY)
        {
            if (_beginPos.y > obj.ScreenPosition.y && jelly.transform.localScale.y > 0.2f)
            {
                ResizeWithTouch(jelly.gameObject, 1);
                ResizeWithTouch(jellyCopy.gameObject, 1);
                ResizeWithTouch(jellyWay.gameObject, 1);
            }
            else if (_beginPos.y < obj.ScreenPosition.y && jelly.transform.localScale.y < 1.8f)
            {
                ResizeWithTouch(jelly.gameObject, 2);
                ResizeWithTouch(jellyCopy.gameObject, 2);
                ResizeWithTouch(jellyWay.gameObject, 2);
            }
            _beginPos = obj.ScreenPosition;
        }
    }


}
