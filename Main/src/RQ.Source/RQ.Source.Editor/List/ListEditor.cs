using RQ.Editor.Common;
using RQ.Editor.GUIStyles;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RQ.Editor.List
{
    public class ListEditor
    {
        public class DrawItemData<T> where T : new()
        {
            public T data;
            public string name;
        }
        //public event Action<bool> Dirty;
        //public static event Action ListSizeChange;

        public static bool DrawList<T>(List<T> list, Func<T, int, DrawItemData<T>> drawItem, Func<T> newItemAction = null, Action ListSizeChange = null) where T : new()
        {
            bool isDirty = false;
            //GUI.changed = false;
            //if (agent.ChapterConfigs == null)
            //    agent.ChapterConfigs = new List<StoryChapterConfig>();

            if (GUILayout.Button("Add", GUILayout.Width(80)))
            {
                T newItem;
                if (newItemAction == null)
                    newItem = new T();
                else
                    newItem = newItemAction();
                list.Add(newItem);
                if (ListSizeChange != null)
                    ListSizeChange();
            }

            // Table Header
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Id", GUILayout.Width(20));
            //EditorGUILayout.LabelField("Chapter");
            //EditorGUILayout.EndHorizontal();

            int removeIndex = -1;
            int moveDown = -1;
            int moveUp = -1;

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                //GUILayout.Space(4.0f);
                //GUILayout.BeginVertical(tk2dExternal.Skin.Inst.GetStyle("InspectorHeaderBG"));
                EditorGUILayout.BeginHorizontal();

                var drawItemData = drawItem(item, i);
                list[i] = drawItemData.data;

                GUI.enabled = (i != list.Count - 1);
                if (GUILayout.Button("", Utils.SimpleButton("btn_down")))
                {
                    moveDown = i;
                }

                GUI.enabled = (i != 0);
                if (GUILayout.Button("", Utils.SimpleButton("btn_up")))
                {
                    moveUp = i;
                }

                GUI.enabled = true;

                if (GUILayout.Button("", Styles.CreateTilemapDeleteItemStyle()))
                {
                    //var chapterName = chapter == null ? "(null)" : chapter.Name;
                    if (EditorUtility.DisplayDialog("Remove?", "Remove " + drawItemData.name + "?", "Yes", "No"))
                    {
                        removeIndex = i;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            if (removeIndex != -1)
            {
                list.RemoveAt(removeIndex);
                if (ListSizeChange != null)
                    ListSizeChange();
                isDirty = true;
                //Dirty(false);
            }
            if (moveDown != -1)
            {
                list.Swap(moveDown, moveDown + 1);
                isDirty = true;
                //Dirty(false);
            }
            if (moveUp != -1)
            {
                list.Swap(moveUp, moveUp - 1);
                isDirty = true;
                //Dirty(false);
            }
            //}

            //if (GUI.changed)
            //{
            //    Dirty(false);
            //}
            return isDirty;
        }

        //private static void OnListSizeChange()
        //{
        //    if (ListSizeChange != null)
        //        ListSizeChange();
        //}
    }
}
