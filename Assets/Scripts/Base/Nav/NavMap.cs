using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMap
{
    private float mapW = 19f;
    private float mapH = 11f;
    private Vector2 nodeOrigin;
    private float mapDelta = 0.5f;
    private float agentRadios;
    private int nodeNumW;
    private int nodeNumH;
    private NavNode[,] nodes;
    private List<NavNode> openList = new List<NavNode>();
    private List<NavNode> closeList = new List<NavNode>();
    public NavMap(float agentRadios)
    {
        this.agentRadios = agentRadios;
    }

    public void InitMap()
    {
        nodeNumW = (int)Mathf.Ceil(mapW/mapDelta) * 2;
        nodeNumH = (int)Mathf.Ceil(mapH/mapDelta) * 2;
        nodeOrigin = new Vector2(-(nodeNumW/2 - 0.5f)*mapDelta, -(nodeNumH/2 - 0.5f)*mapDelta);
        
        nodes = new NavNode[nodeNumW, nodeNumH];
        for(int i=0; i < nodeNumW; ++i){
            for(int j=0; j < nodeNumH; ++j){
                NavNode navNode = CreateNode(i, j);
                nodes[i, j] = navNode;
            }
        }
    }

    public NavNode CreateNode(int x, int y)
    {
        Vector2 mapPos =  NodeToMap(new Vector2Int(x, y));
        Collider2D collider = Physics2D.OverlapCircle(new Vector2(mapPos.x, mapPos.y), agentRadios, LayerMask.GetMask("Obstacle"));

        if(collider != null) return new NavNode(x, y, E_Node_Type.stop);
        else return new NavNode(x, y, E_Node_Type.walk);
    }

    public List<NavNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        // 判断startPos, endPos合法性
        if(Mathf.Abs(startPos.x) > -nodeOrigin.x || Mathf.Abs(endPos.x) > -nodeOrigin.x) return null;
        if(Mathf.Abs(startPos.y) > -nodeOrigin.y || Mathf.Abs(endPos.y) > -nodeOrigin.y) return null;
        // 转换pos
        Vector2Int startNodePos = MapToNode(startPos);
        Vector2Int endNodePos = MapToNode(endPos);
        // 获取Node
        NavNode startNode = nodes[startNodePos.x, startNodePos.y];
        NavNode endNode = nodes[endNodePos.x, endNodePos.y];
        // 判断是否可走，若不可走选择附近点
        if(endNode.type == E_Node_Type.stop){
            for(int i=0; i < 3; ++i){
                for(int j=0; j < 3; ++j){
                    NavNode node = nodes[endNode.x-1 + i, endNode.y-1 + j];
                    if(node.type == E_Node_Type.walk){
                        endNode = node;
                        break;
                    }
                }
                if(endNode.type == E_Node_Type.walk) break;
            }
            if(endNode.type == E_Node_Type.stop) return null;
        }
        // 初始化
        openList.Clear();
        closeList.Clear();

        startNode.father = null;
        startNode.f = 0;
        startNode.g = 0;
        startNode.h = 0;
        closeList.Add(startNode);
        // 循环查找
        while(true)
        {
            AddNodeToOpenList(startNode.x-1, startNode.y-1, startNode, endNode);
            AddNodeToOpenList(startNode.x-1, startNode.y+1, startNode, endNode);
            AddNodeToOpenList(startNode.x+1, startNode.y+1, startNode, endNode);
            AddNodeToOpenList(startNode.x+1, startNode.y-1, startNode, endNode);
            AddNodeToOpenList(startNode.x, startNode.y-1, startNode, endNode);
            AddNodeToOpenList(startNode.x-1, startNode.y, startNode, endNode);
            AddNodeToOpenList(startNode.x, startNode.y+1, startNode, endNode);
            AddNodeToOpenList(startNode.x+1, startNode.y, startNode, endNode);
            
            if(openList.Count == 0) return null;
            NavNode minfNode = null;
            foreach (NavNode node in openList){
                if(minfNode == null) minfNode = node;
                else if(minfNode.f > node.f) minfNode = node;
            }
            closeList.Add(minfNode);
            openList.Remove(minfNode);
            startNode = minfNode;

            if(startNode == endNode)
            {
                List<NavNode> path = new List<NavNode>();
                path.Add(endNode);
                while(endNode.father != null)
                {
                    path.Add(endNode.father);
                    endNode = endNode.father;
                }
                path.Reverse();

                return path;
            }
        }
    }

    private Vector2Int MapToNode(Vector2 mapPos)
    {
        return new Vector2Int((int)((mapPos.x - nodeOrigin.x)/mapDelta), (int)((mapPos.y - nodeOrigin.y)/mapDelta));
    }

    public Vector2 NodeToMap(Vector2Int nodePos)
    {
        return new Vector2(nodeOrigin.x + nodePos.x*mapDelta, nodeOrigin.y + nodePos.y*mapDelta);
    }

    private void AddNodeToOpenList(int x, int y, NavNode father, NavNode end)
    {
        NavNode node = nodes[x, y];

        // TODO 优化：用node属性代替contains判断
        if(node == null || 
            node.type == E_Node_Type.stop ||
            closeList.Contains(node) ||
            openList.Contains(node))
            return;

        //计算f = h, 贪婪最佳优先搜索
        node.father = father;
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
        node.f = node.h;
        openList.Add(node);
    }
}
