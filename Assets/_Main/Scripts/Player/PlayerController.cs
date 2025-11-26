using UnityEngine;
using UnityEngine.InputSystem;
using Main.Character;

namespace Main.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;

        [SerializeField]
        private PlayerCharacter character;
        public PlayerCharacter Character => character;

        private void OnValidate()
        {
            playerInput ??= GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            playerInput.actions.FindAction("Move").performed += ctx => character.UpdateMoveInput(ctx.ReadValue<Vector2>());
            playerInput.actions.FindAction("Move").canceled += _ => character.UpdateMoveInput(Vector2.zero);
            playerInput.actions.FindAction("Dash").performed += _ => character.Dash();
        }

        private void OnDisable()
        {
            playerInput.actions.FindAction("Move").performed -= ctx => character.UpdateMoveInput(ctx.ReadValue<Vector2>());
            playerInput.actions.FindAction("Move").canceled -= _ => character.UpdateMoveInput(Vector2.zero);
            playerInput.actions.FindAction("Dash").performed -= _ => character.Dash();
        }
    }
}