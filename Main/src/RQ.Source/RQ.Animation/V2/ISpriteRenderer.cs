//using RQ.Entities.Common;
using RQ.Physics;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace RQ.AnimationV2
{
    public interface ISpriteRenderer
    {
        string GetClipName();
        void SetMaterial(Material material);
        string[] GetAllClipNames();
        Vector3 GetPosition();
        Vector2D GetOrigSpritePosition();
        List<SpriteAnimationType> GetStoredSpriteAnimations();
        string GetIdByType(string type);
        SpriteAnimation GetAnimation(string id, Direction direction);
        bool Render(string id);
        bool Render(string id, Direction direction);
        bool Render(Direction direction);
        bool RenderByName(SpriteAnimation animation);
        bool RenderPrevious();
        //AnimationType GetAnimationType();
        void ProcessDirectionChange(Direction direction);
        void SetColor(string name, Color color);
        void RemoveColor(string name);
        void SetPosition(Vector3 localPosition);
        void SetPosition(Vector2D localPosition);
        void SetSpriteOffsetPos(Vector2D pos);
        event Action AnimComplete;
        //void Start();

        //void Awake();

        int GetClipIdByName(string animationName);

        void SetDirection(Direction direction);
    }
}
