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

    public void Init(AttackRangeType condition)
    {
        this.condition = condition;

        model_anticipate.localScale = Vector3.one * distance_max;


        if (condition == AttackRangeType.Close)
        {
            mat_anticipate.SetFloat("InnerCircle" ,0f);
            condition = AttackRangeType.Far;
        }
        else
        {
            mat_anticipate.SetFloat("InnerCircle", distance_middle);
            condition = AttackRangeType.Close;
        }
    }
    public void EnableAnticipater()
    {

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
            if(dist > distance_middle)
            {
                target.Hit(damage);
            }
        }
    }
}
