using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public static void SmoothRotate(Transform transform, Quaternion targetRotation, float speed, float deltaTime)
    {
        transform.eulerAngles = Quaternion.Lerp(transform.rotation, targetRotation, Mathf.Clamp((speed) * deltaTime, 0f, .99f)).eulerAngles;
    }
}
