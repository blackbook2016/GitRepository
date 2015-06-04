namespace TheVandals
{
	using UnityEngine;
	using System.Collections;

	public class CameraController : MonoBehaviour {

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

		void Update()
		{
			Vector3 translation = Vector3.zero;


			float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime;

			if (zoomDelta!=0)
			{
				translation -= Vector3.up * ZoomSpeed * zoomDelta;
			}
			
			// Start panning camera if zooming in close to the ground or if just zooming out.
			float pan = GetComponent<Camera>().transform.eulerAngles.x - zoomDelta * PanSpeed;
			pan = Mathf.Clamp(pan, PanAngleMin, PanAngleMax);
			//print (pan + " " + (ZoomMin+((ZoomMax-ZoomMin)/2)) + " " + zoomDelta);
			if (zoomDelta < 0 || GetComponent<Camera>().transform.position.y < (ZoomMin+((ZoomMax-ZoomMin)/2)))
			{
				GetComponent<Camera>().transform.eulerAngles = new Vector3(pan, 0, 0);
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
			else
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
			Vector3 desiredPosition = GetComponent<Camera>().transform.position + translation;
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

			transform.position += translation;
		}
	}
}