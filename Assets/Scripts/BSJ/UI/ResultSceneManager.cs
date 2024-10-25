using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class ResultSceneManager : MonoBehaviour
{
    [SerializeField] GameObject _startButton;
    [SerializeField] InputManager _inputManager;
    [SerializeField] Text _resultTitle;
    [SerializeField] Text _resultCurrency;
    
    bool isEnd = false;
    Scene _scene;




    private void Start()
    {
        _resultTitle.text = GetResultTitle();
        _resultCurrency.text = GameManager.Instance.EarnedCurrency.ToString();
    }

    private string GetResultTitle() => GameManager.Instance.ResultSceneType switch
    {
        ResultSceneType.Clear => "클리어하셨습니다!",
        ResultSceneType.Dead => "실패하셨습니다!",
        _ => throw new ArgumentOutOfRangeException(),
    };

    

    public void ToStartScreen()
    {
        if(GameManager.Instance.IsLoading)
            return;
        if(GameManager.Instance.BlockSceneChange)
            return;
        _scene = SceneManager.GetActiveScene();
        GameManager.Instance.LoadMainScene();
    }

    private void Update() {
        Vector2 moveVector2 = _inputManager.MoveVector2_Left_WASD;
        if (moveVector2.magnitude != 0f)
        {
            EventSystem.current.SetSelectedGameObject(_startButton);
        }
    }
}
