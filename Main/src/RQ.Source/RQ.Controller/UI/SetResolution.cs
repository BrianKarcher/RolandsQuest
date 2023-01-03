using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.;

namespace RQ.Controller.UI
{
    [AddComponentMenu("RQ/UI/Set Resolution")]
    public class SetResolution : MonoBehaviour
    {
        private bool _fullScreen = false;
        // 0 = 768 x 432
        // 1 = 1152 x 648
        private int _width = 1152;
        private int _height = 648;
        private bool _isVsync = true;

        //public Button text;
        public Toggle FullScreenToggle;
        public Toggle VSyncToggle;
        //public UnityEngine.UI.
        //public Selectable ResolutionDropdown;
        public Dropdown ResolutionDropdown;
        public Button QuitButton;

        private KeyValuePair<int, int>[] _resolutions = new KeyValuePair<int, int>[]
        {
            new KeyValuePair<int, int>(768, 432),
            new KeyValuePair<int, int>(1152, 648)
        };

        //Resolution[] resolutions = new Resolution[]
        //{
        //    new Resolution()
        //};

        public void Start()
        {
            //_height = Screen.currentResolution.height;
            //_width = Screen.currentResolution.width;
            _height = Screen.height;
            _width = Screen.width;
            _fullScreen = Screen.fullScreen;
            //_isVsync = QualitySettings.vSyncCount == 1;
            //Debug.LogError($"Initial screen resolution: {_width}x{_height}, fullscreen: {_fullScreen}");
            for (int i = 0; i < _resolutions.Length; i++)
            {
                //Debug.LogError($"Comparing to resolution {_resolutions[i].Key}x{_resolutions[i].Value}");
                if (_resolutions[i].Value == _height)
                {
                    //Debug.LogError($"Setting resolution index to {i}");
                    ResolutionDropdown.value = i;
                    break;
                }
            }
            
            FullScreenToggle.isOn = _fullScreen;
            VSyncToggle.isOn = _isVsync;
            //var resolution = _resolutions.Where(i => i.Value == currentHeight);
            //if (resolution != null)
            //{

            //}
            VSyncToggle.onValueChanged.AddListener((value) =>
            {
                _isVsync = value;
                UpdateResolution();
            });
            FullScreenToggle.onValueChanged.AddListener((value) =>
            {
                _fullScreen = value;
                UpdateResolution();
            });
            ResolutionDropdown.onValueChanged.AddListener((value) =>
            {
                Debug.LogError($"Setting resolution to index {value}");
                _width = _resolutions[value].Key;
                _height = _resolutions[value].Value;
                UpdateResolution();
            });
            QuitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }

        public void UpdateResolution()
        {
            Screen.SetResolution(_width, _height, _fullScreen);
            QualitySettings.vSyncCount = _isVsync ? 1 : 0;
        }
    }
}
