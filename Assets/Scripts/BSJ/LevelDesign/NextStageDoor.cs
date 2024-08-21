using System;
using UnityEngine;

public class NextStageDoor : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private RewardType _rewardType;
    [SerializeField] private GameObject _currencyIcon;
    [SerializeField] private GameObject _blueChipIcon;

    public void Init(RewardType rewardType)
    {
        _currencyIcon.SetActive(false);
        _blueChipIcon.SetActive(false);


        _rewardType = rewardType;

        switch (_rewardType)
        {
            case RewardType.Currency:
                _currencyIcon.SetActive(true);
                break;
            case RewardType.BlueChip:
                _blueChipIcon.SetActive(true);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.attachedRigidbody != null)
        {
            GameManager.Instance.SetRewordType(_rewardType);
            GameManager.Instance.LoadNextStage();
        }
    }

    public void OnClear()
    {
        gameObject.SetActive(true);
    }
}