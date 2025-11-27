using UnityEngine;
using UnityEngine.InputSystem;
using Main.Character;
using Main;
using Unity.Cinemachine;

namespace Main.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;

        [SerializeField]
        private float stopDistance = 0.1f;

        [SerializeField]
        private PlayerCharacter character;
        public PlayerCharacter Character => character;

        private Camera mainCamera;

        private void OnValidate()
        {
            playerInput ??= GetComponent<PlayerInput>();
        }

        private void Start()
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                mainCamera = FindFirstObjectByType<Camera>();
            }
        }

        private void OnEnable()
        {
            playerInput.actions.FindAction("Dash").performed += _ => character.Dash();
        }

        private void OnDisable()
        {
            playerInput.actions.FindAction("Dash").performed -= _ => character.Dash();
        }

        private void Update()
        {
            bool isGameStarted = IsGameStarted();
            
            if (!isGameStarted)
            {
                // Clear movement input when paused or game not started
                character?.UpdateMoveInput(Vector2.zero);
                if (character?.Rb2d != null)
                {
                    character.Rb2d.linearVelocity = Vector2.zero;
                }
                return;
            }

            MoveToMouse();
        }

        private bool IsGameStarted()
        {
            if (GameManager.Instance == null) return false;
            return GameManager.Instance.IsGameStarted && Time.timeScale > 0;
        }

        private void MoveToMouse()
        {
            if (Mouse.current == null) return;
            
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                if (mainCamera == null)
                {
                    mainCamera = FindFirstObjectByType<Camera>();
                    if (mainCamera == null) return;
                }
            }
            
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            
            // For orthographic cameras, use 0 as z-depth
            float zDepth = mainCamera.orthographic ? 0f : Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, zDepth));
            Vector2 targetPos = new Vector2(worldPosition.x, worldPosition.y);

            float distance = Vector2.Distance(targetPos, transform.position);

            if (distance > stopDistance)
            {
                Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
                character.UpdateMoveInput(direction);
            }
            else
            {
                character.UpdateMoveInput(Vector2.zero);
                if (character.Rb2d != null)
                {
                    character.Rb2d.linearVelocity = Vector2.zero;
                }
            }
        }
    }
}