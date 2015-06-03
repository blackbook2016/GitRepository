using UnityEngine;
using System.Collections;

public class CapsuleController : MonoBehaviour {
	

	private NavMeshAgent agent;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent>();
	}	

	void Update () 
	{
		if(Input.GetButtonDown("Fire1"))			
			agent.SetDestination(RetrieveMousePosition());
	}

	Vector3 RetrieveMousePosition()
	{
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if(Physics.Raycast(mouseRay,out hit))
			return hit.point;
		
		return transform.position;
	}
}
