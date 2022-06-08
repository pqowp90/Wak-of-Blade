using UnityEngine;
using System.Collections.Generic;

public class SimpleSpinBlur : MonoBehaviour
{
    private Vector3 rotDelta_Prev = Vector3.one;
    Mesh SSB_Mesh;
    [Range(1, 128)][Tooltip("Motion Blur Amount")]
    public int shutterSpeed = 4;

    [Range(1, 50)][Tooltip("Motion Blur Samples")]
    public int Samples = 8;
    Queue<Vector3> rotationQueue = new Queue<Vector3>();
    public Material SSB_Material;
    [Range(-0.1f, 0.1f)][Tooltip("Motion Blur Opacity")]
    public float alphaOffset;
    public AdvancedSettings advancedSettings;

    void Start()
    {
        SSB_Mesh = GetComponent<MeshFilter>().mesh;
        SSB_Material.enableInstancing = advancedSettings.enableGPUInstancing;
    }
    private void Update()
    {
        if (rotationQueue.Count >= shutterSpeed)
        {
            rotationQueue.Dequeue();
            //Second Dequeue to reduce queue size
            if (rotationQueue.Count >= shutterSpeed)
            {
                rotationQueue.Dequeue();
            }
        }
        rotationQueue.Enqueue(transform.position);
        
            if (advancedSettings.unitLocalScale)
            {
                for (int i = 0; i <= Samples; i++)
                {
                    Graphics.DrawMesh(SSB_Mesh, Vector3.Lerp(rotationQueue.Peek(), transform.position, (float)i / (float)Samples), transform.rotation, SSB_Material, 0, null, advancedSettings.subMaterialIndex);
                }
            }

            else
            {
                for (int i = 0; i <= Samples; i++)
                {
                    Matrix4x4 matrix = Matrix4x4.TRS(Vector3.Lerp(rotationQueue.Peek(), transform.position, (float)i / (float)Samples), transform.rotation, transform.localScale);
                    Graphics.DrawMesh(SSB_Mesh, matrix, SSB_Material, 0, null, advancedSettings.subMaterialIndex);
                }

            }

            Color tempColor;
            tempColor = new Color(SSB_Material.color.r, SSB_Material.color.g, SSB_Material.color.b, Mathf.Abs((2 / (float)Samples) + alphaOffset));
            SSB_Material.color = tempColor;
        
        
    }
}
[System.Serializable]
public class AdvancedSettings
{
    [Tooltip("[Optimization] Enables material's GPU Instancing property")]
    public bool enableGPUInstancing;
    [Tooltip("Index for objects with multiple materials")]
    public int subMaterialIndex = 0;
    [Tooltip("[Optimization] Check this box if the scale of your model is globalScale (1,1,1)")]
    public bool unitLocalScale = false;
}