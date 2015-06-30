namespace TheVandals
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public enum WaypointType{
		None,
		ClosedLoop,
		PingPong,
		RandomPath,
	}

	public class WaypointManager : MonoBehaviour 
	{
		#region Properties
		public WaypointType wpType = WaypointType.None;
		public List<Waypoint> points;
		
		private Waypoint nextPoint;
		private Waypoint StartPoint = null;
		private Waypoint EndPoint = null;

		public bool direction = true;
		private int currentPosition;
		private WaypointType guizmosType = WaypointType.None;
		#endregion
		#region Editor API
		void OnDrawGizmosSelected()
		{
			if(points.Count == transform.childCount && EndPoint != null && guizmosType == wpType)
			{
				Gizmos.color = Color.blue;
				Waypoint currentpoint = StartPoint;
					switch (wpType)
					{
					case WaypointType.None:
					{
						while(currentpoint != EndPoint)
						{
							nextPoint = points[points.IndexOf(currentpoint) + 1];
							Gizmos.DrawLine(currentpoint.transform.position, nextPoint.transform.position);
							currentpoint = nextPoint;
						}
						break;
					}
					case WaypointType.ClosedLoop:
					{			

						while(currentpoint != EndPoint)
						{
							nextPoint = points[points.IndexOf(currentpoint) + 1];
							Gizmos.DrawLine(currentpoint.transform.position, nextPoint.transform.position);
							currentpoint = nextPoint;
						}
					Gizmos.DrawLine(StartPoint.transform.position, EndPoint.transform.position);
						break;
					}
					case WaypointType.PingPong:
					{
						while(currentpoint != EndPoint)
						{
							nextPoint = points[points.IndexOf(currentpoint) + 1];
							Gizmos.DrawLine(currentpoint.transform.position, nextPoint.transform.position);
							currentpoint = nextPoint;
						}
						break;
					}
					case WaypointType.RandomPath:
					{
					for(int i = points.IndexOf(StartPoint); i < points.IndexOf(EndPoint); i++)
					{
						for(int y = i+1; y <= points.IndexOf(EndPoint); y++)
							Gizmos.DrawLine(points[i].transform.position, points[y].transform.position);
					}
						break;
					}
				}
			}
			else
			Refresh();
		}
		#endregion
		#region Unity
		void Awake() 
		{
			Refresh();
		}
		#endregion

		#region API
		public Waypoint NextPoint
		{
			get
			{
				return nextPoint;
			}
		}
		
		public void SetNextPoint()
		{
			if(EndPoint == null || StartPoint == null)
			{
				print ("Path endpoint or startpoint aren't defined");
			}
			currentPosition = points.IndexOf(nextPoint);

			switch (wpType)
			{
			case WaypointType.None:
			{
				if(currentPosition == points.IndexOf(EndPoint))
					nextPoint = EndPoint;
				else 
					nextPoint = points[points.IndexOf(nextPoint) + 1];
				break;
			}
			case WaypointType.ClosedLoop:
			{			
				if (currentPosition + 1 > points.Count - 1)
				{
					nextPoint = points[0];
				}				
				else 
					nextPoint = points[points.IndexOf(nextPoint) + 1];
				break;
			}
			case WaypointType.PingPong:
			{
				if(currentPosition == points.IndexOf(EndPoint) || (currentPosition == 0 && !direction))
				{
					direction = !direction;
				}
				if(direction)
					nextPoint = points[currentPosition+1];
				else
					nextPoint = points[currentPosition-1];
				print(currentPosition);
				break;
			}
			case WaypointType.RandomPath:
			{
				nextPoint = points[Random.Range(0, points.IndexOf(EndPoint) + 1)];
				break;
			}
			}

		}
		#endregion		

		#region Private
		private void Refresh()
		{
			points.Clear();

			Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
			
			foreach(Waypoint waypoint in waypoints)
			{
				points.Add(waypoint);
				if(waypoint.pointType == PointType.Start)
					StartPoint = waypoint;
				else if(waypoint.pointType == PointType.End)
					EndPoint = waypoint;
			}			

			nextPoint = StartPoint;
			guizmosType = wpType;
		}
		#endregion		
	}
}
