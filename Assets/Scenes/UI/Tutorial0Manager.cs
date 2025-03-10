using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial0Manager : MonoBehaviour
{

    public GameObject vase; // ���_0
    public GameObject AttackText; // Attack �ؽ�Ʈ
    public GameObject Attack;     // Attack �̹���
    public GameObject SummonText;
    public SpriteRenderer portalSpriteRenderer; // ��Ż�� SpriteRenderer
    public GameObject portalText; // ��Ż �ؽ�Ʈ
    public WispSceneManager wispSceneManager; // WispSceneManager ������Ʈ ����

    private bool isActivated = false; // ���� UI Ȱ��ȭ ����
    private bool isPortalTextActivated = false; // ��Ż �ؽ�Ʈ Ȱ��ȭ ����

    public void Start()
    {
        // �ʱ�ȭ: �ؽ�Ʈ�� �̹����� ��Ȱ��ȭ
        AttackText.SetActive(false);
        SummonText.SetActive(false);
        Attack.SetActive(false);
        portalText.SetActive(false);

        // ��Ż�� SpriteRenderer�� ������ ��� �ʱ� ��Ȱ��ȭ ���� Ȯ��
        if (portalSpriteRenderer != null)
        {
            portalSpriteRenderer.enabled = false; // ��Ż �ʱ� ��Ȱ��ȭ
        }
    }
    public void Update()
    {
        if (vase == null && !isActivated)
        {
            ActivateAttackUI(); // UI Ȱ��ȭ �޼��� ȣ��
            isActivated = true; // �� ���� ����ǵ��� ����

            // WispSceneManager�� StartCutscene ȣ��
            if (wispSceneManager != null)
            {
                wispSceneManager.StartCutscene(); // �ƾ� ����
            }
        }
        if (portalSpriteRenderer != null && portalSpriteRenderer.enabled && !isPortalTextActivated)
        {
            ActivatePortalText(); // ��Ż �ؽ�Ʈ Ȱ��ȭ
            isPortalTextActivated = true; // �� ���� ����ǵ��� ����
        }
    }
    // UI Ȱ��ȭ �޼���
    private void ActivateAttackUI()
    {
        AttackText.SetActive(true); // "���� : ctrl" �ؽ�Ʈ Ȱ��ȭ
        SummonText.SetActive(true); // �߰� ���� �ؽ�Ʈ Ȱ��ȭ
        Attack.SetActive(true); // "CTRL" Ű �̹��� Ȱ��ȭ
    }
    // ��Ż �ؽ�Ʈ Ȱ��ȭ �޼���
    private void ActivatePortalText()
    {
        portalText.SetActive(true); // "��Ż ���� �ؽ�Ʈ" Ȱ��ȭ
    }
}
