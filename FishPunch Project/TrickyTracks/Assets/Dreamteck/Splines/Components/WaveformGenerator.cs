using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dreamteck.Splines
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [AddComponentMenu("Dreamteck/Splines/Waveform Generator")]
    public class WaveformGenerator : MeshGenerator
    {
        public enum Axis { X, Y, Z }
        public enum UVWrapMode { Clamp, UniformX, UniformY, Uniform }

        public Axis axis
        {
            get { return _axis; }
            set
            {
                if (value != _axis)
                {
                    _axis = value;
                    Rebuild(false);
                }
            }
        }

        public bool symmetry
        {
            get { return _symmetry; }
            set
            {
                if (value != _symmetry)
                {
                    _symmetry = value;
                    Rebuild(false);
                }
            }
        }

        public UVWrapMode uvWrapMode
        {
            get { return _uvWrapMode; }
            set
            {
                if (value != _uvWrapMode)
                {
                    _uvWrapMode = value;
                    Rebuild(false);
                }
            }
        }

        public int slices
        {
            get { return _slices; }
            set
            {
                if (value != _slices)
                {
                    if (value < 1) value = 1;
                    _slices = value;
                    Rebuild(false);
                }
            }
        }

        [SerializeField]
        [HideInInspector]
        private Axis _axis = Axis.Y;
        [SerializeField]
        [HideInInspector]
        private bool _symmetry = false;
        [SerializeField]
        [HideInInspector]
        private UVWrapMode _uvWrapMode = UVWrapMode.Clamp;
        [SerializeField]
        [HideInInspector]
        private int _slices = 1;

        protected override void Awake()
        {
            base.Awake();
            mesh.name = "waveform";
        }

        protected override void BuildMesh()
        {
            base.BuildMesh();
            Generate();
        }

        protected override void Build()
        {

            base.Build();
        }

        protected override void LateRun()
        {
            base.LateRun();
        }

        void Generate()
        {
            if (_symmetry) GenerateSymmetrical();
            else GenerateDefault();
            if (doubleSided) MeshUtility.MakeDoublesided(tsMesh);
            if (calculateTangents) MeshUtility.CalculateTangents(tsMesh);
        }

        private void GenerateDefault()
        {
            int vertexCount = clippedSamples.Length * (_slices + 1);
            if (tsMesh.vertexCount != vertexCount)
            {
                tsMesh.vertices = new Vector3[vertexCount];
                tsMesh.normals = new Vector3[vertexCount];
                tsMesh.uv = new Vector2[vertexCount];
                tsMesh.colors = new Color[vertexCount];
            }
            int vertIndex = 0;
            float avgTop = 0f;
            float avgBottom = 0f;
            float totalLength = 0f;
            for (int i = 0; i < clippedSamples.Length; i++)
            {
                Vector3 top = clippedSamples[i].position;
                Vector3 bottom = top;
                Vector3 normal = Vector3.right;
                float heightPercent = 1f;
                if (_uvWrapMode == UVWrapMode.UniformX || _uvWrapMode == UVWrapMode.Uniform)
                {
                    if (i > 0) totalLength += Vector3.Distance(clippedSamples[i].position, clippedSamples[i - 1].position);
                }
                switch (_axis)
                {
                    case Axis.X: avgBottom = bottom.x = computer.position.x; heightPercent = uvScale.y * Mathf.Abs(top.x - bottom.x); avgTop += top.x; break;
                    case Axis.Y: avgBottom = bottom.y = computer.position.y; heightPercent = uvScale.y * Mathf.Abs(top.y - bottom.y); normal = Vector3.up; avgTop += top.y; break;
                    case Axis.Z: avgBottom = bottom.z = computer.position.z; heightPercent = uvScale.y * Mathf.Abs(top.z - bottom.z); normal = Vector3.forward; avgTop += top.z; break;
                }
                Vector3 right = Vector3.Cross(normal, clippedSamples[i].direction).normalized;
                Vector3 offsetRight = Vector3.Cross(clippedSamples[i].normal, clippedSamples[i].direction);
                for (int n = 0; n < _slices + 1; n++)
                {
                    float slicePercent = ((float)n / _slices);
                    tsMesh.vertices[vertIndex] = Vector3.Lerp(bottom, top, slicePercent) + normal * offset.y + offsetRight * offset.x;
                    tsMesh.normals[vertIndex] = right;
                    switch (_uvWrapMode)
                    {
                        case UVWrapMode.Clamp: tsMesh.uv[vertIndex] = new Vector2((float)clippedSamples[i].percent * uvScale.x + uvOffset.x, slicePercent * uvScale.y + uvOffset.y); break;
                        case UVWrapMode.UniformX: tsMesh.uv[vertIndex] = new Vector2(totalLength * uvScale.x + uvOffset.x, slicePercent * uvScale.y + uvOffset.y); break;
                        case UVWrapMode.UniformY: tsMesh.uv[vertIndex] = new Vector2((float)clippedSamples[i].percent * uvScale.x + uvOffset.x, heightPercent * slicePercent * uvScale.y + uvOffset.y); break;
                        case UVWrapMode.Uniform: tsMesh.uv[vertIndex] = new Vector2(totalLength * uvScale.x + uvOffset.x, heightPercent * slicePercent * uvScale.y + uvOffset.y); break;
                    }
                    tsMesh.colors[vertIndex] = clippedSamples[i].color * color;
                    vertIndex++;
                }
            }
            if (clippedSamples.Length > 0) avgTop /= clippedSamples.Length;
            tsMesh.triangles = MeshUtility.GeneratePlaneTriangles(_slices, clippedSamples.Length, avgTop < avgBottom);
        }

        private void GenerateSymmetrical()
        {
            tsMesh.vertices = new Vector3[clippedSamples.Length * _slices * 2];
            tsMesh.normals = new Vector3[tsMesh.vertices.Length];
            tsMesh.uv = new Vector2[tsMesh.vertices.Length];
            tsMesh.colors = new Color[tsMesh.vertices.Length];
            int vertIndex = 0;
            float avgTop = 0f;
            float avgBottom = 0f;
            for (int i = 0; i < clippedSamples.Length; i++)
            {
                Vector3 top = clippedSamples[i].position;
                Vector3 bottom = top;
                Vector3 normal = Vector3.right;
                float heightPercent = 1f;
                switch (_axis)
                {
                    case Axis.X: bottom.x = computer.position.x + (computer.position.x - top.x); heightPercent = uvScale.y * Mathf.Abs(top.x - bottom.x); avgTop += top.x; avgBottom = computer.position.x; break;
                    case Axis.Y: bottom.y = computer.position.y + (computer.position.y - top.y); heightPercent = uvScale.y * Mathf.Abs(top.y - bottom.y); normal = Vector3.up; avgTop += top.y; avgBottom = computer.position.y; break;
                    case Axis.Z: bottom.z = computer.position.z + (computer.position.z - top.z); heightPercent = uvScale.y * Mathf.Abs(top.z - bottom.z); normal = Vector3.forward; avgTop += top.z; avgBottom = computer.position.z; break;
                } 
                Vector3 right = Vector3.Cross(normal, clippedSamples[i].direction).normalized;
                Vector3 offsetRight = Vector3.Cross(clippedSamples[i].normal, clippedSamples[i].direction);
                for (int n = 0; n < _slices * 2; n++)
                {
                    float slicePercent = ((float)n / _slices);
                    tsMesh.vertices[vertIndex] = Vector3.Lerp(bottom, top, slicePercent) + normal * offset.y + offsetRight * offset.x;
                    tsMesh.normals[vertIndex] = right;
                    if (_uvWrapMode == UVWrapMode.Clamp) tsMesh.uv[vertIndex] = new Vector2((float)clippedSamples[i].percent * uvScale.x + uvOffset.x, slicePercent * uvScale.y + uvOffset.y);
                    else tsMesh.uv[vertIndex] = new Vector2((float)clippedSamples[i].percent * uvScale.x + uvOffset.x, (0.5f - 0.5f * heightPercent + heightPercent * slicePercent) * uvScale.y + uvOffset.y);
                    
                    tsMesh.colors[vertIndex] = clippedSamples[i].color * color;
                    vertIndex++;
                }
            }
            if (clippedSamples.Length > 0) avgTop /= clippedSamples.Length;
            tsMesh.triangles = MeshUtility.GeneratePlaneTriangles(_slices * 2 - 1, clippedSamples.Length, avgTop * 2f < avgBottom);
        }

    }
}