using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGenerator : MonoBehaviour
{
    public GameObject firePrefab;
    public float span = 1.0f;
    public float delta = 0;

    //운석이 생성될 범위
    public int minRange = -9;
    public int maxRange = 25;

    void Start()
    {
        
    }

    void Update()
    {
        this.delta += Time.deltaTime;
        if (this.delta > this.span)
        {
            this.delta = 0;
            GameObject fire1 = Instantiate(firePrefab);
            GameObject fire2 = Instantiate(firePrefab);
            //범위 중 무작위 좌표 선택 후 생성
            int px1 = Random.Range(minRange, maxRange);
            int px2 = Random.Range(minRange, maxRange);

            if (px1 == px2)
            {
                px2 = Random.Range(-9, 25);
            }

            fire1.transform.position = new Vector3(px1, 18, 0);
            fire2.transform.position = new Vector3(px2, 18, 0);
        }
    }
}
