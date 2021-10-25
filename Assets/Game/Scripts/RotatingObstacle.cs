using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatingObstacle : MonoBehaviour
{
    [SerializeField] Transform RotatingObject;
    [SerializeField] bool isYROtate;
    [SerializeField] bool isXRotate;
    [SerializeField] bool isZRotate;
    [SerializeField] float rotateSpeed;

    void Update()
    {
        if (isXRotate)
        {
            RotatingObject.Rotate(rotateSpeed*Time.deltaTime,0,0);
        }else if (isYROtate)
        {
            RotatingObject.Rotate(0,rotateSpeed*Time.deltaTime,0);
        }else if (isZRotate)
        {
            RotatingObject.Rotate(0,0,rotateSpeed*Time.deltaTime);
        }
    }
}
