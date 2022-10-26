using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{
    [SerializeField]
    Material material;

    [SerializeField]
    int width, height;

    [SerializeField]
    List<GameObject> visibleUnits = new List<GameObject>();

    public Vector2 center;
    public float radius;

    byte[] visionGrid;

    public FilterMode filter = FilterMode.Bilinear;

    Texture2D texture;

    // Start is called before the first frame update
    void Start()
    {
        visionGrid = new byte[width * height];
        
        CreateTexture();
    }

    // Update is called once per frame
    void Update()
    {
        ResetVisionGrid();
        UpdateVisionGrid();
        DrawVisionOnTextureWithRawData();

    }

    void CreateTexture()
    {
        texture = new Texture2D(width, height, TextureFormat.Alpha8, false);
        texture.filterMode = filter;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.name = "FOW grid (Generated)";
        material.SetTexture("_MainTex", texture);
    }//end CreateTexture

    private void DrawVisionOnTextureWithRawData()
    {
        float length = width * height;

        Unity.Collections.NativeArray<byte> data = texture.GetRawTextureData<byte>();

        for (int i = 0; i < length; i++)
        {
            data[i] = visionGrid[i];
        }

        texture.Apply();
    }//end DrawVisionOnTextureWithRawData

    private void ResetVisionGrid()
    {
        byte invisible = 255;
        for (int i = 0; i < visionGrid.Length; i++)
        {
            visionGrid[i] = invisible;
        }
    }

    private void UpdateVisionGrid()
    {
        byte visible = 100;
        byte invisible = 255;
        float centerX = width / 2;
        float centerY = height / 2;
        float ratioX = (width / GetComponent<Projector>().orthographicSize) / 2;
        float ratioY = (height / GetComponent<Projector>().orthographicSize) / 2;

        foreach (GameObject unit in visibleUnits)
        {
            float xOffset = unit.transform.position.x - transform.position.x;
            float yOffset = unit.transform.position.z - transform.position.z;
            xOffset *= ratioX;
            yOffset *= ratioY;
            float posX = centerX + xOffset;
            float posY = centerY + yOffset;

            for (float y = posY - radius; y <= (posY + radius); y++)
            {
                if (y < 0)
                    continue;
                if (y >= height)
                    break;
                for(float x = posX - radius; x <= (posX + radius); x++)
                {
                    if (x < 0)
                        continue;
                    if (x >= width)
                        break;
                    float distance = Vector2.Distance(new Vector2(posX, posY), new Vector2(x, y));
                    if (distance <= radius)
                    {
                        int index = (int)((int)y * width + x);
                        byte offset = (byte)(((distance * distance) / (radius * radius)) * (invisible - visible));
                        visionGrid[index] = (byte)(visible + offset);
                    }
                }
            }
        }

    }//end UpdateVisionGrid
}
