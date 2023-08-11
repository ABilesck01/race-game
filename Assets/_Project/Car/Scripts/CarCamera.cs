using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;
    [SerializeField] private float speed2;
    [SerializeField] private Rigidbody car;
    [SerializeField] private Transform targetLook;

    private Camera cam;
    private Transform target;
    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
        cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        CarStats.OnCarSpawn += CarStats_OnCarSpawn;
    }

    private void OnDisable()
    {
        CarStats.OnCarSpawn -= CarStats_OnCarSpawn;
    }

    private void CarStats_OnCarSpawn(object sender, CarStats.OnCarSpawnEventArgs e)
    {
        Debug.Log("car spawn");

        this.car = e.target.GetComponent<Rigidbody>();
        target = e.target;
        targetLook = e.lookAt;
    }

    private void FixedUpdate()
    {
        if (car == null) return;

        Vector3 playerForward = (car.velocity + target.forward).normalized;
        myTransform.position = Vector3.Lerp(myTransform.position,
            target.position + target.TransformVector(offset)
            + playerForward * (-speed2),
            speed * Time.deltaTime);

        myTransform.LookAt(targetLook);
    }
}
