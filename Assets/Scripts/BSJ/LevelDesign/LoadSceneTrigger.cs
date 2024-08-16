using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private RewordType _rewordType;
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.SetRewordType(_rewordType);
        GameManager.Instance.LoadNextStage();
    }
}