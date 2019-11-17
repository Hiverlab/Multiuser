// Alan Zucconi
// www.alanzucconi.com
using UnityEngine;
using System.Collections;

public class Heatmap : MonoBehaviour
{
    public Vector4[] positions;
    public Vector4[] properties;

    public Material material;

    public int count = 50;

    public static Heatmap instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    public void Initialize(int _count)
    {
        count = _count;

        positions = new Vector4[count];
        properties = new Vector4[count];

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector4(Random.Range(-0.4f, +0.4f), Random.Range(-0.4f, +0.4f), 0, 0);
            properties[i] = new Vector4(Random.Range(0f, 0.25f), Random.Range(-0.25f, 1f), 0, 0);
        }
    }

    public void UpdatePosition(int index, Vector3 position)
    {
        positions[index] = position;
    }

    public void UpdateProperties(int index, float radius, float intensity)
    {
        properties[index] = new Vector4(radius, intensity, 0, 0);
    }

    void Start()
    {
        /*
        positions = new Vector4[count];
        properties = new Vector4[count];

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector4(Random.Range(-0.4f, +0.4f), Random.Range(-0.4f, +0.4f), 0, 0);
            properties[i] = new Vector4(Random.Range(0f, 0.25f), Random.Range(-0.25f, 1f), 0, 0);
        }
        */

        Initialize(count);
    }

    void Update()
    {
        material.SetInt("_Points_Length", count);
        material.SetVectorArray("_Points", positions);
        material.SetVectorArray("_Properties", properties);
    }
}
