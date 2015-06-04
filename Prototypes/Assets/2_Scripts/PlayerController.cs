﻿namespace TheVandals
{
	using UnityEngine;
	using System.Collections;

	public enum State {
		Idle,
		Walk,
		Run,
	}

	[ExecuteInEditMode]
	public class PlayerController : MonoBehaviour {
		
		#region Properties
		[Header("Configuration")]
		[SerializeField]
		float walkSpeed = 3;
		[SerializeField]
		float runSpeed = 15;
		[Tooltip("Double Click Delays")]
		float delay = 0.25F;
		[SerializeField]
		State state = State.Idle;

		private float lastClickTime = 0F;
		private NavMeshAgent agent;
		#endregion

		#region Unity
		void Start () 
		{
			agent = GetComponent<NavMeshAgent>();
		}	

		void Update () 
		{
			if(state != State.Idle && agent.remainingDistance == 0)
				state = State.Idle;

			if(Input.GetKeyDown(KeyCode.Mouse0))	
				MovePlayer();

			if(Input.GetKeyDown(KeyCode.Mouse1))
			{
				agent.SetDestination(transform.position);
				state = State.Idle;
			}
		}
		#endregion

		#region Private

		void MovePlayer()
		{
			//Vector3 destination ?= RetrieveMousePosition()
			agent.SetDestination(RetrieveMousePosition());
			//agent.Resume();

			if(state != State.Run && Time.time - lastClickTime < delay)
			{
				//agent.velocity = Vector3.forward * runSpeed;
				state = State.Run;
				agent.speed = runSpeed;
			}
			else if(state != State.Walk && Time.time - lastClickTime >= delay)
			{
				//agent.velocity = Vector3.forward * walkSpeed;
				state = State.Walk;
				agent.speed = walkSpeed;
			}
			lastClickTime = Time.time;
		}
		
		Vector3 RetrieveMousePosition()
		{
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(mouseRay, out hit))
				return hit.point;
			
			return transform.position;
		}
		#endregion

		#region Editor API
//		void OnGUI()
//		{
//			int speed = (int)agent.speed;
//			#if UNITY_EDITOR
//			GUI.Label(new Rect(25, 5, 100, 100),"PlayerSpeed: " + speed.ToString());
//			agent.speed = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), agent.speed, 0.0F, 10.0F);
//
//			#endif
//		}
		#endregion 
	}
}