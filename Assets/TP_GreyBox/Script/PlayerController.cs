using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public enum MovementType
	{
		Manual,
		FreeFollowView
	}
	private MovementType _movementType = MovementType.Manual;
	public float speed = 10.0f;

	Rigidbody _rigidbody = null;
	protected bool IsActive { get; private set; }

	[Header("Views")]
	[SerializeField] private FreeFollowView _freeFollowView;


	public void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			NextMovementType();
		}
	}
	
	public void Jump()
	{
		if (_rigidbody != null)
		{
			_rigidbody.AddForce(Vector3.up * 10, ForceMode.Impulse);
		}
	}

	void FixedUpdate()
    {
		switch (_movementType)
		{
			case MovementType.Manual:
				ManualMovement();
				break;
			case MovementType.FreeFollowView:
				FreeFollowViewMovement();
				break;
		}
		
	}

    private void FreeFollowViewMovement()
    {
		
    }

    private void ManualMovement()
    {
        //reading the input:
		float horizontalAxis = Input.GetAxis("Horizontal");
		float verticalAxis = Input.GetAxis("Vertical");
		 
		//assuming we only using the single camera:
		var camera = Camera.main;

		//camera forward and right vectors:
		var forward = camera.transform.forward;
		var right = camera.transform.right;

		//project forward and right vectors on the horizontal plane (y = 0)
		forward.y = 0f;
		right.y = 0f;
		forward.Normalize();
		right.Normalize();

		//this is the direction in the world space we want to move:
		var desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;

		//now we can apply the movement using the rigidbody:
		if (_rigidbody != null)
		{
			_rigidbody.MovePosition(_rigidbody.position + desiredMoveDirection * speed * Time.deltaTime);
		}
    }

    public void ChangeMovementType(MovementType movementType)
	{
		_movementType = movementType;
		switch (movementType)
		{
			case MovementType.Manual:
				_freeFollowView.IsActive = false;
				break;
			case MovementType.FreeFollowView:
				_freeFollowView.IsActive = true;
				break;
		}
	}

	private void NextMovementType()
	{
		_movementType = (MovementType)(((int)_movementType + 1) % System.Enum.GetValues(typeof(MovementType)).Length);
		ChangeMovementType(_movementType);
	}
}
