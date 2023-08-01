using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    //[Header("Effects")]
    //[SerializeField] private float minFov = 40;
    //[SerializeField] private float maxFov = 100;
    //[Header("Position")]
    //[SerializeField] private float moveSmoothness;
    //[SerializeField] private Vector3 endMoveOffset;
    //[Header("Rotation")]
    //[SerializeField] private float rotationSmoothness;
    //[SerializeField] private Vector3 startRotationOffset;
    //[SerializeField] private Vector3 endRotationOffset;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody car;
    [SerializeField] private Transform targetLook;

    private Camera cam;
    private Transform target;
    private Transform myTransform;
    //private Vector3 moveOffset;
    //private Vector3 rotationOffset;

    private void Awake()
    {
        myTransform = transform;
        cam = GetComponent<Camera>();
        target = car.transform;
    }

    //private void FixedUpdate()
    //{
    //    handleMovement();
    //    HandleRotation();
    //}

    private void FixedUpdate()
    {
        //HandleSpeed();
        Vector3 playerForward = (car.velocity + target.forward).normalized;
        myTransform.position = Vector3.Lerp(myTransform.position,
            target.position + target.TransformVector(offset)
            + playerForward * (-5f),
            speed * Time.deltaTime);
        myTransform.LookAt(targetLook);
    }

    //private void HandleSpeed()
    //{
    //    float lerpSpeed = 0; // car.GetNormalizedSpeed();
    //    cam.fieldOfView = Mathf.Lerp(minFov, maxFov, lerpSpeed);
    //    moveOffset = Vector3.Lerp(startMoveOffset, endMoveOffset, lerpSpeed);
    //    rotationOffset = Vector3.Lerp(startRotationOffset, endRotationOffset, lerpSpeed);
    //}

    //private void handleMovement()
    //{
    //    Vector3 targetPos = target.TransformPoint(moveOffset);

    //    myTransform.position = Vector3.Lerp(myTransform.position, targetPos, moveSmoothness);
    //}

    //private void HandleRotation()
    //{
    //    var direction = target.position - myTransform.position;
    //    Quaternion rot = Quaternion.LookRotation(direction + rotationOffset, Vector3.up);

    //    myTransform.rotation = Quaternion.Lerp(myTransform.rotation, rot, rotationSmoothness);
    //}
}
