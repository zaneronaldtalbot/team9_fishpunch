using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Dreamteck.Splines{
	[System.Serializable]
    //A control point used by the SplineClass
	public struct SplinePoint{
        public enum Type {Smooth, Broken};
        public Type type
        {
            get { return _type; }
            set
            {
                _type = value;
                if(value == Type.Smooth) SmoothTangent2();
            }
        }
        [FormerlySerializedAs("type")]
        public Type _type;
        public Vector3 position;
        public Color color;
        public Vector3 normal;
        public float size;
        public Vector3 tangent;
        public Vector3 tangent2;

        public static SplinePoint Lerp(SplinePoint a, SplinePoint b, float t)
        {
            SplinePoint result = a;
            result.position = Vector3.Lerp(a.position, b.position, t);
            result.color = Color.Lerp(a.color, b.color, t);
            result.size = Mathf.Lerp(a.size, b.size, t);
            result.normal = Vector3.Slerp(a.normal, b.normal, t);
            return result;
        }


        public void SetPosition(Vector3 pos)
        {
            tangent -= position - pos;
            tangent2 -= position - pos;
            position = pos;
        }

        public void SetTangentPosition(Vector3 pos)
        {
            tangent = pos;
            if (type == Type.Smooth) SmoothTangent2();
        }

        public void SetTangent2Position(Vector3 pos)
        {
            tangent2 = pos;
            if (type == Type.Smooth) SmoothTangent();
        }

        public SplinePoint(Vector3 p)
        {
            position = p;
            tangent = p;
            tangent2 = p;
            color = Color.white;
            normal = Vector3.up;
            size = 1f;
            _type = Type.Smooth;
            if (_type == Type.Smooth) SmoothTangent2();
        }
		
		public SplinePoint(Vector3 p, Vector3 t){
			position = p;
			tangent = t;
            tangent2 = p + (p - t);
            color = Color.white;
            normal = Vector3.up;
            size = 1f;
            _type = Type.Smooth;
            if (_type == Type.Smooth) SmoothTangent2();
        }	
		
		public SplinePoint(Vector3 pos, Vector3 tan, Vector3 nor, float s, Color col){
			position = pos;
			tangent = tan;
            tangent2 = pos + (pos - tan);
			normal = nor;
			size = s;
			color = col;
            _type = Type.Smooth;
            if (_type == Type.Smooth) SmoothTangent2();
        }

        public SplinePoint(Vector3 pos, Vector3 tan, Vector3 tan2, Vector3 nor, float s, Color col)
        {
            position = pos;
            tangent = tan;
            tangent2 = tan2;
            normal = nor;
            size = s;
            color = col;
            _type = Type.Broken;
            if (_type == Type.Smooth) SmoothTangent2();
        }

        public SplinePoint(SplinePoint source)
        {
            position = source.position;
            tangent = source.tangent;
            tangent2 = source.tangent2;
            color = source.color;
            normal = source.normal;
            size = source.size;
            _type = source.type;
            if (_type == Type.Smooth) SmoothTangent2();
        }

        private void SmoothTangent2()
        {
            tangent2 = position + (position - tangent);
        }

        private void SmoothTangent()
        {
            tangent = position + (position - tangent2);
        }

    }
}
