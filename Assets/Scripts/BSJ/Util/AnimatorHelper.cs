using UnityEngine;

//현재 필요한것
//애니메이션이 공격에서 완전히 나갔는지 검사 필요
//g

public class AnimatorHelper : MonoBehaviour
{
    ////private int attackHash;

    ////private Animator _animator;

    ////private int prevHash;

    ////public int CurrentAnimTagHash;

    ////private void Awake()
    ////{
    ////    _animator = GetComponent<Animator>();
    ////    attackHash = Animator.StringToHash("Attack");
    ////}

    ////private void Update()
    ////{
    ////    //Debug.Log("시작시작시작시작시작");
    ////    //var a = _animator.GetCurrentAnimatorClipInfo(0);
    ////    //{
    ////    //    foreach (var b in a)
    ////    //    {
    ////    //        Debug.LogFormat(b.clip.name);
    ////    //    }
    ////    //}
    ////    //Debug.Log("끝끝끝끝끝끝끝끝끝끝");
    ////    //Debug.Log("클립 인포 카운트" + _animator.GetCurrentAnimatorClipInfoCount(0));
    ////    //Debug.Log("클립 인포" + _animator.GetCurrentAnimatorClipInfo(0)[0]);
    ////    //Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).nameHash);

    ////    CurrentAnimTagHash = _animator.GetCurrentAnimatorStateInfo(0).tagHash;

    ////    int nameHash = _animator.GetAnimatorTransitionInfo(0).nameHash;
    ////    if (prevHash != nameHash)
    ////    {
    ////        prevHash = nameHash;
    ////    }

    ////    var a = _animator.GetNextAnimatorStateInfo(0).nameHash;
    ////    var b = _animator.GetCurrentAnimatorStateInfo(0).nameHash;

    ////    Debug.Log(a + "  " + b);

    ////}

    ////public bool IsAttackAnimTag()
    ////{
    ////    bool r = _animator.GetCurrentAnimatorStateInfo(0).tagHash == attackHash;
    ////    if (r)
    ////        Debug.Log("!!!!!!!!!!!!!!");
    ////    return r;
    ////}

    //current state next state 는 따로다
    public static bool IsAnimationPlaying(Animator animator, int layer, string fullPath)
    {
        return animator.GetCurrentAnimatorStateInfo(layer).fullPathHash == Animator.StringToHash(fullPath);
    }
    public static bool IsAnimationPlaying_Tag(Animator animator, int layer, string tag)
    {
        return animator.GetCurrentAnimatorStateInfo(layer).tagHash == Animator.StringToHash(tag);
    }

    //알고싶은 애니메이션이 현재 트랜지션되어 입장하고있는시점부터 나가기 시작하는시점까지 True
    public static bool IsOnlyAnimationPlaying(Animator animator, int layer, string fullPath)
    {
        int target = Animator.StringToHash(fullPath);
        int cur = animator.GetCurrentAnimatorStateInfo(layer).fullPathHash;
        int next = animator.GetNextAnimatorStateInfo(layer).fullPathHash;
        if (target == next)
        {
            return true;
        }
        if (target == cur && next == 0)
        {
            return true;
        }
        return false;
    }
    public static bool IsTagedAnimPlaying(Animator animator, int layer, string tag)
    {
        bool iscurr= animator.GetCurrentAnimatorStateInfo(layer).IsTag(tag);
        bool isnext= animator.GetNextAnimatorStateInfo(layer).IsTag(tag);
        if(iscurr || isnext)
        {
            return true;
        }
        return false;
    }
}
