using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Framework;

public class NavMapPool : Singleton<NavMapPool>
{
    NavMapPool(){}
    Dictionary<float, NavMap> NavMapDic = new Dictionary<float, NavMap>();
    public List<NavNode> FindPath(Vector2 startPos, Vector2 endPos, float agentRadios)
    {
        if(!NavMapDic.ContainsKey(agentRadios)) NewMap(agentRadios);
        return NavMapDic[agentRadios].FindPath(startPos, endPos);
    }
    private void NewMap(float agentRadios)
    {
        NavMap map = new NavMap(agentRadios);
        map.InitMap();
        NavMapDic.Add(agentRadios, map);
    }
}
