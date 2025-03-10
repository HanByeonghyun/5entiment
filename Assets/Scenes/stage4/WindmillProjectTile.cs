using UnityEngine;

public class WindmillProjectTile : MonoBehaviour
{
    private float lifetime = 2.0f; // 윈드밀이 맞지 않았을 때 지속시간

    private void Start()
    {
        // lifetime 후에 윈드밀 삭제하는 함수 호출
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player")
        {
            // Wind 프로젝트 타격 후 삭제
            Destroy(this.gameObject);
        }
    }

    private void GiveRewardIfMissed()
    {
        // 윈드밀 제거
        Destroy(this.gameObject);
    }
}
