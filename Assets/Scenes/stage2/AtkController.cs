using UnityEngine;

public class AtkController : MonoBehaviour
{
    private float lifetime = 0.5f; // 바람이 맞지 않았을 때 지속시간(0.5초)

    public float damageInterval = 3.0f; //데미지 간격
    private float nextDamgeTime = 0f; //다음 데미지를 줄 시간

    private void Start()
    {
        // lifetime 후에 공격이 맞지 않아 삭제하는 함수
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "player") && (Time.time >= nextDamgeTime))
        {
            GameObject director = GameObject.Find("GameDirector");
            //플레이어 체력 감소
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
            // Wind 프로젝트 타격 후 삭제
            Destroy(this.gameObject);

            nextDamgeTime = Time.time + damageInterval;
        }
    }

    private void GiveRewardIfMissed()
    {
        Destroy(this.gameObject);
    }
}
