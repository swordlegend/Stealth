using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家角色类
/// </summary>
public class PlayerCharacter : MonoBehaviour
{
    float moveSpeed = 2.5f; //移动速度
    public Transform enemys; //在编辑器将所有敌人的父物体"Enemys"拖进去
    public Transform enemy; //声明敌人
    public GameObject schedule; //在编辑器界面把进度条"Schedule"拖进去进度条
    ExpressionManager expresMana; //声明表情管理类
    SpriteRenderer spriteRen; //声明用来改变玩家表情
    public bool inCarry; //判断是否进入拖拽范围
    public bool carry; //判断是否正在拖拽
    Vector3 scheduleV3;
    void Start()
    {
        expresMana = FindObjectOfType<ExpressionManager>();
        spriteRen = GetComponentInChildren<SpriteRenderer>();
        scheduleV3 = schedule.transform.localScale;
    }
    public void Move(float x, float z) //移动方法
    {
        transform.Translate(x * moveSpeed * Time.deltaTime, 0, z * moveSpeed * Time.deltaTime);
    }
    public void Attack() //攻击
    {
        if (enemy.GetComponent<EnemyAI>())//只要不是昏迷
        {
            //显示进度条并递减
            schedule.SetActive(true);
            scheduleV3.z -= Time.deltaTime;
            schedule.transform.localScale = scheduleV3;
            if (schedule.transform.localScale.z <= 0) //进度条减完,使敌人昏迷
            {
                enemy.GetComponent<EnemyCharacter>().state = EnemyState.HunMi; //敌人进入昏迷状态
                schedule.SetActive(false); //隐藏进度条
            }
        }
    }
    public void CarryEnemy() //拖拽敌人
    {
        if (!carry && inCarry && !enemy.GetComponent<EnemyAI>()) //如果空着手,在拖拽范围内,且敌人处于昏迷状态
        {
            enemy.SetParent(transform); //作为自己的子物体,跟随自己移动
            moveSpeed = 1f; //限制速度
            carry = true; //正在拖拽
            spriteRen.sprite = expresMana.GetExpression("Player1");
        }
        else if(enemy) //敌人引用不为空
        {
            enemy.SetParent(enemys);
            moveSpeed = 2.5f;
            carry = false; //回到空手状态
            spriteRen.sprite = expresMana.GetExpression("Player2");
        }
    }
    public void InitializationSchedule() //初始化进度条
    {
        scheduleV3.z = 1; //长度变为1
        schedule.transform.localScale = scheduleV3;
        schedule.SetActive(false); //禁用
    }
}
