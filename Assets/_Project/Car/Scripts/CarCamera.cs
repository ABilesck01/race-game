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
    [SerializeField] private float speed2;
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
    }

    private void Start()
    {
        if(car != null)
            SetCar(car);
    }

    //private void FixedUpdate()
    //{
    //    handleMovement();
    //    HandleRotation();
    //}

    private void FixedUpdate()
    {
        //HandleSpeed();

        if (car == null) return;

        Vector3 playerForward = (car.velocity + target.forward).normalized;
        myTransform.position = Vector3.Lerp(myTransform.position,
            target.position + target.TransformVector(offset)
            + playerForward * (-speed2),
            speed * Time.deltaTime);

        myTransform.LookAt(targetLook);
    }

    public void SetCar(Rigidbody car)
    {
        this.car = car;
        target = car.transform;
        targetLook = target;

    }
}
