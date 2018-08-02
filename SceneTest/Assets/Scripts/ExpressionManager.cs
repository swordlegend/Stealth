using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 表情管理
/// </summary>
public class ExpressionManager : MonoBehaviour
{
    public List<SpriteRenderer> ExpressionList;
    void Start()
    {
        Object[] Expressions = Resources.LoadAll("Prefab/Emoticon"); //加载所有表情
        foreach (var item in Expressions)
        {
            GameObject expression = Instantiate(item as GameObject); //实例化表情
            expression.transform.SetParent(transform); //作为脚本管理器的子物体
            expression.transform.localPosition = Vector3.zero; //位置居中
            expression.name = expression.GetComponent<SpriteRenderer>().sprite.name; //重新命名
            ExpressionList.Add(expression.GetComponent<SpriteRenderer>()); //添加进集合中
        }
    }
    public Sprite GetExpression(string name) //根据名字获取表情
    {
        foreach (var item in ExpressionList)
        {
            if (item.name == name)
                return item.sprite;
        }
        return null;
    }
}
