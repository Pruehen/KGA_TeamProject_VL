using UnityEngine;

public class Chest : MonoBehaviour
{
    RewardType _rewardType;
    int _goldMin;
    int _goldMax;


    public void Init(RewardType rewardType)
    {
        _rewardType = rewardType;
    }


    public void UseItemGet()
    {
        Debug.Log("상자깡");

        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Chest");
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        Invoke("UI", 0.5f);

        SpawnItem(_rewardType);
    }

    private void SpawnItem(RewardType rewardType)
    {
        switch (rewardType)
        {
            case RewardType.Currency:
                SpawnRandomGold(500, 700);
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
        //GameObject blueChipGO = Instantiate(_blueChipPrefab, transform.position, transform.rotation);
        //blueChipGO.GetComponent<BlueChipItem>().RandomInit());
    }

    private void SpawnRandomGold(int v1, int v2)
    {
        //GameObject goldGO = Instantiate(_goldPrefab, transform.position, transform.rotation);
        //goldGO.GetComponent<Gold>().Init(Random.Range(_goldMin, _goldMax + 1));
    }

    public void UI()
    {
        UIManager.Instance.BlueChipUI();
    }
}