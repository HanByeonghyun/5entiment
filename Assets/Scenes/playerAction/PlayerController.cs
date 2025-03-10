using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;

    //플레이어 효과음
    private PlayerSoundManager soundManager;

    //이동 및 점프
    float jumpForce = 500.0f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 3.0f;

    //구르기 가능 여부와 쿨타임
    bool canRoll = true;
    float rollCooldown = 1f;

    //변신할때 나오는 이펙트 프리팹
    public GameObject change1Prefab;
    public GameObject change2Prefab;
    public GameObject change3Prefab;
    public GameObject change4Prefab;

    //화살 프리팹과 발사 위치, 속도
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 10.0f;
    //스킬로 강화된 화살 프리팹들과 각 스킬별 쿨타임
    public GameObject RainPrefab;
    public GameObject skill1Prefab;
    public float skill1Speed = 10.0f;
    public GameObject skill2Prefab;
    public float skill2Speed = 10.0f;
    public GameObject skill3Prefab;
    public float skill3Speed = 10.0f;

    //플레이어의 상태 플래그(사망, 공격중, 맞는중 움직임 정지)
    bool isDead = false;
    bool nowAttack = false;
    bool nowHit = false;

    //플레이어의 기본공격, 스킬 사용 가능 여부
    bool canAttack = true;
    bool canAttack2 = true;
    //공격별 쿨타임
    float attackCooldown = 0.7f;
    float skill1Cooldown = 0f;
    float skill2Cooldown = 1.0f;
    float skill3Cooldown = 0.7f;
    // 플레이어가 돌아올 위치
    public Vector3 respawnPosition1 = new Vector3(-8.6f, 0.3f, 0);  //stage2_3
    public Vector3 respawnPosition2 = new Vector3(4.5f, 3.55f, 0);  //stage4_Boss
    public Vector3 respawnPosition3 = new Vector3(-8.1f, 12.5f, 0); //stage3_3
    public Vector3 respawnPosition4 = new Vector3(-7.6f, 6.6f, 0);  //stage4_2
    public Vector3 respawnPosition5 = new Vector3(38.0f, 1.5f, 0);  //stage4_3

    //점프, 2단점프 가능 변수
    private bool isGrounded = false;
    private bool canDoubleJump = false;

    // 플랫폼 Effector2D 관련(stage1_Boss맵에서 사용하는 하향 점프)
    private Collider2D playerCollider;
    private bool canDown = false;

    //변신할 프리팹
    public GameObject player0Prefab;
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public GameObject player3Prefab;
    public GameObject player4Prefab;

    // 키 설정
    private KeyCode moveLeftKey;
    private KeyCode moveRightKey;
    private KeyCode moveDownKey;
    private KeyCode jumpKey;
    private KeyCode attackKey;
    private KeyCode rollKey;

    //일시정지할 때 참조할 스크립트 객체
    public PauseManager pauseManager;

    // 변신 가능한지 참고할 스크립트 객체와 오브젝트
    public GameObject playerStateManager;
    public PlayerStateManager playerState;

    //플레이어의 체력과 스킬 횟수 오브젝트
    GameObject playerHP;
    GameObject playerMP;

    //보스가 죽었을 때 정령 획득 가능 여부
    public bool visible = false; 

    //플레이어가 죽은 위치
    private Vector3 deathPosition; 

    void Start()
    {
        
        Application.targetFrameRate = 60; //게임 프레임 속도
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

        this.playerHP = GameObject.Find("Hpbar");
        this.playerMP = GameObject.Find("Mpbar");

        this.playerStateManager = GameObject.Find("PlayerStateManager");
        playerState = FindAnyObjectByType<PlayerStateManager>();

        // 다음 스테이지로 넘어가도 플레이어가 유지되게 하는 함수
        DontDestroyOnLoad(this.gameObject);

        // Hpbar, Mpbar 오브젝트를 찾아서 유지되도록 설정
        GameObject playerUI = GameObject.FindWithTag("playerUI");
        if (playerUI != null)
        {
            DontDestroyOnLoad(playerUI); // Hpbar, Mpbar도 씬 전환 시 유지
        }

        //일시정지 UI가 업을 때 로그 생성
        if (pauseManager == null)
        {
            pauseManager = FindAnyObjectByType<PauseManager>();
            if (pauseManager != null )
            {
                Debug.Log("PauseManager가 없습니다");
            }
        }
        LoadKeyBindings();

        SettingControl.OnKeyBindingsUpdated += LoadKeyBindings;

        // 씬 변경 시 호출되는 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;

        //플레이어 효과음 참조 스크립트
        soundManager = GetComponent<PlayerSoundManager>();
    }

    void OnDestroy()
    {
        // 씬 로딩 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SettingControl.OnKeyBindingsUpdated -= LoadKeyBindings;
    }

    //씬이 전환될 때(스테이지가 넘어갈 때)
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // MainMenu씬으로 넘어갈 때 플레이어 오브젝트와 UI 삭제
        if (scene.name == "Main menu")
        {
            Destroy(this.gameObject); // 플레이어 오브젝트 삭제
            GameObject playerUI = GameObject.Find("playerUI");
            if (playerUI != null)
            {
                Destroy(playerUI); // 플레이어 UI 삭제
            }
            return;
        }

        //MianMenu가 아닌 다른 씬으로 넘어갈 때
        // appearPrefab의 위치에 플레이어 생성
        GameObject appearPrefab = GameObject.Find("appearPrefab");
        if (appearPrefab != null)
        {
            transform.position = appearPrefab.transform.position;
        }
        else
        {
            Debug.LogWarning("appearPrefab을 찾을 수 없습니다.");
        }
    }

    //항상 호출되고 있는 함수
    void Update()
    {

        //일시정지--------------------------------------------------------
        if (pauseManager == null)
        {
            pauseManager = FindAnyObjectByType<PauseManager>();
            if (pauseManager != null)
            {
                Debug.Log("PauseManager가 없습니다");
            }
        }
        //ESC눌러 일시정지 됐을 때 움직임 불가 
        if (pauseManager != null && pauseManager.isPaused) return;
        // -----------------------------------------------------------------

        
        // 플레이어가 죽거나 공격중이거나 맞는중일 때 움직임과 공격을 막음
        if (isDead) return;
        if (nowAttack) return;
        if (nowHit) return;

        // 기본 모드일 때 스킬 사용 여부가 true일 때 QWER키 누르면 다른 플레이어 프리팹으로 변신
        if (transform.name == "player0Prefab(Clone)" || transform.name == "player0Prefab")
        {
            if (Input.GetKeyDown(KeyCode.Q) && playerState.skill1Count == 0 && playerState.canSkill1)
            {
                // 변신 함수에 변신할 프리팹과 그 때 나타날 이펙트를 넣어서 호출
                TransformToPrefab(player1Prefab, change1Prefab);
            }
            if (Input.GetKeyDown(KeyCode.W) && playerState.skill2Count == 0 && playerState.canSkill2)
            {
                TransformToPrefab(player2Prefab, change2Prefab);
            }
            if (Input.GetKeyDown(KeyCode.E) && playerState.skill3Count == 0 && playerState.canSkill3)
            {
                TransformToPrefab(player3Prefab, change3Prefab);
            }
            if (Input.GetKeyDown(KeyCode.R) && playerState.rainCount == 0 && playerState.canSkill4)
            {
                TransformToPrefab(player4Prefab, change4Prefab);
            }
        }

        // 하향 점프
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(jumpKey))
        {
            if (canDown)
            { 
                soundManager.PlayJumpSound();
                StartCoroutine(EnableTriggerTemporarily());
               
            }
        }
        else if (Input.GetKeyDown(jumpKey)) //기본 점프
        {
            if (isGrounded) //바닥에 붙어있다면
            {
                soundManager.PlayJumpSound();
                Jump1(); //기본 점프
                canDoubleJump = true; // 첫 점프 후 2단 점프 가능
            }
            else if (canDoubleJump)
            {
                Jump2(); //2단 점프
                canDoubleJump = false; // 2단 점프 후 더 이상 점프 불가
            }
        }

        //좌우 이동 + 구르기------------------------------------------------------------------------
        int key = 0;
        if (Input.GetKey(moveRightKey))
        {
            key = 1; //플레이어 방향
            this.animator.SetBool("RunBool", true); //뛰는 애니메이션 활성화
            if (Input.GetKeyDown(rollKey) && canRoll) //구르기
            {
                soundManager.PlayRollSound();
                StartCoroutine(RollCooldown()); // 롤 쿨타임 코루틴 호출
                this.animator.SetTrigger("RollTrigger"); //구르는 애니메이션을 재생하는 트리거 호출
                //구르기 사용시 속도 증가
                maxWalkSpeed = 7.0f;
            }
        }
        else if (Input.GetKey(moveLeftKey))
        {
            key = -1;
            this.animator.SetBool("RunBool", true);
            if (Input.GetKeyDown(rollKey) && canRoll) //구르기
            {
                soundManager.PlayRollSound();
                StartCoroutine(RollCooldown()); // 롤 쿨다운 코루틴 호출
                this.animator.SetTrigger("RollTrigger");
                maxWalkSpeed = 7.0f;
            }
        }
        else
        {
            key = 0;
            this.animator.SetBool("RunBool", false);
        }
        //플레이어의 속도
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        //스피드 제한
        if (speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }
        else
        {
            maxWalkSpeed = 3.0f;
        }

        //움직이는 방향에 따라 반전한다.
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }
        //--------------------------------------------------------------------------------------



        // 기본 공격 + 강화 ---------------------------------------------------------------
        if (Input.GetKeyDown(attackKey) && canAttack && playerState.canSkill0)
        {
            // player4Prefab일 때 화살의 위치를 살짝 아래로 조정
            float arrowOffsetY = transform.name == "player4Prefab(Clone)" ? -0.2f : 0f;

            GameObject director = GameObject.Find("GameDirector");
            
            //강화된 기본 공격
            if (transform.name == "player1Prefab(Clone)" && playerState.skill1Count < 8 && playerState.canSkill1) //스킬1
            {
                //강화된 기본 공격 사용시 사용 횟수 감소
                director.GetComponent<GameDirector>().DecreasePlayerMp(0.125f);
                nowAttack = true;
                this.animator.SetTrigger("AtkTrigger");
                canAttack = false; //공격 직후 공격할 수 없도록 설정
                StartCoroutine(Skill1Cooldown()); //쿨다운 코루틴 시작
                StartCoroutine(Skill1WithDelay(0.18f, arrowOffsetY));
            }
            else if (transform.name == "player2Prefab(Clone)" && playerState.skill2Count < 5 && playerState.canSkill2) //스킬2
            {
                director.GetComponent<GameDirector>().DecreasePlayerMp(0.2f);
                nowAttack = true;
                this.animator.SetTrigger("AtkTrigger");
                canAttack = false; //공격 직후 공격할 수 없도록 설정
                StartCoroutine(Skill2Cooldown()); //쿨다운 코루틴 시작
                StartCoroutine(Skill2WithDelay(0.18f, arrowOffsetY));
            }
            else if (transform.name == "player3Prefab(Clone)" && playerState.skill3Count < 8 && playerState.canSkill3) //스킬3
            {
                director.GetComponent<GameDirector>().DecreasePlayerMp(0.125f);
                nowAttack = true;
                this.animator.SetTrigger("AtkTrigger");
                canAttack = false; //공격 직후 공격할 수 없도록 설정
                StartCoroutine(Skill3Cooldown()); //쿨다운 코루틴 시작
                StartCoroutine(Skill3WithDelay(0.18f, arrowOffsetY));
            }
            else //기본 공격
            {
                nowAttack = true;
                this.animator.SetTrigger("AtkTrigger");
                canAttack = false; //공격 직후 공격할 수 없도록 설정
                StartCoroutine(AttackCooldown()); //쿨다운 코루틴 시작
                StartCoroutine(AttackWithDelay(0.18f, arrowOffsetY));
            }
            //공격 효과음
            soundManager.PlayAttackSound();
        } 
        //-------------------------------------------------------------------------------------------
        // 스킬4 ---------------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.LeftShift) && transform.name == "player4Prefab(Clone)" && canAttack2 && playerState.canSkill4)
        {
            if (playerState.rainCount < 2)
            {
                GameObject director = GameObject.Find("GameDirector");
                director.GetComponent<GameDirector>().DecreasePlayerMp(0.5f);
                canAttack2 = false;
                nowAttack = true;
                this.animator.SetTrigger("Atk2Trigger");
                canAttack = false; //공격 직후 공격할 수 없도록 설정
                StartCoroutine(SpawnRainPrefabs());
                soundManager.PlayAttackSound();
            }
        }//-----------------------------------------------------------------------------------

        //플레이어가 화면 밖으로 나가면 처음부터
        if (transform.position.y < -10)
        {
            //현재 실행중인 씬 가져오기
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        // 정령 획득 가능 여부-------------------------------------------------------------
        GameObject[] boss1 = GameObject.FindGameObjectsWithTag("Boss1");
        GameObject[] boss2 = GameObject.FindGameObjectsWithTag("Boss2");
        GameObject[] boss3 = GameObject.FindGameObjectsWithTag("Boss3");
        GameObject[] boss4 = GameObject.FindGameObjectsWithTag("Boss4");

        if (boss1.Length == 0 && boss2.Length == 0 && boss3.Length == 0 && boss4.Length == 0)
        {
            visible = true;
        }
        else
        {
            visible = false;
        }
        //---------------------------------------------------------------------------------
    }

    private void LoadKeyBindings()
    {
        moveLeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("좌측 이동", KeyCode.LeftArrow.ToString()));
        moveRightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("우측 이동", KeyCode.RightArrow.ToString()));
        moveDownKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("아래", KeyCode.DownArrow.ToString()));
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("점프", KeyCode.LeftAlt.ToString()));
        attackKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("공격", KeyCode.LeftControl.ToString()));
        rollKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("구르기", KeyCode.Z.ToString()));
    }

    // 구르기 쿨타임 시작 함수
    IEnumerator RollCooldown()
    {
        canRoll = false; // 구르기 사용 불가
        yield return new WaitForSeconds(rollCooldown); // 설정한 쿨타임 시간 동안 대기
        canRoll = true; // 구르기 다시 사용 가능
    }

    // 스킬 1 ---------------------------------------------------------------------------
    //공격 모션 후 skill1 발사(파라미터(공격 모션 몇초 후 나갈지, 화살 발사 높이)
    IEnumerator Skill1WithDelay(float delay, float offsetY)
    {
        yield return new WaitForSeconds(delay);
        // 모션 시간 후 공격 발사
        Skill1(offsetY);
        nowAttack = false;
    }
    //1초동안 skill1을 할 수 없게 하는 코루틴
    IEnumerator Skill1Cooldown()
    {
        yield return new WaitForSeconds(skill1Cooldown);
        canAttack = true; //1초후 공격 가능
    }
    //플레이어 skill1
    void Skill1(float offsetY)
    {
        if (skill1Prefab != null && arrowSpawnPoint != null)
        {
            //화살 위치 조정
            Vector3 arrowPosition = arrowSpawnPoint.position + new Vector3(0, offsetY, 0);
            //화살 방향
            float arrowDirection = transform.localScale.x;

            // 화살 생성 시 100도 반시계 방향으로 회전
            Quaternion arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, 115); // Z축 기준 115도 회전

            // 플레이어가 왼쪽을 보고 있으면 반대 방향으로 회전 (Z축 기준 115도 회전)
            if (arrowDirection < 0)
            {
                arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, -115);
            }

            // 조정한 위치와 회전으로 화살 생성
            GameObject arrow = Instantiate(skill1Prefab, arrowPosition, arrowRotation);

            arrow.transform.localScale = new Vector3(arrowDirection, 1, 1);

            //화살에 물리적 힘을 추가해 발사
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();

            if (arrowRb != null)
            {
                arrowRb.velocity = new Vector2(arrowDirection * skill1Speed, 0);
            }
        }
        else
        {
            Debug.LogError("skill1 prefab or spawn point is not set");
        }
        //스킬 사용 횟수 증가(=스킬 사용 가능 횟수 감소)
        playerStateManager.GetComponent<PlayerStateManager>().Skill1CountUp();
    }
    // --------------------------------------------------------------------------------------

    // 스킬 2 ---------------------------------------------------------------------------
    //공격 모션 후 skill2 발사
    IEnumerator Skill2WithDelay(float delay, float offsetY)
    {
        yield return new WaitForSeconds(delay);
        Skill2(offsetY);
        nowAttack = false;
    }
    //1초동안 skill2을 할 수 없게 하는 코루틴
    IEnumerator Skill2Cooldown()
    {
        yield return new WaitForSeconds(skill2Cooldown);
        canAttack = true; //1초후 공격 가능
    }
    //플레이어 skill2
    void Skill2(float offsetY)
    {
        if (skill2Prefab != null && arrowSpawnPoint != null)
        {
            //화살 위치 조정
            Vector3 arrowPosition = arrowSpawnPoint.position + new Vector3(0, offsetY, 0);

            float arrowDirection = transform.localScale.x;

            // 화살 생성 시 100도 반시계 방향으로 회전
            Quaternion arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, 115); // Z축 기준 115도 회전

            // 플레이어가 왼쪽을 보고 있으면 반대 방향으로 회전 (Z축 기준 230도 회전)
            if (arrowDirection < 0)
            {
                arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, -115);
            }

            // 조정한 위치와 회전으로 화살 생성
            GameObject arrow = Instantiate(skill2Prefab, arrowPosition, arrowRotation);

            arrow.transform.localScale = new Vector3(arrowDirection, 1, 1);

            //화살에 물리적 힘을 추가해 발사
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();

            if (arrowRb != null)
            {
                arrowRb.velocity = new Vector2(arrowDirection * skill1Speed, 0);
            }
        }
        else
        {
            Debug.LogError("skill2 prefab or spawn point is not set");
        }
        playerStateManager.GetComponent<PlayerStateManager>().Skill2CountUp();
    }
    // --------------------------------------------------------------------------------------

    // 스킬 3 ---------------------------------------------------------------------------
    //공격 모션 후 skill3 발사
    IEnumerator Skill3WithDelay(float delay, float offsetY)
    {
        yield return new WaitForSeconds(delay);
        Skill3(offsetY);
        nowAttack = false;
    }
    //1초동안 skill3을 할 수 없게 하는 코루틴
    IEnumerator Skill3Cooldown()
    {
        yield return new WaitForSeconds(skill3Cooldown);
        canAttack = true; //1초후 공격 가능
    }
    //플레이어 skill3
    void Skill3(float offsetY)
    {
        if (skill3Prefab != null && arrowSpawnPoint != null)
        {
            //화살 위치 조정
            Vector3 arrowPosition = arrowSpawnPoint.position + new Vector3(0, offsetY, 0);

            float arrowDirection = transform.localScale.x;

            // 화살 생성 시 100도 반시계 방향으로 회전
            Quaternion arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, 115); // Z축 기준 115도 회전

            // 플레이어가 왼쪽을 보고 있으면 반대 방향으로 회전 (Z축 기준 230도 회전)
            if (arrowDirection < 0)
            {
                arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, -115);
            }

            // 조정한 위치와 회전으로 화살 생성
            GameObject arrow = Instantiate(skill3Prefab, arrowPosition, arrowRotation);

            arrow.transform.localScale = new Vector3(arrowDirection, 1, 1);

            //화살에 물리적 힘을 추가해 발사
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();

            if (arrowRb != null)
            {
                arrowRb.velocity = new Vector2(arrowDirection * skill1Speed, 0);
            }
        }
        else
        {
            Debug.LogError("skill3 prefab or spawn point is not set");
        }
        playerStateManager.GetComponent<PlayerStateManager>().Skill3CountUp();
    }
    // --------------------------------------------------------------------------------------

    // 스킬4(플레이어 머리 위에서 여러발의 화살 생성 후 대각선으로 발사)------------------------------------
    // RainPrefab을 순서대로 생성하는 코루틴
    IEnumerator SpawnRainPrefabs()
    {
        Vector3 playerPos = transform.position;
        float direction = transform.localScale.x; // 플레이어가 오른쪽을 보고 있으면 1, 왼쪽을 보고 있으면 -1

        yield return new WaitForSeconds(1.0f); // 1초 대기
        nowAttack = false;

        float offsetY = 5.0f; // 플레이어 머리 위 일정 거리
        float spawnInterval = 0.2f; // 생성 간격
        float rowOffset = 0.5f; // 각 줄의 x 오프셋
        int rainCount = 5; // 생성할 RainPrefab 수
        float fallAngleY = -0.90f; // 대각선 각도 조절 변수 (더 작으면 완만, 더 크면 가파름)

        for (int i = 0; i < rainCount; i++)
        {
            // 세 줄의 화살을 나란히 생성
            for (int j = -1; j <= 1; j++)
            {
                // RainPrefab을 플레이어 머리 위에 생성
                Vector3 spawnPosition = playerPos + new Vector3(j * rowOffset, offsetY, 0);
                GameObject rain = Instantiate(RainPrefab, spawnPosition, Quaternion.identity);

                // 플레이어가 바라보는 방향에 따라 대각선으로 떨어지도록 설정
                Rigidbody2D rainRb = rain.GetComponent<Rigidbody2D>();
                Vector2 fallDirection = new Vector2(direction, fallAngleY).normalized; // 대각선 방향 설정
                float rainSpeed = 15.0f; // RainPrefab의 떨어지는 속도
                rainRb.velocity = fallDirection * rainSpeed;

                // 오른쪽을 보고 있을 때 이미지 좌우 반전
                if (direction > 0)
                {
                    rain.transform.localScale = new Vector3(-1, 1, 1); // 좌우 반전
                    rain.transform.rotation = Quaternion.Euler(0, 0, 15); // 오른쪽 대각선 각도 (예: -45도)
                }
                else
                {
                    rain.transform.localScale = new Vector3(1, 1, 1); // 원래 방향 유지
                    rain.transform.rotation = Quaternion.Euler(0, 0, -15); // 왼쪽 대각선 각도 (예: 45도)
                }
            }

            // 다음 RainPrefab 생성까지 대기
            yield return new WaitForSeconds(spawnInterval);
        }
        canAttack2 = true;
        playerStateManager.GetComponent<PlayerStateManager>().RainCountUp();
    }
    // -------------------------------------------------------------------------------------------


    
    // 기본 공격 ---------------------------------------------------------------------------------
    //공격 모션 후 화살 발사
    IEnumerator AttackWithDelay(float delay, float offsetY)
    {
        yield return new WaitForSeconds(delay);
        Attack(offsetY);
        nowAttack = false;
    }
    //1초동안 공격을 할 수 없게 하는 코루틴
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true; //1초후 공격 가능
    }
    //플레이어 공격
    void Attack(float offsetY)
    {
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            //화살 위치 조정
            Vector3 arrowPosition = arrowSpawnPoint.position + new Vector3(0, offsetY, 0);
            //조정한 위치에 화살 생성
            GameObject arrow = Instantiate(arrowPrefab, arrowPosition, arrowSpawnPoint.rotation);

            //플레이어가 보는 방향
            float arrowDirection = transform.localScale.x;
            arrow.transform.localScale = new Vector3(arrowDirection, 1, 1);

            //화살에 물리적 힘을 추가해 발사
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
            if (arrowRb != null)
            {
                arrowRb.velocity = new Vector2(arrowDirection * arrowSpeed, 0);
            }
        }
        else
        {
            Debug.LogError("Arrow prefab or spawn point is not set");
        }
    }
    // --------------------------------------------------------------------------------------

    //플레이어 공격 당함---------------------------------------------------------------------
    public void PlayerHit()
    {
        nowHit = true;
        soundManager.PlayHitSound();
        this.animator.SetTrigger("HitTrigger");
        StartCoroutine(HitTime(0.5f));
    }
    //맞는 동안 움직임 정지
    IEnumerator HitTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        nowHit = false;
    }
    // ---------------------------------------------------------------------------------------

    //플레이어 죽는 코드 ---------------------------------------------------------------------
    //플레이어가 죽는 애니메이션 발생 후 2초 후 사라지게 딜레이를 주기 위함
    IEnumerator PlayerDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RespawnAtDeathPosition(); // 죽은 위치에서 부활
    }
    public void PlayerDeath()
    {
        if (!isDead)
        {
            isDead = true;  // 플레이어가 죽었음을 표시
            deathPosition = transform.position; // 죽은 위치 저장
            this.animator.SetTrigger("DeathTrigger");
            StartCoroutine(PlayerDeathWithDelay(2.0f));
        }
    }

    // 죽었던 자리에서 부활하는 메서드
    private void RespawnAtDeathPosition()
    {
        transform.position = deathPosition; // 저장된 위치로 이동
        rigid2D.velocity = Vector2.zero; // 속도 초기화
        isDead = false;
        nowAttack = false;
        nowHit = false;
        //체력 초기화
        this.playerHP.GetComponent<Image>().fillAmount = 1.0f;
    }
    //----------------------------------------------------------------------------------------

    //점프-------------------------------------------------------------------------------------
    void Jump1()
    {
        this.animator.SetTrigger("JumpTrigger");
        this.rigid2D.velocity = new Vector2(rigid2D.velocity.x, 0); // 기존 y 속도 제거
        this.rigid2D.AddForce(Vector2.up * this.jumpForce);
    }
    void Jump2()
    {
        this.animator.SetTrigger("Jump2Trigger");
        this.rigid2D.velocity = new Vector2(rigid2D.velocity.x, 0); // 기존 y 속도 제거
        this.rigid2D.AddForce(Vector2.up * this.jumpForce); 
    }
    //-----------------------------------------------------------------------------------------------

    // 어딘가에 닿거나 떨어졌을 때-----------------------------------------------------------------------
    //바닥, 낭떠러지에 닿았는지 확인
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //하향점프(아래로 내려가는 점프)가능한 블럭
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            isGrounded = true;
            canDoubleJump = false; // 바닥에 닿으면 2단 점프 초기화
            canDown = true; //하향점프 가능 여부
        }
        else if (collision.gameObject.CompareTag("Ground")) //하향점프 불가능 블럭
        {
            isGrounded = true;
            canDoubleJump = false; // 바닥에 닿으면 2단 점프 초기화
            canDown = false;

        }
        else if (collision.gameObject.CompareTag("Cliff")) //stage4_Boss
        {
            Respawn2(); // 플레이어를 리스폰 위치로 이동시키고 체력을 달게 하는 함수 호출
        }
        else if (collision.gameObject.CompareTag("FireCliff")) //stage2_3
        {
            Respawn1();
        } 
        else if (collision.gameObject.CompareTag("FireCliff2")) //staeg3_3
        {
            Respawn3();
        }
        else if (collision.gameObject.CompareTag("Fall")) //stage4_2
        {
            Respawn4();
        }
        else if (collision.gameObject.CompareTag("Fall2")) //stage4_3
        {
            Respawn5();
        }

        //정령에 닿았을 때
        if (collision.gameObject.CompareTag("sentiment0"))
        {
            playerState.canSkill0 = true; //기본공격 활성화
            Destroy(collision.gameObject); // sentiment0 제거
        }
        //정령 획득 가능하고 정령에 닿았을 때
        if (collision.gameObject.CompareTag("sentiment1") && visible)
        {
            playerState.canSkill1 = true;   // 공격강화1 가능
            Destroy(collision.gameObject);  // sentiment1 제거
        }
        if (collision.gameObject.CompareTag("sentiment2") && visible)
        {
            playerState.canSkill2 = true;   // 공격강화2 가능
            Destroy(collision.gameObject);  // sentiment2 제거
        }
        if (collision.gameObject.CompareTag("sentiment3") && visible)
        {
            playerState.canSkill3 = true;   // 공격강화3 가능
            Destroy(collision.gameObject);  // sentiment3 제거
        }
        if (collision.gameObject.CompareTag("sentiment4") && visible)
        {
            playerState.canSkill4 = true;   // 스킬4 가능
            Destroy(collision.gameObject);  // sentiment4 제거
        }
    }
    //어딘가에서 떨어졌을 때(공중에 있을 때)
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tilemap") || collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            canDoubleJump = true;
            canDown = false;
        }
    }
    // ---------------------------------------------------------------------------------------

    // 플레이어의 isTrigger를 잠시 활성화하는 코루틴(물체에 닿지 않아서 하향 점프가 가능함)------
    IEnumerator EnableTriggerTemporarily()
    {
        if (playerCollider != null)
        {
            this.animator.SetTrigger("DownTrigger");
            playerCollider.isTrigger = true;  // 플레이어 콜라이더 트리거(물체에 닿지 않게함) 활성화
            yield return new WaitForSeconds(0.3f);  // 0.2초 동안 활성화 유지
            playerCollider.isTrigger = false;  // 트리거 비활성화
        }
    }
    //-----------------------------------------------------------------------------------------

    //플레이어가 특정 좌표로 복귀하는 메서드-----------------------------------------------------
    void Respawn1() //stage2_3
    {
        transform.position = respawnPosition1; // 리스폰 위치로 플레이어 이동
        rigid2D.velocity = Vector2.zero; // 속도 초기화
        isDead = false;
        nowAttack = false;
        nowHit = false;
        GameObject director = GameObject.Find("GameDirector");
        if (director != null)
        {
            //체력 감소
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
        }
    }

    void Respawn2() //stage4_Boss
    {
        transform.position = respawnPosition2; // 리스폰 위치로 플레이어 이동
        rigid2D.velocity = Vector2.zero; // 속도 초기화
        isDead = false;
        nowAttack = false;
        nowHit = false;
        GameObject director = GameObject.Find("GameDirector");
        if (director != null)
        {
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
        }
    }
    
    void Respawn3() //stage3_3
    {
        transform.position = respawnPosition3; // 리스폰 위치로 플레이어 이동
        rigid2D.velocity = Vector2.zero; // 속도 초기화
        isDead = false;
        nowAttack = false;
        nowHit = false;
    }
    
    void Respawn4() //stage4_2
    {
        transform.position = respawnPosition4; // 리스폰 위치로 플레이어 이동
        rigid2D.velocity = Vector2.zero; // 속도 초기화
        isDead = false;
        nowAttack = false;
        nowHit = false;
        GameObject director = GameObject.Find("GameDirector");
        if (director != null)
        {
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
        }
    }
    
    void Respawn5() //stage4_3
    {
        transform.position = respawnPosition5; // 리스폰 위치로 플레이어 이동
        rigid2D.velocity = Vector2.zero; // 속도 초기화
        isDead = false;
        nowAttack = false;
        nowHit = false;
        GameObject director = GameObject.Find("GameDirector");
        if (director != null)
        {
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
        }
    }

    // 변신 코드(변신할 프리팹, 변신할 때 이펙트 프리팹)------------------------------------------------
    public void TransformToPrefab(GameObject playerPrefab, GameObject changePrefab) //변신1
    {
        // 프리팹이 설정되어 있는지 확인
        if (playerPrefab != null)
        {
            //공격 가능 횟수 초기화
            this.playerMP.GetComponent<Image>().fillAmount = 1.0f;
            if (playerPrefab != player0Prefab)
            {
                ShowChangePrefab(changePrefab);
            }
            // 현재 위치에 플레이어 지정한 프리팹을 생성하고 속성(위치, 방향) 복사
            GameObject newPlayer = Instantiate(playerPrefab, transform.position, transform.rotation);
            newPlayer.transform.localScale = transform.localScale; // 방향 유지

            // CameraController에 새로운 플레이어 오브젝트를 설정
            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            if (cameraController != null)
            {
                cameraController.SetPlayer(newPlayer);
            }
            //보스들, 스킬 여부 확인 오브젝트에게 변신한 플레이어 오브젝트 연결
            Boss1Controller boss1Controller = FindAnyObjectByType<Boss1Controller>();
            if (boss1Controller != null)
            {
                boss1Controller.SetPlayer(newPlayer);
            }
            BossAgent bossAgent = FindAnyObjectByType<BossAgent>();
            if (bossAgent != null)
            {
                bossAgent.SetPlayer(newPlayer);
            }
            Boss2Controller boss2Controller = FindAnyObjectByType<Boss2Controller>();
            if (boss2Controller != null)
            {
                boss2Controller.SetPlayer(newPlayer);
            }
            BossAgent2 bossAgent2 = FindAnyObjectByType<BossAgent2>();
            if (bossAgent2 != null)
            {
                bossAgent2.SetPlayer(newPlayer);
            }
            Boss3Controller boss3Controller = FindAnyObjectByType<Boss3Controller>();
            if (boss3Controller != null)
            {
                boss3Controller.SetPlayer(newPlayer);
            }
            BossAgent3 bossAgent3 = FindAnyObjectByType<BossAgent3>();
            if (bossAgent3 != null)
            {
                bossAgent3.SetPlayer(newPlayer);
            }
            Boss4Controller boss4Controller = FindAnyObjectByType<Boss4Controller>();
            if (boss4Controller != null)
            {
                boss4Controller.SetPlayer(newPlayer);
            }
            BossAgent4 bossAgent4 = FindAnyObjectByType<BossAgent4>();
            if (bossAgent4 != null)
            {
                bossAgent4.SetPlayer(newPlayer);
            }
            PlayerStateManager playerStateManager = FindAnyObjectByType<PlayerStateManager>();
            if (playerStateManager != null)
            {
                playerStateManager.SetPlayer(newPlayer);
            }

            // 기존 플레이어 삭제
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Player prefab is not set.");
        }
    }

    //플레이어 변신시 변신한 오브젝트를 연결
    public void ShowChangePrefab(GameObject prefab)
    {
        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
    //---------------------------------------------------------------------------------------
}