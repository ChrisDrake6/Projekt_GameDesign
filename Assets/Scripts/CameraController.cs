using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }

    public float speed = 10;
    Vector3 currentTargetPosition;

    public CameraController()
    {
        instance = this;
    }

    private void Start()
    {
        currentTargetPosition = transform.position;
    }

    void Update()
    {
        if (transform.position != currentTargetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, speed * Time.deltaTime);
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentTargetPosition = targetPosition;
    }
}
