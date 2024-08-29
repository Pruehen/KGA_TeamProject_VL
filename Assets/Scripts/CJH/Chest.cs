using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] RewardType _rewardType;
    int _goldMin = 100;
    int _goldMax = 500;

    Animator animator;
    Collider chestCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        chestCollider = GetComponent<Collider>();
    }


    public void Init(RewardType rewardType)
    {
        _rewardType = rewardType;
    }


    public void UseItemGet()
    {
        Debug.Log("상자깡");

        SpawnItem(_rewardType);
    }

    private void SpawnItem(RewardType rewardType)
    {
        switch (rewardType)
        {
            case RewardType.Currency:
                SpawnRandomGold(_goldMin, _goldMax);
                break;
            case RewardType.BlueChip:
                SpawnRandomBlueChip();
                break;
            default:
                break;
        }
    }

    private void SpawnRandomBlueChip()
    {
        animator.SetTrigger("Chest");
        chestCollider.enabled = false;
        Invoke("BlueChipSelectUI", 0.5f);
    }

    private void SpawnRandomGold(int v1, int v2)
    {
        animator.SetTrigger("Chest");
        chestCollider.enabled = false;
        int amount = Random.Range(v1, v2 + 1);
        GameManager.Instance._PlayerMaster._PlayerInstanteState.AddGold(amount);
    }

    public void BlueChipSelectUI()
    {
        UIManager.Instance.BlueChipUI();
    }
}