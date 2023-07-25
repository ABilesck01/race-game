using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private float translateSpeed;
    [SerializeField] private float rotationSpeed;

    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    private void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
    }

    private void HandleTranslation()
    {
        var targetPosition = target.TransformPoint(offset);
        myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, translateSpeed * Time.deltaTime);
    }
    private void HandleRotation()
    {
        var direction = target.position - myTransform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
