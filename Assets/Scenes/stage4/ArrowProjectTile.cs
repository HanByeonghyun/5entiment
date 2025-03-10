using UnityEngine;

public class ArrowProjectTile : MonoBehaviour
{
    private BossAgent4_1 bossAgent4;   // 보스 에이전트를 참조하여 보상을 줄 수 있도록 설정
    private float lifetime = 2.0f; // 화살이 맞지 않았을 때 보상을 줄 시간 (2초 후)

    //public GameObject boss4;

    private void Start()
    {
        // BossAgent4를 참조합니다.
        bossAgent4 = FindFirstObjectByType<BossAgent4_1>();

        // lifetime 후에 화살이 맞지 않은 것으로 간주하고 보상을 주는 메서드 호출
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    //화살이 보스에게 맞았을 때
    private void OnTriggerEnter2D(Collider2D other)
    {
        //화살이 보스1에게 맞았을 때
        if (other.gameObject.tag == "Boss1")
        {
            //화살 삭제
            Destroy(this.gameObject);
        }
        //화살이 보스2에게 맞았을 때
        if (other.gameObject.tag == "Boss2")
        {
            //화살 삭제
            Destroy(this.gameObject);
        }
        //화살이 보스3에게 맞았을 때
        if (other.gameObject.tag == "Boss3")
        {
            //화살 삭제
            Destroy(this.gameObject);
        }
        //화살이 보스4에게 맞았을 때
        if (other.gameObject.tag == "Boss4")
        {
            //화살 삭제
            Destroy(this.gameObject);
        }
    }

    private void GiveRewardIfMissed()
    {
        //화살을 맞지 않고 지속 시간이 지났을 때(보스4에만 적용)
        if (bossAgent4 != null)
        {   // 화살을 피했을 때 보상 + 연속으로 피했을 때 추가 보상
            bossAgent4.SetReward(10.0f + (bossAgent4.consecutiveHits * 5.0f)); 
            //연속으로 피한 횟수 증가
            bossAgent4.GetComponent<BossAgent4_1>().IncreaseConsecutiveHits();
            Debug.Log("Boss moving");
        }
        // 화살 제거
        Destroy(this.gameObject);
    }
}
