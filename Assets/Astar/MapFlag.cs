namespace AStar
{
    /// <summary>
    /// 地图格子类型
    /// @author cary
    /// </summary>
    public enum MapFlag
    {
        /// <summary>
        /// 可行走 
        /// </summary>
        walkable,

        /// <summary>
        /// 不可行走 
        /// </summary>
        unwalkable,

        /// <summary>
        /// 可通过有遮挡 
        /// </summary>
        shadow,
    }
}
