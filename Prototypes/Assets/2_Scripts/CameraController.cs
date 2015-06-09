﻿namespace TheVandals
{
	using UnityEngine;
	using System.Collections;

	public struct TargetDestination
	{
		public Vector3 position;
		public Vector3 eulerAngles;

		public TargetDestination(Vector3 position,Vector3 eulerAngles)
		{
			this.position = position;
			this.eulerAngles = eulerAngles;
		}
	}

	public class CameraController : MonoBehaviour {

		#region Properties
		[Header("Configuration")]
		[SerializeField]
		[Tooltip("Camera limits in width and length")]
		int LevelArea = 50;
		[SerializeField]
		[Tooltip("Move camera if mouse pointer reaches screen borders")]
		int ScrollArea = 25;
		[SerializeField]
		int ScrollSpeed = 25;
		[SerializeField]
		int DragSpeed = 100;
		
		[SerializeField]
		int ZoomSpeed = 25;
		[SerializeField]
		int ZoomMin = 25;
		[SerializeField]
		int ZoomMax = 50;
		
		[SerializeField]
		int PanSpeed = 50;
		[SerializeField]
		int PanAngleMin = 50;
		[SerializeField]
		int PanAngleMax = 80;

		[SerializeField]
		int RotSpeed = 25;

		[Header("Function")]
		[SerializeField]
		bool draggable = true;
		[SerializeField]
		bool mouseBorders = true;
		[SerializeField]
		bool playCinematique = false;
		[SerializeField]
		Transform cinematiqueTarget;
		[SerializeField]
		bool NewCameraRotation = false;

		private TargetDestination td = new TargetDestination(Vector3.zero,Vector3.zero);
		private bool isplayingCinematique = false;

		private float smooth = 1.5f;
		private Vector3 rotationTarget = Vector3.zero;
		#endregion

		#region Events
		#endregion
		
		#region Editor API
		void OnGUI()
		{

		}
		#endregion
		
		#region API
		#endregion

		#region Unity

		void Start()
		{
			td.position = transform.position;
			td.eulerAngles.x = Mathf.Clamp(transform.eulerAngles.x, PanAngleMin, PanAngleMax);
			transform.eulerAngles = td.eulerAngles;
		}

		void Update()
		{
			if(!isplayingCinematique)
			{
				if(playCinematique && cinematiqueTarget )
				{
					isplayingCinematique = true;
					StopCoroutine("PlayCinematique");
					StartCoroutine("PlayCinematique", cinematiqueTarget);
				}
				else
					UpdateCamera();
			}
		}
		#endregion

		#region Handlers
		#endregion
		
		#region Private

		void UpdateCamera()
		{
			Vector3 translation = Vector3.zero;
			
			// Move camera with arrow keys
			translation += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			
			// Move camera with mouse
			if (Input.GetKey(KeyCode.Mouse1) && draggable) 
			{
				// Hold button and drag camera around
				translation -= new Vector3(Input.GetAxis("Mouse X") * DragSpeed * Time.deltaTime, 0,
				                           Input.GetAxis("Mouse Y") * DragSpeed * Time.deltaTime);
			}
			else if (mouseBorders)
			{
				// Move camera if mouse pointer reaches screen borders
				if (Input.mousePosition.x < ScrollArea)
				{
					translation += Vector3.right * -ScrollSpeed * Time.deltaTime;
				}
				
				if (Input.mousePosition.x >= Screen.width - ScrollArea)
				{
					translation += Vector3.right * ScrollSpeed * Time.deltaTime;
				}
				
				if (Input.mousePosition.y < ScrollArea)
				{
					translation += Vector3.forward * -ScrollSpeed * Time.deltaTime;
				}
				
				if (Input.mousePosition.y > Screen.height - ScrollArea)
				{
					translation += Vector3.forward * ScrollSpeed * Time.deltaTime;
				}
			}

			translation = Camera.main.transform.TransformDirection(translation);
			translation.y = 0;

			if(Input.GetAxis("Mouse ScrollWheel")!=0)
			{
				float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime;
				
				if (zoomDelta!=0)
				{
					translation -= Vector3.up * ZoomSpeed * zoomDelta;
				}
				
				// Start panning camera if zooming in close to the ground or if just zooming out.
				float pan = transform.eulerAngles.x - zoomDelta * PanSpeed;
				pan = Mathf.Clamp(pan, PanAngleMin, PanAngleMax);
				
				if (zoomDelta < 0 || td.position.y < (ZoomMin+((ZoomMax-ZoomMin)/2)))
				{
					//camera.transform.eulerAngles = new Vector3(pan, 0, 0);
					//td.eulerAngles.x = (int)pan;
				}
				
			}
			//CAMERA MOVEMENT CONTROLS
//			float MoveVertical = Input.GetAxis("Vertical")  * Time.deltaTime;
//			float MoveHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
//			CameraHolder.transform.Translate (Vector3.forward * MoveVertical);
//			CameraHolder.transform.Translate (Vector3.right * MoveHorizontal);
//			float Rotation = Input.GetAxis("Mouse X");
//			if (Input.GetKey(KeyCode.Mouse2))
//			{
//				CameraHolder.transform.Rotate(Vector3.up, Rotation * CameraRotateSpeed * Time.deltaTime,Space.World);
//			}
			//Camera Rotation on Y axis
			if(Input.GetKey(KeyCode.Mouse2))
			{
				if(NewCameraRotation)
				{

					Ray ray = new Ray(transform.position,transform.forward);
					// create a plane at 0,0,0 whose normal points to +Y:
					Plane hPlane = new Plane(Vector3.up, Vector3.zero);
					// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
					float distance = 0; 
					// if the ray hits the plane...
					if (hPlane.Raycast(ray, out distance)){
						// get the hit point:
						rotationTarget = ray.GetPoint(distance);
					}

//					Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
//					RaycastHit hit;
					
//					(Physics.Raycast(mouseRay, out hit,100,LayerMask.NameToLayer("Floor")))
//					if(Physics.Raycast(transform.position,transform.forward, out hit))
//					{
//					transform.RotateAround (new Vector3(0,transform.localEulerAngles.y,0),Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * 20);
//					Vector3 _direction = (Vector3.zero - transform.position).normalized;
//					Quaternion lookRot = Quaternion.LookRotation(_direction);
//					perform rotation-over-time on rot variable
//					td.eulerAngles = lookRot.eulerAngles;
//					transform.rotation = lookRot;
//
//					transform.Translate(Vector3.right * Time.deltaTime);
//						rotationTarget = hit.point;
//					}
					translation += transform.right * Input.GetAxis("Mouse X") ;
				}
				else
				{
					td.eulerAngles.y += Input.GetAxis("Mouse X") * RotSpeed;
				}

			}
			
			// Keep camera within level and zoom area
			Vector3 desiredPosition = td.position + translation;
			if (desiredPosition.x < -LevelArea || LevelArea < desiredPosition.x)
			{
				translation.x = 0;
			}
			if (desiredPosition.y < ZoomMin || ZoomMax < desiredPosition.y)
			{
				translation.y = 0;
			}
			if (desiredPosition.z < -LevelArea || LevelArea < desiredPosition.z)
			{
				translation.z = 0;
			}

			td.position += translation;


			transform.position = Vector3.Slerp (transform.position, td.position, smooth);
			transform.rotation = Quaternion.Lerp (Quaternion.Euler(transform.eulerAngles), Quaternion.Euler(td.eulerAngles), Time.deltaTime * 15);
			if(Input.GetKey(KeyCode.Mouse2) || Input.GetAxis("Mouse ScrollWheel")!=0)
			{
				transform.LookAt(rotationTarget);
				td.eulerAngles = transform.localEulerAngles;
//				transform.rotation = Quaternion.Lerp (Quaternion.Euler(transform.eulerAngles), Quaternion.Euler(td.eulerAngles), Time.deltaTime * 15);
			}
		}

		IEnumerator PlayCinematique(Transform target)
		{
			//TargetDestination initial = new TargetDestination(transform.position,transform.eulerAngles);

			float distance = Vector3.Distance(transform.position, target.position);
			float maxdistance = distance;
			while(distance> 2 * maxdistance /10)
			{
				SmoothLookAt(target);
				transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * distance / maxdistance);
				distance = Vector3.Distance(transform.position, target.position);
				yield return null;
			}
			
			print ("WaitForSeconds");
			yield return new WaitForSeconds(3f);
			
			print ("finished");
			playCinematique = false;
			isplayingCinematique = false;
		}

		void SmoothLookAt (Transform target)
		{
			// Create a vector from the camera towards the player.
			Vector3 relPlayerPosition = target.position - transform.position;
			
			// Create a rotation based on the relative position of the player being the forward vector.
			Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
			
			// Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
			transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
		}
		#endregion
	}
}