namespace TheVandals
{
	using UnityEngine;
	using System.Collections;

	[ExecuteInEditMode]
	public class CapsuleController : MonoBehaviour {
		
		#region Properties

		public enum State {
			Idle,
			Walk,
			Run,
		}
		
		[Header("Configuration")]
		[SerializeField]
		float walkSpeed = 3;
		[SerializeField]
		float runSpeed = 5;
		[Tooltip("Double Click DelayS")]
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
			if(agent.velocity == Vector3.zero)
				state = State.Idle;

			if(Input.GetButtonDown("Fire2"))	
				MovePlayer();

			if(Input.GetButtonDown("Fire1"))
				agent.Stop ();
		}
		#endregion

		#region Private

		void MovePlayer()
		{	
			//Vector3 destination ?= RetrieveMousePosition()
			agent.SetDestination(RetrieveMousePosition());

			if(state != State.Run && Time.time - lastClickTime < delay)
			{
				state = State.Run;
				agent.speed = runSpeed;
			}
			else if(state != State.Walk && Time.time - lastClickTime >= delay)
			{
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
		void OnGUI()
		{
			int speed = (int)agent.speed;
			#if UNITY_EDITOR
			GUI.Label(new Rect(25, 5, 100, 100),"PlayerSpeed: " + speed.ToString());
			agent.speed = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), agent.speed, 0.0F, 10.0F);

			#endif
		}
		#endregion 
	}
}
