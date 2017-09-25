using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dreamteck
{
    //Thread-safe mesh & bounds classes for working with threads.
    public class TS_Mesh
    {
        public int vertexCount
        {
            get { return vertices.Length; }
            set { }
        }
        public Vector3[] vertices = new Vector3[0];
        public Vector3[] normals = new Vector3[0];
        public Vector4[] tangents = new Vector4[0];
        public Color[] colors = new Color[0];
        public Vector2[] uv = new Vector2[0];
        public int[] triangles = new int[0];
        public List<int[]> subMeshes = new List<int[]>();
        public TS_Bounds bounds = new TS_Bounds(Vector3.zero, Vector3.zero);

        public volatile bool hasUpdate = false;

        public TS_Mesh()
        {

        }

        public TS_Mesh(Mesh mesh)
        {
            CreateFromMesh(mesh);
        }

        public void Clear()
        {
            vertices = new Vector3[0];
            normals = new Vector3[0];
            tangents = new Vector4[0];
            colors = new Color[0];
            uv = new Vector2[0];
            triangles = new int[0];
            subMeshes = new List<int[]>();
            bounds = new TS_Bounds(Vector3.zero, Vector3.zero);
        }

        public void CreateFromMesh(Mesh mesh)
        {
            vertices = mesh.vertices;
            normals = mesh.normals;
            tangents = mesh.tangents;
            colors = mesh.colors;
            uv = mesh.uv;
            triangles = mesh.triangles;
            bounds = new TS_Bounds(mesh.bounds);
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                subMeshes.Add(mesh.GetTriangles(i));
            }
        }

        public void Combine(List<TS_Mesh> newMeshes)
        {
            int newVerts = 0;
            int newTris = 0;
            List<int> newSubs = new List<int>();
            for(int i = 0; i < newMeshes.Count; i++)
            {
                newVerts += newMeshes[i].vertexCount;
                newTris += newMeshes[i].triangles.Length;
                for(int n = 0; n < newMeshes[i].subMeshes.Count; n++)
                {
                    if (n >= newSubs.Count) newSubs.Add(newMeshes[i].subMeshes[n].Length);
                    else newSubs[n] += newMeshes[i].subMeshes[n].Length;
                }
            }
            Vector3[] newVertices = new Vector3[vertices.Length + newVerts];
            Vector3[] newNormals = new Vector3[vertices.Length + newVerts];
            Vector2[] newUvs = new Vector2[vertices.Length + newVerts];
            Color[] newColors = new Color[vertices.Length + newVerts];
            Vector4[] newTangents = new Vector4[tangents.Length + newVerts];
            int[] newTriangles = new int[triangles.Length + newTris];
            List<int[]> newSubmeshes = new List<int[]>();
            for(int i = 0; i < newSubs.Count; i++)
            {
                newSubmeshes.Add(new int[newSubs[i]]);
                if (i < subMeshes.Count) newSubs[i] = subMeshes[i].Length;
                else newSubs[i] = 0;
            }
            newVerts = vertexCount;
            newTris = triangles.Length;
            vertices.CopyTo(newVertices, 0);
            normals.CopyTo(newNormals, 0);
            uv.CopyTo(newUvs, 0);
            colors.CopyTo(newColors, 0);
            tangents.CopyTo(newTangents, 0);
            triangles.CopyTo(newTriangles, 0);

            for (int i = 0; i < newMeshes.Count; i++)
            {
                newMeshes[i].vertices.CopyTo(newVertices, newVerts);
                newMeshes[i].normals.CopyTo(newNormals, newVerts);
                newMeshes[i].uv.CopyTo(newUvs, newVerts);
                newMeshes[i].colors.CopyTo(newColors, newVerts);
                newMeshes[i].tangents.CopyTo(newTangents, newVerts);

                for (int n = newTris; n < newTris + newMeshes[i].triangles.Length; n++)
                {
                    newTriangles[n] = newMeshes[i].triangles[n - newTris] + newVerts;
                }


                for (int n = 0; n < newMeshes[i].subMeshes.Count; n++)
                {
                    for (int x = newSubs[n]; x < newSubs[n] + newMeshes[i].subMeshes[n].Length; x++)
                    {
                        newSubmeshes[n][x] = newMeshes[i].subMeshes[n][x - newSubs[n]] + newVerts;
                    }
                    newSubs[n] += newMeshes[i].subMeshes[n].Length;
                }
                newTris += newMeshes[i].triangles.Length;
                newVerts += newMeshes[i].vertexCount;
            }

            vertices = newVertices;
            normals = newNormals;
            uv = newUvs;
            colors = newColors;
            tangents = newTangents;
            triangles = newTriangles;
            subMeshes = newSubmeshes;
        }

        public void Combine(TS_Mesh newMesh)
        {
            Vector3[] newVertices = new Vector3[vertices.Length + newMesh.vertices.Length];
            Vector3[] newNormals = new Vector3[normals.Length + newMesh.normals.Length];
            Vector2[] newUvs = new Vector2[uv.Length + newMesh.uv.Length];
            Color[] newColors = new Color[colors.Length + newMesh.colors.Length];
            Vector4[] newTangents = new Vector4[tangents.Length + newMesh.tangents.Length];
            int[] newTriangles = new int[triangles.Length + newMesh.triangles.Length];

            vertices.CopyTo(newVertices, 0);
            newMesh.vertices.CopyTo(newVertices, vertices.Length);

            normals.CopyTo(newNormals, 0);
            newMesh.normals.CopyTo(newNormals, normals.Length);

            uv.CopyTo(newUvs, 0);
            newMesh.uv.CopyTo(newUvs, uv.Length);

            colors.CopyTo(newColors, 0);
            newMesh.colors.CopyTo(newColors, colors.Length);

            tangents.CopyTo(newTangents, 0);
            newMesh.tangents.CopyTo(newTangents, tangents.Length);

            for(int i = 0; i < newTriangles.Length; i++)
            {
                if (i < triangles.Length) newTriangles[i] = triangles[i];
                else  newTriangles[i] = (newMesh.triangles[i - triangles.Length] + vertices.Length);
            }

            for(int i = 0; i < newMesh.subMeshes.Count; i++)
            {
                if(i >= subMeshes.Count) subMeshes.Add(newMesh.subMeshes[i]);
                else
                {
                    int[] newTris = new int[subMeshes[i].Length + newMesh.subMeshes[i].Length];
                    subMeshes[i].CopyTo(newTris, 0);
                    for(int n = 0; n < newMesh.subMeshes[i].Length; n++)
                    {
                        newTris[subMeshes[i].Length + n] = newMesh.subMeshes[i][n] + vertices.Length;
                    }
                    subMeshes[i] = newTris;
                }
            }
            vertices = newVertices;
            normals = newNormals;
            uv = newUvs;
            colors = newColors;
            tangents = newTangents;
            triangles = newTriangles;
        }

        public static TS_Mesh Copy(TS_Mesh input)
        {
            TS_Mesh result = new TS_Mesh();
            result.vertices = new Vector3[input.vertices.Length];
            input.vertices.CopyTo(result.vertices, 0);
            result.normals = new Vector3[input.normals.Length];
            input.normals.CopyTo(result.normals, 0);
            result.uv = new Vector2[input.uv.Length];
            input.uv.CopyTo(result.uv, 0);
            result.colors = new Color[input.colors.Length];
            input.colors.CopyTo(result.colors, 0);
            result.tangents = new Vector4[input.tangents.Length];
            input.tangents.CopyTo(result.tangents, 0);
            result.triangles = new int[input.triangles.Length];
            input.triangles.CopyTo(result.triangles, 0);
            result.subMeshes = new List<int[]>();
            for(int i = 0; i < input.subMeshes.Count; i++)
            {
                result.subMeshes.Add(new int[input.subMeshes[i].Length]);
                input.subMeshes[i].CopyTo(result.subMeshes[i], 0);
            }
            result.bounds = new TS_Bounds(input.bounds.center, input.bounds.size);
            return result;
        }

        public void Absorb(TS_Mesh input)
        {
            if (vertices.Length != input.vertexCount) vertices = new Vector3[input.vertexCount];
            if (normals.Length != input.normals.Length) normals = new Vector3[input.normals.Length];
            if (colors.Length != input.colors.Length) colors = new Color[input.colors.Length];
            if (uv.Length != input.uv.Length) uv = new Vector2[input.uv.Length];
            if (tangents.Length != input.tangents.Length) tangents = new Vector4[input.tangents.Length];
            if (triangles.Length != input.triangles.Length) triangles = new int[input.triangles.Length];

            input.vertices.CopyTo(vertices, 0);
            input.normals.CopyTo(normals, 0);
            input.colors.CopyTo(colors, 0);
            input.uv.CopyTo(uv, 0);
            input.tangents.CopyTo(tangents, 0);
            input.triangles.CopyTo(triangles, 0);

            if (subMeshes.Count == input.subMeshes.Count)
            {
                for (int i = 0; i < subMeshes.Count; i++)
                {
                    if (input.subMeshes[i].Length != subMeshes[i].Length) subMeshes[i] = new int[input.subMeshes[i].Length];
                    input.subMeshes[i].CopyTo(subMeshes[i], 0);
                }
            }
            else
            {
                subMeshes = new List<int[]>();
                for (int i = 0; i < input.subMeshes.Count; i++)
                {
                    subMeshes.Add(new int[input.subMeshes[i].Length]);
                    input.subMeshes[i].CopyTo(subMeshes[i], 0);
                }
            }
            bounds = new TS_Bounds(input.bounds.center, input.bounds.size);
        }

        public void WriteMesh(ref Mesh input)
        {
            if (input == null) input = new Mesh();
            if (vertices == null || vertices.Length <= 65000)
            {
                input.Clear();
                input.vertices = vertices;
                input.normals = normals;
                input.tangents = tangents;
                input.colors = colors;
                input.uv = uv;
                input.triangles = triangles;
                if (subMeshes.Count > 0)
                {
                    input.subMeshCount = subMeshes.Count;
                    for (int i = 0; i < subMeshes.Count; i++) input.SetTriangles(subMeshes[i], i);
                }
                input.RecalculateBounds();
                hasUpdate = false;
            }
        }
    }
}
