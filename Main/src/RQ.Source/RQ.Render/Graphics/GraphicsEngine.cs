using UnityEngine;
using System.Collections.Generic;
using RQ.Messaging;
using System;
using RQ.Model;
using RQ.Physics;
using RQ.Common.Container;
using RQ.Common.Components;
using RQ.Render.UI;

namespace RQ.Render
{
    [AddComponentMenu("RQ/Manager/Graphics Engine")]
    public class GraphicsEngine : ComponentBase<GraphicsEngine>
	{
        [SerializeField]
        private int screenWidth = 1280;
        [SerializeField]
        private int screenHeight = 720;
		private Dictionary<string, Effect> lstEffects;
        //private Stopwatch _lightningStopwatch;
        //[SerializeField]
        //private UISprite _overlayColorWindow = null;
        [SerializeField]
        private GameObject OverlayWindow;
        private IOverlayWindow _overlayWindow;
        [SerializeField]
        private GameObject _invertColors;
        public GameObject InvertColors { get { return _invertColors; } }
        private GameObject _invertColorsObject;
		//private Texture2D texture;
		//private static Texture2D blackTexture = CreateSingleColorTexture (Color.black);
		//private static Texture2D whiteTexture = CreateSingleColorTexture (Color.white);
		//private float shade = 0f;
        public override string UniqueId { get { return "Graphics Engine"; } set { } }

	    private long _startInvertGrowId, _destroyInvertGrowId, _tweenToWhiteId, _tweenToTransparentId, _tweenToColorId, _tweenToColorId2, _createLightningEffectId;

	    private Action<Telegram2> _startInvertGrowDelegate, _destroyInvertGrowDelegate, _tweenToWhiteDelegate, _tweenToTransparentDelegate,
            _tweenToColorDelegate, _createLightningEffectDelegate;


        public override void Awake()
		{
            this.UniqueId = Guid.NewGuid().ToString();
            base.Awake();
            lstEffects = new Dictionary<string, Effect>();
            if (OverlayWindow != null)
                _overlayWindow = OverlayWindow.GetComponent<IOverlayWindow>();
		    _startInvertGrowDelegate = (data) =>
		    {
		        //Vector2D cameraPos = GameObject.FindObjectOfType<CameraClass>(); //GameController.Instance.GetCamera().GetPos();
		        Vector2D cameraPos = EntityContainer._instance.GetMainCharacter().transform.position;
		        _invertColorsObject =
		            Instantiate(InvertColors, cameraPos.ToVector3(-1.5f), Quaternion.identity) as GameObject;
		    };
		    _destroyInvertGrowDelegate = (data) =>
		    {
		        GameObject.Destroy(_invertColorsObject);
		        _invertColorsObject = null;
		    };
		    _tweenToWhiteDelegate = (data) =>
		    {
		        float duration;
		        float.TryParse((string) data.ExtraInfo, out duration);
		        var colorInfo = new TweenToColorInfo(new Color(1f, 1f, 1f, 1f), .1f, duration);
		        TweenOverlayToColor(colorInfo);
		    };
		    _tweenToTransparentDelegate = (data) =>
		    {
		        float duration;
		        float.TryParse((string) data.ExtraInfo, out duration);
		        var colorInfo = new TweenToColorInfo(new Color(0f, 0f, 0f, 0f), .1f, duration);
		        TweenOverlayToColor(colorInfo);
		    };
		    _tweenToColorDelegate = TweenToColor;
		    _createLightningEffectDelegate = (data) =>
		    {
		        CreateLightning();
		    };


		}

		//public override void Start()
		//{
  //          base.Start();

		//	//blackTexture = new Texture2D(1, 1);
		//	//blackTexture 
		//	//whiteTexture 
		//}

		//public override void Update()
		//{
  //          base.Update();
		//	//foreach (Effect effect in lstEffects)
		//	//{
		//	//	effect.Update ();
		//	//}
		//}

        public override void StartListening()
        {
            base.StartListening();
            _startInvertGrowId = MessageDispatcher2.Instance.StartListening("StartInvertGrow", _componentRepository.UniqueId, 
                _startInvertGrowDelegate);
            //_componentRepository.StartListening("StartInvertGrow", this.UniqueId, );
            _destroyInvertGrowId = MessageDispatcher2.Instance.StartListening("DestroyInvertGrow", _componentRepository.UniqueId,
                _destroyInvertGrowDelegate);
            //_componentRepository.StartListening("DestroyInvertGrow", this.UniqueId, );
            _tweenToWhiteId = MessageDispatcher2.Instance.StartListening("TweenToWhite", _componentRepository.UniqueId,
                _tweenToWhiteDelegate);
            //_componentRepository.StartListening("TweenToWhite", this.UniqueId, );
            _tweenToTransparentId = MessageDispatcher2.Instance.StartListening("TweenToTransparent", _componentRepository.UniqueId,
                _tweenToTransparentDelegate);
            //_componentRepository.StartListening("TweenToTransparent", this.UniqueId, );
            _tweenToColorId = MessageDispatcher2.Instance.StartListening("TweenToColor", _componentRepository.UniqueId,
                _tweenToColorDelegate);
            //_componentRepository.StartListening("TweenToColor", this.UniqueId, );
            _tweenToColorId2 = MessageDispatcher2.Instance.StartListening("TweenToColor", this.UniqueId, _tweenToColorDelegate);
            _createLightningEffectId = MessageDispatcher2.Instance.StartListening("CreateLightningEffect", _componentRepository.UniqueId,
                _createLightningEffectDelegate);
            //_componentRepository.StartListening("CreateLightningEffect", this.UniqueId, );
            //MessageDispatcher2.Instance.StartListening("TweenOverlayToColor", this.UniqueId, (data) =>
            //{
            //    var colorData = data.ExtraInfo as TweenOverlayToColor;
            //});
        }

        private void TweenToColor(Telegram2 data)
        {
            var tweenToColorInfo = data.ExtraInfo as TweenToColorInfo;

            //var colorInfo = new TweenToColorInfo(tweenToColorInfo.Color, .1f, duration);
            TweenOverlayToColor(tweenToColorInfo);
        }

        public override void StopListening()
        {
            base.StopListening();

            MessageDispatcher2.Instance.StopListening("StartInvertGrow", _componentRepository.UniqueId, _startInvertGrowId);
            //_componentRepository.StopListening("StartInvertGrow", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("DestroyInvertGrow", _componentRepository.UniqueId, _destroyInvertGrowId);
            //_componentRepository.StopListening("DestroyInvertGrow", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("TweenToWhite", _componentRepository.UniqueId, _tweenToWhiteId);
            //_componentRepository.StopListening("TweenToWhite", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("TweenToTransparent", _componentRepository.UniqueId, _tweenToTransparentId);
            //_componentRepository.StopListening("TweenToTransparent", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("TweenToColor", _componentRepository.UniqueId, _tweenToColorId);
            //_componentRepository.StopListening("TweenToColor", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("TweenToColor", this.UniqueId, _tweenToColorId2);
            MessageDispatcher2.Instance.StopListening("CreateLightningEffect", this.UniqueId, _createLightningEffectId);
            //_componentRepository.StopListening("CreateLightningEffect", this.UniqueId);
        }

        public void SetScreenResolution(int width, int height, bool fullScreen)
        {
            //int width = Screen.currentResolution.width;
            //int height = Screen.currentResolution.height;
            //var resolution = Screen.resolutions.Last();
            //int width = resolution.width;
            //int height = resolution.height;
            //Screen.aspe
            //Debug.Log("Setting screen resolution to " + width + ", " + height);
            Screen.SetResolution(width, height, fullScreen);
            //Screen.SetResolution(screenWidth, screenHeight, true);
        }

        public Resolution GetHighestResolution()
        {
            return Screen.resolutions[Screen.resolutions.Length - 1];
            //return Screen.resolutions.Last();
        }

		//public void SetShade(float darkness)
		//{
		//	shade = 1 - darkness;
		//}
		public void AddEffect(Effect effect)
		{
			lstEffects.Add (effect.UniqueId, effect);
			//Debug.Log ("Adding effect, count = " + lstEffects.Count);
			//Instantiate (effect);
            //return lstEffects.Count;
		}

		public void RemoveEffect(Effect effect)
		{
            RemoveEffect(effect.UniqueId);
		}

        public void RemoveEffect(string effectId)
        {
            // Already removed?
            //if (effect >= lstEffects.Count)
            //    return;
            var effect = lstEffects[effectId];
            Destroy(effect);
            lstEffects.Remove(effectId);

            //Debug.Log("Removing effect, count = " + lstEffects.Count);
        }

        //public Effect GetEffect(int index)
        //{
        //    return lstEffects[index];
        //}

        public void ClearEffects()
        {
            lstEffects.Clear();
        }

        //public ColorOverlayEffect CreateShade(float alpha)
        //{
        //    ColorOverlayEffect colorOverlayEffect = gameObject.AddComponent<ColorOverlayEffect>();
        //    // Stop the effect after one second
        //    //colorOverlayEffect.KillTimer = 1f;
        //    //colorOverlayEffect.StartConstant(new Color(1,1,1, .50f));
        //    colorOverlayEffect.StartConstant(new Color(0, 0, 0, alpha));
        //    AddEffect(colorOverlayEffect);
        //    //colorOverlayEffect.
        //    return colorOverlayEffect;
        //}

        public void CreateLightning()
        {
            var tweenToWhite = new TweenToColorInfo()
            {
                Duration = 0f,
                Color = new Color(1, 1, 1, .3f)
            };
            //SetOverlayToColor(new Color(1, 1, 1, .5f));
            TweenOverlayToColor(tweenToWhite);
            var tweenToColor = new TweenToColorInfo()
            {
                Duration = 1f,
                Color = new Color(0, 0, 0, 0.44f)
            };
            TweenOverlayToColor(tweenToColor);



            //ColorOverlayEffect colorOverlayEffect = gameObject.AddComponent<ColorOverlayEffect>();
            //// Stop the effect after one second
            //colorOverlayEffect.KillTimer = 1f;
            ////colorOverlayEffect.StartConstant(new Color(1,1,1, .50f));
            //colorOverlayEffect.StartFade(Color.white, effect => RemoveEffect(effect), 1f, 1f, 0f);
            //AddEffect(colorOverlayEffect);
            ////colorOverlayEffect.
            //return colorOverlayEffect;
        }

        //public ColorOverlayEffect CreateFadeOut()
        //{
        //    ColorOverlayEffect colorOverlayEffect = gameObject.AddComponent<ColorOverlayEffect>();
        //    // Stop the effect after one second
        //    colorOverlayEffect.KillTimer = 0.5f;
        //    //colorOverlayEffect.StartConstant(new Color(1,1,1, .50f));
        //    colorOverlayEffect.StartFade(Color.black, effectIndex => RemoveEffect(effectIndex), 0.5f, 0f, 1f);
        //    AddEffect(colorOverlayEffect);
        //    //colorOverlayEffect.
        //    return colorOverlayEffect;
        //}

        //public ColorOverlayEffect CreateFadeIn()
        //{
        //    ColorOverlayEffect colorOverlayEffect = gameObject.AddComponent<ColorOverlayEffect>();
        //    // Stop the effect after one second
        //    colorOverlayEffect.KillTimer = 0.5f;
        //    //colorOverlayEffect.StartConstant(new Color(1,1,1, .50f));
        //    colorOverlayEffect.StartFade(Color.black, effectIndex => RemoveEffect(effectIndex), 0.5f, 1f, 0f);
        //    AddEffect(colorOverlayEffect);
        //    //colorOverlayEffect.
        //    return colorOverlayEffect;
        //}

        //private ColorOverlayEffect CreateStorm()
        //{
        //    return CreateShade(.50f);
        //    // Creates a lightning flash 5 to 10 seconds apart, then restarts the timer
        //    //_lightningStopwatch = Stopwatch.Start(5f, 10f, () =>
        //    //{
        //    //    CreateLightning();
        //    //    _lightningStopwatch.ReStart();
        //    //});
        //    //GameController._instance.AddStopwatchToAutoUpdate(_lightningStopwatch);
        //    //lstStopwatch.Add (_lightningStopwatch);
        //}

        // TODO This needs to be serialized - kept in memory - and removed
        // from memory when the tween is complete
        public void TweenOverlayToColor(TweenToColorInfo colorInfo)
        {
            _overlayWindow.TweenOverlayToColor(colorInfo);

            ////Debug.LogWarning("Tweening overlay to " + colorInfo.Color.a);
            ////_overlayColorWindow.color = new Color(0, 0, 0, 1);
            //var tween = TweenColor.Begin(_overlayColorWindow.gameObject, colorInfo.Duration,
            //    colorInfo.Color);
            ////var tween = TweenAlpha.Begin(FadeWindow.gameObject, 20f, 0);
            //tween.delay = colorInfo.Delay;
            ////tween.to = Color.white;
        }

        public IOverlayWindow GetOverlayWindow()
        {
            return _overlayWindow;
        }

        public void SetOverlayToColor(Color color)
        {
            _overlayWindow.SetOverlayToColor(color);
            //Debug.LogWarning("Tweening overlay to " + color.a);
            //_overlayColorWindow.color = color;
        }

        //public void SetTweenColor(Color color)
        //{
        //    TweenColor.current.cur
        //    var tween = TweenColor.Begin(_overlayColorWindow.gameObject, colorInfo.Duration,
        //        colorInfo.Color);
        //}
	}


}