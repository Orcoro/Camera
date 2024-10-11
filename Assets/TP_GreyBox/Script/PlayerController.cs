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
	

	Rigidbody _rigidbody = null;
	protected bool IsActive { get; private set; }
	
	public enum CONTROLS
    {
        WORLD,
        CAMERA,
    }
    public CONTROLS controls = CONTROLS.CAMERA;
	public float speed = 10.0f;

	[Header("Views")]
	[SerializeField] private FreeFollowView _freeFollowView;


	public void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		Cursor.visible = false; 
		Cursor.lockState = CursorLockMode.Locked;
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
        Vector3 direction = Vector3.zero;
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

        switch (controls)
        {
            case CONTROLS.CAMERA:
                forward = Camera.main.transform.forward;
                forward.y = 0;
                forward.Normalize();

                right = Camera.main.transform.right;
                right.y = 0;
                right.Normalize();

                break;
        }
        direction += Input.GetAxisRaw("Horizontal") * right;
        direction += Input.GetAxisRaw("Vertical") * forward;
        direction.Normalize();
        _rigidbody.velocity = direction * speed + Vector3.up * _rigidbody.velocity.y;

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
