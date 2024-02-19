using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InitialCharacteristicLine : MonoBehaviour
{
    public Material Material { get; private set; }
    public float Width { get; private set; }
    public Color Color { get; private set; }

    private static InitialCharacteristicLine _initialLine;

    private InitialCharacteristicLine(Material material, float width, Color color)
    {
        Material = material;
        Width = width;
        Color = color;
    }

    public static InitialCharacteristicLine NonDirectioanlEdge
    {
        get
        {
            if (_initialLine == null)
            {
                Material defaultMaterial = new Material(Shader.Find("Unlit/Color"));
                _initialLine = new InitialCharacteristicLine(defaultMaterial, 0.05f, Color.black);
            }
            return _initialLine;
        }
    }

    public static InitialCharacteristicLine DirectioanlEdge
    {
        get
        {
            if (_initialLine == null)
            {
                Material defaultMaterial = new Material(Shader.Find("Unlit/Color"));
                _initialLine = new InitialCharacteristicLine(defaultMaterial, 0.05f, Color.gray);
            }
            return _initialLine;
        }
    }
}
