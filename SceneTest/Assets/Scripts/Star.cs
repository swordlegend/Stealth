using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 星星类
/// </summary>
public class Star : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 120 * Time.deltaTime, 0);
    }
    void OnTriggerEnter(Collider other) //触发一次
    {
        ReviseUI();
        GameObject starBoom = GameObject.Find("StarBoom");
        starBoom.transform.position = transform.position + Vector3.up;
        starBoom.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }
    void ReviseUI()
    {
        Image image;
        Transform starPanel = GameObject.Find("StarPanel").transform; //获取星星UI的父物体StarPanel
        for (int i = 0; i < starPanel.childCount; i++)
        {
            image = starPanel.GetChild(i).GetComponent<Image>(); //获取星星UI的Image组件
            if (image.color.r == 0) //如果颜色为0,则修改
            {
                image.color = new Color(1, 1, 1, 1);
                if (i < starPanel.childCount - 1) //如果不是最后一个星星直接退出方法,
                    return;
                else //如果是最后一个星星则打开传送门
                   GameObject.Find("DeliveryDoor").transform.Find("Door").gameObject.SetActive(true);
            }
        }
    }
}
