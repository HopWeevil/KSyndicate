using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using System;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private HeroAnimator _heroAnimator;
        private State _state;

        public Action HealthChanged;

        public float CurrentHP
        {
            get => _state.CurrentHP;
            set
            {
                if(_state.CurrentHP != value)
                {
                    _state.CurrentHP = value;
                    HealthChanged?.Invoke();
                }              
            }
        }
        public float MaxHp
        {
            get => _state.MaxHP;
            set => _state.MaxHP = value;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHP = CurrentHP;
            progress.HeroState.MaxHP = MaxHp;
        }

        public void TakeDamage(float damage)
        {
            if(CurrentHP <= 0)
            {
                return;
            }
            CurrentHP -= damage;
            _heroAnimator.PlayHit();
        }
    }
}
