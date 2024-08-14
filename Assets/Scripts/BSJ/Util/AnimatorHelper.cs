using UnityEngine;

//���� �ʿ��Ѱ�
//�ִϸ��̼��� ���ݿ��� ������ �������� �˻� �ʿ�
//g

public class AnimatorHelper : MonoBehaviour
{
    private int attackHash;

    private Animator _animator;

    private int prevHash;

    public int CurrentAnimTagHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        attackHash = Animator.StringToHash("Attack");
    }

    private void Update()
    {
        //Debug.Log("���۽��۽��۽��۽���");
        //var a = _animator.GetCurrentAnimatorClipInfo(0);
        //{
        //    foreach (var b in a)
        //    {
        //        Debug.LogFormat(b.clip.name);
        //    }
        //}
        //Debug.Log("��������������������");
        //Debug.Log("Ŭ�� ���� ī��Ʈ" + _animator.GetCurrentAnimatorClipInfoCount(0));
        //Debug.Log("Ŭ�� ����" + _animator.GetCurrentAnimatorClipInfo(0)[0]);
        //Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).nameHash);

        CurrentAnimTagHash = _animator.GetCurrentAnimatorStateInfo(0).tagHash;

        int nameHash = _animator.GetAnimatorTransitionInfo(0).nameHash;
        if(prevHash != nameHash)
        {
            prevHash = nameHash;
        }

        var a = _animator.GetNextAnimatorStateInfo(0).nameHash;
        var b = _animator.GetCurrentAnimatorStateInfo(0).nameHash;

        Debug.Log(a +"  "+ b);

    }

    public bool IsAttackAnimTag()
    {
        bool r = _animator.GetCurrentAnimatorStateInfo(0).tagHash == attackHash;
        if (r)
            Debug.Log("!!!!!!!!!!!!!!");
        return r;
    }
}
