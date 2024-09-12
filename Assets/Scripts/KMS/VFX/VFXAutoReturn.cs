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
            transform.position = target.position;  // ��� ��ġ�� VFX ��ġ ����
        }
    }
    private void OnEnable()
    {
        // ��ƼŬ�� Ȱ��ȭ�� �� ��ƾ�� ����
        StartCoroutine(WaitForParticleEnd());
    }

    private IEnumerator WaitForParticleEnd()
    {
        // ��ƼŬ�� ��� ���� ������ ���
        while (PS != null && PS.isPlaying)
        {
            yield return null; // ���� �����ӱ��� ���
        }

        // ��ƼŬ ����� �Ϸ�Ǹ� ������Ʈ Ǯ�� ��ȯ
        ObjectPoolManager.Instance.EnqueueObject(gameObject);
    }
    public void SetTarget(Transform tg)
    {
        target = tg;  // Transform�� �޾� target ����
    }
}
