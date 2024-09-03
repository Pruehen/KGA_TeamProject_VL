using System;

[Serializable]
public class Timer
{
    public Action OnEnd;
    private float _time;
    private float _limit;
    private bool _isStop;
    public bool IsStop { get { return _isStop; } private set { _isStop = value; } }


    public void Init(float time, Action endCallback)
    {
        _limit = time;
        OnEnd += endCallback;
    }

    public void DoUpdate(float deltaTime)
    {
        if (_isStop)
            return;

        _time += deltaTime;

        if (_time > _limit)
        {
            _isStop = true;
            OnEnd?.Invoke();
        }
    }

    public void StartTimer()
    {
        IsStop = false;
        _time = 0f;
    }
}
