using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using System;
using System.Linq;
using UnityEngine;

namespace CodeBase.Enemy
{

    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private float _attackCooldown = 3f;
        [SerializeField] private float _cleavage = 0.5f;
        [SerializeField] private float _effectiveDistance = 0.5f;
        [SerializeField] private float _damage = 10f;

        private IGameFactory _gameFactory;
        private Transform _heroTransform;

        private float _currentCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];
        private bool _attackIsActive;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _gameFactory.HeroCreated += OnHeroCreated;
        }

        private void Update()
        {
            UpdateCooldown();
            if (CanAttack())
            {
                StartAttack();
            }

        }

        private void OnAttack()
        {
            if(Hit(out Collider collider))
            {
                PhysicsDebug.DrawDebug(CalculateStartPoint(), _cleavage, 1f);
                collider.transform.GetComponent<IHealth>().TakeDamage(_damage);
            }
        }

       
        private void OnAttackEnded()
        {
            _currentCooldown = _attackCooldown;
            _isAttacking = false;
        }
        public void EnableAttack()
        {
            _attackIsActive = true;
        }

        public void DisableAttack()
        {
            _attackIsActive = false;
        }


        private bool CanAttack()
        {
            return _attackIsActive && !_isAttacking && CooldownIsUp();
        }

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
            {
                _currentCooldown -= Time.deltaTime;
            }
        }
        private bool CooldownIsUp()
        {
            return _currentCooldown <= 0;
        }

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();
            _isAttacking = true;
        }

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(CalculateStartPoint(), _cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitCount > 0;
        }

        private Vector3 CalculateStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * _effectiveDistance;
        }

        private void OnHeroCreated()
        {
            _heroTransform = _gameFactory.HeroGameObject.transform;
        }
    }
}