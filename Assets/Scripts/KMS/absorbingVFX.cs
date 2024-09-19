using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class absorbingVFX : MonoBehaviour
{
    ParticleSystem _particleSystem;

    public float startSize = 0f; // �ʱ� scale ��
    public float endSize = 6f; // ��ǥ scale ��
    public float speed = 1.5f; // ��ȭ �ӵ�
    private float currntSize;

    private void OnEnable()
    {
        currntSize = startSize; // currntSize�� startSize�� �ʱ�ȭ
        transform.localScale = Vector3.one * currntSize; // �ʱ� scale ����
        StartCoroutine(ScaleUp());
    }
    private void OnDisable()
    {
        StopCoroutine(ScaleUp());
    }
    IEnumerator ScaleUp()
    {
        float elapsedTime = 0f;

        while (currntSize < endSize)
        {
            elapsedTime += Time.deltaTime;
            currntSize = Mathf.Lerp(startSize, endSize, elapsedTime / speed);
            transform.localScale = Vector3.one * currntSize; // scale ������Ʈ
            yield return null; // ���� �����ӱ��� ���
        }
    }
}