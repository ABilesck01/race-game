using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [SerializeField] private float moveSmoothness;
    [SerializeField] private float rotationSmoothness;

    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private Vector3 rotationOffset;

    [SerializeField] private Transform target;

    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    private void FixedUpdate()
    {
        handleMovement();
        HandleRotation();
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
