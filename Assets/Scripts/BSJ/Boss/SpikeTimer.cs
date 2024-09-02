using System;
using UnityEngine;

[Serializable]
public class SpikeTimer
{
    [SerializeField] private float _stopTime = 1f;

    private float _time = 0f;
    public Action OnTimerEnd { get; internal set; }
    private bool _isStop = true;
    public bool IsStop { get { return _isStop; } private set { _isStop = value; } }

    public void UpdateTimer(float deltaTime)
    {
        if (IsStop)
            return;

        if (_time <= _stopTime)
        {
            _time += deltaTime;
        }
        else
        {
            IsStop = true;
            OnTimerEnd?.Invoke();
        }

    }

    public void StartTimer()
    {
        IsStop = false;
    }
}
