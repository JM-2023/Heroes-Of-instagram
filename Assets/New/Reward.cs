using UnityEngine;

public class Reward : MonoBehaviour
{
    public enum RewardType { IncreaseTeam, DecreaseTeam, WeaponUpgrade }
    public RewardType rewardType;
    public int value = 1; // The amount to increase or decrease team size

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hero"))
        {
            HeroController hero = collision.GetComponent<HeroController>();
            ApplyReward();
            Destroy(gameObject);
        }
    }

    void ApplyReward()
    {
        switch (rewardType)
        {
            case RewardType.IncreaseTeam:
                TeamManager.Instance.AddHeroes(value);
                break;
            case RewardType.DecreaseTeam:
                TeamManager.Instance.RemoveHeroes(value);
                break;
            case RewardType.WeaponUpgrade:
                TeamManager.Instance.UpgradeWeapons();
                break;
        }
    }
}
