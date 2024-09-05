using EnumTypes;
using System;
using UnityEngine;

[Serializable]
public class BossDoubleAreaAttack : MonoBehaviour
{
    [SerializeField] float distance_middle = 15f;
    [SerializeField] float distance_max = 15f;
    [SerializeField] float damage = 60f;
    [SerializeField] float anticipateTime = 60f;
    [SerializeField] AttackRangeType condition = AttackRangeType.Close;
    [SerializeField] Timer timer;
    [SerializeField] Transform model_anticipate;
    [SerializeField] Material mat_anticipate;

    [SerializeField] Vector3 Pos;
    [SerializeField] int val;

    private void Awake()
    {
        mat_anticipate = model_anticipate.GetComponent<Renderer>().sharedMaterial;   
    }

    public void Init(AttackRangeType condition)
    {
        this.condition = condition;

        model_anticipate.localScale = Vector3.one * distance_max;

        mat_anticipate.SetFloat("_Float", distance_middle);

        if (condition == AttackRangeType.Close)
        {
            mat_anticipate.SetInt("_Inside", 1);
            condition = AttackRangeType.Far;
        }
        else
        {
            mat_anticipate.SetInt("_Inside", 0);
            mat_anticipate.SetFloat("InnerCircle", distance_middle);
            condition = AttackRangeType.Close;
        }
        timer.OnEnd = OnTimeEnd;
        timer.StartTimer();
    }
    private void OnEnable()
    {
        Init(condition);
    }

    private void Update()
    {
        timer.DoUpdate(Time.deltaTime);
    }

    private void OnTimeEnd()
    {
        AreaDamageToPlayer(condition);
        Flip();
        timer.StartTimer();
    }

    public void AreaDamageToPlayer(AttackRangeType range)
    {
        PlayerMaster target = PlayerMaster.Instance;

        float dist = Vector3.Distance(target.transform.position, transform.position);

        if (range == AttackRangeType.Close)
        {
            if (dist < distance_middle)
            {
                target.Hit(damage);
            }
        }
        else
        {
            if (dist > distance_middle)
            {
                target.Hit(damage);
            }
        }
    }

    private void Flip()
    {
        if (condition == AttackRangeType.Close)
        {
            condition = AttackRangeType.Far;
        }
        else
        {
            condition &= ~AttackRangeType.Close;
        }
    }
    private void OnValidate()
    {

        mat_anticipate.SetVector("_Pos", Pos);
        mat_anticipate.SetInt("_Inside", val);
        mat_anticipate.SetFloat("_Float", distance_middle);
    }
}
