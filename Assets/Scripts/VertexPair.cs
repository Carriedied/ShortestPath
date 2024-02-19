using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class VertexPair : MonoBehaviour
    {
        [SerializeField] private Transform _startVertex;
        [SerializeField] private Transform _endVertex;
        [SerializeField] private bool _isEdgeDirected;

        public Transform SetStartVertex()
        {
            return _startVertex;
        }

        public Transform SetEndVertex()
        {
            return _endVertex;
        }

        public bool SetIsEdgeDirected()
        {
            return _isEdgeDirected;
        }
    }
}
