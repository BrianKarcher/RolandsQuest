using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Animation.Tile
{
    [Serializable]
    public class AnimatedTileset
    {
        public Texture TileSet;
        public int TileSetWidth;
        public int TileSetHeight;
        public List<AnimatedMaterial> AnimatedMaterials;
    }
}
