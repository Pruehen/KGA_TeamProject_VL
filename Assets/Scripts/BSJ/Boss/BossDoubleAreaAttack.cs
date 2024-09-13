using EnumTypes;
using System;
using UnityEngine;

[Serializable]
public class BossDoubleAreaAttack : MonoBehaviour
{
    [SerializeField] float _distance_middle = 15f;
    [SerializeField] float _distance_max = 15f;
    [SerializeField] float _damage = 60f;
    [SerializeField] AttackRangeType _condition = AttackRangeType.Close;
    [SerializeField] Transform _model_anticipate;
    [SerializeField] Material _mat_anticipate;

    [SerializeField] Vector3 _pos;
    [SerializeField] bool _inside;

    [SerializeField] int _attackCount = 2;
    int _curAttackCount;
    [SerializeField] LayerMask _targetLayer;

    private void Awake()
    {
        _mat_anticipate = _model_anticipate.GetComponent<Renderer>().sharedMaterial;
    }

    public void Init(float damage, float sizeMid, AttackRangeType inside)
    {
        this._condition = inside;
        _distance_middle = sizeMid;
        _damage = damage;

        _model_anticipate.localScale = Vector3.one * _distance_max * 2f;

        _mat_anticipate.SetFloat("_Float", _distance_middle);

        _mat_anticipate.SetVector("_Pos", transform.position);

        if (inside == AttackRangeType.Close)
        {
            _mat_anticipate.SetInt("_Inside", 1);
            inside = AttackRangeType.Far;
        }
        else
        {
            _mat_anticipate.SetInt("_Inside", 0);
            _mat_anticipate.SetFloat("InnerCircle", _distance_middle);
            inside = AttackRangeType.Close;
        }
    }

    public void AreaDamageToPlayer(AttackRangeType range)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _distance_max, _targetLayer);
        if(cols.Length == 0)
            return;

        foreach (Collider targetCol in cols)
        {
            if (!targetCol.TryGetComponent(out ITargetable target))
                continue;
            Vector3 a = target.GetPosition();
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

    public void Trigger()
    {
        _curAttackCount++;
        AreaDamageToPlayer(_condition);
        if (_curAttackCount >= _attackCount)
        {
            gameObject.SetActive(false);
            return;
        }
        Flip();
    }
}
