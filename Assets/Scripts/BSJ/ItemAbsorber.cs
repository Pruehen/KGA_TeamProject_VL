using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemAbsorber : MonoBehaviour
{
    [SerializeField] PlayerMaster _PlayerMaster;

    // Todo 석진 끌어당기는 클래스와 돌리는 클래스를 따로 생성하면 더 읽기 쉬울것 같다.
    // 실행은 update를 통해
    List<GameObject> items = new List<GameObject>();

    [Header("회전 관련")]
    [SerializeField] private float RevolveRadious = 5f;
    [SerializeField] private float RevolveSpeed = 30f;
    [SerializeField] private AnimationCurve RevolveSpeedCurve;

    [Header("획득 관련")]
    [SerializeField] private float Radious = 5f;
    [SerializeField] private float Height = 1f;
    [SerializeField] private float AbsolsionSpeed = 30f;
    [SerializeField] private AnimationCurve RadiusExpandCurve;


    //캐시
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
    /// 아이템 습득 시작
    /// 범위가 옴션에 따라 점점 늘어남
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
    /// 아이템 획등 성공시 호출
    /// 범위 내의 아이템을 삭제하고
    /// 아이템 갯수를 리턴
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

        _PlayerMaster._PlayerInstanteState.AcquireBullets(count);
        return count;
    }

    /// <summary>
    /// F1 과 F2 키를 사용해 디버그
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
        Rigidbody rb = other.attachedRigidbody;
        if(rb == null)
        {
            return;
        }
        if (IsItem(rb.gameObject))
        {
            if (items.Contains(rb.gameObject))
            {
                Debug.Log("어케 이미 들어가 있음;;");
            }
            else
            {
                items.Add(rb.gameObject);
                StartRevolve(rb.transform);
            }
        }
    }
    private void StartRevolve(Transform itemTr)
    {
        itemTr.transform.SetParent(transform, true);

        TrashItem item = itemTr.GetComponent<TrashItem>();

        item.DisablePhysics();

        StartCoroutine(PullToCenter(itemTr));
    }
    private IEnumerator PullToCenter(Transform item)
    {
        //회전 범위까지 점점 끌어당김
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
        foreach (GameObject item in items)
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
