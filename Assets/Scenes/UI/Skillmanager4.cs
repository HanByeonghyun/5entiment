using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Skillmanager4 : MonoBehaviour
{
    public GameObject vase;         // ����_0
    public GameObject Text;         // �ؽ�Ʈ
    public GameObject Image1;        // �̹���
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
        Text.SetActive(true);  // �ؽ�Ʈ Ȱ��ȭ
        Image1.SetActive(true); // �̹��� Ȱ��ȭ
        Image2 .SetActive(true);
    }
}
