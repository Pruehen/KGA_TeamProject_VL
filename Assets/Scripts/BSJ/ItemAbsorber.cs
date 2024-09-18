using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemAbsorber : MonoBehaviour
{
    /// <summary>
    /// ��� ���Ÿ���� ��� ���� ����ϰ� ���� ��ȯ
    /// �ٰŸ� ����� ���ο� ���°͸� ����ϰ� ���� ��ȯ
    /// </summary>


    [SerializeField] PlayerMaster _PlayerMaster;

    // Todo ���� ������� Ŭ������ ������ Ŭ������ ���� �����ϸ� �� �б� ����� ����.
    // ������ update�� ����
    List<TrashItem> absorbingItems = new List<TrashItem>();
    List<TrashItem> revorvingItems = new List<TrashItem>();

    [Header("��� ����")]
    [SerializeField] private float Radious = 5f;
    [SerializeField] private float Height = 1f;
    [SerializeField] private float AbsolsionSpeed = 30f;
    [Header("ȸ�� ����")]
    [SerializeField] private float RevolveRadious = 5f;
    [SerializeField] private float RevolveSpeed = 30f;
    [SerializeField] private AnimationCurve RevolveSpeedCurve;
    [Header("ȹ�� ����")]
    [SerializeField] private float AcquireSpeed = 30f;
    [SerializeField] private AnimationCurve RadiusExpandCurve;


    //ĳ��
    private CapsuleCollider _collider;
    private Coroutine _expendCoroutine;

    bool _isInit = false;

    public void Init(SO_Player playerData)
    {
        RevolveRadious = playerData.RevolveRadious;
        RevolveSpeed = playerData.RevolveSpeed;
        RevolveSpeedCurve = playerData.RevolveSpeedCurve;
        Radious = playerData.Radious;
        Height = playerData.Height;
        AbsolsionSpeed = playerData.AbsolsionSpeed;
        AcquireSpeed = playerData.AcquireSpeed;
        RadiusExpandCurve = playerData.RadiusExpandCurve;

        _collider = GetComponent<CapsuleCollider>();
        SetRadius(0f);
        SetHeight(Height);

        _PlayerMaster.Mod.OnActiveAbsorb += StartAbsorb;
        _PlayerMaster.Mod.OnSucceseRange += AcquireAll;
        _PlayerMaster.Mod.OnSucceseMelee += AcquireOnlyRevolve;
        _PlayerMaster.Mod.OnEndAbsorptState += DropAbsorbingItems;

        _isInit = true;
    }
    private float absorbTimeStamp = 0f;
    private void Update()
    {
        if (_isInit)
        {
            float timeFromAbsorb = Time.time - absorbTimeStamp;
            transform.Rotate(0, Time.deltaTime * RevolveSpeed * RevolveSpeedCurve.Evaluate(timeFromAbsorb), 0);
        }
    }

    public bool _isAbsorbing = false;
    /// <summary>
    /// ������ ���� ����
    /// ������ �ȼǿ� ���� ���� �þ
    /// </summary>
    public void StartAbsorb()
    {
        bool isAbsorbable =
            _expendCoroutine == null
            && Time.time - .5f >= absorbTimeStamp;
        if (isAbsorbable)
        {
            _isAbsorbing = true;
            _expendCoroutine = StartCoroutine(RadiusExpand());
            absorbTimeStamp = Time.time;
        }
    }
    /// <summary>
    /// ������ ȹ�� ������ ȣ��
    /// ���� ���� �������� �����ϰ�
    /// ������ ������ ����
    /// </summary>
    public int AcquireOnlyRevolve()
    {
        if (_isAbsorbing == false) return 0;
        _isAbsorbing = false;
        int count = revorvingItems.Count;
        ClearAcquireRadius();

        foreach (TrashItem item in revorvingItems)
        {
            item.PullToCenterAndDestroy(AcquireSpeed);
            absorbingItems.Remove(item);
        }
        revorvingItems.Clear();
        DropAbsorbingItems();


        return count;
    }
    public int AcquireAll()
    {
        if (_isAbsorbing == false) return 0;
        _isAbsorbing = false;

        int count = absorbingItems.Count;
        ClearAcquireRadius();

        foreach (TrashItem item in absorbingItems)
        {
            item.PullToCenterAndDestroy(AcquireSpeed);
        }
        foreach (TrashItem item in revorvingItems)
        {
            item.PullToCenterAndDestroy(AcquireSpeed);
        }

        return count;
    }
    private void ClearAcquireRadius()
    {
        if (_expendCoroutine != null)
        {
            StopCoroutine(_expendCoroutine);
            _expendCoroutine = null;
        }
        SetRadius(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null)
        {
            return;
        }
        if (IsAbourbableItem(rb.gameObject))
        {
            TrashItem trashItem = rb.gameObject.GetComponent<TrashItem>();
            if (absorbingItems.Contains(trashItem))
            {
                Debug.Log("���� �̹� �� ����;; Ʈ���� �ι��� �� ���� �ʿ�");
            }
            else
            {
                StartAbsorbe(trashItem);
                trashItem.OnRevolve += AddRevolve;
            }
        }
    }

    private bool IsAbourbableItem(GameObject gameObject)
    {
        TrashItem item;
        if(gameObject.TryGetComponent(out item))
        {
            if(item.IsAborbable())
            {
                return true;
            }
        }
        return false;
        
    }

    private void AddRevolve(TrashItem item)
    {
        revorvingItems.Add(item);
    }

    private void StartAbsorbe(TrashItem trashItem)
    {
        absorbingItems.Add(trashItem);
        trashItem.StartAbsorbe(transform);
        trashItem.PullToRevolve(RevolveRadious, AbsolsionSpeed);
    }

    private void DropAbsorbingItems()
    {
        ClearAcquireRadius();

        foreach (TrashItem item in absorbingItems)
        {
            item.DropItem();
            revorvingItems.Remove(item);
        }

        absorbingItems.Clear();
    }
    private void SetRadius(float radius)
    {
        _collider.enabled = true;
        _collider.radius = radius;
        if (radius <= 0f)
        {
            _collider.enabled = false;
        }
    }
    //Ŀ�긦 ����� ���� Ŀ�� �ڷ�ƾ ������ �̿��� ��������� üũ
    private IEnumerator RadiusExpand()
    {
        float time = 0;
        while (RadiusExpandCurve.keys[RadiusExpandCurve.length - 1].time > time)
        {
            time += Time.deltaTime;

            SetRadius(RadiusExpandCurve.Evaluate(time) * Radious);
            yield return null;
        }
        _expendCoroutine = null;
    }
    private void SetHeight(float height)
    {
        _collider.height = height;
    }
}
