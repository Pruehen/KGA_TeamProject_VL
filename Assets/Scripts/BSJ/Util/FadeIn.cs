using UnityEngine;
using BSJ;
using UnityEngine.UI;
public class FadeIn : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Image>().FadeIn(this, 5f);
    }
}
