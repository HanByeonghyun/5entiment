using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Skillmanager : MonoBehaviour
{
    public GameObject vase;         // 정령_0
    public GameObject Text;         // 텍스트
    public GameObject Image;        // 이미지
    public void Start()
    {
        Text.SetActive(false);
        Image.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (vase == null)
        {
            ActivateUI();
        }
    }
    private void ActivateUI()
    {
        Text.SetActive(true);  // 텍스트 활성화
        Image.SetActive(true); // 이미지 활성화
    }
}
