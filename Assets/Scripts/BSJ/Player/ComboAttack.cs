//using System;
//using System.Collections;
//using UnityEngine;

//[Serializable]
//public class ComboAttack
//{
//    private float _lastTryTime;
//    private float _comboEndTime;
//    private int _attackIndex;
//    private int _attackCount;
//    private MonoBehaviour _owner;

//    Coroutine _endCombo;

//    private Animator _animator;
//    /// <summary>
//    /// 어택이 가능하면 공격합니다
//    /// 실패할 수 도 있습니다
//    /// 쿨타임 공격중 등등
//    /// </summary>
//    public void TryAttack()
//    {
//        if (Time.time - _comboEndTime > 0.5f && _attackIndex < _attackCount)
//        {
//            _owner.StopCoroutine(_endCombo);

//            if(Time.time - _lastTryTime >= 0.2f)
//            {
//                _animator.SetInteger("AttackIndex", _attackIndex);
//                _animator.SetTrigger("Attack");
//                _attackIndex++;
//                _lastTryTime = Time.time;

//                if(_attackIndex >= _attackCount)
//                {
//                    _attackIndex = 0;
//                }
//            }
//        }
//    }


//    private AnimatorStateInfo GetAnimatorStateInfo()
//    {
//        return _animator.GetCurrentAnimatorStateInfo(0);
//    }
//    private float GetCurrentStateNormalizedTime()
//    {
//        return GetAnimatorStateInfo().normalizedTime;
//    }

//    private void ExitAttack()
//    {
//        if (GetCurrentStateNormalTime() > 0.9f
//           &&
//           GetAnimatorStateInfo().IsTag("Attack"))
//        {
//            _owner.StartCoroutine(EndCombo());
//        }
//    }

//    private IEnumerator EndCombo()
//    {
//        yield return new WaitForSeconds(1.0f);
//        _attackIndex = 0;
//        _comboEndTime = Time.time;
       
//    }

//    private float GetCurrentStateNormalTime()
//    {
//        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
//    }

//}
