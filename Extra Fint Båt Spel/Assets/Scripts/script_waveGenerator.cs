using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class script_waveGenerator : MonoBehaviour
{
    //private MeshFilter m_meshFilter;

    //private void Start()
    //{
    //    m_meshFilter = GetComponent<MeshFilter>();
    //}

    //private void Update()
    //{
    //    Vector3[] verts = m_meshFilter.mesh.vertices;

    //    for(int i = 0; i < verts.Length; i++)
    //    {
    //        verts[i].y = script_waveAlgorithm.instance.GetWaveHeight(transform.position.x + verts[i].x);
    //    }

    //    m_meshFilter.mesh.vertices = verts;
    //    m_meshFilter.mesh.RecalculateNormals();
    //}

    [SerializeField] private int size = 50;
    private int scaleMultiplier = 10;
    [SerializeField] private float UVScale;
    public Wave[] m_wave;

    private MeshFilter m_meshFilter;
    private Mesh m_mesh;

    [Serializable]
    public struct Wave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }

    private void Start()
    {
        m_mesh = new Mesh();
        m_mesh.name = gameObject.name;

        m_mesh.vertices = GenerateVertex();
        m_mesh.triangles = GenerateTriangles();
        m_mesh.uv = GenerateUV();
        m_mesh.RecalculateBounds();
        m_mesh.RecalculateNormals();

        m_meshFilter = gameObject.GetComponent<MeshFilter>();
        m_meshFilter.mesh = m_mesh;

        transform.position = new Vector3((-(float)size * scaleMultiplier) / 2, transform.position.y, (-(float)size * scaleMultiplier) / 2);
        transform.localScale *= scaleMultiplier;
    }

    private void Update()
    {
        Vector3[] verts = m_mesh.vertices;

        for (int x = 0; x <= size; x++)
        {
            for (int z = 0; z <= size; z++)
            {
                float y = 0f;

                for (int i = 0; i < m_wave.Length; i++)
                {
                    if (m_wave[i].alternate)
                    {
                        float perlin = Mathf.PerlinNoise((x * m_wave[i].scale.x) / size, (z * m_wave[i].scale.y) / size * Mathf.PI * 2f);
                        y += Mathf.Sin(perlin + m_wave[i].speed.magnitude * Time.timeSinceLevelLoad) * m_wave[i].height;
                    }
                    else
                    {
                        float perlin = Mathf.PerlinNoise(
                            (x * m_wave[i].scale.x + Time.timeSinceLevelLoad * m_wave[i].speed.x) / size,
                            (z * m_wave[i].scale.y + Time.timeSinceLevelLoad * m_wave[i].speed.y) / size * Mathf.PI * 2f
                            ) - 0.5f;
                        // -0.5f to push the wave down so it goes from -0.5 to 0.5 instead of 0 t 1 on y

                        y += perlin * m_wave[i].height;
                    }
                }

                verts[GetIndex(x, z)] = new Vector3(x, y, z);
            }
        }

        m_mesh.vertices = verts;
        m_mesh.RecalculateNormals();
    }

    private Vector3[] GenerateVertex()
    {
        Vector3[] verts = new Vector3[(size + 1) * (size + 1)];

        for (int i = 0; i <= size; i++)
        {
            for (int j = 0; j <= size; j++)
            {
                verts[GetIndex(i, j)] = new Vector3(i, 0f, j);
            }
        }

        return verts;
    }

    private int[] GenerateTriangles()
    {
        int[] triangles = new int[m_mesh.vertices.Length * 6]; // 1 square, 2 triangles, 6 points

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // points of first triangle
                triangles[GetIndex(i, j) * 6] = GetIndex(i, j);
                triangles[GetIndex(i, j) * 6 + 1] = GetIndex(i + 1, j + 1);
                triangles[GetIndex(i, j) * 6 + 2] = GetIndex(i + 1, j);

                // points of second triangle
                triangles[GetIndex(i, j) * 6 + 3] = GetIndex(i, j);
                triangles[GetIndex(i, j) * 6 + 4] = GetIndex(i, j + 1);
                triangles[GetIndex(i, j) * 6 + 5] = GetIndex(i + 1, j + 1);
            }
        }

        return triangles;
    }

    private Vector2[] GenerateUV()
    {
        Vector2[] uvs = new Vector2[m_mesh.vertices.Length];

        for (int x = 0; x <= size; x++)
        {
            for (int z = 0; z <= size; z++)
            {
                var vector = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[GetIndex(x, z)] = new Vector2(vector.x <= 1 ? vector.x : 2 - vector.x, vector.y <= 1 ? vector.y : 2 - vector.y);
            }
        }

        return uvs;
    }

    // get the (x, y) position from a one dimensional array
    // (0, 0) = 0    (1, 0) = 61    (2, 0) = 121    (2, 1) = 122
    private int GetIndex(int x, int y)
    {
        return x * (size + 1) + y;
    }

    public float GetHeight(Vector3 position)
    {
        // scale factor and position in local space
        Vector3 scale = new Vector3(1f / transform.lossyScale.x, 0f, 1f / transform.lossyScale.z);
        Vector3 localPosition = Vector3.Scale((position - transform.position), scale);

        // get edge points
        Vector3 p1 = new Vector3(Mathf.Floor(localPosition.x), 0f, Mathf.Floor(localPosition.z));
        Vector3 p2 = new Vector3(Mathf.Floor(localPosition.x), 0f, Mathf.Ceil(localPosition.z));
        Vector3 p3 = new Vector3(Mathf.Ceil(localPosition.x), 0f, Mathf.Floor(localPosition.z));
        Vector3 p4 = new Vector3(Mathf.Ceil(localPosition.x), 0f, Mathf.Ceil(localPosition.z));

        // clamp if the position is outside of the plane
        p1.x = Mathf.Clamp(p1.x, 0f, size);
        p1.z = Mathf.Clamp(p1.z, 0f, size);
        p2.x = Mathf.Clamp(p2.x, 0f, size);
        p2.z = Mathf.Clamp(p2.z, 0f, size);
        p3.x = Mathf.Clamp(p3.x, 0f, size);
        p3.z = Mathf.Clamp(p3.z, 0f, size);
        p4.x = Mathf.Clamp(p4.x, 0f, size);
        p4.z = Mathf.Clamp(p4.z, 0f, size);

        // get the max distance to one of the edges and take that to compute max - dist
        float max = Mathf.Max(
            Vector3.Distance(p1, localPosition),
            Vector3.Distance(p2, localPosition),
            Vector3.Distance(p3, localPosition),
            Vector3.Distance(p4, localPosition) + Mathf.Epsilon);

        float distance = 
            (max - Vector3.Distance(p1, localPosition)) + 
            (max - Vector3.Distance(p2, localPosition)) + 
            (max - Vector3.Distance(p3, localPosition)) + 
            (max - Vector3.Distance(p4, localPosition)) + 
            Mathf.Epsilon;

        // get the weighted sum
        float height = 
            m_mesh.vertices[GetIndex((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localPosition)) + 
            m_mesh.vertices[GetIndex((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localPosition)) + 
            m_mesh.vertices[GetIndex((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localPosition)) + 
            m_mesh.vertices[GetIndex((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localPosition));

        return height * transform.lossyScale.y / distance;
    }
}
