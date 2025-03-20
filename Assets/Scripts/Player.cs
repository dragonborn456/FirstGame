using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }



    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
	{
        public ClearCounter selectedCounter;
	}

    // VARIABLES
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMasks;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;

	private void Awake()
	{
		if(Instance != null)
		{
            Debug.LogError("More than one Player instance have been created!");
		}
        Instance = this;
	}
	private void Start()
	{
		gameInput.OnInteractAction += GameInput_OnInteractAction;
	}

	private void GameInput_OnInteractAction(object sender, System.EventArgs e)
	{
        if(selectedCounter != null)
		{
            selectedCounter.Interact();
		}
    }

	// Update is called once per frame
	private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
	{
        return isWalking;

    }

    private void HandleInteractions()
	{
        float interactDistance = 2f;

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Save latest move direction when stopped.
        if (moveDir != Vector3.zero) lastInteractDir = moveDir;

        // Check object interaction
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMasks))
		{
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearcounter))
            {
                // Clear counter available
                if(clearcounter != selectedCounter)
				{
                    SetSelectedCounter(clearcounter);
				}
			}
			else
			{
                SetSelectedCounter(null);
			}
		}
		else
		{
            SetSelectedCounter(null);
        }
	}

    private void HandleMovement()
	{
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Move the object
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Detect collision
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        // Identify object to move correct direction when colliding object and moving to different direction. (TL;DR: Hug wall)
        if (!canMove)
        {
            //Cannot move toward moveDir

            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }

            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        // Return player walking status
        isWalking = moveDir != Vector3.zero;

        // Rotate the object based on the move direction
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
	{
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
}
