using CodeBase.Infrastructure;

namespace Assets.CodeBase.Infrastructure
{
    public class LoadLevelState : IState
    {
        private GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.Load("Main");
        }

        public void Exit()
        {

        }
    }
}