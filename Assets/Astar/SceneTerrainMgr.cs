namespace AStar
{
    /// <summary>
    /// 地形管理器
    /// @author cary
    /// </summary>
    internal class SceneTerrainMgr
    {
        private AStar m_astar;
        private AStarGrid m_grid;

        private int m_cols;
        private int m_rows;
        private MapFlag[] m_mapGrids;
        private static SceneTerrainMgr m_instance;

        private SceneTerrainMgr()
        {
            m_astar = new AStar();
        }

        public static SceneTerrainMgr Instance
        {
            get
            {
                if (SceneTerrainMgr.m_instance == null)
                {
                    SceneTerrainMgr.m_instance = new SceneTerrainMgr();
                }
                return SceneTerrainMgr.m_instance;
            }
        }

        public void InitMap(IMapInfo mapData)
        {
            m_cols = mapData.gridCols;
            m_rows = mapData.gridRows;
            m_mapGrids = mapData.grids;

            AStarGrid grid = m_grid = new AStarGrid(m_cols, m_rows);
            for (int i = 0; i < m_cols; i++)
            {
                for (int j = 0; j < m_rows; j++)
                {
                    grid.setWalkable(i, j, m_mapGrids[i + j * m_cols] != MapFlag.unwalkable);
                }
            }
            grid.cacheAroundLinks();
        }

        public bool isWalkable(int x, int y)
        {
            int index = x + y * m_cols;
            if (index < 0 || index > m_mapGrids.Length - 1)
            {
                return false;
            }
            return m_mapGrids[index] != MapFlag.unwalkable;
        }

        public bool isShadow(int x, int y)
        {
            int index = x + y * m_cols;
            if (index < 0 || index > m_mapGrids.Length - 1)
            {
                return false;
            }
            return m_mapGrids[index] == MapFlag.shadow;
        }

        public AStarNode[] findPath(int startX, int startY, int endX, int endY)
        {
            m_grid.setStartNode(startX, startY);
            m_grid.setEndNode(endX, endY);
            bool find = m_astar.findPath(m_grid);
            if (find)
            {
                AStarNode[] path = new AStarNode[m_astar.path.Length - 1];
                for (int i = 0; i < path.Length; i++)
                {
                    path[i] = m_astar.path[i + 1];
                }

                return path;
            }
            return null;
        }


        public void clear()
        {
            m_mapGrids = null;
            m_grid = null;
        }
    }
}
