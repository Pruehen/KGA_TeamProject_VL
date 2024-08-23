using System;
using UnityEngine;

public class NextStageObjects : MonoBehaviour
{
    [SerializeField] NextStageDoor[] _nextStageDoors;
    [SerializeField] Chest _rewardChest;
    [SerializeField] Chest _additionalReward;

    [SerializeField] bool _testing = true;

    public void Init(RewardType rewardType)
    {
        _nextStageDoors[0].gameObject.SetActive(_testing);
        _nextStageDoors[1].gameObject.SetActive(_testing);
        _rewardChest.gameObject.SetActive(_testing);
        _additionalReward.gameObject.SetActive(_testing);

        _nextStageDoors[0].Init(RewardType.Currency);
        _nextStageDoors[1].Init(RewardType.BlueChip);
        _rewardChest.Init(rewardType);

        GameManager.Instance.OnGameClear += OnClear;
    }
    private void OnClear()
    {
        foreach ( NextStageDoor nextStageDoor in _nextStageDoors )
        {
            nextStageDoor.OnClear();
        }
        _rewardChest.gameObject.SetActive(true);

        if(GameManager.Instance.IsCurrentUnexpectedQuestCleared())
        {
            _additionalReward.gameObject.SetActive(true);
        }
    }
}