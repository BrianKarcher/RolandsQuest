using UnityEngine;
using System.Collections;

namespace RQ
{
	public class ToggleFullscreen : MonoBehaviour
	{
		void Update()
		{
            // @todo Integrate this into a central input system
			if (UnityEngine.Input.GetKeyDown(KeyCode.F))
				Screen.fullScreen = !Screen.fullScreen;
		}
	}
}