using TMPro;
using UI.Extension;
using UnityEngine;

public class DmgText : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] TextMeshProUGUI text;
    float activeTime = 0;
    Vector3 _originPos;

    public void Init(float dmg, Vector3 originPos)
    {
        _originPos = originPos;

        activeTime = 0;
        text.text = dmg.ToString("0");
        transform.position = _originPos + Vector3.up * 1f;
    }

    private void Update()
    {
        _originPos += new Vector3(0, 2, 0) * Time.deltaTime;
        transform.position = _originPos;
        activeTime += Time.deltaTime;

        if(activeTime > 1)
        {
            ObjectPoolManager.Instance.EnqueueObject(this.gameObject);
        }

        transform.LookAt(Camera.main.transform);
        transform.Rotate(Vector3.up, 180);
        
    }
}
