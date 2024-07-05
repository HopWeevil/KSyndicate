using System;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToPlayer : Follow
    {
        private const float MinimalDistance = 1;
    
        public NavMeshAgent Agent;
    
        private Transform _heroTransform;
        public void Construct(Transform heroTransform)
        {
            _heroTransform = heroTransform;
        }

        private void Update()
        {
            if (IsInitialized() && IsHeroNotReached())
            {
                Agent.destination = _heroTransform.position;
            }
        }

        private bool IsInitialized()
        {
            return _heroTransform != null;
        }
     
        private bool IsHeroNotReached()
        {
            return Agent.transform.position.SqrMagnitudeTo(_heroTransform.position) >= MinimalDistance;
        }    
    }
}