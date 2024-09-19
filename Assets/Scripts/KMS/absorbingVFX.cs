using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class absorbingVFX : MonoBehaviour
{
    ParticleSystem _particleSystem;

    public float startSize = 0f; // 초기 scale 값
    public float endSize = 6f; // 목표 scale 값
    public float speed = 1.5f; // 변화 속도
    private float currntSize;

    private void OnEnable()
    {
        currntSize = startSize; // currntSize를 startSize로 초기화
        transform.localScale = Vector3.one * currntSize; // 초기 scale 설정
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
            transform.localScale = Vector3.one * currntSize; // scale 업데이트
            yield return null; // 다음 프레임까지 대기
        }
    }
}