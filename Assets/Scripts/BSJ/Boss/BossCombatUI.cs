using EnumTypes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BossCombatUI : MonoBehaviour
{
    [SerializeField] Image _healthPoint;
    [SerializeField] Text _bossName;

    [SerializeField] EnemyBase _enemyBase;

    public void Init(EnemyBase target)
    {
        _enemyBase = target;

        gameObject.SetActive(true);
        _healthPoint.fillAmount = target.Health[0].GetHpRatio();

        for (int i = 0; i < target.Health.Length; i ++)
        {
            target.Health[i].OnDamaged += UpdateHealth;
            target.Health[i].OnHeal += UpdateHealth;
        }

        target.Health[0].OnDead += InitNextHp;
        target.Health[target.Health.Length - 1].OnDead += Clear;
    }

    private void Clear(Combat target)
    {
        gameObject.SetActive(false);
    }

    private void UpdateHealth(Combat target, DamageType type)
    {
        UpdateHealth(target);
    }
    private void UpdateHealth(Combat target)
    {
        if (target.IsDead())
            return;
        _healthPoint.fillAmount = target.GetHpRatio();
    }
    private void InitNextHp(Combat target)
    {
        _healthPoint.fillAmount = _enemyBase.Health[1].GetHpRatio();
    }
}
