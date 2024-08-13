using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileCalc
{

    public static Vector3 CalculateInitialVelocity(Transform target, Transform origin, float initialSpeed, Vector3 offset)
    {
        return (-origin.position + (target.position + offset)).normalized * initialSpeed;
    }

    public static Vector3 CalcLaunch(Vector3 origin, Vector3 target, float angle)
    {
        float g = Physics.gravity.magnitude;
        Vector3 dirToTarget = -origin + target;
        float distH = new Vector3(dirToTarget.x, 0f, dirToTarget.z).magnitude;
        float distV = dirToTarget.y;
        float D = distH + distV;
        float angleV = angle * Mathf.Deg2Rad;

        float v0 = Mathf.Sqrt((D * g) / (Mathf.Sin(2f * angleV)));


        dirToTarget.y = 0f;
        dirToTarget = dirToTarget.normalized;

        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget);
        Quaternion additionalAngle = Quaternion.Euler(angle, 0f, 0f);
        Vector3 lauchDir = (lookRotation * additionalAngle) * Vector3.forward;
        lauchDir.y = -lauchDir.y;
        Vector3 res = lauchDir * v0;
        if (float.IsNaN(res.x))
            return Vector3.zero;
        return res;
    }

    public static void Homming(Rigidbody projectile, Vector3 target, float hommingPower)
    {
        Vector3 targetDir = (-projectile.position + target).normalized;
        Vector3 targetDirH = targetDir;
        targetDirH.y = 0f;
        targetDirH = targetDirH.normalized;

        Vector3 velocityH = projectile.velocity;
        velocityH.y = 0f;
        float velocityHMag = velocityH.magnitude;

        Vector3 newVelocityH = targetDirH * velocityHMag;

        if (Vector3.Dot(velocityH, targetDirH) >= .9f)
        {
            Vector3 result = new Vector3(newVelocityH.x, projectile.velocity.y, newVelocityH.z);

            projectile.velocity = Vector3.Lerp(projectile.velocity, result, Time.deltaTime * hommingPower);
        }
    }
    public static void Homming(Rigidbody projectile, Transform target, float hommingPower)
    {
        Homming(projectile, target.position, hommingPower);
    }
}
