////using RQ.Entities.Common;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace RQ.AnimationV2
//{
//    [AddComponentMenu("RQ/AnimatorV2/Manual Animator")]
//    [Serializable]
//    public class SpriteManualAnimator : SpriteRendererBase2
//    {        
//        public tk2dSprite Sprite;

//        public void Awake(/*IBaseGameEntity sprite*/)
//        {
//            if (Sprite == null)
//            {
//                Sprite = GetComponent<tk2dSprite>();
//            }
//            base.Awake(/*sprite, */Sprite.transform);
//            base.SetBaseSprite(Sprite);
//        }

//        /// <summary>
//        /// Returns true if the animation changed
//        /// </summary>
//        /// <param name="name"></param>
//        /// <returns></returns>
//        public override bool RenderByName(SpriteAnimation animation)
//        {
//            int spriteId = Sprite.GetSpriteIdByName(animation.AnimationName);
//            Render(spriteId);
//            return true;
//        }

//        public void Render(int spriteId, bool setOffsetDelta = true)
//        {
//            string oldName = GetClipName();
//            if (oldName != null)
//                _previousRender = oldName;
//            Sprite.SetSprite(spriteId);
//            _currentRender = Sprite.name;
//            // Since we are switching animations, set the anchor for the new animation
//            //SetOffset(_currentRender, setOffsetDelta);
//        }

//        public override bool Render(string id, Direction direction)
//        {
//            Debug.Log("(" + transform.parent.name + ") Setting animation to " + id + " " + direction);
//            var animationName = GetAnimation(id, direction);
//            if (animationName == null)
//            {
//                Debug.LogError("Could not locate animation " + id + " in " + transform.parent.name);
//                return false;
//            }
//            if (String.IsNullOrEmpty(animationName.AnimationName))
//                return false;
//            _currentID = id;
//            return RenderByName(animationName);
//        }

//        public override string GetClipName()
//        {
//            if (Sprite == null)
//                return null;
//            return Sprite.name;
//        }

//        public override IEnumerable<string> GetAllClipNames()
//        {
//            var clips = new List<string>();
//            for (int i = 0; i < Sprite.Collection.spriteDefinitions.Length; i++)
//            {
//                clips.Add(Sprite.Collection.spriteDefinitions[i].name);
//            }
//            return clips;
//        }

//        public override int GetClipIdByName(string name)
//        {
//            return Sprite.GetSpriteIdByName(name);
//        }
//    }
//}
