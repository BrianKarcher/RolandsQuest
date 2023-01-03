using UnityEngine;
using System;

namespace RQ
{
    public class ColorOverlayEffect : Effect
    {
        private Color _color { get; set; }
        private Texture2D _texture;
        //private bool _isFading;
        //private Stopwatch _fadeTimer;
        private float _currentAlpha;
        private float _endingAlpha;
        // Change in Alpha per second
        private float _alphaDelta = 0f;

        //private bool _isFadeIn;

        public ColorOverlayEffect()
            : base()
        {

        }

        public void StartConstant(Color color)
        {
            //_isFading = false;
            _isActive = true;
            //_fadeTimer = null;
            base.Begin();
            _texture = CommonGraphics.CreateSingleColorTexture(color);
        }

        public void StartFade(Color color, Action<Effect> callBack, float seconds, float startingAlpha)
        {
            StartFade(color, callBack, seconds, startingAlpha, 1f);
        }

        public void StartFade(Color color, Action<Effect> callBack, float seconds, float startingAlpha, float endingAlpha)
        {
            _color = color;
            //_isFading = true;
            _isActive = true;
            //_fadeTimer = Stopwatch.Start (seconds);
            //_isFadeIn = true;
            _currentAlpha = startingAlpha;
            _endingAlpha = endingAlpha;
            _alphaDelta = (_endingAlpha - startingAlpha) / seconds;
            _callBack = callBack;
            Debug.Log("Fade out: alpha delta = " + _alphaDelta);
            base.Begin();
            _texture = CommonGraphics.CreateSingleColorTexture(color);
        }
    }
}