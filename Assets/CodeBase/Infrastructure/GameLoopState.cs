﻿namespace Assets.CodeBase.Infrastructure
{
    public class GameLoopState : IState
    {
        private GameStateMachine _stateMachine;
        public GameLoopState(GameStateMachine stateMachine) 
        { 
            _stateMachine = stateMachine;
        }
        public void Enter()
        {

        }

        public void Exit()
        {

        }
    }
}