using System;
using UnityEngine;

namespace RQ.Animation.Tile
{
    [Serializable]
    public class AnimatedMaterial
    {
        public Material Material;
        //[NonSerialized]
        //public Texture TileSet;
        //public int TileSetWidth;
        //public int TileSetHeight;
        public string Name;
        //public int TileCountX;
        //public int TileCountY;
        public int Frames;
        public int FirstIndex;
        public AnimatedTile[] AnimatedTiles;
        //[NonSerialized]
        //public int MaterialIndex { get; set; }
    }
}
