#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_5_3 || UNITY_5_4_OR_NEWER
using UnityEditor.SceneManagement;
#endif


namespace Dreamteck.Splines {
    [InitializeOnLoad]
    public static class SplineDrawer
    {
        private static bool refreshComputers = false;
        private static List<SplineComputer> drawComputers = new List<SplineComputer>();
        private static Vector3 fromPos = Vector3.zero;
        private static Vector3 toPos = Vector3.zero;
        private static SplineResult fromResult = new SplineResult();
        private static SplineResult toResult = new SplineResult();
#if UNITY_5_3 || UNITY_5_4_OR_NEWER
        private static UnityEngine.SceneManagement.Scene currentScene;
#else
        private static string currentScene = "";
#endif

        static SplineDrawer()
        {
            SceneView.onSceneGUIDelegate += AutoDrawComputers;
            FindComputers();
            EditorApplication.hierarchyWindowChanged += HerarchyWindowChanged;
            EditorApplication.playmodeStateChanged += ModeChanged;
        }

        static void ModeChanged()
        {
            refreshComputers = true;
        }

        static void HerarchyWindowChanged()
        {
            #if UNITY_5_3 || UNITY_5_4_OR_NEWER
        if (currentScene != EditorSceneManager.GetActiveScene())
            {
                currentScene = EditorSceneManager.GetActiveScene();
                FindComputers();
            }
#else
        if(EditorApplication.currentScene != currentScene)
            {
                currentScene = EditorApplication.currentScene;
                FindComputers();
            }
#endif
            
        }

        static void FindComputers()
        {
            drawComputers.Clear();
            SplineComputer[] computers = GameObject.FindObjectsOfType<SplineComputer>();
            drawComputers.AddRange(computers);
        }

        private static void AutoDrawComputers(SceneView current)
        {
            if (refreshComputers)
            {
                refreshComputers = false;
                FindComputers();
            }
            for (int i = 0; i < drawComputers.Count; i++)
            {
                if (!drawComputers[i].alwaysDraw)
                {
                    drawComputers.RemoveAt(i);
                    i--;
                    continue;
                }
                DrawSplineComputer(drawComputers[i]);
            }
        }

        public static void RegisterComputer(SplineComputer comp)
        {
            if (drawComputers.Contains(comp)) return;
            comp.alwaysDraw = true;
            drawComputers.Add(comp);
        }

        public static void UnregisterComputer(SplineComputer comp)
        {
            for(int i = 0; i < drawComputers.Count; i++)
            {
                if(drawComputers[i] == comp)
                {
                    drawComputers[i].alwaysDraw = false;
                    drawComputers.RemoveAt(i);
                    return;
                }
            }
        }

        public static void DrawSplineComputer(SplineComputer comp, double fromPercent = 0.0, double toPercent = 1.0, float alpha = 1f)
        {
            if (comp == null) return;
            Color prevColor = Handles.color;
            Color orange = new Color(1f, 0.564f, 0f);
            Color handleColor = comp.hasMorph && !MorphWindow.editShapeMode ? orange : comp.editorPathColor;
            handleColor.a = alpha;
            Handles.color = handleColor;
            if (comp.pointCount < 2) return;
            double add = comp.moveStep;
            if (add < 0.0025) add = 0.0025;

            if (comp.type == Spline.Type.BSpline && comp.pointCount > 1)
            {
                SplinePoint[] compPoints = comp.GetPoints();
                Handles.color = new Color(handleColor.r, handleColor.g, handleColor.b, 0.5f * alpha);
                for (int i = 0; i < compPoints.Length - 1; i++)
                {
                    Handles.DrawLine(compPoints[i].position, compPoints[i + 1].position);
                }
                Handles.color = handleColor;
            }

            if (!comp.drawThinckness)
            {
                double percent = fromPercent;
                fromPos = comp.EvaluatePosition(percent);
                toPos = Vector3.zero;
                while (true)
                {
                    percent = DMath.Move(percent, toPercent, add);
                    toPos = comp.EvaluatePosition(percent);
                    Handles.DrawLine(fromPos, toPos);
                    if (percent == toPercent) break;
                    fromPos = toPos;
                }
                return;
            }
            else
            {
                Camera editorCamera = SceneView.currentDrawingSceneView.camera;
                double percent = fromPercent;
                comp.Evaluate(fromResult, percent);
                Vector3 fromNormal = fromResult.normal;
                if (comp.billboardThickness) fromNormal = (editorCamera.transform.position - fromResult.position).normalized;
                Vector3 fromRight = Vector3.Cross(fromResult.direction, fromNormal).normalized * fromResult.size * 0.5f;
                while (true)
                {
                    percent = DMath.Move(percent, toPercent, add);
                    toResult = comp.Evaluate(percent);
                    Vector3 toNormal = toResult.normal;
                    if (comp.billboardThickness) toNormal = (editorCamera.transform.position - toResult.position).normalized;
                    Vector3 toRight = Vector3.Cross(toResult.direction, toNormal).normalized * toResult.size * 0.5f;

                    Handles.DrawLine(fromResult.position + fromRight, toResult.position + toRight);
                    Handles.DrawLine(fromResult.position - fromRight, toResult.position - toRight);
                    Handles.DrawLine(fromResult.position + fromRight, fromResult.position - fromRight);
                    if (percent == toPercent) break;
                    fromResult = toResult;
                    fromNormal = toNormal;
                    fromRight = toRight;
                }
            }
            Handles.color = prevColor;
        }

        public static void DrawSpline(Spline spline, Color color, double from = 0.0, double to = 1.0, bool drawThickness = false, bool thicknessAutoRotate = false)
        {
            double add = spline.moveStep;
            if (add < 0.0025) add = 0.0025;
            double percent = from;
            Color prevColor = Handles.color;
            Handles.color = color;
            if (drawThickness)
            {
                Camera editorCamera = SceneView.currentDrawingSceneView.camera;
                SplineResult fromResult = spline.Evaluate(percent);
                SplineResult toResult = new SplineResult();
                Vector3 fromNormal = fromResult.normal;
                if (thicknessAutoRotate) fromNormal = (editorCamera.transform.position - fromResult.position).normalized;
                Vector3 fromRight = Vector3.Cross(fromResult.direction, fromNormal).normalized * fromResult.size * 0.5f;
                while (true)
                {
                    percent = DMath.Move(percent, to, add);
                    spline.Evaluate(toResult, percent);
                    Vector3 toNormal = toResult.normal;
                    if (thicknessAutoRotate) toNormal = (editorCamera.transform.position - toResult.position).normalized;
                    Vector3 toRight = Vector3.Cross(toResult.direction, toNormal).normalized * toResult.size * 0.5f;

                    Handles.DrawLine(fromResult.position + fromRight, toResult.position + toRight);
                    Handles.DrawLine(fromResult.position - fromRight, toResult.position - toRight);
                    Handles.DrawLine(fromResult.position + fromRight, fromResult.position - fromRight);
                    if (percent == to) break;
                    fromResult.CopyFrom(toResult);
                    fromNormal = toNormal;
                    fromRight = toRight;
                }
            }
            else
            {
                Vector3 fromPoint = spline.EvaluatePosition(percent);
                Vector3 toPoint = new Vector3();
                while (true)
                {
                    percent = DMath.Move(percent, to, add);
                    toPoint = spline.EvaluatePosition(percent);
                    Handles.DrawLine(fromPoint, toPoint);
                    if (percent == to) break;
                    fromPoint = toPoint;
                }
            }
            Handles.color = prevColor;
        }
    }
}
#endif
