using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyController : MonoBehaviour
{

    private RaycastHit _hit;
    [SerializeField] private LayerMask _layerMask;

    private void Start()
    {
     
    }
    private void FixedUpdate()
    {
        //JellyObj Movement
        //      transform.Translate(0, 0, 0.05f);
        RaycastToNextObstacle();


    }

    private RaycastHit RaycastToNextObstacle()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out _hit, 3000, _layerMask))
        {
            Debug.DrawLine(transform.position, _hit.point, Color.red);
            Debug.Log(_hit.collider.transform.parent.name);
        }
        return _hit;
    }

}
