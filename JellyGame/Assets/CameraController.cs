using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void FixedUpdate()
    {
        //JellyObj Movement
        transform.Translate(0, 0, 0.05f);
    }
}
