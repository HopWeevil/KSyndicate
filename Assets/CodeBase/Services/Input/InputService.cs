using UnityEngine;

namespace CodeBase.Services.Input
{
    public abstract class InputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        private const string Button = "Fire";
        public abstract Vector2 Axis { get; }
        public bool IsAttackButtonUp() => SimpleInput.GetButtonUp(Button);

        protected private Vector2 SimpleInputAxis()
        {
            return new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
        }
            
        protected private Vector2 UnityAxis()
        {
            return new Vector2(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));
        }
    }
}