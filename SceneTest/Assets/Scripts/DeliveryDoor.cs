using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 传送门
/// </summary>
public class DeliveryDoor : MonoBehaviour
{
    void OnTriggerEnter(Collider other) //触发一次
    {
        GameObject.Find("LevelText").GetComponent<Text>().text = "过关";
        Time.timeScale = 0;
    }
}
