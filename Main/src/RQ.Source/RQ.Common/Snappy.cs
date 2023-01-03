using UnityEngine;
using System.Collections;
using RQ.Extensions;

namespace RQ
{
    [AddComponentMenu("RQ/Common/Snappy")]
    [ExecuteInEditMode]
    public class Snappy : MonoBehaviour
    {
        public bool SnapToGrid = true;
        public Vector3 tileOffset = new Vector3(.08f, .08f, 0f);

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        public void Update()
        {
            if (SnapToGrid && !Application.isPlaying)
            {
                //tk2dTileMap tileMap = GameObject.FindObjectOfType<tk2dTileMap>();
                int x, y;
                //if (tileMap != null)
                //{
                //tileMap.GetTileAtPosition(this.transform.localPosition - tileOffset, out x, out y);
                //tileMap.GetTileAtPosition(new Vector3(this.transform.localPosition.x, transform.localPosition.y, 0f) - tileOffset, out x, out y);

                //Vector3 newPos = tileMap.GetTilePosition(x, y);

                //Vector2 newPos = transform.localPosition.SnapToGrid(tileMap, tileOffset);
                //Mathf.FloorToInt()
                //x = Mathf.FloorToInt(transform.localPosition.x / .16f);
                //y = Mathf.FloorToInt(transform.localPosition.y / .16f);
                //this.transform.localPosition = new Vector3(x * .16f, y * .16f, transform.localPosition.z) + tileOffset;
                
                // Doing the math in integers removes floating point innacuracies
                x = Mathf.RoundToInt(transform.localPosition.x * 100f) / 16;
                y = Mathf.RoundToInt(transform.localPosition.y * 100f) / 16;
                //x = Mathf.FloorToInt(() / .16f);
                //y = Mathf.FloorToInt(transform.localPosition.y / .16f);
                this.transform.localPosition = new Vector3((x * 16) / 100f, (y * 16) / 100f, transform.localPosition.z) + tileOffset;
                //Vector3 newPos = new Vector3(Mathf.FloorToInt(this.transform.localPosition.x * 100 / 16) * 16 / 100, Mathf.FloorToInt(this.transform.localPosition.y * 100 / 16) * 16 / 100, this.transform.localPosition.z);
                //this.transform.localPosition = new Vector3(newPos.x, newPos.y, transform.localPosition.z);

                //this.transform.localPosition = new Vector3(newPos.x, newPos.y, transform.localPosition.z) + tileOffset;
                //}
            }
        }

        //public void UpdateOld()
        //{
        //    if (SnapToGrid && !Application.isPlaying)
        //    {
        //        tk2dTileMap tileMap = GameObject.FindObjectOfType<tk2dTileMap>();
        //        int x, y;
        //        if (tileMap != null)
        //        {
        //            //tileMap.GetTileAtPosition(this.transform.localPosition - tileOffset, out x, out y);
        //            //tileMap.GetTileAtPosition(new Vector3(this.transform.localPosition.x, transform.localPosition.y, 0f) - tileOffset, out x, out y);

        //            //Vector3 newPos = tileMap.GetTilePosition(x, y);

        //            Vector2 newPos = transform.localPosition.SnapToGrid(tileMap, tileOffset);



        //            //Vector3 newPos = new Vector3(Mathf.FloorToInt(this.transform.localPosition.x * 100 / 16) * 16 / 100, Mathf.FloorToInt(this.transform.localPosition.y * 100 / 16) * 16 / 100, this.transform.localPosition.z);
        //            this.transform.localPosition = new Vector3(newPos.x, newPos.y, transform.localPosition.z);
        //            //this.transform.localPosition = new Vector3(newPos.x, newPos.y, transform.localPosition.z) + tileOffset;
        //        }
        //    }
        //}
    }
}