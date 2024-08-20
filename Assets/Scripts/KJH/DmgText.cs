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
    }

    private void Update()
    {
        rect.SetUIPos_WorldToScreenPos(_originPos);
        _originPos += new Vector3(0, 2, 0) * Time.deltaTime;
        activeTime += Time.deltaTime;

        if(activeTime > 1)
        {
            ObjectPoolManager.Instance.EnqueueObject(this.gameObject);
        }
    }
}
