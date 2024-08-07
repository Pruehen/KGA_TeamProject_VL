using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemAbsorber : MonoBehaviour
{
    // Todo ���� ������� Ŭ������ ������ Ŭ������ ���� �����ϸ� �� �б� ����� ����.
    // ������ update�� ����
    List<GameObject> items = new List<GameObject>();

    [Header ("ȸ�� ����")]
    [SerializeField] private float RevolveRadious = 5f;
    [SerializeField] private float RevolveSpeed = 30f;
    [SerializeField] private AnimationCurve RevolveSpeedCurve;

    [Header("ȹ�� ����")]
    [SerializeField] private float Radious = 5f;
    [SerializeField] private float Height = 1f;
    [SerializeField] private float AbsolsionSpeed = 30f;
    [SerializeField] private AnimationCurve RadiusExpandCurve;
    //��

    //ĳ��
    private CapsuleCollider _collider;
    private Coroutine _expendCoroutine;
    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        SetRadius(0f);
        SetHeight(Height);
    }

    private float absorbTimeStamp = 0f;
    private void Update()
    {
        Test();
        
        float timeFromAbsorb = Time.time - absorbTimeStamp;
        transform.Rotate(0, Time.deltaTime * RevolveSpeed * RevolveSpeedCurve.Evaluate(timeFromAbsorb), 0);
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
    public int SucceseAbsorb()
    {
        int count = items.Count;
        if (_expendCoroutine != null)
        {
            StopCoroutine(_expendCoroutine);
            _expendCoroutine = null;
        }
        SetRadius(0f);
        DestroyAllItem();

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
            SucceseAbsorb();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsItem(other.gameObject))
        {
            if (items.Contains(other.gameObject))
            {
                Debug.Log("���� �̹� �� ����;;");
            }
            else
            {
                items.Add(other.gameObject);
                StartRevolve(other.transform);
            }
        }
    }
    private void StartRevolve(Transform item)
    {
        item.transform.SetParent(transform, true);

        Collider collider = item.GetComponent<Collider>();
        Rigidbody rigidbody = collider.GetComponent<Rigidbody>();

        collider.isTrigger = true;
        rigidbody.isKinematic = true;

        StartCoroutine(PullToCenter(item));
    }
    private IEnumerator PullToCenter(Transform item)
    {
        //ȸ�� �������� ���� ������
        //while (Vector3.Distance(item.transform.position, transform.position) >= RevolveRadious)
        //{
        //    item.transform.position -= DirectionItemToPlayer(item) * Time.deltaTime * AbsolsionSpeed;
        //    yield return null;
        //}
        while (true)
        {

            Vector3 targetPos = item.localPosition;
            targetPos = targetPos.normalized * RevolveRadious;
            if (Vector3.Distance(targetPos, item.localPosition) <= 0.1f)
            {
                break;
            }
            item.localPosition = Vector3.Lerp(item.localPosition, targetPos, 0.002f);

            item.transform.position -= DirectionItemToPlayer(item) * Time.deltaTime * AbsolsionSpeed;
            yield return null;
        }
        Debug.Log("EndOfAbsolsion");
    }


    private Vector3 DirectionItemToPlayer(Transform item)
    {
        return (item.position - transform.position).normalized;
    }
    private Vector3 DirectionItemToPlayer2D(Transform item)
    {
        Vector3 dir = DirectionItemToPlayer(item);
        dir = new Vector3(dir.x, 0f, dir.z).normalized;
        return dir;
    }


    private float GetCurrentItemAmount()
    {
        return items.Count;
    }
    private void DestroyAllItem()
    {
        foreach(GameObject item in items)
        {
            Destroy(item);
        }
        items.Clear();
        StopAllCoroutines();
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
    private void ClearRadius()
    {
        SetRadius(0f);
    }
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
