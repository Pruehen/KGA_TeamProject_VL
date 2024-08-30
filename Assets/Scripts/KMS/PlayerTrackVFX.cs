using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackVFX : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;  // ��� ��ġ�� VFX ��ġ ����
        }
    }

    public void SetTarget(Transform tg)
    {
        target = tg;  // Transform�� �޾� target ����
    }
}
