using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    private Vector3 touchStart;
    Vector3 direction;
    public float cameraApertureLimit = 8;
    [SerializeField] private GameObject jelly;
    [SerializeField] private GameObject jellyCopy;
    [SerializeField] private GameObject jellyWay;
    //GameControl gameControlObj;

    private Vector3 beginPos;
    private Vector3 jellyScale;
    private Vector3 jellyCopyScale;
    private Vector3 jellyWayScale;

    void Start()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerSet += OnFingerSet;
        LeanTouch.OnFingerExpired += OnFingerExpired;
    }

    private void OnFingerExpired(LeanFinger obj)
    {

    }

    private void OnFingerSet(LeanFinger obj)
    {
        if (beginPos.y > obj.ScreenPosition.y)
        {
            if (jelly.transform.localScale.y > 0.2f)
            {
                jellyScale = jelly.transform.localScale - new Vector3(-0.1f, 0.1f, 0);
                jelly.transform.localScale = jellyScale;
                jelly.transform.position -= new Vector3(0, 0.1f, 0) / 2;

                jellyCopyScale = jellyCopy.transform.localScale - new Vector3(-0.1f, 0.1f, 0);
                jellyCopy.transform.localScale = jellyCopyScale;
                jellyCopy.transform.position -= new Vector3(0, 0.1f, 0) / 2;

                jellyWayScale = jellyWay.transform.localScale - new Vector3(-0.1f, 0.1f, 0);
                jellyWay.transform.localScale = jellyWayScale;
                jellyWay.transform.position -= new Vector3(0, 0.1f, 0) / 2;
            }

        }
        else if (beginPos.y < obj.ScreenPosition.y)
        {
            if (jelly.transform.localScale.y < 1.8f)
            {
                jellyScale = jelly.transform.localScale + new Vector3(-0.1f, 0.1f, 0);
                jelly.transform.localScale = jellyScale;
                jelly.transform.position += new Vector3(0, 0.1f, 0) / 2;

                jellyCopyScale = jellyCopy.transform.localScale + new Vector3(-0.1f, 0.1f, 0);
                jellyCopy.transform.localScale = jellyCopyScale;
                jellyCopy.transform.position += new Vector3(0, 0.1f, 0) / 2;

                jellyWayScale = jellyWay.transform.localScale + new Vector3(-0.1f, 0.1f, 0);
                jellyWay.transform.localScale = jellyWayScale;
                jellyWay.transform.position += new Vector3(0, 0.1f, 0) / 2;
            }
        }
        beginPos = obj.ScreenPosition;
    }

    private void OnFingerDown(LeanFinger obj)
    {
        beginPos = obj.ScreenPosition;
    }

    void Update()
    {



    }
}
