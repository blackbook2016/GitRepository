namespace TheVandals
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

		[Header("Function")]
		[SerializeField]
		bool draggable = true;
		[SerializeField]
		bool mouseBorders = true;

		private TargetDestination target = new TargetDestination(Vector3.zero,Vector3.zero);
		#endregion

		#region Events
		#endregion
		
		#region Editor API
		#endregion
		
		#region API
		#endregion

		#region Unity

		void Start()
		{
			target.position = transform.position;
			target.eulerAngles.x = 45;
		}

		void Update()
		{
			UpdateCamera();
		}
		#endregion

		#region Handlers
		#endregion
		
		#region Private
		void UpdateCamera()
		{
			Vector3 translation = Vector3.zero;
			
			

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
				//print (pan + " " + (ZoomMin+((ZoomMax-ZoomMin)/2)) + " " + zoomDelta);
				if (zoomDelta < 0 || target.position.y < (ZoomMin+((ZoomMax-ZoomMin)/2)))
				{
					//camera.transform.eulerAngles = new Vector3(pan, 0, 0);
					target.eulerAngles.x = (int)pan;
				}

			}
			
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
			
			// Keep camera within level and zoom area
			Vector3 desiredPosition = target.position + translation;
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
			Vector3 velocity = Vector3.zero;
			target.position += translation;
				//transform.position = Vector3.SmoothDamp(transform.position, transform.position + translation, ref velocity, 0.3F);
			transform.eulerAngles = Vector3.Lerp (transform.eulerAngles, target.eulerAngles, Time.deltaTime * 15);
			transform.position = Vector3.Lerp (transform.position, target.position, Time.deltaTime * 1.5F);
		}
		#endregion
	}
}