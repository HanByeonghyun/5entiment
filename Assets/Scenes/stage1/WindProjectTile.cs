using UnityEngine;

public class WindProjectTile : MonoBehaviour
{
    private BossAgent1 bossAgent1;   // 보스 에이전트를 참조하여 보상을 줄 수 있도록 설정
    private float lifetime = 2.0f; // 바람의 지속 시간

    private void Start()
    {
        // BossAgent를 찾거나 설정합니다. 이 코드로 씬에 있는 BossAgent 컴포넌트를 참조합니다.
        bossAgent1 = FindFirstObjectByType<BossAgent1>();

        // lifetime 후에 바람 삭제
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player") // 플레이어에게 공격을 맞췄을 때
        {
            PlayerProjectTile playerHealth = other.GetComponent<PlayerProjectTile>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();  // 플레이어에게 10의 데미지를 줌
            }
            // 보상 지급
            if (bossAgent1 != null)
            {   // 보상 + 연속으로 맞출 시에 보상을 더 줌
                bossAgent1.AddReward(15.0f + (5.0f * bossAgent1.consecutiveHits));
                bossAgent1.IncreaseConsecutiveHits();  // 연속으로 맞춘 횟수 증가
            }

            // Wind 프로젝트 타격 후 삭제
            Destroy(this.gameObject);
        }
    }

    private void GiveRewardIfMissed()
    {
        // 바람이 충돌 없이 lifetime이 경과한 경우
        // 바람 제거
        Destroy(this.gameObject);
    }
}
