using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
	[Header("Player Movement")]
	public float moveForce = 30f;
	public float maxSpeed = 12f;
	public float drag = 5f;
	public float rotationSpeed = 10f;

	private Rigidbody2D rb;
	private Camera mainCam;
	private PlayerInputHandler input;
	private PlayerWeapon weapon;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		input = GetComponent<PlayerInputHandler>();
		weapon = GetComponent<PlayerWeapon>();
		mainCam = Camera.main;
		rb.gravityScale = 0f;
		rb.linearDamping = drag;
	}

	void FixedUpdate()
	{
		Move();
		Rotate();

		if (input.FirePressed)
		{
			weapon.TryShoot();
		}
	}

	private void Move()
	{
		if (input.MoveInput.sqrMagnitude > 0.01f)
			rb.AddForce(input.MoveInput.normalized * moveForce, ForceMode2D.Force);

		if (rb.linearVelocity.magnitude > maxSpeed)
			rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
	}

	private void Rotate()
	{
		Vector3 mousePos = Mouse.current.position.ReadValue();
		Vector3 worldMousePos = mainCam.ScreenToWorldPoint(mousePos);
		Vector2 direction = worldMousePos - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

		float smoothAngle = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
		rb.MoveRotation(smoothAngle);
	}
}
