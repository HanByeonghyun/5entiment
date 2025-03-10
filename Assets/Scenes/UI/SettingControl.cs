using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingControl : MonoBehaviour
{
    // 키 설정 UI
    public Text moveLeftText;
    public Text moveRightText;
    public Text attackText;
    public Text jumpText;
    public Text rollText;
    public Text pauseText;

    public Button moveLeftbutton;
    public Button moveRightbutton;
    public Button attackbutton;
    public Button jumpbutton;
    public Button rollbutton;
    public Button pausebutton;

    public Text message;

    // 키 설정 저장
    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    private string keyToRebind = null; // 변경할 키 액션 이름을 저장하는 변수

    // 키 변경 완료 시 호출할 이벤트
    public delegate void KeyBindingsUpdated();
    public static event KeyBindingsUpdated OnKeyBindingsUpdated;

    private void Start()
    {
        // 키 설정 불러오기
        LoadKeyBindings();

        // UI에 텍스트 업데이트
        UpdateKeyBindingText();

        // 버튼 클릭 시 키 변경 시작
        moveLeftbutton.onClick.AddListener(() => StartRebinding("좌측 이동", moveLeftbutton));
        moveRightbutton.onClick.AddListener(() => StartRebinding("우측 이동", moveRightbutton));
        attackbutton.onClick.AddListener(() => StartRebinding("공격", attackbutton));
        jumpbutton.onClick.AddListener(() => StartRebinding("점프", jumpbutton));
        rollbutton.onClick.AddListener(() => StartRebinding("구르기", rollbutton));
        pausebutton.onClick.AddListener(() => StartRebinding("일시정지", pausebutton));

        message.text = "";
    }

    private void Update()
    {
        // 키 변경 모드일 때 새로운 키 입력받기
        if (keyToRebind != null && Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // 변경할 키 저장
                    keyBindings[keyToRebind] = keyCode;
                    keyToRebind = null;

                    // 키 변경된 버튼 텍스트 업데이트
                    UpdateKeyBindingText();

                    // 설정 저장
                    SaveKeyBindings();

                    // 키 설정 변경 이벤트 호출
                    OnKeyBindingsUpdated?.Invoke();

                    // 텍스트를 다시 활성화
                    message.text = "";

                    break;
                }
            }
        }
    }

    private void UpdateKeyBindingText()
    {
        // UI 텍스트를 현재 키 설정으로 업데이트
        moveLeftText.text = keyBindings["좌측 이동"].ToString();
        moveRightText.text = keyBindings["우측 이동"].ToString();
        attackText.text = keyBindings["공격"].ToString();
        jumpText.text = keyBindings["점프"].ToString();
        rollText.text = keyBindings["구르기"].ToString();
        pauseText.text = keyBindings["일시정지"].ToString();
    }

    // 키 설정 저장
    private void SaveKeyBindings()
    {
        foreach (var binding in keyBindings)
        {
            PlayerPrefs.SetString(binding.Key, binding.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    // 키 설정 불러오기
    private void LoadKeyBindings()
    {
        keyBindings["좌측 이동"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("좌측 이동", KeyCode.LeftArrow.ToString()));
        keyBindings["우측 이동"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("우측 이동", KeyCode.RightArrow.ToString()));
        keyBindings["아래"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("아래", KeyCode.DownArrow.ToString()));
        keyBindings["공격"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("공격", KeyCode.LeftControl.ToString()));
        keyBindings["점프"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("점프", KeyCode.LeftAlt.ToString()));
        keyBindings["구르기"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("구르기", KeyCode.Z.ToString()));
        keyBindings["일시정지"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("일시정지", KeyCode.Escape.ToString()));
    }

    // 키 변경 시작
    public void StartRebinding(string action, Button button)
    {
        keyToRebind = action;

        message.text = "변경하고 싶은 키를 입력하세요.";
    }
    public void RebindMoveLeft()
    {
        StartRebinding("좌측 이동", moveLeftbutton);
    }

    public void RebindMoveRight()
    {
        StartRebinding("우측 이동", moveRightbutton);
    }
    public void Rebindattack()
    {
        StartRebinding("공격", attackbutton);
    }
    public void Rebindjump()
    {
        StartRebinding("점프", jumpbutton);
    }

    public void Rebindroll()
    {
        StartRebinding("구르기", rollbutton);
    }
    public void Rebindpause()
    {
        StartRebinding("일시정지", pausebutton);
    }
}
