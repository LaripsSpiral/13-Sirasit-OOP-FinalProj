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
            playerInput.actions.FindAction("Move").performed += ctx => character.MoveDir = ctx.ReadValue<Vector2>();
            playerInput.actions.FindAction("Move").canceled += _ => character.MoveDir = Vector2.zero;
        }

        private void OnDisable()
        {
            playerInput.actions.FindAction("Move").performed -= ctx => character.MoveDir = ctx.ReadValue<Vector2>();
            playerInput.actions.FindAction("Move").canceled -= _ => character.MoveDir = Vector2.zero;
        }
    }
}