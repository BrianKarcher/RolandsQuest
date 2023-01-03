using RQ.Common;
using RQ.Physics;
using System;
using UnityEngine;
namespace RQ.Model
{
    public interface ICameraClass : IBaseObject
    {
        Vector2 GetScreenSize();
        Rect GetViewport();
        void SetMaxBounds(int tileMapWidth, int tileMapHeight);
        bool IsPosInViewport(Vector2D pos);
        float ShakeAmount { get; set; }
        float ShakeInterval { get; set; }
        //void SetPos(Vector3 newpos);
        Coroutine StartCoroutine(string name);
        void StopCoroutine(string name);
        void SetClamping(bool isClamping);
    }
}
