using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 射线检测玩家
/// </summary>
public class TestingPlayer : MonoBehaviour
{
    EnemyCharacter enemychar;
    EnemyState state;
    Mesh mesh; //声明Mesh组件
    void Start()
    {
        enemychar = GetComponent<EnemyCharacter>();
        state = enemychar.state; //获取同伴状态
        mesh = gameObject.GetComponent<MeshFilter>().mesh; //添加MeshFilter组件并找到mesh
       // gameObject.AddComponent<MeshRenderer>(); //添加MeshRenderer组件,使其可见
    }
    void NoticeAllEnemy(EnemyState state) //通知所有敌人
    {
        EnemyCharacter[] enemychars = FindObjectsOfType<EnemyCharacter>();
        foreach (var item in enemychars)
        {
            item.state = state;
        }
    }
    public void RayTesting() //扇形射线检测玩家(幅度为72度)
    {
        RaycastHit hit; //声明照射点
        Vector3 StartDir = Quaternion.AngleAxis(-36, Vector3.up) * transform.forward; //开始方向
        //从开始方向每3度发射一根射线
        for (int i = 0; i <= 72;)
        {
            //往当前方向右偏移i度发射一根射线
            Vector3 dir = Quaternion.AngleAxis(i, Vector3.up) * StartDir;
            if (Physics.Raycast(transform.position, dir, out hit, 3)) //如果打到东西
            {
                if (hit.collider.tag == "Player") //如果是玩家                    
                    NoticeAllEnemy(EnemyState.ZhuiBu); //所有的敌人都成为追捕状态
                //追捕状态以外,如果发现昏迷的同伴
                else if (state != EnemyState.ZhuiBu && hit.collider.tag == "Enemy" && !hit.collider.GetComponent<EnemyAI>())
                    if (enemychar.hunMiEnemy == null) //如果之前没有昏迷的同伴
                    {
                        enemychar.hunMiEnemy = hit.transform; //获取昏迷的同伴                               
                        NoticeAllEnemy(EnemyState.JingJue); //所有的敌人进入警觉状态
                    }
            }
            i += 3;
        }
    }

    public void DrawDraw()
    {
        Vector3 p = transform.position; //当前位置
        Quaternion r = Quaternion.AngleAxis(-transform.eulerAngles.y, Vector3.up); //当前旋转角度的反方向
        Vector3[] ds = new Vector3[25]; //25个方向
        RaycastHit[] hits = new RaycastHit[25]; //25个照射点
        Vector3[] vp = new Vector3[26]; //26个图形顶点坐标
        vp[0] = p - p; //第一个坐标点与脚本物体重合
        //自定义25个方向,绘制射线,设置图形后面25个顶点坐标
        for (int i = 0; i < ds.Length; i++)
        {
            ds[i] = Quaternion.AngleAxis(i * 3 - 36, Vector3.up) * transform.forward;          
            vp[i + 1] = Physics.Raycast(p, ds[i], out hits[i], 3) ? r * (hits[i].point - p) : r * ds[i] * 3;
            //Debug.DrawLine(p, Physics.Raycast(p, v3s[i], out hits[i], 10) ? hits[i].point : p + v3s[i] * 10); //绘制射线
        }
        mesh.vertices = vp; //根据顶点进行绘制      
        mesh.triangles = VertexRanking((ds.Length - 1) * 3); //顶点排序,每3个绘制成一个三角形

        // mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 ,0,5,6,
        //                               0,6,7, 0,7,8,0,8,9,0,9,10,0,10,11,
        //                               0,11,12,0,12,13,0,13,14,0,14,15,0,15,16,
        //                               0,16,17,0,17,18,0,18,19,0,19,20,0,20,21,
        //                               0,21,22, 0,22,23, 0,23,24,0,24,25}; //顶点排序,每3个绘制成一个三角形
    }
    int[] VertexRanking(int length) //顶点排序
    {
        int offs = -2;    
        int[] vertexs = new int[length]; //获取所有顶点
        for (int i = 0; i < vertexs.Length; i++)
        {
            if (i % 3 == 0) //当i为0,3,6,9...时,该索引处值为0,offs+2
            {
                vertexs[i] = 0;
                offs += 2;
            }
            else  
                vertexs[i] =i - offs;
        }
        return vertexs;
    }
}
