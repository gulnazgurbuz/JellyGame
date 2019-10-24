using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _jellyObj;
    private Transform _parent;
    Vector3 cameraPosition;

    private void Start()
    {
        _jellyObj = GameObject.Find("JellyObj").transform;
        _parent = transform.parent;
    }
    void FixedUpdate()
    {
        _parent.transform.position = _jellyObj.position;
        _parent.transform.rotation = _jellyObj.rotation;
    }
}
