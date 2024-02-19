using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class Edge
    {
        public Edge(Vertex start, Vertex end, float weight, LineRenderer lineRenderer)
        {
            Start = start;
            End = end;
            Weight = weight;
            LineRenderer = lineRenderer;
        }

        public Vertex Start { get; private set; }
        public Vertex End { get; private set; }
        public float Weight { get; private set; }
        public LineRenderer LineRenderer { get; private set; }
    }
}
