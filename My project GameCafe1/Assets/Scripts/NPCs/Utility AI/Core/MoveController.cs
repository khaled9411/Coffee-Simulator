using UnityEngine;
using UnityEngine.AI;

namespace TL.Core
{
    public class MoveController : MonoBehaviour
    {
        private NavMeshAgent agent;
        private NPCAnimationEvents animationEvents;

        private Transform _destination;
        public Transform destination
        {
            get { return _destination; }
            set
            {
                _destination = value;
                Move();
            }
        }

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animationEvents = GetComponent<NPCAnimationEvents>();
        }

        public void Move()
        {
            agent.enabled = true;
            agent.destination = destination.position;

            animationEvents?.TriggerStartWalking();
        }

        void Update()
        {
            if (agent.enabled && !agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    agent.enabled = false;
                    animationEvents?.TriggerStopWalking();
                }
            }
        }
    }
}