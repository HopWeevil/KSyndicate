using CodeBase.Infrastructure;

namespace Assets.CodeBase.Infrastructure
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName);
        }

        public void Exit()
        {

        }
    }
}