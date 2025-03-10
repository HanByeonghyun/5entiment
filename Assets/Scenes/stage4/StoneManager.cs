using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MonoBehaviour
{

    public GameObject stonePrefab;
    public float span = 3.0f;
    public float delta = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //3�ʸ��� ������ ��ǥ�� ��ֹ� ����
        this.delta += Time.deltaTime;
        if (this.delta > this.span)
        {
            this.delta = 0;
            GameObject stone1 = Instantiate(stonePrefab);

            stone1.transform.position = new Vector3(-12, 13, 0);
        }
    }
}
