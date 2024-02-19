using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexInitializer : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToAddVertex;

    private static int lastId = 1;

    public int Id { get; private set; }

    private void Start()
    {
        if (_objectsToAddVertex != null && _objectsToAddVertex.Length > 0)
        {
            foreach (GameObject obj in _objectsToAddVertex)
            {
                if (obj != null)
                {
                    Vertex vertexComponent = obj.AddComponent<Vertex>();
                    vertexComponent.Id = lastId++;
                }
            }
        }
        else
        {
            Debug.LogWarning("objectsToAddVertex is either null or empty.");
        }
    }
}
