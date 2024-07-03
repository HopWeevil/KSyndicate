using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private GameObject _deathFx;
        [SerializeField] private float _destroyAfterDeathTime;

       public event Action Happened;

        private void Start()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if (_health.Current <= 0)
            {
                Die();
            }          
        }

        private void Die()
        {
            _health.HealthChanged -= OnHealthChanged;
      
            _animator.PlayDeath();
            SpawnDeathFx();

            StartCoroutine(DestroyTimer());
      
            Happened?.Invoke();
        }

        private void SpawnDeathFx()
        {
            Instantiate(_deathFx, transform.position, Quaternion.identity);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}