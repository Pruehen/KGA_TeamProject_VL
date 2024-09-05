using EnumTypes;
using System;
using UnityEngine;

[Serializable]
public class BossDoubleAreaAttack : MonoBehaviour
{
    [SerializeField] float _distance_middle = 15f;
    [SerializeField] float _distance_max = 15f;
    [SerializeField] float _damage = 60f;
    [SerializeField] float _anticipateTime = 60f;
    [SerializeField] AttackRangeType _condition = AttackRangeType.Close;
    [SerializeField] Timer _timer;
    [SerializeField] Transform _model_anticipate;
    [SerializeField] Material _mat_anticipate;

    [SerializeField] Vector3 _pos;
    [SerializeField] bool _inside;

    [SerializeField] int _attackCount;
    int _curAttackCount;

    private void Awake()
    {
        _mat_anticipate = _model_anticipate.GetComponent<Renderer>().sharedMaterial;
    }

    public void Init(AttackRangeType condition)
    {
        this._condition = condition;

        _model_anticipate.localScale = Vector3.one * _distance_max * 2f;

        _mat_anticipate.SetFloat("_Float", _distance_middle);

        _mat_anticipate.SetVector("_Pos", transform.position);

        if (condition == AttackRangeType.Close)
        {
            _mat_anticipate.SetInt("_Inside", 1);
            condition = AttackRangeType.Far;
        }
        else
        {
            _mat_anticipate.SetInt("_Inside", 0);
            _mat_anticipate.SetFloat("InnerCircle", _distance_middle);
            condition = AttackRangeType.Close;
        }
        _timer.OnEnd += OnTimeEnd;
        _timer.StartTimer();
    }
    private void OnDisable()
    {
        _timer.ResetTimer();
    }
    private void Update()
    {
        _timer.DoUpdate(Time.deltaTime);
    }

    private void OnTimeEnd()
    {
        _curAttackCount++;
        if(_curAttackCount >= _attackCount)
        {
            _timer.ResetTimer();
            _timer.OnEnd = null;
            gameObject.SetActive(false);
            return;
        }
        AreaDamageToPlayer(_condition);
        Flip();
        _timer.StartTimer();
    }

    public void AreaDamageToPlayer(AttackRangeType range)
    {
        PlayerMaster target = PlayerMaster.Instance;

        Vector3 a = target.transform.position;
        Vector3 b = transform.position - a;
        b.y = 0f;

        float dist = b.magnitude;

        if (range == AttackRangeType.Close)
        {
            if (dist < _distance_middle)
            {
                target.Hit(_damage);
            }
        }
        else
        {
            if (dist > _distance_middle)
            {
                target.Hit(_damage);
            }
        }
    }

    private void Flip()
    {
        if (_condition == AttackRangeType.Close)
        {
            _condition = AttackRangeType.Far;
            _mat_anticipate.SetInt("_Inside", 0);
        }
        else
        {
            _condition = AttackRangeType.Close;
            _mat_anticipate.SetInt("_Inside", 1);
        }
    }
}
