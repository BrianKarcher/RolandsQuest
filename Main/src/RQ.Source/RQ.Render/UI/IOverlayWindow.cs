using RQ.Model;
using UnityEngine;

namespace RQ.Render.UI
{
    public interface IOverlayWindow
    {
        void SetOverlayToColor(Color color);
        void TweenOverlayToColor(TweenToColorInfo colorInfo);
        Color GetOverlayColor();
    }
}
