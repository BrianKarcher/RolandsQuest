using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;

namespace RQ.Model.Containers
{
    [Serializable]
    public class UsableContainer
    {
        //[Serializable]
        public string CurrentUsableObject { get; set; }
        public string CurrentBubbleText { get; set; }

        // TODO Make this Serialized, needs to persist
        [NonSerialized]
        public List<UsableObjectInfo> UsableObjects;

        public event System.Action<string> UsableChanged;
        //public Dictionary<string, UsableObjectInfo> UsableObjects;
        //private List<string> _usableObjectsSerialized;

        public UsableContainer()
        {
            //UsableObjects = new Dictionary<string, UsableObjectInfo>();
            UsableObjects = new List<UsableObjectInfo>();
        }

        public void Add(string uniqueId, string bubbleText)
        {
            // Prevent duplicates
            for (int i = 0; i < UsableObjects.Count; i++)
            {
                if (UsableObjects[i].UniqueId == uniqueId)
                    return;
            }
            UsableObjects.Add(new UsableObjectInfo()
            {
                UniqueId = uniqueId,
                BubbleText = bubbleText
            });
                //if (!UsableObjects.ContainsKey(uniqueId))
                //    UsableObjects.Add(uniqueId, new UsableObjectInfo() { UniqueId = uniqueId,
                //        BubbleText = bubbleText });
            }

        public void Remove(string uniqueId)
        {
            //if (CurrentUsableObject == uniqueId)
            //{
            //    SetCurrentUsable(null);
            //}
                //CurrentUsableObject = null;
            for (int i = 0; i < UsableObjects.Count; i++)
            {
                if (UsableObjects[i].UniqueId == uniqueId)
                {
                    UsableObjects.RemoveAt(i);
                    return;
                }                    
            }
            //UsableObjects.Remove(uniqueId);
        }

        public void SetCurrentUsable(string uniqueId)
        {
            var oldCurrentUsable = CurrentUsableObject;
            for (int i = 0; i < UsableObjects.Count; i++)
            {
                var usableObjectInfo = UsableObjects[i];
                if (usableObjectInfo.UniqueId == uniqueId)
                {
                    CurrentUsableObject = uniqueId;
                    CurrentBubbleText = usableObjectInfo.BubbleText;
                    return;
                }
            }
            //if (uniqueId != null && UsableObjects.TryGetValue(uniqueId, out var usableObjectInfo))
            //{
            //    CurrentUsableObject = uniqueId;
            //    CurrentBubbleText = usableObjectInfo.BubbleText;
            //}
            //else
            //{
            //    CurrentUsableObject = null;
            //    CurrentBubbleText = null;
            //}
            CurrentUsableObject = null;
            CurrentBubbleText = null;

            // Usable changed? Notify the UsableContainerController
            if (oldCurrentUsable != CurrentUsableObject)
            {
                UsableChanged?.Invoke(CurrentUsableObject);
                //MessageDispatcher2.Instance.DispatchMsg("UsableChanged", 0f, null, "Usable Controller", null);
            }
        }

        public void ClearList()
        {
            UsableObjects.Clear();
        }

        public List<UsableObjectInfo> GetList()
        {
            return UsableObjects;
        }

        public string GetCurrentUsable()
        {
            return CurrentUsableObject;
        }

        //public void SetCurrentUsable(string newCurrentUsable)
        //{
        //    CurrentUsableObject = newCurrentUsable;
        //}

        //public void Serialize()
        //{
        //    //_usableObjectsSerialized = UsableObjects.ToList();
        //}

        //public void Deserialize()
        //{
        //    //UsableObjects = new HashSet<string>();
        //    //if (_usableObjectsSerialized != null)
        //    //{
        //    //    foreach (var usableObject in _usableObjectsSerialized)
        //    //    {
        //    //        UsableObjects.Add(usableObject);
        //    //    }
        //    //}
        //}

        //public void CalculateAndSetClosest(string targetUniqueId)
        //{
        //    if (!UsableObjects.Any())
        //        return;
        //    string closestObject = string.Empty;

        //    MessageDispatcher.Instance.DispatchMsg(0f, string.Empty, targetUniqueId,
        //        RQ.Enums.Telegrams.GetClosestObjectFromSuppliedList, _usableObjects.AsEnumerable(),
        //        (closest) => closestObject = closest as string );

        //    if (_currentUsableObject != closestObject)
        //    {
        //        // Send a message to the previous usable object that it is
        //        // no longer the usable object
        //        _currentUsableObject = closestObject;
        //        // Send a message to the new usable object that it has
        //        // been activated as the usable object
        //    }
        //}
    }
}
