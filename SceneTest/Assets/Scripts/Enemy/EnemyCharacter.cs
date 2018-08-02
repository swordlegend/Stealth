using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
/// <summary>
/// 敌人角色类
/// </summary>
public enum EnemyState
{
    ZhanGang, //站岗
    XunLuo, //巡逻   
    ZhuiBu, //追捕
    HunMi, //昏迷
    JingJue, //警觉
    SouCha, //搜查
}
public class EnemyCharacter : MonoBehaviour
{
    public EnemyState state; //声明状态

    public Transform[] fixedPoint; //固定寻路点,在编辑器拖进去
    int fixedIndex = 0; //固定寻路点数组索引
    Transform randomPoints; //所有随机寻路点的父物体
    Transform[] randomPoint; //随机寻路点
    int randomIndex = 0; //随机寻路点数组索引
    NavMeshAgent agent; //寻路组件

    Transform player; //玩家
    public Transform hunMiEnemy; //获取昏迷的同伴
    
    private void Start()
    {
        //获取寻路组件并禁用
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

        //为随机寻路点数组赋值
        randomPoints = GameObject.Find("RandomPoints").transform;//获取所有随机寻路点的父物体
        randomPoint = new Transform[randomPoints.childCount]; //实例化寻路点数组
        for (int i = 0; i < randomPoint.Length; i++) //将所有随机寻路点都放进去
        {
            randomPoint[i] = randomPoints.GetChild(i);
        }
        randomIndex = Random.Range(0, randomPoint.Length); //随获取一个寻路点索引
        player = GameObject.Find("Player").transform; //获取玩家      
    }

    //*****************************************巡逻*****************************************
    public void XunLuo()
    {
        LookAtTarget(fixedPoint[fixedIndex], 2); //面向寻路点进发
        //接近寻路点,去往下个寻路点
        if ((transform.position - fixedPoint[fixedIndex].position).magnitude <= 0.1f)
        {
            if (fixedIndex < fixedPoint.Length - 1) //索引受数组长度限制
                fixedIndex++;
            else
                fixedIndex = 0;
        }
        transform.Rotate(0, Time.deltaTime, 0);//巡逻时让敌人始终从右方转身
        //面向前方(自动寻路转身太慢)
        Quaternion dir = Quaternion.LookRotation(fixedPoint[fixedIndex].position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, dir, 100 * Time.deltaTime);
    }
    //*****************************************追捕*****************************************
    public void ZhuiBu()
    {       
        LookAtTarget(player, 3); //去往玩家处
        if ((transform.position - player.position).magnitude <= 0.5f)
        {
            GameObject.Find("LevelText").GetComponent<Text>().text = "GameOver";
            Time.timeScale = 0;
        }
    }
    //*****************************************昏迷*****************************************
    public void HunMi()
    {
        Destroy(GetComponent<EnemyAI>()); //销毁AI
        Destroy(GetComponent<MeshRenderer>()); //销毁视锥渲染
        Destroy(agent); //销毁寻路组件
    }
    //*****************************************警觉*****************************************
    public void JingJue()
    {    
        if ((transform.position - hunMiEnemy.position).magnitude >= 0.5f) //如果和昏迷同伴距离较远
            LookAtTarget(hunMiEnemy, 3); //去往昏迷同伴处
        else //否则进入搜查状态
        {
            agent.enabled = false;//禁用寻路组件
            state = EnemyState.SouCha;
        }
    }
    //*****************************************搜查*****************************************
    float timer = 0; //记录每次观望的时间
    float Looktimer = 0; //记录观望总时长
    int random = 5; //随机数决定转头方向
    int lastRandom; //记录上一次随机数 
    public void SouCha()
    {  
        if (Looktimer < 10) //原地随机观察10秒
        {
            Looktimer += Time.deltaTime; //记录观望总时间
            timer += Time.deltaTime; //开始计时
            if (timer >= 2) //每观望2秒,随机换一个观望方向
            {
                //给一个和上次不同的随机数
                do { random = Random.Range(0, 4); }
                while (random == lastRandom);
                lastRandom = random;
                timer -= 2;
            }
            switch (random) //根据随机数旋转不同观望方向
            {
                case 0:
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.1f);
                    break;
                case 1:
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.1f);
                    break;
                case 2:
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.1f);
                    break;
                case 3:
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.1f);
                    break;
            }          
        }
        else //开始随机寻路
        {
            LookAtTarget(randomPoint[randomIndex], 1); //面向寻路点进发
            //接近寻路点,去往下个寻路点
            if ((transform.position - randomPoint[randomIndex].position).magnitude <= 0.1f)
                randomIndex = Random.Range(0, randomPoint.Length); //随获取一个寻路点索引
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void LookAtTarget(Transform target, float speed) //寻路
    {
        if (agent.enabled == false) //启动寻路组件
            agent.enabled = true;
        agent.speed = speed;
        agent.SetDestination(target.position);
    }    
}
