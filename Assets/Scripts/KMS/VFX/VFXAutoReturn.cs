using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXAutoReturn : MonoBehaviour
{
    private ParticleSystem PS;
    public bool Tracking;
    public Transform target;

    private void Awake()
    {
        PS = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;  // 대상 위치로 VFX 위치 설정
        }
    }
    private void OnEnable()
    {
        // 파티클이 활성화될 때 루틴을 시작
        StartCoroutine(WaitForParticleEnd());
    }

    private IEnumerator WaitForParticleEnd()
    {
        // 파티클이 재생 중일 때까지 대기
        while (PS != null && PS.isPlaying)
        {
            yield return null; // 다음 프레임까지 대기
        }

        // 파티클 재생이 완료되면 오브젝트 풀로 반환
        ObjectPoolManager.Instance.EnqueueObject(gameObject);
    }
    public void SetTarget(Transform tg)
    {
        target = tg;  // Transform을 받아 target 설정
    }
}
