using RQ.Enum;
using RQ.Physics.Components;

namespace RQ.Physics.Pathfinding
{
    public class Graph
    {
        public static int GetGraphMask(FloorComponent source, FloorComponent target)
        {
            LevelLayer? targetLevel = null;
            LevelLayer sourceLevel = source.GetLevel();
            if (target != null)
                targetLevel = target.GetLevel();
            return GetGraphMask(sourceLevel, targetLevel);
        }

        public static int GetGraphMask(LevelLayer sourceLevel, LevelLayer? targetLevel)
        {
            //int graphMask;

            if (targetLevel == null)
            {
                // Graph Shared
                return (1 << 2);
            }

            if (sourceLevel == targetLevel)
            {
                if (sourceLevel == Enum.LevelLayer.LevelOne)
                {
                    // Graph 1
                    return (1 << 0);
                }
                else if (sourceLevel == Enum.LevelLayer.LevelTwo)
                {
                    // Graph 2
                    return (1 << 1);
                }
                else if (sourceLevel == Enum.LevelLayer.LevelThree)
                {
                    // Graph 3
                    return (1 << 3);
                }
                else
                {
                    // Graph 2
                    return (1 << 1);
                }
            }
            else
            {
                // Graph Shared
                return (1 << 2);
            }

            //return 0;
        }

        //public static int GetGraphMask(CollisionComponent entity, CollisionComponent target)
        //{
        //    int graphMask;

        //    if (target != null)
        //    {
        //        var followLevel = target.GetLevel();
        //        var entityLevel = entity.GetLevel();
        //        if (followLevel == entityLevel)
        //        {
        //            if (entityLevel == Enum.LevelLayer.LevelOne)
        //            {
        //                // Graph 1
        //                graphMask = (1 << 3);
        //            }
        //            else if (entityLevel == Enum.LevelLayer.LevelTwo)
        //            {
        //                // Graph 2
        //                graphMask = (1 << 3);
        //            }
        //            else
        //            {
        //                // Graph 3
        //                graphMask = (1 << 3);
        //            }
        //        }
        //        else
        //        {
        //            // Graph 3
        //            graphMask = (1 << 3);
        //        }
        //    }
        //    else
        //    {
        //        // Graph 3
        //        graphMask = (1 << 3);
        //    }

        //    return graphMask;
        //}
    }
}
