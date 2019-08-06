using System;
using System.Collections.Generic;

namespace AStar
{

    /// <summary>
    /// 寻路
    /// @author cary
    /// </summary>
    internal class AStar
    {

        public static List<AStarNode> m_NodeList = new List<AStarNode>();

        /// <summary>
        /// 上下左右的移动成本
        /// </summary>
        internal static int STRAIGHT_COST = 10;

        /// <summary>
        /// 斜角的移动成本
        /// </summary>
        internal static int DIAG_COST = 14;

        /// <summary>
        /// 曼哈顿启发函数
        /// </summary>
        internal static int manhattan(AStarNode node1, AStarNode node2)
        {
            int dx = node1.X > node2.X ? node1.X - node2.X : node2.X - node1.X;
            int dy = node1.Y > node2.Y ? node1.Y - node2.Y : node2.Y - node1.Y;
            return (dx + dy) * AStar.STRAIGHT_COST;
        }

        /// <summary>
        /// 欧式启发函数
        /// </summary>

        internal static int euclidian(AStarNode node1, AStarNode node2)
        {
            int dx = node1.X > node2.X ? node1.X - node2.X : node2.X - node1.X;
            int dy = node1.Y > node2.Y ? node1.Y - node2.Y : node2.Y - node1.Y;
            return (dx * dx + dy * dy) * AStar.STRAIGHT_COST;
        }

        /// <summary>
        /// 对角启发函数
        /// </summary>
        internal static int diagonal(AStarNode node1, AStarNode node2)
        {
            int dx = node1.X > node2.X ? node1.X - node2.X : node2.X - node1.X;
            int dy = node1.Y > node2.Y ? node1.Y - node2.Y : node2.Y - node1.Y;
            return dx > dy ? AStar.DIAG_COST * dy + AStar.STRAIGHT_COST * (dx - dy) : AStar.DIAG_COST * dx + AStar.STRAIGHT_COST * (dy - dx);
        }


        private Func<AStarNode, AStarNode, int> _heuristic;

        protected AStarGrid _grid;

        private AStarNode _startNode;
        private AStarNode _endNode;

        private int _nowCheckNum = 1;
        private BinaryHeaps<AStarNode> _binaryHeaps;

        private List<AStarNode> _path;

        internal AStar(Func<AStarNode, AStarNode, int> heuristic = null)
        {
            _binaryHeaps = new BinaryHeaps<AStarNode>(compare);
            if (heuristic == null)
            {
                _heuristic = AStar.manhattan;
            }
            else
            {
                _heuristic = heuristic;
            }
        }

        private int compare(AStarNode a, AStarNode b)
        {
            return b._f - a._f;
        }

        internal AStarNode[] path => _path.ToArray();


        internal bool findPath(AStarGrid grid)
        {
            if (_grid != null)
            {
                clear();
            }
            _grid = grid;
            _startNode = _grid.startNode;
            _endNode = _grid.endNode;
            _startNode._g = 0;
            _startNode._h = _heuristic(_startNode, _endNode);
            _startNode._f = _startNode._g + _startNode._h;
            return search();
        }

        protected bool search()
        {
            AStarNode node = _startNode;
            while (node != _endNode)
            {
                List<AStarLink> aroundLinks = node._aroundLinks;
                for (int i = 0, len = aroundLinks.Count; i < len; i++)
                {
                    AStarNode test = aroundLinks[i].Node;
                    //记录路径
                    m_NodeList.Add(test);

                    int cost = aroundLinks[i].Cost;
                    int g = node._g + cost;
                    int h = _heuristic(test, _endNode);
                    int f = g + h;
                    if (test._checkNum == _nowCheckNum)
                    {
                        if (test._f > f)
                        {
                            test._f = f;
                            test._g = g;
                            test._h = h;
                            test._parent = node;
                            _binaryHeaps.modify(test, test);
                        }
                    }
                    else
                    {
                        test._f = f;
                        test._g = g;
                        test._h = h;
                        test._parent = node;
                        _binaryHeaps.enqueue(test);
                        test._checkNum = _nowCheckNum;
                    }
                }
                node._checkNum = _nowCheckNum;
                if (_binaryHeaps.Length == 0)
                {
                    _nowCheckNum++;
                    return false;
                }
                node = _binaryHeaps.dequeue();
            }
            buildPath();
            _nowCheckNum++;
            return true;
        }

        private void buildPath()
        {
            _path = new List<AStarNode>();
            AStarNode node = _endNode;
            _path.Add(node);
            while (node != _startNode)
            {
                node = node._parent;
                _path.Add(node);
            }
            _path.Reverse();
        }

        internal void clear()
        {
            _grid = null;
            _startNode = null;
            _endNode = null;
            _binaryHeaps.Dispose();
            _path = null;
        }
    }
}
