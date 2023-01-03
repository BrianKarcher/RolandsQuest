using UnityEngine;

namespace RQ.Controller.Startup
{
    [AddComponentMenu("RQ/Common")]
    public class MainStart : MonoBehaviour
    {
        //[SerializeField]
        //private GameController _gameController = null;

        public void Awake()
        {
            Debug.Log("MainStart Awake called");
            if (!Application.isPlaying)
                return;

            //if (GameController.Instance == null)
            //{
            //    Instantiate(_gameController, Vector3.zero, Quaternion.identity);
            //}
        }
    }
}
