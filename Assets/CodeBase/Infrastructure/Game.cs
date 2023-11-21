using Assets.CodeBase.Infrastructure;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public static IInputService InputService;
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner));
        }

        
    }
}