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
            transform.position = target.position;  // 대상 위치로 VFX 위치 설정
        }
    }

    public void SetTarget(Transform tg)
    {
        target = tg;  // Transform을 받아 target 설정
    }
}
