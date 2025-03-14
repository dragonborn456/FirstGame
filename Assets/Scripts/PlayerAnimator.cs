using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	// CONSTANTS
	private const string IS_WALKING = "IsWalking";

	// VARIABLES
	[SerializeField] private Player player;
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	private void Update()
    {
		animator.SetBool(IS_WALKING, player.IsWalking());
	}
}
