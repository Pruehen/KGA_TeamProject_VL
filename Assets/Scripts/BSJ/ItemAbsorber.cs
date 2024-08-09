using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemAbsorber : MonoBehaviour
{
    /// <summary>
    /// ��� ���Ÿ���� ��� ���� ����ϰ� ���� ��ȯ
    /// �ٰŸ������ ���ο� ���°͸� ����ϰ� ���� ��ȯ
    /// </summary>


    [SerializeField] PlayerMaster _PlayerMaster;

    // Todo ���� ������� Ŭ������ ������ Ŭ������ ���� �����ϸ� �� �б� ����� ����.
    // ������ update�� ����
    List<GameObject> absorbingItems = new List<GameObject>();
    List<GameObject> revorvingItems = new List<GameObject>();
    List<Coroutine> absorbingCoroutines = new List<Coroutine>();

    [Header("ȸ�� ����")]
    [SerializeField] private float RevolveRadious = 5f;
    [SerializeField] private float RevolveSpeed = 30f;
    [SerializeField] private AnimationCurve RevolveSpeedCurve;

    [Header("ȹ�� ����")]
    [SerializeField] private float Radious = 5f;
    [SerializeField] private float Height = 1f;
    [SerializeField] private float AbsolsionSpeed = 30f;
    [SerializeField] private AnimationCurve RadiusExpandCurve;


    //ĳ��
    private CapsuleCollider _collider;
    private Coroutine _expendCoroutine;

    bool _isInit = false;

    public void Init()
    {
        _collider = GetComponent<CapsuleCollider>();
        SetRadius(0f);
        SetHeight(Height);
        _PlayerMaster.Register_PlayerModChangeManager(StartAbsorb, AcquireOnlyRevolve, AcquireAll, DropAbsorbingItems);
        _isInit = true;
    }
    private float absorbTimeStamp = 0f;
    private void Update()
    {
        if (_isInit)
        {
            Test();

            float timeFromAbsorb = Time.time - absorbTimeStamp;
            transform.Rotate(0, Time.deltaTime * RevolveSpeed * RevolveSpeedCurve.Evaluate(timeFromAbsorb), 0);
        }
    }
    /// <summary>
    /// ������ ���� ����
    /// ������ �ȼǿ� ���� ���� �þ
    /// </summary>
    public void StartAbsorb()
    {
        if (_expendCoroutine == null)
        {
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
        int count = revorvingItems.Count;
        ClearAcquireRadius();

        foreach (GameObject item in revorvingItems)
        {
            StartCoroutine(PullToCenterAndDestroy(item.transform));
        }

        DropAbsorbingItems();


        return count;
    }
    public int AcquireAll()
    {
        int count = absorbingItems.Count;
        ClearAcquireRadius();

        foreach (GameObject item in absorbingItems)
        {
            StartCoroutine(PullToCenterAndDestroy(item.transform));
        }
        foreach (GameObject item in revorvingItems)
        {
            StartCoroutine(PullToCenterAndDestroy(item.transform));
        }

        return count;
    }
    /// <summary>
    /// F1 �� F2 Ű�� ����� �����
    /// </summary>
    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartAbsorb();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            AcquireOnlyRevolve();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            AcquireAll();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            DropAbsorbingItems();
        }
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
        if (IsItem(rb.gameObject))
        {
            if (absorbingItems.Contains(rb.gameObject))
            {
                Debug.Log("���� �̹� �� ����;; Ʈ���� �ι��� �� ���� �ʿ�");
            }
            else
            {
                absorbingItems.Add(rb.gameObject);
                StartAbsorbe(rb.transform);
            }
        }
    }
    private void StartAbsorbe(Transform itemTr)
    {

        itemTr.transform.SetParent(transform, true);
        TrashItem item = itemTr.GetComponent<TrashItem>();

        item.DisablePhysics();

        absorbingCoroutines.Add(StartCoroutine(PullToRevolve(itemTr)));
    }
    private IEnumerator PullToRevolve(Transform item)
    {
        while (true)
        {
            Vector3 targetPos = item.localPosition;
            targetPos = targetPos.normalized * RevolveRadious;
            if (Vector3.Distance(targetPos, item.localPosition) <= 0.1f)
            {
                break;
            }
            item.localPosition = Vector3.Lerp(item.localPosition, targetPos, 0.02f);
            yield return null;
        }
        revorvingItems.Add(item.gameObject);
        absorbingItems.Remove(item.gameObject);
        Debug.Log("EndOfAbsolsion");
    }
    private IEnumerator PullToCenterAndDestroy(Transform item)
    {
        //ȸ�� �������� ���� ������
        while (true)
        {
            Vector3 targetPos = Vector3.zero;
            if (Vector3.Distance(targetPos, item.localPosition) <= 0.1f)
            {
                break;
            }
            item.localPosition = Vector3.Lerp(item.localPosition, targetPos, 0.02f);
            yield return null;
        }
        revorvingItems.Remove(item.gameObject);
        Destroy(item.gameObject);
        Debug.Log("EndOfAbsolsion");
    }

    private void DropAbsorbingItems()
    {
        foreach (GameObject item in absorbingItems)
        {
            item.GetComponent<TrashItem>().EnablePhysics();
            item.transform.SetParent(null, true);
        }
        foreach (Coroutine coroutine in absorbingCoroutines)
        {
            StopCoroutine(coroutine);
        }

        absorbingItems.Clear();
    }
    private Vector3 GetDirItemToPlayer(Transform item)
    {
        return (item.position - transform.position).normalized;
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
    private bool IsItem(GameObject gameObject)
    {
        if (gameObject.CompareTag("Item"))
        {
            return true;
        }
        return false;
    }
}
