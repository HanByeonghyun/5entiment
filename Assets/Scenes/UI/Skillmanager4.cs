using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Skillmanager4 : MonoBehaviour
{
    public GameObject vase;         // 정령_0
    public GameObject Text;         // 텍스트
    public GameObject Image1;        // 이미지
    public GameObject Image2;


    // Start is called before the first frame update
    public void Start()
    {
        Text.SetActive(false);
        Image1.SetActive(false);
        Image2.SetActive(false);
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
        Image1.SetActive(true); // 이미지 활성화
        Image2 .SetActive(true);
    }
}
