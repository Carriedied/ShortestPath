using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal interface IGraph
    {
        void AddVertex(VertexPair pair);
        void AddEdge(Vertex startVertex, Vertex endVertex, LineRenderer lineRenderer);
    }
}
