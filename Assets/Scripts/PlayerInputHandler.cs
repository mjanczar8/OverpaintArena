using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
	public Vector2 MoveInput { get; private set; }
	public bool FirePressed { get; private set; }

	private PlayerInput playerInput;

	void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
	}

	private void OnEnable()
	{
		playerInput.actions["Move"].performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
		playerInput.actions["Move"].canceled += ctx => MoveInput = Vector2.zero;
		playerInput.actions["Fire"].performed += ctx => FirePressed = true;
		playerInput.actions["Fire"].canceled += ctx => FirePressed = false;
	}

	private void OnDisable()
	{
		playerInput.actions["Move"].performed -= ctx => MoveInput = ctx.ReadValue<Vector2>();
		playerInput.actions["Move"].canceled -= ctx => MoveInput = Vector2.zero;
		playerInput.actions["Fire"].performed -= ctx => FirePressed = true;
		playerInput.actions["Fire"].canceled -= ctx => FirePressed = false;
	}
}
