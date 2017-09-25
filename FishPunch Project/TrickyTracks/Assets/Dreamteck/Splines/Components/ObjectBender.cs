using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Dreamteck.Splines
{
    [AddComponentMenu("Dreamteck/Splines/Object Bender")]
    public class ObjectBender : SplineUser
    {
        public enum Axis { X, Y, Z }
        public bool bend
        {
            get { return _bend; }
            set
            {
               if(_bend != value)
                {
                    _bend = value;
                    if (value)
                    {
                        UpdateReferences();
                        Rebuild(false);
                    } else Revert();
                }
            }
        }

        [SerializeField]
        [HideInInspector]
        private bool _bend = false;
        public Axis axis
        {
            get { return _axis; }
            set
            {
                if (computer != null && value != _axis)
                {
                    _axis = value;
                    UpdateReferences();
                    Rebuild(false);
                }
                else _axis = value;
            }
        }
        public bool autoNormals
        {
            get { return _autoNormals; }
            set
            {
                if (computer != null && value != _autoNormals)
                {
                    _autoNormals = value;
                    Rebuild(false);
                } else _autoNormals = value;
            }
        }

        public Vector3 upVector
        {
            get { return _upVector; }
            set
            {
                if (computer != null && value != _upVector)
                {
                    _upVector = value;
                    Rebuild(false);
                }
                else _upVector = value;
            }
        }
        [HideInInspector]
        public BendProperty[] bendProperties = new BendProperty[0];
        [SerializeField]
        [HideInInspector]
        private TS_Bounds bounds = null;

        [SerializeField]
        [HideInInspector]
        private Axis _axis = Axis.Z;
        [SerializeField]
        [HideInInspector]
        private bool _autoNormals = false;
        [SerializeField]
        [HideInInspector]
        private Vector3 _upVector = Vector3.up;
        private SplineResult bendResult = new SplineResult();

        private void GetTransformsRecursively(Transform current, ref List<Transform> transformList)
        {
            transformList.Add(current);
            foreach (Transform child in current)
            {
                GetTransformsRecursively(child, ref transformList);
            }
        }

        private void GetObjects()
        {
            List<Transform> found = new List<Transform>();
            GetTransformsRecursively(this.transform, ref found);
            BendProperty[] newProperties = new BendProperty[found.Count];
            for (int i = 0; i < found.Count; i++)
            {
                CreateProperty(ref newProperties[i], found[i]);
            }
            bendProperties = newProperties;
        }

        public TS_Bounds GetBounds()
        {
            return new TS_Bounds(bounds.min, bounds.max, bounds.center);
        }

        private void CreateProperty(ref BendProperty property, Transform t)
        {
            property = new BendProperty(t, t == this.transform); //Create a new bend property for each child
            for (int i = 0; i < bendProperties.Length; i++)
            {
                //Search for properties that have the same trasform and copy their settings
                if (bendProperties[i].transform.transform == t)
                {
                    property.applyRotation = bendProperties[i].applyRotation;
                    property.applyScale = bendProperties[i].applyScale;
                    property.bendMesh = bendProperties[i].bendMesh;
                    property.bendCollider = bendProperties[i].bendCollider;
                    property.colliderUpdateRate = bendProperties[i].colliderUpdateRate;
                    break;
                }
            }
            if (t != this.transform)
            {
                property.originalPosition = this.transform.InverseTransformPoint(t.position);
                property.originalRotation = Quaternion.Inverse(this.transform.rotation) * t.rotation;
            }
        }

        private void CalculateBounds()
        {
            if (bounds == null) bounds = new TS_Bounds(Vector3.zero, Vector3.zero);
            bounds.min = bounds.max = Vector3.zero;
            for (int i = 0; i < bendProperties.Length; i++)
            {
                CalculatePropertyBounds(ref bendProperties[i]);
            }
            for (int i = 0; i < bendProperties.Length; i++)
            {
                GetPercent(bendProperties[i]);
            }
        }

        private void CalculatePropertyBounds(ref BendProperty property)
        {
            if (property.transform.transform == this.transform)
            {
                if (0f < bounds.min.x) bounds.min.x = 0f;
                if (0f < bounds.min.y) bounds.min.y = 0f;
                if (0f < bounds.min.z) bounds.min.z = 0f;
                if (0f > bounds.max.x) bounds.max.x = 0f;
                if (0f > bounds.max.y) bounds.max.y = 0f;
                if (0f > bounds.max.z) bounds.max.z = 0f;
            }
            else
            {
                if (property.originalPosition.x < bounds.min.x) bounds.min.x = property.originalPosition.x;
                if (property.originalPosition.y < bounds.min.y) bounds.min.y = property.originalPosition.y;
                if (property.originalPosition.z < bounds.min.z) bounds.min.z = property.originalPosition.z;
                if (property.originalPosition.x > bounds.max.x) bounds.max.x = property.originalPosition.x;
                if (property.originalPosition.y > bounds.max.y) bounds.max.y = property.originalPosition.y;
                if (property.originalPosition.z > bounds.max.z) bounds.max.z = property.originalPosition.z;
            }
            if (property.editMesh != null)
            {
                for (int n = 0; n < property.editMesh.vertices.Length; n++)
                {
                    Vector3 localPos = property.transform.TransformPoint(property.editMesh.vertices[n]);
                    localPos = this.transform.InverseTransformPoint(localPos);
                    if (localPos.x < bounds.min.x) bounds.min.x = localPos.x;
                    if (localPos.y < bounds.min.y) bounds.min.y = localPos.y;
                    if (localPos.z < bounds.min.z) bounds.min.z = localPos.z;
                    if (localPos.x > bounds.max.x) bounds.max.x = localPos.x;
                    if (localPos.y > bounds.max.y) bounds.max.y = localPos.y;
                    if (localPos.z > bounds.max.z) bounds.max.z = localPos.z;
                }
            }

            if (property.editColliderMesh != null)
            {
                for (int n = 0; n < property.editColliderMesh.vertices.Length; n++)
                {
                    Vector3 localPos = property.transform.TransformPoint(property.editColliderMesh.vertices[n]);
                    localPos = this.transform.InverseTransformPoint(localPos);
                    if (localPos.x < bounds.min.x) bounds.min.x = localPos.x;
                    if (localPos.y < bounds.min.y) bounds.min.y = localPos.y;
                    if (localPos.z < bounds.min.z) bounds.min.z = localPos.z;
                    if (localPos.x > bounds.max.x) bounds.max.x = localPos.x;
                    if (localPos.y > bounds.max.y) bounds.max.y = localPos.y;
                    if (localPos.z > bounds.max.z) bounds.max.z = localPos.z;
                }
            }

            if (property.originalSpline != null)
            {
                for (int n = 0; n < property.originalSpline.points.Length; n++)
                {
                    Vector3 localPos = this.transform.InverseTransformPoint(property.originalSpline.points[n].position);
                    if (localPos.x < bounds.min.x) bounds.min.x = localPos.x;
                    if (localPos.y < bounds.min.y) bounds.min.y = localPos.y;
                    if (localPos.z < bounds.min.z) bounds.min.z = localPos.z;
                    if (localPos.x > bounds.max.x) bounds.max.x = localPos.x;
                    if (localPos.y > bounds.max.y) bounds.max.y = localPos.y;
                    if (localPos.z > bounds.max.z) bounds.max.z = localPos.z;
                }
            }
            bounds.CreateFromMinMax(bounds.min, bounds.max);
        }

        public void GetPercent(BendProperty property)
        {
            if (property.transform.transform != this.transform) property.positionPercent = GetPercentage(this.transform.InverseTransformPoint(property.transform.position));
            else property.positionPercent = GetPercentage(Vector3.zero);
            if (property.editMesh != null)
            {
                if (property.vertexPercents.Length != property.editMesh.vertexCount) property.vertexPercents = new Vector3[property.editMesh.vertexCount];
                if (property.editColliderMesh != null)
                {
                    if (property.colliderVertexPercents.Length != property.editMesh.vertexCount) property.colliderVertexPercents = new Vector3[property.editColliderMesh.vertexCount];
                }
                for (int i = 0; i < property.editMesh.vertexCount; i++)
                {
                    Vector3 localVertex = property.transform.TransformPoint(property.editMesh.vertices[i]);
                    localVertex = this.transform.InverseTransformPoint(localVertex);
                    property.vertexPercents[i] = GetPercentage(localVertex);
                    
                }
                if (property.editColliderMesh != null)
                {
                    for (int i = 0; i < property.editColliderMesh.vertexCount; i++)
                    {
                        Vector3 localVertex = property.transform.TransformPoint(property.editColliderMesh.vertices[i]);
                        localVertex = this.transform.InverseTransformPoint(localVertex);
                        property.colliderVertexPercents[i] = GetPercentage(localVertex);
                    }
                }
            }
            if (property.splineComputer != null)
            {
                SplinePoint[] points = property.splineComputer.GetPoints();
                property.splinePointPercents = new Vector3[points.Length];
                property.primaryTangentPercents = new Vector3[points.Length];
                property.secondaryTangentPercents = new Vector3[points.Length];
                for (int i = 0; i < points.Length; i++)
                {
                    property.splinePointPercents[i] = GetPercentage(this.transform.InverseTransformPoint(points[i].position));
                    property.primaryTangentPercents[i] = GetPercentage(this.transform.InverseTransformPoint(points[i].tangent));
                    property.secondaryTangentPercents[i] = GetPercentage(this.transform.InverseTransformPoint(points[i].tangent2));
                }
            }
        }

        private void Revert()
        {
            for (int i = 0; i < bendProperties.Length; i++)
            {
                bendProperties[i].Revert();
            }
        }


        public void UpdateReferences()
        {
#if UNITY_EDITOR
            if (PrefabUtility.GetPrefabType(this.gameObject) == PrefabType.Prefab) return;
#endif
            if (_bend)
            {
                for (int i = 0; i < bendProperties.Length; i++) bendProperties[i].Revert();
            }
            GetObjects();
            CalculateBounds();
            if (_bend)
            {
                Bend();
                for (int i = 0; i < bendProperties.Length; i++)
                {
                    bendProperties[i].Apply(i > 0 || this.transform != rootUser.computer.transform);
                    bendProperties[i].Update();
                }
            }
        }

        private void GetBendResult(Vector3 percentage)
        {
            Evaluate(bendResult, percentage.z);
            Vector3 right = bendResult.right;
            bendResult.position += right * Mathf.Lerp(bounds.min.x, bounds.max.x, percentage.x) * bendResult.size;
            if (_autoNormals)
            {
                Vector3 worldRight = Vector3.Cross(bendResult.direction, _upVector).normalized;
                bendResult.position += Vector3.Cross(worldRight, bendResult.direction).normalized * Mathf.Lerp(bounds.min.y, bounds.max.y, percentage.y) * bendResult.size;
            } else bendResult.position += bendResult.normal * Mathf.Lerp(bounds.min.y, bounds.max.y, percentage.y) * bendResult.size;
        }

        private Vector3 GetPercentage(Vector3 point)
        {
            float x = 0f, y = 0f, z = 0f;
            switch (axis)
            {
                case Axis.X:
                    x = Mathf.Clamp01(Mathf.InverseLerp(bounds.max.z, bounds.min.z, point.z));
                    y = Mathf.Clamp01(Mathf.InverseLerp(bounds.min.y, bounds.max.y, point.y));
                    z = Mathf.Clamp01(Mathf.InverseLerp(bounds.min.x, bounds.max.x, point.x));
                    break;
                case Axis.Y:
                    x = Mathf.Clamp01(Mathf.InverseLerp(bounds.min.x, bounds.max.x, point.x));
                    y = Mathf.Clamp01(Mathf.InverseLerp(bounds.max.z, bounds.min.z, point.z));
                    z = Mathf.Clamp01(Mathf.InverseLerp(bounds.min.y, bounds.max.y, point.y));
                    break;
                case Axis.Z:
                    x = Mathf.Clamp01(Mathf.InverseLerp(bounds.min.x, bounds.max.x, point.x));
                    y = Mathf.Clamp01(Mathf.InverseLerp(bounds.min.y, bounds.max.y, point.y));
                    z = Mathf.Clamp01(Mathf.InverseLerp(bounds.min.z, bounds.max.z, point.z));
                    break;
            }
            return new Vector3(x, y, z);
        }

        protected override void Build()
        {
            base.Build();
            if (_bend) Bend();
        }

        private void Bend()
        {
            if (samples.Length <= 1)
            {
                return;
            }
            for (int i = 0; i < bendProperties.Length; i++)
            {
                BendObject(bendProperties[i]);
            }
        }

        public void BendObject(BendProperty p)
        {
            if (!p.enabled) return;
            Quaternion axisRotation = Quaternion.identity;
            switch (axis)
            {
                case Axis.X: axisRotation = Quaternion.AngleAxis(-90f, Vector3.up); break;
                case Axis.Y: axisRotation = Quaternion.AngleAxis(90f, Vector3.right); break;
            }

            GetBendResult(p.positionPercent);

            p.transform.position = bendResult.position;
            if (p.applyRotation) p.transform.rotation = bendResult.rotation * axisRotation * p.originalRotation;
            else p.transform.rotation = p.originalRotation; 
            if (p.applyScale) p.transform.scale = p.originalScale * bendResult.size;

            if (p.editMesh != null)
            {
                for (int n = 0; n < p.vertexPercents.Length; n++)
                {
                    GetBendResult(p.vertexPercents[n]);
                    p.editMesh.vertices[n] = bendResult.position;
                    switch (axis)
                    {
                        case Axis.X: p.editMesh.normals[n] = Quaternion.LookRotation(bendResult.direction, bendResult.normal) * axisRotation * Quaternion.FromToRotation(Vector3.up, bendResult.normal) * p.normals[n]; break;
                        case Axis.Y: p.editMesh.normals[n] = Quaternion.LookRotation(bendResult.direction, bendResult.normal) * axisRotation * Quaternion.FromToRotation(Vector3.up, bendResult.normal) * p.normals[n]; break;
                        case Axis.Z: p.editMesh.normals[n] = Quaternion.LookRotation(bendResult.direction, bendResult.normal) * p.normals[n];
                            break;
                    }
                }
                p.editMesh.hasUpdate = true;
            }

            if (p._editColliderMesh != null)
            {
                for (int n = 0; n < p.colliderVertexPercents.Length; n++)
                {

                    GetBendResult(p.colliderVertexPercents[n]);
                    p.editColliderMesh.vertices[n] = bendResult.position;
                    switch (axis)
                    {
                        case Axis.X: p.editColliderMesh.normals[n] = Quaternion.LookRotation(bendResult.direction, bendResult.normal) * axisRotation * Quaternion.FromToRotation(Vector3.up, bendResult.normal) * p.colliderNormals[n]; break;
                        case Axis.Y: p.editColliderMesh.normals[n] = Quaternion.LookRotation(bendResult.direction, bendResult.normal) * axisRotation * Quaternion.FromToRotation(Vector3.up, bendResult.normal) * p.colliderNormals[n]; break;
                        case Axis.Z: p.editColliderMesh.normals[n] = Quaternion.LookRotation(bendResult.direction, bendResult.normal) * p.colliderNormals[n]; break;
                    }
                }
                p.editColliderMesh.hasUpdate = true;
            }

            if (p.originalSpline != null)
            {
                for (int n = 0; n < p.splinePointPercents.Length; n++)
                {
                    SplinePoint point = p.originalSpline.points[n];
                    GetBendResult(p.splinePointPercents[n]);
                    point.position = bendResult.position;
                    GetBendResult(p.primaryTangentPercents[n]);
                    point.tangent = bendResult.position;
                    GetBendResult(p.secondaryTangentPercents[n]);
                    point.tangent2 = bendResult.position;
                    switch (axis)
                    {
                        case Axis.X: point.normal = Quaternion.LookRotation(bendResult.direction, bendResult.normal) * axisRotation * Quaternion.FromToRotation(Vector3.up, bendResult.normal) * point.normal; break;
                        case Axis.Y: point.normal = Quaternion.LookRotation(bendResult.direction, bendResult.normal) * axisRotation * Quaternion.FromToRotation(Vector3.up, bendResult.normal) * point.normal; break;
                        case Axis.Z: point.normal = Quaternion.LookRotation(bendResult.direction, bendResult.normal) * point.normal; break;
                    }
                    p.destinationSpline.points[n] = point;
                }
            }
        }

        protected override void PostBuild()
        {
            base.PostBuild();
            if (!_bend) return;
            for (int i = 0; i < bendProperties.Length; i++)
            {
                bendProperties[i].Apply(i > 0 || this.transform != rootUser.computer.transform);
                bendProperties[i].Update();
            }
        }

        protected override void LateRun()
        {
            base.LateRun();
            for (int i = 0; i < bendProperties.Length; i++)
            {
                bendProperties[i].Update();
            }
        }


        [System.Serializable]
        public class BendProperty
        {
            public bool enabled = true;
            public bool isValid
            {
                get
                {
                    return transform != null && transform.transform != null;
                }
            }
            public TS_Transform transform;
            public bool applyRotation = true;
            public bool applyScale = true;
            public bool bendMesh
            {
                get { return _bendMesh; }
                set
                {
                    if (value != _bendMesh)
                    {
                        _bendMesh = value;
                        if (value)
                        {
                            if (filter != null && filter.sharedMesh != null)
                            {
                                normals = originalMesh.normals;
                                for (int i = 0; i < normals.Length; i++) normals[i] = transform.transform.TransformDirection(normals[i]);
                            }
                        } else RevertMesh();
                    }
                }
            }
            public bool bendCollider
            {
                get { return _bendCollider; }
                set
                {
                    if (value != _bendCollider)
                    {
                        _bendCollider = value;
                        if (value)
                        {
                            if (collider != null && collider.sharedMesh != null && collider.sharedMesh != originalMesh) colliderNormals = originalColliderMesh.normals;
                        }
                        else RevertCollider();
                    }
                }
            }
            public bool bendSpline
            {
                get { return _bendSpline; }
                set
                {
                    _bendSpline = value;
                    if (value)
                    {

                    }
                }
            }
            [SerializeField]
            [HideInInspector]
            private bool _bendMesh = true;
            [SerializeField]
            [HideInInspector]
            private bool _bendSpline = true;
            [SerializeField]
            [HideInInspector]
            private bool _bendCollider = true;

            private float colliderUpdateDue = 0f;
            public float colliderUpdateRate = 0.2f;
            private bool updateCollider = false;

            public Vector3 originalPosition = Vector3.zero;
            public Vector3 originalScale = Vector3.one;
            public Quaternion originalRotation = Quaternion.identity;
            public Vector3 positionPercent;

            public Vector3[] vertexPercents = new Vector3[0];
            public Vector3[] normals = new Vector3[0];
            public Vector3[] colliderVertexPercents = new Vector3[0];
            public Vector3[] colliderNormals = new Vector3[0];

            [SerializeField]
            [HideInInspector]
            private Mesh originalMesh = null;
            [SerializeField]
            [HideInInspector]
            private Mesh originalColliderMesh = null;
            private Spline _originalSpline;

            [SerializeField]
            [HideInInspector]
            private Mesh destinationMesh = null;
            [SerializeField]
            [HideInInspector]
            private Mesh destinationColliderMesh = null;
            public Spline destinationSpline;

            public TS_Mesh editMesh
            {
                get
                {
                    if (!bendMesh || originalMesh == null) _editMesh = null;
                    else if (_editMesh == null && originalMesh != null) _editMesh = new TS_Mesh(originalMesh);
                    return _editMesh;
                }
            }
            public TS_Mesh editColliderMesh
            {
                get
                {
                    if (!bendCollider || originalColliderMesh == null) _editColliderMesh = null;
                    else if (_editColliderMesh == null && originalColliderMesh != null && originalColliderMesh != originalMesh) _editColliderMesh = new TS_Mesh(originalColliderMesh);
                    return _editColliderMesh;
                }
            }
            public Spline originalSpline
            {
                get
                {
                    if (!bendSpline || splineComputer == null) _originalSpline = null;
                    else if (_originalSpline == null && splineComputer != null) {
                        _originalSpline = new Spline(splineComputer.type);
                        _originalSpline.points = splineComputer.GetPoints();
                    }
                    return _originalSpline;
                }
            }

            public TS_Mesh _editMesh = null;
            public TS_Mesh _editColliderMesh = null;

            public MeshFilter filter = null;
            public MeshCollider collider = null;
            public SplineComputer splineComputer = null;

            public Vector3[] splinePointPercents = new Vector3[0];
            public Vector3[] primaryTangentPercents = new Vector3[0];
            public Vector3[] secondaryTangentPercents = new Vector3[0];

            [SerializeField]
            [HideInInspector]
            private bool parent;

            public BendProperty(Transform t, bool isParent = false)
            {
                parent = isParent;
                transform = new TS_Transform(t);
                originalPosition = t.localPosition;
                originalScale = t.localScale;
                originalRotation = t.localRotation;
                filter = t.GetComponent<MeshFilter>();
                collider = t.GetComponent<MeshCollider>();
                if (filter != null && filter.sharedMesh != null)
                {
                    originalMesh = filter.sharedMesh;
                    normals = originalMesh.normals;
                    for (int i = 0; i < normals.Length; i++) normals[i] = transform.transform.TransformDirection(normals[i]).normalized;
                }

                if (collider != null && collider.sharedMesh != null)
                {
                    originalColliderMesh = collider.sharedMesh;
                    colliderNormals = originalColliderMesh.normals;
                    for (int i = 0; i < colliderNormals.Length; i++) colliderNormals[i] = transform.transform.TransformDirection(colliderNormals[i]);
                }
                if (!parent) splineComputer = t.GetComponent<SplineComputer>();
                if (splineComputer != null)
                {
                    if (splineComputer.isClosed) originalSpline.Close();
                    destinationSpline = new Spline(originalSpline.type);
                    destinationSpline.points = new SplinePoint[originalSpline.points.Length];
                    destinationSpline.points = splineComputer.GetPoints();
                    if (splineComputer.isClosed) destinationSpline.Close();
                }
            }

            public void Revert()
            {
                if (!isValid) return;
                RevertTransform();
                RevertCollider();
                RevertMesh();
                if (splineComputer != null) splineComputer.SetPoints(_originalSpline.points);
            }

            private void RevertMesh()
            {
                if (filter != null) filter.sharedMesh = originalMesh;
                destinationMesh = null;
            }

            private void RevertTransform()
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    transform.transform.localPosition = originalPosition;
                    transform.transform.localRotation = originalRotation;
                }
                else
                {
                    transform.localPosition = originalPosition;
                    transform.localRotation = originalRotation;
                    transform.Update();
                }
#else
                transform.localPosition = originalPosition;
                transform.localRotation = originalRotation;
                transform.Update();
#endif
                transform.scale = originalScale;
                transform.Update();
            }

            private void RevertCollider()
            {
                if (collider != null) collider.sharedMesh = originalColliderMesh;
                destinationColliderMesh = null;
            }

            public void Apply(bool applyTransform)
            {
                if (!enabled) return;
                if (!isValid) return;
                if(applyTransform) transform.Update();
                if (editMesh != null && editMesh.hasUpdate)  ApplyMesh();
                if (bendCollider && collider != null)
                {
                    if (!updateCollider)
                    {
                        if((editColliderMesh == null && editMesh != null) || editColliderMesh != null)
                        {
                            updateCollider = true;
                            if(Application.isPlaying) colliderUpdateDue = Time.time + colliderUpdateRate;
                        }
                    }
                }
                if (splineComputer != null) ApplySpline();
            }

            public void Update()
            {
                if (Time.time >= colliderUpdateDue && updateCollider)
                {
                    updateCollider = false;
                    ApplyCollider();
                }
            }

            private void ApplyMesh()
            {
                if (filter == null) return;
                MeshUtility.InverseTransformMesh(editMesh, transform.transform);
                MeshUtility.CalculateTangents(editMesh);
                if (destinationMesh == null)
                {
                    destinationMesh = new Mesh();
                    destinationMesh.name = originalMesh.name;
                }

                editMesh.WriteMesh(ref destinationMesh);
                destinationMesh.RecalculateBounds();
                filter.sharedMesh = destinationMesh;
            }

            private void ApplyCollider()
            {
                if (collider == null) return;
                if (originalColliderMesh == originalMesh) collider.sharedMesh = filter.sharedMesh; //if the collider has the same mesh as the filter - just copy it
                else
                {
                    MeshUtility.InverseTransformMesh(editColliderMesh, transform.transform);
                    MeshUtility.CalculateTangents(editColliderMesh);
                    if (destinationColliderMesh == null)
                    {
                        destinationColliderMesh = new Mesh();
                        destinationColliderMesh.name = originalColliderMesh.name;
                    }
                    editColliderMesh.WriteMesh(ref destinationColliderMesh);
                    destinationColliderMesh.RecalculateBounds();
                    collider.sharedMesh = destinationColliderMesh;
                }
            }

            private void ApplySpline()
            {
                if (destinationSpline == null) return;
                splineComputer.SetPoints(destinationSpline.points);
            }
        }
    }
}
