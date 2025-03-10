using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingResolution : MonoBehaviour
{
    FullScreenMode screenMode;
    public Dropdown resolutionDropdown;
    public Toggle fullscreenbutton;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionnum=0;
    public MenuController menucontroller;
    // Start is called before the first frame update
    void Start()
    {
        InitUI();
    }


    void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            resolutions.Add(Screen.resolutions[i]);
        }
        resolutionDropdown.options.Clear();

        int optionnum = 0;
        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option= new Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRateRatio + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width&&item.height==Screen.height)
            {
                resolutionDropdown.value = optionnum;
                resolutionnum = optionnum;
            }
            optionnum++;
        }
        resolutionDropdown.RefreshShownValue();
        // ���� ȭ�� ��带 �������� Ǯ��ũ�� ��ư �ʱ�ȭ
        fullscreenbutton.isOn = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionnum = x;
    }

    public void FullScreenbutton(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    public void Okbuttonclick()
    {
        if (resolutionnum >= 0 && resolutionnum < resolutions.Count)
        {
            Screen.SetResolution(resolutions[resolutionnum].width, resolutions[resolutionnum].height, screenMode);
            Debug.Log("�ػ� ������ ����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("�ػ� ������ �����߽��ϴ�: resolutionnum�� ��ȿ���� �ʽ��ϴ�.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
