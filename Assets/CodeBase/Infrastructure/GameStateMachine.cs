using CodeBase.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Infrastructure
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _activeState;

        public GameStateMachine(SceneLoader sceneLoader)
        {
            _states = new Dictionary<Type, IState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader),
            };


        }

        public void Enter<TSate>() where TSate : IState
        {
            _activeState?.Exit();
            IState state = _states[typeof(TSate)];
            _activeState = state;
            state.Enter();
        }       
    }
}