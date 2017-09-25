using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Dreamteck.Splines
{
    public class SplineTool
    {
        protected List<SplineComputer> computers = new List<SplineComputer>();
        protected bool promptSave = false;
        protected EditorWindow windowInstance = null;

        public virtual string GetName()
        {
            return "Tool";
        }

        public virtual void Open(EditorWindow window)
        {
            windowInstance = window;
        }

        public virtual void Close()
        {
            if(promptSave) ClosingDialog();
        }

        private void ClosingDialog()
        {
            if (EditorUtility.DisplayDialog("Unsaved Changes", ClosingDialogText(), "Yes", "No")) Save();
            else Cancel();
        }

        protected virtual string ClosingDialogText()
        {
            return "There are unsaved changes. Do you wish to save them?";
        }

        protected virtual void Save()
        {
            promptSave = false;
        }

        protected virtual void Cancel()
        {
            promptSave = false;
        }

        protected virtual string GetPrefix()
        {
            return "SplineTool";
        }

        public virtual void Draw(Rect rect)
        {
            //EditorGUILayout.LabelField("Spline User", EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Selected Splines", EditorStyles.boldLabel);
            for (int i = 0; i < computers.Count; i++)
            {
                SplineComputer lastComputer = computers[i];
                computers[i] = (SplineComputer)EditorGUILayout.ObjectField(computers[i], typeof(SplineComputer), true);
                if (computers[i] == null)
                {
                    computers.RemoveAt(i);
                    i--;
                    OnSplineRemoved(lastComputer);
                    continue;
                }
                if (lastComputer != computers[i])
                {
                    for (int j = 0; j < computers.Count; j++)
                    {
                        if (j == i) continue;
                        if (computers[j] == computers[i])
                        {
                            computers[i] = lastComputer;
                            break;
                        }
                    }
                }
            }
            SplineComputer newComp = null;
            newComp = (SplineComputer)EditorGUILayout.ObjectField(newComp, typeof(SplineComputer), true);
            if(newComp != null)
            {
                for (int i = 0; i < computers.Count; i++)
                {
                    if (computers[i] == newComp)
                    {
                        newComp = null;
                        break;
                    }
                }
                if (newComp != null)
                {
                    computers.Add(newComp);
                    OnSplineAdded(newComp);
                }
            }
            EditorGUILayout.Space();


            /*
            SplineComputer lastComputer = computer;
            computer = (SplineComputer)EditorGUILayout.ObjectField("Computer", computer, typeof(SplineComputer), true);
            if (computer != lastComputer) Selection.activeGameObject = computer.gameObject;
            if(computer == null) EditorGUILayout.HelpBox("No SplineComputer is selected. Reference a spline computer!", MessageType.Error);
            if (showResolution) resolution = EditorGUILayout.Slider("Resolution", resolution, 0f, 1f);
            */
        }

        protected virtual void OnSplineAdded(SplineComputer spline)
        {

        }

        protected virtual void OnSplineRemoved(SplineComputer spline)
        {

        }

        protected void ClipUI(SplineUser user)
        {
            float fclipFrom = (float)user.clipFrom, fclipTo = (float)user.clipTo;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.MinMaxSlider(new GUIContent("Clip range:"), ref fclipFrom, ref fclipTo, 0f, 1f);
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(30));
            user.clipFrom = EditorGUILayout.FloatField(fclipFrom);
            user.clipTo = EditorGUILayout.FloatField(fclipTo);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
        }

        protected void SaveCancelUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save")) Save();
            if (GUILayout.Button("Cancel")) Cancel();
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void Rebuild()
        {
            
        }

        protected void Repaint()
        {
            windowInstance.Repaint();
        }

        protected void GetSplines()
        {
            computers.Clear();
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                computers.Add(Selection.gameObjects[i].GetComponent<SplineComputer>());
            }
        }

        protected float LoadFloat(string name, float d)
        {
            return EditorPrefs.GetFloat(GetPrefix() + "_" + name, d);
        }

        protected string LoadString(string name, string d)
        {
            return EditorPrefs.GetString(GetPrefix() + "_" + name, d);
        }

        protected bool LoadBool(string name, bool d)
        {
            return EditorPrefs.GetBool(GetPrefix() + "_" + name, d);
        }

        protected int LoadInt(string name, int d)
        {
            return EditorPrefs.GetInt(GetPrefix() + "_" + name, d);
        }

        protected void SaveFloat(string name, float value)
        {
             EditorPrefs.SetFloat(GetPrefix() + "_" + name, value);
        }

        protected void SaveString(string name, string value)
        {
             EditorPrefs.SetString(GetPrefix() + "_" + name, value);
        }

        protected void SaveBool(string name, bool value)
        {
             EditorPrefs.SetBool(GetPrefix() + "_" + name, value);
        }

        protected void SaveInt(string name, int value)
        {
             EditorPrefs.SetInt(GetPrefix() + "_" + name, value);
        }
    }

}