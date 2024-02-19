using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Graph : MonoBehaviour, IGraph
{
    [SerializeField, Range(1, 6)] private int _startId;
    [SerializeField, Range(1, 6)] private int _endId;

    private List<Vertex> _vertexShortestPath = new List<Vertex>();
    private List<Vertex> _vertices = new List<Vertex>();
    private List<Edge> _edges = new List<Edge>();

    private void OnEnable()
    {
        GraphLineRenderer.OnActionTriggered += ChangingColorShortestPathActionTriggered;
    }

    private void OnDisable()
    {
        GraphLineRenderer.OnActionTriggered -= ChangingColorShortestPathActionTriggered;
    }

    public void AddVertex(VertexPair pair)
    {
        if (!_vertices.Contains(pair.SetStartVertex().GetComponent<Vertex>()))
        {
            _vertices.Add(pair.SetStartVertex().GetComponent<Vertex>());
        }

        if (!_vertices.Contains(pair.SetEndVertex().GetComponent<Vertex>()))
        {
            _vertices.Add(pair.SetEndVertex().GetComponent<Vertex>());
        }
    }

    public void AddEdge(Vertex start, Vertex end, LineRenderer lineRenderer)
    {
        Edge edge = new Edge(start, end, CalculateLineLength(lineRenderer), lineRenderer);
        _edges.Add(edge);
    }

    private List<Vertex> FindShortestPath(int startId, int endId)
    {
        var distances = new Dictionary<Vertex, float>();
        var previous = new Dictionary<Vertex, Vertex>();
        var nodes = new List<Vertex>(_vertices);

        foreach (var v in _vertices)
        {
            distances[v] = float.MaxValue;
            previous[v] = null;
        }

        distances[_vertices.Find(v => v.Id == startId)] = 0;

        while (nodes.Count != 0)
        {
            nodes.Sort((x, y) => distances[x].CompareTo(distances[y]));
            var smallest = nodes[0];
            nodes.Remove(smallest);

            if (smallest.Id == endId)
            {
                var path = new List<Vertex>();
                while (previous[smallest] != null)
                {
                    path.Add(smallest);
                    smallest = previous[smallest];
                }
                path.Add(_vertices.Find(v => v.Id == startId));
                path.Reverse();
                return path;
            }

            if (distances[smallest] == float.MaxValue)
            {
                break;
            }

            foreach (var neighbor in _edges.FindAll(e => e.Start == smallest))
            {
                var alt = distances[smallest] + neighbor.Weight;
                if (alt < distances[neighbor.End])
                {
                    distances[neighbor.End] = alt;
                    previous[neighbor.End] = smallest;
                }
            }
        }

        Debug.Log("ѕуть не найден");

        return new List<Vertex>();
    }

    private void ChangingColorShortestPathActionTriggered()
    {
        LineRenderer lineRenderer;

        _vertexShortestPath = FindShortestPath(_startId, _endId);

        for(int i = 0; i < _vertexShortestPath.Count; i++)
        {
            if (i + 1 < _vertexShortestPath.Count)
            {
                Edge foundEdge = _edges.Find(edge => edge.Start == _vertexShortestPath[i] && edge.End == _vertexShortestPath[i + 1]);
                lineRenderer = foundEdge.LineRenderer;
                lineRenderer.material.color = Color.blue;
            }
        }
    }

    private float CalculateLineLength(LineRenderer lineRenderer)
    {
        float length = 0f;

        if (lineRenderer.positionCount < 2) return length; 

        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            Vector3 startPosition = lineRenderer.GetPosition(i);
            Vector3 endPosition = lineRenderer.GetPosition(i + 1);

            length += Vector3.Distance(startPosition, endPosition);
        }

        return length;
    }
}
