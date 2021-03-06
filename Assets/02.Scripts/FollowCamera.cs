using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 vec = new Vector3(0f, -1f, 0f);
    private Transform cameraTransform;

    [Range(0.0f, 20.0f)]
    public float distance = 10.0f;

    [Range(0.0f, 10.0f)]
    public float height = 2.0f;

    public float moveDamping = 15f;
    public float rotateDamping = 10f;

    public float targetOffset = 2.0f;

    void Start()
    {
        cameraTransform = GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 pos = targetTransform.position
                      + (-targetTransform.forward * distance)
                      + (Vector3.up * height)
                      +vec;
        cameraTransform.position = Vector3.Slerp(cameraTransform.position, pos, moveDamping * Time.deltaTime);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetTransform.rotation, rotateDamping * Time.deltaTime);

        cameraTransform.LookAt(targetTransform.position + (targetTransform.up * targetOffset));
    }
}
