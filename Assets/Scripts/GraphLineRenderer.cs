using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;

public class GraphLineRenderer : MonoBehaviour
{
    [SerializeField] private List<VertexPair> _vertexPairs = new List<VertexPair>();
    [SerializeField] private Transform _controlPointOne;
    [SerializeField] private Transform _controlPointTwo;
    [SerializeField] private int _resolution = 50;

    public delegate void ActionTrigger();
    public static event ActionTrigger OnActionTriggered;

    private IGraph _graphComponent;

    private void Start()
    {
        _graphComponent = GetComponent<IGraph>();

        DrawLines();
    }

    public static void TriggerAction()
    {
        OnActionTriggered?.Invoke();
    }

    private void DrawLines()
    {
        foreach (var pair in _vertexPairs)
        {

            if (pair.SetIsEdgeDirected())
            {
                DrawCurveBezier(pair);
            }
            else
            {
                DrawStraightLine(pair);
            }
        }

        TriggerAction();
    }

    private void DrawStraightLine(VertexPair pair)
    {
        GameObject lineObject = new GameObject("StraightLine");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.material = InitialCharacteristicLine.NonDirectioanlEdge.Material;

        lineRenderer.startWidth = InitialCharacteristicLine.NonDirectioanlEdge.Width;
        lineRenderer.endWidth = InitialCharacteristicLine.NonDirectioanlEdge.Width;

        lineRenderer.material.color = InitialCharacteristicLine.NonDirectioanlEdge.Color;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, pair.SetStartVertex().position);
        lineRenderer.SetPosition(1, pair.SetEndVertex().position);

        FillingLists(pair, lineRenderer);
    }

    private void DrawCurveBezier(VertexPair pair)
    {
        GameObject lineObject = new GameObject("CurveBezier");

        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.material = InitialCharacteristicLine.DirectioanlEdge.Material;

        lineRenderer.startWidth = InitialCharacteristicLine.DirectioanlEdge.Width;
        lineRenderer.endWidth = InitialCharacteristicLine.DirectioanlEdge.Width;

        lineRenderer.material.color = InitialCharacteristicLine.DirectioanlEdge.Color;

        Vector3[] curvePoints = GenerateCurvePoints(pair);
        lineRenderer.positionCount = curvePoints.Length;
        lineRenderer.SetPositions(curvePoints);

        FillingLists(pair, lineRenderer);
    }

    private Vector3 CalculatePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * p0;
        point += 3 * uu * t * p1;
        point += 3 * u * tt * p2;
        point += ttt * p3;

        return point;
    }

    private Vector3[] GenerateCurvePoints(VertexPair pair)
    {
        Vector3[] points = new Vector3[_resolution];
        for (int i = 0; i < _resolution; i++)
        {
            float t = i / (float)(_resolution - 1);
            points[i] = CalculatePoint(t, pair.SetStartVertex().position, _controlPointOne.position, _controlPointTwo.position, pair.SetEndVertex().position);
        }
        return points;
    }

    private void FillingLists(VertexPair pair, LineRenderer lineRenderer)
    {
        Vertex startVertexComponent = pair.SetStartVertex().GetComponent<Vertex>();
        Vertex endVertexComponent = pair.SetEndVertex().GetComponent<Vertex>();

        if (_graphComponent == null)
        {
            Debug.LogError("Graph component is not found.");
            return;
        }

        if (startVertexComponent == null || endVertexComponent == null)
        {
            Debug.LogError("One of the vertices is missing a Vertex component.");
            return;
        }

        if (!pair.SetIsEdgeDirected())
        {
            _graphComponent.AddEdge(startVertexComponent, endVertexComponent, lineRenderer);
            _graphComponent.AddEdge(endVertexComponent, startVertexComponent, lineRenderer);
        }
        else
        {
            _graphComponent.AddEdge(startVertexComponent, endVertexComponent, lineRenderer);
        }

        _graphComponent.AddVertex(pair);
    }
}
