using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStar;
using System;

public class Test : MonoBehaviour
{
    private Color m_WalkColor = Color.green;
    private Color m_WallColor = Color.black;
    private Color m_NormalColor = Color.white;
    private Color m_SearchColor = Color.red;

    private float m_CubePadding = 0.2f;
    private Vector3 m_InitPos = new Vector3(0.5f, 0, 0.5f);
    private IMapInfo m_MapData;
    private int[] m_Grides;

    private AStarNode[] m_Path;


    // Start is called before the first frame update
    void Start()
    {
        SceneTerrainMgr instance = SceneTerrainMgr.Instance;

        /**
         * 
         0 ：路
         1 ：障碍物
         2 ：比较艰难的路
         */
        m_Grides = new int[] {
            0,0,0,0,0,1,0,0,0,0,
            0,1,1,1,0,1,0,1,1,0,
            0,0,0,1,0,1,0,1,0,0,
            1,1,0,1,0,1,0,1,0,0,
            0,1,0,1,0,1,0,1,0,0,
            0,1,0,1,0,1,0,1,1,0,
            0,0,0,1,0,1,0,1,1,1,
            1,1,1,1,0,1,0,0,0,0,
            0,0,0,1,0,1,1,1,1,0,
            0,0,0,1,0,0,0,0,0,0,
        };

        MapFlag[] grids = new MapFlag[m_Grides.Length];
        for (int i = 0; i < m_Grides.Length; i++)
        {
            int v = m_Grides[i];
            switch (v)
            {
                case 0:
                    grids[i] = MapFlag.walkable;
                    break;
                case 1:
                    grids[i] = MapFlag.unwalkable;
                    break;
                case 2:
                    grids[i] = MapFlag.shadow;
                    break;
            }
        }

        m_MapData = new IMapInfo()
        {
            gridCols = 10,
            gridRows = 10,
            grids = grids
        };

        instance.InitMap(m_MapData);
        findPath(0, 5, 9, 5);
    }

    private void OnDrawGizmos()
    {
        if (null == m_Grides || m_Grides.Length == 0) return;
        for (int i = 0; i < m_Grides.Length; i++)
        {
            var row = i / m_MapData.gridRows % m_MapData.gridRows;
            var col = i % m_MapData.gridCols;

            int v = m_Grides[i];
            switch ((MapFlag)v)
            {
                case MapFlag.walkable:
                    Gizmos.color = m_NormalColor;
                    break;
                case MapFlag.unwalkable:
                    Gizmos.color = m_WallColor;
                    break;
            }

            var nodeList = AStar.AStar.m_NodeList;
            if (null != nodeList && nodeList.Count > 0)
            {
                for (int k = 0; k < nodeList.Count; k++)
                {
                    var val = nodeList[k];
                    if (val.X == col && val.Y == row)
                    {
                        Gizmos.color = m_SearchColor;
                        break;
                    }
                }
            }

            if (null != m_Path)
            {
                for (int j = 0; j < m_Path.Length; j++)
                {
                    var val = m_Path[j];
                    if (val.X == col && val.Y == row)
                    {
                        Gizmos.color = m_WalkColor;
                        break;
                    }
                }
            }

            var targetPosition = new Vector3(m_InitPos.x + col + m_CubePadding * col, 0, m_InitPos.z - row - m_CubePadding * row);
            Gizmos.DrawCube(targetPosition, Vector3.one);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void findPath(float startX, float startY, float endX, float endY)
    {
        m_Path = AStarUtil.findPath(startX, startY, endX, endY);
        if (m_Path == null)
        {
            Debug.Log("没有找到路径");
        }
        else
        {
            for (int i = 0; i < m_Path.Length; i++)
            {
                AStarNode node = m_Path[i];
                //Debug.Log("(X:" + node.X + "，Y:" + node.Y + ")");
            }
        }
    }
}
