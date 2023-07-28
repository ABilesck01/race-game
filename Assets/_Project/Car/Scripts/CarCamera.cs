using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private float minFov = 40;
    [SerializeField] private float maxFov = 100;
    [Header("Position")]
    [SerializeField] private float moveSmoothness;
    [SerializeField] private float rotationSmoothness;
    [Header("Rotation")]
    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private Vector3 rotationOffset;

    [SerializeField] private CarController car;

    private Camera cam;
    private Transform target;
    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
        cam = GetComponent<Camera>();
        target = car.transform;
    }

    private void FixedUpdate()
    {
        handleMovement();
        HandleRotation();
    }

    private void LateUpdate()
    {
        HandleCamera();
    }

    private void HandleCamera()
    {
        cam.fieldOfView = Mathf.Lerp(minFov, maxFov, car.GetNormalizedSpeed());
    }

    private void handleMovement()
    {
        Vector3 targetPos = target.TransformPoint(moveOffset);

        myTransform.position = Vector3.Lerp(myTransform.position, targetPos, moveSmoothness);
    }

    private void HandleRotation()
    {
        var direction = target.position - myTransform.position;
        Quaternion rot = Quaternion.LookRotation(direction + rotationOffset, Vector3.up);

        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, rot, rotationSmoothness);
    }
}
