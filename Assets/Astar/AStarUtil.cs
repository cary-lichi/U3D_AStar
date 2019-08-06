using UnityEngine;
using System;

/// <summary>
///  地图工具类
///  @author cary
/// </summary>
namespace AStar
{
    public static class AStarUtil
    {

        public const int mapTileWidth = 512;
        public const int mapTileHeight = 512;

        public const int mapGridWidth = 64;
        public const int mapGridHeight = 64;

        /// <summary>
        /// 格子转像素
        /// </summary>
        /// <param name="position">需要转换的点</param>
        /// <returns>空则表示直接修改</returns>
        public static Vector2 getPixel(Vector2 position)
        {

            Vector2 result = position;
            result.x = position.x * mapGridWidth + (mapGridWidth >> 1);
            result.y = position.y * mapGridHeight + (mapGridHeight >> 1);
            return result;
        }

        public static Vector2 getPixel(Vector2 position, Vector2 result)
        {
            result.x = position.x * mapGridWidth + (mapGridWidth >> 1);
            result.y = position.y * mapGridHeight + (mapGridHeight >> 1);
            return result;
        }


        public static Vector2 getPixelPoint(Vector2 position)
        {
            Vector2 point = new Vector2();
            point.x = position.x;
            point.y = position.y;
            getPixel(point);
            return point;
        }

        /// <summary>
        /// 像素转格子
        /// </summary>
        /// <param name="position">需要转换的点</param>
        /// <returns>空则表示直接修改</returns>
        public static Vector2 getGrid(Vector2 position)
        {
            Vector2 result = position;

            result.x = (int)Math.Floor(position.x / mapGridWidth);
            result.y = (int)Math.Floor(position.y / mapGridHeight);
            return result;
        }
        public static Vector2 getGrid(Vector2 position, Vector2 result)
        {

            result = position;

            result.x = (int)Math.Floor(position.x / mapGridWidth);
            result.y = (int)Math.Floor(position.y / mapGridHeight);
            return result;
        }

        public static Vector2 getGridPoint(Vector2 position)
        {
            Vector2 point = new Vector2();
            point.x = position.x;
            point.y = position.y;
            getGrid(point);
            return point;
        }

        public static AStarNode[] findPath(float startX, float startY, float endX, float endY)
        {
            Vector2 start = new Vector2(startX, startY);

            AStarUtil.getGrid(start);
            Vector2 end = new Vector2(endX, endY);
            AStarUtil.getGrid(end);
            AStarNode[] path = SceneTerrainMgr.Instance.findPath((int)start.x, (int)start.y, (int)end.x, (int)end.y);
            return path;
        }
    }
}
