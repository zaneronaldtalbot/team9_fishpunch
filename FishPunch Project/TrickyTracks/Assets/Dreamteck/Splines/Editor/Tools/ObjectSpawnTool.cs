using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace Dreamteck.Splines
{
    public class ObjectSpawnTool : SplineTool
    {
        protected List<ObjectController> controllers = new List<ObjectController>();

        public override string GetName()
        {
            return "Spawn Objects";
        }

        protected override string GetPrefix()
        {
            return "ObjectSpawnTool";
        }

        public override void Close()
        {
            base.Close();
            if (promptSave)
            {
                if (EditorUtility.DisplayDialog("Save changes?", "You are about to close the Object Spawn Tool, do you want to save the generated objects?", "Yes", "No")) Save();
                else Cancel();
            }
            else Cancel();
            promptSave = false;
        }

        public override void Open(EditorWindow window)
        {
            base.Open(window);

            GetSplines();
            controllers.Clear();
            for (int i = 0; i < computers.Count; i++)
            {
                controllers.Add(CreateController(computers[i], computers[i].name + "_objects"));
            }
            Rebuild();
        }

        protected override void OnSplineAdded(SplineComputer spline)
        {
            base.OnSplineAdded(spline);
            controllers.Add(CreateController(spline, spline.name + "_objects"));
            Rebuild();
        }

        protected override void OnSplineRemoved(SplineComputer spline)
        {
            base.OnSplineAdded(spline);
            for (int i = 0; i < controllers.Count; i++)
            {
                if(controllers[i].computer == spline)
                {
                    GameObject.DestroyImmediate(controllers[i].gameObject);
                    controllers.RemoveAt(i);
                    i--;
                    continue;
                }
            }
            Rebuild();
        }

        public override void Draw(Rect windowRect)
        {
            base.Draw(windowRect);
            if (computers.Count == 0)
            {
                EditorGUILayout.HelpBox("No spline selected! Select an object with a SplineComputer component.", MessageType.Warning);
                return;
            }
            EditorGUI.BeginChangeCheck();
            ObjectController controller = controllers[0];
            ClipUI(controller);
            float labelWidth = EditorGUIUtility.labelWidth;
            float fieldWidth = EditorGUIUtility.fieldWidth;
            EditorGUIUtility.labelWidth = 0;
            EditorGUIUtility.fieldWidth = 0;

            EditorGUILayout.BeginVertical();
            for (int i = 0; i < controller.objects.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                controller.objects[i] = (GameObject)EditorGUILayout.ObjectField(controller.objects[i], typeof(GameObject), true);
                if (GUILayout.Button("x", GUILayout.Width(20)))
                {
                    GameObject[] newObjects = new GameObject[controller.objects.Length - 1];
                    for (int n = 0; n < controller.objects.Length; n++)
                    {
                        if (n < i) newObjects[n] = controller.objects[n];
                        else if (n == i) continue;
                        else newObjects[n - 1] = controller.objects[n];
                    }
                    controller.objects = newObjects;
                }
                if (i > 0)
                {
                    if (GUILayout.Button("▲", GUILayout.Width(20)))
                    {
                        GameObject temp = controller.objects[i - 1];
                        controller.objects[i - 1] = controller.objects[i];
                        controller.objects[i] = temp;
                    }
                }
                if (i < controller.objects.Length - 1)
                {
                    if (GUILayout.Button("▼", GUILayout.Width(20)))
                    {
                        GameObject temp = controller.objects[i + 1];
                        controller.objects[i + 1] = controller.objects[i];
                        controller.objects[i] = temp;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            GameObject newObj = null;
            newObj = (GameObject)EditorGUILayout.ObjectField("Add Object", newObj, typeof(GameObject), true);
            if (newObj != null)
            {
                GameObject[] newObjects = new GameObject[controller.objects.Length + 1];
                controller.objects.CopyTo(newObjects, 0);
                newObjects[newObjects.Length - 1] = newObj;
                controller.objects = newObjects;
            }

            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUIUtility.fieldWidth = fieldWidth;
            bool hasObj = false;
            for (int i = 0; i < controller.objects.Length; i++)
            {
                if (controller.objects[i] != null)
                {
                    hasObj = true;
                    break;
                }
            }

            if (hasObj) controller.spawnCount = EditorGUILayout.IntField("Spawn count", controller.spawnCount);
            else controller.spawnCount = 0;
            controller.iteration = (ObjectController.Iteration)EditorGUILayout.EnumPopup("Iteration", controller.iteration);
            

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Transform", EditorStyles.boldLabel);
            controller.applyRotation = EditorGUILayout.Toggle("Apply Rotation", controller.applyRotation);
            if (controller.applyRotation)
            {
                EditorGUI.indentLevel++;
                controller.minRotationOffset = EditorGUILayout.Vector3Field("Min. Rotation Offset", controller.minRotationOffset);
                controller.maxRotationOffset = EditorGUILayout.Vector3Field("Max. Rotation Offset", controller.maxRotationOffset);
                EditorGUI.indentLevel--;
            }
            controller.applyScale = EditorGUILayout.Toggle("Apply Scale", controller.applyScale);
            if (controller.applyScale)
            {
                EditorGUI.indentLevel++;
                controller.minScaleMultiplier = EditorGUILayout.Vector3Field("Min. Scale Multiplier", controller.minScaleMultiplier);
                controller.maxScaleMultiplier = EditorGUILayout.Vector3Field("Max. Scale Multiplier", controller.maxScaleMultiplier);
                EditorGUI.indentLevel--;
            }

            controller.objectPositioning = (ObjectController.Positioning)EditorGUILayout.EnumPopup("Object Positioning", controller.objectPositioning);
            controller.positionOffset = EditorGUILayout.Slider("Evaluate Offset", controller.positionOffset, -1f, 1f);

            controller.offset = EditorGUILayout.Vector2Field("Offset", controller.offset);
            controller.randomizeOffset = EditorGUILayout.Toggle("Randomize Offset", controller.randomizeOffset);
            if (controller.randomizeOffset)
            {
                controller.randomSize = EditorGUILayout.Vector2Field("Size", controller.randomSize);
                controller.randomSeed = EditorGUILayout.IntField("Random Seed", controller.randomSeed);
                //user.randomOffsetSize = EditorGUILayout.FloatField("Size", user.randomOffsetSize);
                controller.shellOffset = EditorGUILayout.Toggle("Shell", controller.shellOffset);
                controller.useRandomOffsetRotation = EditorGUILayout.Toggle("Apply offset rotation", controller.useRandomOffsetRotation);
            }

            if (EditorGUI.EndChangeCheck())
            {
                promptSave = true;
                Rebuild();
            }

            EditorGUILayout.BeginHorizontal();
            if (controllers.Count == 0)
            {
                if (GUILayout.Button("New"))
                {
                    foreach (SplineComputer spline in computers)
                    {
                        controllers.Add(CreateController(spline, spline.name + "_objects"));
                    }
                    Rebuild();
                }
            }
            else
            {
                if (GUILayout.Button("Save"))
                {
                    Save();
                }
                if (GUILayout.Button("Cancel"))
                {
                    Cancel();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        protected override void Save()
        {
            base.Save();
            foreach (ObjectController controller in controllers)
            {
                controller.transform.parent = controller.computer.transform;
                GameObject.DestroyImmediate(controller);
            }
            controllers.Clear();
        }

        protected override void Cancel()
        {
            base.Cancel();
            foreach(ObjectController controller in controllers) GameObject.DestroyImmediate(controller.gameObject);
            controllers.Clear();
        }

        protected ObjectController CreateController(SplineComputer computer, string name)
        {
            GameObject obj = new GameObject(name);
            obj.transform.position = computer.transform.position;
            obj.transform.rotation = computer.transform.rotation;
            obj.transform.localScale = computer.transform.localScale;
            obj.transform.parent = computer.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            ObjectController controller = obj.AddComponent<ObjectController>();
            controller.computer = computer;
            return controller;
        }

        protected override void Rebuild()
        {
            base.Rebuild();
            if (controllers.Count == 0) return;
            ObjectController controller = controllers[0];
            foreach (ObjectController c in controllers)
            {
                if(c == null)
                {
                    controllers.Remove(null);
                    continue;
                }
                controller.enabled = false;
                c.resolution = controller.resolution;
                c.clipFrom = controller.clipFrom;
                c.clipTo = controller.clipTo;
                c.objectMethod = ObjectController.ObjectMethod.Instantiate;
                c.delayedSpawn = false;
                c.objects = controller.objects;
                c.spawnCount = controller.spawnCount;
                c.iteration = controller.iteration;
                c.applyRotation = controller.applyRotation;
                c.applyScale = controller.applyScale;
                c.objectPositioning = controller.objectPositioning;
                c.positionOffset = controller.positionOffset;
                c.offset = controller.offset;
                c.randomizeOffset = controller.randomizeOffset;
                c.randomSize = controller.randomSize;
                c.randomSeed = controller.randomSeed;
                c.shellOffset = controller.shellOffset;
                c.useRandomOffsetRotation = controller.useRandomOffsetRotation;
                c.Rebuild(true);
                EditorUtility.SetDirty(c);
            }
        }
    }
}
