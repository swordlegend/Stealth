using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家控制类
/// </summary>
public class PlayerController : MonoBehaviour
{
    PlayerCharacter playerchar;
    Rigidbody rigid;
    void Start()
    {
        playerchar = GetComponent<PlayerCharacter>();
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirY = Input.GetAxis("Vertical");
        if (dirX != 0 || dirY != 0)
            playerchar.Move(dirX, dirY);
        if (Input.GetKeyDown(KeyCode.Space)) //按空格拖拽昏迷的敌人
            playerchar.CarryEnemy();
    }
    void OnCollisionStay(Collision other)//碰撞中
    {
        if (other.collider.tag == "Enemy") //如果碰撞的是敌人
        {
            rigid.WakeUp(); //刚体保持清醒状态          
            if (playerchar.carry == false) //如果空着手
            {              
                playerchar.inCarry = true; //进入拖拽范围
                playerchar.enemy = other.transform; //获取该敌人引用 
                playerchar.Attack(); //攻击敌人 
            }
        }
    }
    void OnCollisionExit(Collision other)//离开碰撞时
    {
        if (other.collider.tag == "Enemy") //如果离开的是敌人
        {
            playerchar.InitializationSchedule(); //初始化进度条
            if (playerchar.carry == false) //如果空着手
            {
                playerchar.inCarry = false; //离开拖拽范围
                playerchar.enemy = null; //失去该敌人引用 
            }
        }
    }
}