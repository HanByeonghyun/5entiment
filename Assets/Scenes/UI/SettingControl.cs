using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingControl : MonoBehaviour
{
    // Ű ���� UI
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

    // Ű ���� ����
    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    private string keyToRebind = null; // ������ Ű �׼� �̸��� �����ϴ� ����

    // Ű ���� �Ϸ� �� ȣ���� �̺�Ʈ
    public delegate void KeyBindingsUpdated();
    public static event KeyBindingsUpdated OnKeyBindingsUpdated;

    private void Start()
    {
        // Ű ���� �ҷ�����
        LoadKeyBindings();

        // UI�� �ؽ�Ʈ ������Ʈ
        UpdateKeyBindingText();

        // ��ư Ŭ�� �� Ű ���� ����
        moveLeftbutton.onClick.AddListener(() => StartRebinding("���� �̵�", moveLeftbutton));
        moveRightbutton.onClick.AddListener(() => StartRebinding("���� �̵�", moveRightbutton));
        attackbutton.onClick.AddListener(() => StartRebinding("����", attackbutton));
        jumpbutton.onClick.AddListener(() => StartRebinding("����", jumpbutton));
        rollbutton.onClick.AddListener(() => StartRebinding("������", rollbutton));
        pausebutton.onClick.AddListener(() => StartRebinding("�Ͻ�����", pausebutton));

        message.text = "";
    }

    private void Update()
    {
        // Ű ���� ����� �� ���ο� Ű �Է¹ޱ�
        if (keyToRebind != null && Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // ������ Ű ����
                    keyBindings[keyToRebind] = keyCode;
                    keyToRebind = null;

                    // Ű ����� ��ư �ؽ�Ʈ ������Ʈ
                    UpdateKeyBindingText();

                    // ���� ����
                    SaveKeyBindings();

                    // Ű ���� ���� �̺�Ʈ ȣ��
                    OnKeyBindingsUpdated?.Invoke();

                    // �ؽ�Ʈ�� �ٽ� Ȱ��ȭ
                    message.text = "";

                    break;
                }
            }
        }
    }

    private void UpdateKeyBindingText()
    {
        // UI �ؽ�Ʈ�� ���� Ű �������� ������Ʈ
        moveLeftText.text = keyBindings["���� �̵�"].ToString();
        moveRightText.text = keyBindings["���� �̵�"].ToString();
        attackText.text = keyBindings["����"].ToString();
        jumpText.text = keyBindings["����"].ToString();
        rollText.text = keyBindings["������"].ToString();
        pauseText.text = keyBindings["�Ͻ�����"].ToString();
    }

    // Ű ���� ����
    private void SaveKeyBindings()
    {
        foreach (var binding in keyBindings)
        {
            PlayerPrefs.SetString(binding.Key, binding.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    // Ű ���� �ҷ�����
    private void LoadKeyBindings()
    {
        keyBindings["���� �̵�"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("���� �̵�", KeyCode.LeftArrow.ToString()));
        keyBindings["���� �̵�"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("���� �̵�", KeyCode.RightArrow.ToString()));
        keyBindings["�Ʒ�"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("�Ʒ�", KeyCode.DownArrow.ToString()));
        keyBindings["����"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("����", KeyCode.LeftControl.ToString()));
        keyBindings["����"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("����", KeyCode.LeftAlt.ToString()));
        keyBindings["������"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("������", KeyCode.Z.ToString()));
        keyBindings["�Ͻ�����"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("�Ͻ�����", KeyCode.Escape.ToString()));
    }

    // Ű ���� ����
    public void StartRebinding(string action, Button button)
    {
        keyToRebind = action;

        message.text = "�����ϰ� ���� Ű�� �Է��ϼ���.";
    }
    public void RebindMoveLeft()
    {
        StartRebinding("���� �̵�", moveLeftbutton);
    }

    public void RebindMoveRight()
    {
        StartRebinding("���� �̵�", moveRightbutton);
    }
    public void Rebindattack()
    {
        StartRebinding("����", attackbutton);
    }
    public void Rebindjump()
    {
        StartRebinding("����", jumpbutton);
    }

    public void Rebindroll()
    {
        StartRebinding("������", rollbutton);
    }
    public void Rebindpause()
    {
        StartRebinding("�Ͻ�����", pausebutton);
    }
}
