using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;
using TMPro;

public class ShowKeyboard : MonoBehaviour
{
    private TMP_InputField _inputField;
    void Start()
    {
        _inputField = GetComponent<TMP_InputField>();
        _inputField.onSelect.AddListener(x=>OnSelect());
    }

    private void OnSelect()
    {
        if(GameManager.Instance.IsXREnabled())
        {
            NonNativeKeyboard.Instance.InputField = _inputField;
            NonNativeKeyboard.Instance.PresentKeyboard(_inputField.text);
        }
    }
}
