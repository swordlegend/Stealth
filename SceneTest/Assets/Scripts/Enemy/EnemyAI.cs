using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人AI
/// </summary>
public class EnemyAI : MonoBehaviour
{
    ExpressionManager expresMana; //声明表情管理类
    EnemyCharacter enemyChar; //声明敌人角色脚本
    TestingPlayer testing; //声明检测脚本
    SpriteRenderer spriteRen; //声明该组件用来更换表情
    public void Start()
    {
        expresMana = FindObjectOfType<ExpressionManager>();
        enemyChar = GetComponent<EnemyCharacter>();
        testing = GetComponent<TestingPlayer>();
        spriteRen = GetComponentInChildren<SpriteRenderer>();
    }
    public void Update()
    {
        switch (enemyChar.state)
        {
            case EnemyState.XunLuo:
                enemyChar.XunLuo();
                break;
            case EnemyState.ZhuiBu:
                spriteRen.sprite = expresMana.GetExpression("ZhuiBu"); //变表情为追捕
                enemyChar.ZhuiBu();
                break;
            case EnemyState.HunMi:
                spriteRen.sprite = expresMana.GetExpression("HunMi"); //表情变为昏迷              
                enemyChar.HunMi();
                break;
            case EnemyState.JingJue:
                spriteRen.sprite = expresMana.GetExpression("ZhuiBu"); //表情变为追捕
                enemyChar.JingJue();
                break;
            case EnemyState.SouCha:
                spriteRen.sprite = expresMana.GetExpression("SouCha"); //表情变为搜查
                enemyChar.SouCha();
                break;
        }
        testing.RayTesting(); //射线检测玩家
        testing.DrawDraw(); //绘制视锥        
    }
}
