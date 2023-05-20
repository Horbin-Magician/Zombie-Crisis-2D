using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum E_Node_Type
{
    walk,
    stop,
}

public class NavNode
{
    // 格子对象的坐标
    public int x;
    public int y;
    public float f; // 寻路消耗
    public float g; // 离起点的距离
    public float h; // 离终点的距离
    public NavNode father; // 父对象
    public E_Node_Type type; // 格子类型

    public NavNode(int x, int y, E_Node_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}
