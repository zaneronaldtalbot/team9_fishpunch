using UnityEngine;
using System.Collections;
using UnityEditor;
using Dreamteck.Splines.Primitives;

namespace Dreamteck.Splines
{
    public class CapsuleEditor : PrimitiveEditor
    {
        Capsule capsule = new Capsule();

        public override string GetName()
        {
            return "Capsule";
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            AxisGUI(capsule);
            OffsetGUI(capsule);
            RotationGUI(capsule);
            capsule.radius = EditorGUILayout.FloatField("Radius", capsule.radius);
            capsule.height = EditorGUILayout.FloatField("Height", capsule.height);
        }

        protected override void Update()
        {
            capsule.UpdateSplineComputer(computer);
            base.Update();
        }
    }
}
