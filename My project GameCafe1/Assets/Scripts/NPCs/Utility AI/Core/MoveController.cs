using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TL.Core
{
    public class MoveController : MonoBehaviour
    {
        private NavMeshAgent agent;

        private Transform _destination;
        public Transform destination { 
            get { return _destination; } 
            set {
                _destination = value;
                Move();
            } 
        }


        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }


        public void Move()
        {
            agent.destination = destination.position;
        }

        
    }
}