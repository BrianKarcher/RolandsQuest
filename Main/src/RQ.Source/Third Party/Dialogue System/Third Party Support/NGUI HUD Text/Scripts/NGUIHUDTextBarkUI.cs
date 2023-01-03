//using UnityEngine;
//using System.Collections;
//using PixelCrushers.DialogueSystem;

//namespace PixelCrushers.DialogueSystem.NGUI {
	
//    /// <summary>
//    /// Implements IBarkUI using NGUI HUDText to show bark text above a character's head.
//    /// Assign a HUD Text prefab and a Follow Transform (usually a child object above the
//    /// character's head).
//    /// 
//    /// Your scene should also have a UIRoot with a HUDRoot somewhere in it.
//    /// </summary>
//    [AddComponentMenu("Dialogue System/Third Party/NGUI/HUD Text/Bark UI")]
//    public class NGUIHUDTextBarkUI : MonoBehaviour, IBarkUI {
		
//        /// <summary>
//        /// The HUD Text prefab to instantiate for the NPC's barks.
//        /// </summary>
//        public GameObject hudTextPrefab;
		
//        /// <summary>
//        /// The transform to follow (usually a point above the NPC's head).
//        /// </summary>
//        public Transform followTransform;
		
//        /// <summary>
//        /// The default color, unless overridden by an emphasis tag.
//        /// </summary>
//        public Color defaultColor = Color.white;
	
//        /// <summary>
//        /// The duration in seconds to show the bark text.
//        /// </summary>
//        public float duration = 5f;
		
//        /// <summary>
//        /// Set <c>true</c> to run a raycast to the player. If the ray is blocked (e.g., a wall
//        /// blocks visibility to the player), don't show the bark.
//        /// </summary>
//        public bool checkIfPlayerVisible = true;
		
//        /// <summary>
//        /// The layer mask to use when checking for player visibility.
//        /// </summary>
//        public LayerMask visibilityLayerMask = 1;
		
//        /// <summary>
//        /// The HUD Text instance created from the prefab.
//        /// </summary>
//        private HUDText hudText = null;

//        private UIFollowTarget followTarget = null;

//        private Transform playerCameraTransform = null;
		
//        private Collider playerCameraCollider = null;
		
//        /// <summary>
//        /// Starts the component by adding a bark label to the UIBarkRoot object, which should
//        /// be found in an NGUI UI.
//        /// </summary>
//        public void Start() {
//            if (HUDRoot.go == null) {
//                if (DialogueDebug.LogErrors) Debug.LogWarning(string.Format("{0}: No HUDRoot found in scene.", DialogueDebug.Prefix));
//            } else {
//                GameObject child = NGUITools.AddChild(HUDRoot.go, hudTextPrefab);
//                hudText = child.GetComponentInChildren<HUDText>();
//                child.AddComponent<UIFollowTarget>().target = (followTransform != null) ? followTransform : transform;
//                child.name = string.Format("Bark ({0})", name);
//            }
//        }
	
//        /// <summary>
//        /// If the barker is destroyed, also destroy its HUD Text.
//        /// </summary>
//        public void OnDestroy() {
//            if (hudText != null) Destroy(hudText.gameObject);
//        }

//        /// <summary>
//        /// Barks the specified subtitle.
//        /// </summary>
//        /// <param name='subtitle'>
//        /// Subtitle to bark.
//        /// </param>
//        public void Bark(Subtitle subtitle) {
//            if (hudText != null) {
//                Color color = (subtitle.formattedText.emphases.Length > 0) ? subtitle.formattedText.emphases[0].color : defaultColor;
//                hudText.Add(subtitle.formattedText.text, color, duration);
//                followTarget = hudText.GetComponent<UIFollowTarget>();
//                playerCameraTransform = Camera.main.transform;
//                playerCameraCollider = (playerCameraTransform != null) ? playerCameraTransform.collider : null;
//            }
//        }

//        /// <summary>
//        /// Indicates whether a bark is playing or not.
//        /// </summary>
//        /// <value>
//        /// <c>true</c> if this instance is playing; otherwise, <c>false</c>.
//        /// </value>
//        public bool IsPlaying { 
//            get { return (hudText != null) && hudText.isVisible; }
//        }

//        void Update() {
//            if (IsPlaying && checkIfPlayerVisible) CheckPlayerVisibility();
//        }
		
//        private void CheckPlayerVisibility() {
//            bool canSeePlayer = true;
//            if ((playerCameraTransform != null) && (followTarget != null)) {
//                RaycastHit hit;
//                if (Physics.Linecast(followTarget.target.position, playerCameraTransform.position, out hit, visibilityLayerMask)) {
//                    canSeePlayer = (hit.collider == playerCameraCollider);
//                }
//            }
//            if (!canSeePlayer && hudText.gameObject.activeInHierarchy) {
//                hudText.gameObject.SetActive(false);
//            } else if (canSeePlayer && !hudText.gameObject.activeInHierarchy) {
//                hudText.gameObject.SetActive(true);
//            }
//        }
		
//        void OnDrawGizmos() {
//            if (IsPlaying) {
//                Gizmos.DrawLine(followTarget.target.position, playerCameraTransform.position);
//            }
//        }
//    }
	
//}