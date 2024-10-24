using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;
using TMPro;

public class ShowKeyboard : MonoBehaviour
{
    private TMP_InputField _inputField;
    public ChatUI ChatUI;
    void Start()
    {
        _inputField = GetComponent<TMP_InputField>();
        _inputField.onSelect.AddListener(x=>OnSelect());
        NonNativeKeyboard.Instance.OnTextSubmitted += (sender, e) => OnTextSubmitted();
    }

    private void OnSelect()
    {
        if(GameManager.Instance.IsXREnabled())
        {
            NonNativeKeyboard.Instance.InputField = _inputField;
            NonNativeKeyboard.Instance.PresentKeyboard(_inputField.text);
        }
    }

    private void OnTextSubmitted()
    {
        ChatUI.OnSubmit_ChatMsg();
    }
}
