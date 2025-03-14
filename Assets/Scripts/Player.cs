using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // VARIABLES
    [SerializeField] private float moveSpeed = 7f;

    private bool isWalking;


    // Update is called once per frame
    private void Update()
    {
        Vector2 inputVector = new Vector2(0, 0);
        
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
            inputVector.y = +1;
		};
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            inputVector.x = -1;
        };
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            inputVector.y = -1;
        };
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            inputVector.x = +1;
        };

        //Normalize every value regardless of using different key combination
        inputVector = inputVector.normalized;

        // Move the object
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // Return player walking status
        isWalking = moveDir != Vector3.zero;

        // Rotate the object based on the move direction
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
	{
        return isWalking;

    }
}
