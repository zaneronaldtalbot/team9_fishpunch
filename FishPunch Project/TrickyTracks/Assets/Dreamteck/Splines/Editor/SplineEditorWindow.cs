using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Dreamteck.Splines {
    public class SplineEditorWindow : EditorWindow
    {
        protected Editor editor;
#if UNITY_5_0
        protected GUIContent titleContent;
#endif

        public virtual void init(Editor input, string name, Vector2 minSize)
        {
            editor = input;
            SetName(name);
            Rect size = this.position;
            if (size.width < minSize.x) size.x = minSize.x;
            if (size.height < minSize.y) size.y = minSize.y;
            this.position = size;
        }

        protected void SetName(string name)
        {
            titleContent = new GUIContent(name);
        }
    }
}
