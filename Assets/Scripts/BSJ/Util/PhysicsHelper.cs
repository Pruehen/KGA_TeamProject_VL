using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHelper
{
    public static float Dist2D(Vector3 a, Vector3 b)
    {
        a.y = 0f;
        b.y = 0f;

        return Vector3.Distance(a, b);
    }
    public static float Dist2D(Transform a, Transform b)
    {
        return Dist2D(a.position, b.position);
    }
}
