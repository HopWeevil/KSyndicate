using CodeBase.Logic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Enemy
{

    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _animator;
        public float AttackCooldown = 3f;
        public float Cleavage = 0.5f;
        public float EffectiveDistance = 0.5f;
        public float Damage = 10f;

        private Transform _heroTransform;

        public float _currentCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];
        private bool _attackIsActive;

        public void Construct(Transform heroTransfrom)
        {
            _heroTransform = heroTransfrom;
        }

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Start()
        {
            _animator.StateExited += OnStateExited;
        }

        private void OnStateExited(AnimatorState state)
        {
            if(state == AnimatorState.Attack)
            {
                OnAttackEnded();
            }
        }

        private void OnDestroy()
        {
            _animator.StateExited -= OnStateExited;
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
                PhysicsDebug.DrawDebug(CalculateStartPoint(), Cleavage, 1f);
                collider.transform.GetComponent<IHealth>().TakeDamage(Damage);
            }
        }
      
        private void OnAttackEnded()
        {

            ResetAttack();
        }

      

        public void EnableAttack()
        {
            _attackIsActive = true;
        }

        public void DisableAttack()
        {
            _attackIsActive = false;
            ResetAttack();
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
            _isAttacking = true;
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();
        }

        private void ResetAttack()
        {
            _isAttacking = false;
            _currentCooldown = AttackCooldown;
        }

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(CalculateStartPoint(), Cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitCount > 0;
        }

        private Vector3 CalculateStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;
        }
    }
}