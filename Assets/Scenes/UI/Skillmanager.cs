using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Skillmanager : MonoBehaviour
{
    public GameObject vase;         // ����_0
    public GameObject Text;         // �ؽ�Ʈ
    public GameObject Image;        // �̹���
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
        Text.SetActive(true);  // �ؽ�Ʈ Ȱ��ȭ
        Image.SetActive(true); // �̹��� Ȱ��ȭ
    }
}
