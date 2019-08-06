using System;
using System.Collections.Generic;

namespace AStar
{
    /// <summary>
    /// 寻路地图
    /// @author carry
    /// </summary>
    internal class AStarGrid
    {
        protected AStarNode[][] _grid;
        protected int _cols = 0;
        protected int _rows = 0;
        protected AStarNode _startNode;
        protected AStarNode _endNode;

        internal AStarGrid(int cols, int rows)
        {
            _cols = cols;
            _rows = rows;
            _grid = new AStarNode[cols][];
            for (int i = 0; i < _cols; i++)
            {
                _grid[i] = new AStarNode[rows];
                for (int j = 0; j < _rows; j++)
                {
                    _grid[i][j] = createNode(i, j);
                }
            }
        }

        protected AStarNode createNode(int x, int y)
        {
            return new AStarNode(x, y);
        }


        public int cols => _cols;

        public int rows => _rows;

        public AStarNode startNode
        {
            get
            {
                return _startNode;
            }
            set
            {
                _startNode = value;
            }
        }

        public AStarNode endNode
        {
            get
            {
                return _endNode;
            }
            set
            {
                _endNode = value;
            }
        }


        internal void cacheAroundLinks()
        {
            for (int i = 0; i < _cols; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    AStarNode node = _grid[i][j];
                    node._aroundLinks = new List<AStarLink>();
                    int startX = Math.Max(0, node.X - 1);
                    int endX = Math.Min(_cols - 1, node.X + 1);
                    int startY = Math.Max(0, node.Y - 1);
                    int endY = Math.Min(_rows - 1, node.Y + 1);
                    for (int m = startX; m <= endX; m++)
                    {
                        for (int n = startY; n <= endY; n++)
                        {
                            AStarNode test = _grid[m][n];
                            if (test == node || !test.Walkable || !_grid[node.X][test.Y].Walkable || !_grid[test.X][node.Y].Walkable)
                            {
                                continue;
                            }
                            int cost = AStar.STRAIGHT_COST;
                            if (!((node.X == test.X) || (node.Y == test.Y)))
                            {
                                cost = AStar.DIAG_COST;
                            }
                            node._aroundLinks.Add(new AStarLink(test, cost));
                        }
                    }
                }
            }
        }

        internal AStarNode getNode(int x, int y)
        {
            return _grid[x][y];
        }

        internal void setCostMultiplier(int x, int y, int value)
        {
            _grid[x][y].CostMultiplier = value;
        }

        internal void setWalkable(int x, int y, bool value)
        {
            _grid[x][y].Walkable = value;
        }

        internal void setStartNode(int x, int y)
        {
            _startNode = _grid[x][y];
        }

        internal void setEndNode(int x, int y)
        {
            _endNode = _grid[x][y];
        }

    }

    /// <summary>
    /// 格子属性
    /// @author cary
    /// </summary>

    public class AStarNode
    {
        internal int X = 0;
        internal int Y = 0;
        internal bool Walkable = false;
        internal int CostMultiplier = 0;

        internal int _f = 0;
        internal int _g = 0;
        internal int _h = 0;
        internal AStarNode _parent;
        internal int _checkNum = 0;
        internal List<AStarLink> _aroundLinks;

        internal AStarNode(int x, int y, bool walkable = true, int costMultiplier = 1)
        {
            X = x;
            Y = y;
            Walkable = walkable;
            CostMultiplier = costMultiplier;
        }
    }

    /// <summary>
    /// 可移动到的格子的关系 
    /// @author cary
    /// </summary>
    public class AStarLink
    {

        internal AStarNode Node;
        internal int Cost = 0;

        internal AStarLink(AStarNode node, int cost)
        {
            Node = node;
            Cost = cost;
        }
    }

}
