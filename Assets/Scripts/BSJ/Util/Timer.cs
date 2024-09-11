using System;
using UnityEngine;

[Serializable]
public class Timer
{
    public Action OnEnd;
    private float _time;
    [SerializeField] private float _limit;
    [SerializeField] private bool _isPlaying = false;
    public bool IsPlaying { get { return _isPlaying; } private set { _isPlaying = value; } }


    public void Init(float time, Action endCallback)
    {
        _limit = time;
        OnEnd += endCallback;
    }

    public void DoUpdate(float deltaTime)
    {
        if (!IsPlaying)
            return;

        _time += deltaTime;

        if (_time > _limit)
        {
            IsPlaying = false;
            OnEnd?.Invoke();
        }
    }

    public void StartTimer()
    {
        IsPlaying = true;
        _time = 0f;
    }

    public void ResetTimer()
    {
        IsPlaying = false;
        _time = 0f;
    }
}
