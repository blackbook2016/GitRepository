namespace TheVandals
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public enum WaypointType{
		None,
		Loop,
		ClosedLoop,
		PingPong,
	}

	public class WaypointManager : MonoBehaviour 
	{
		#region Properties
		public WaypointType wpType;
		public List<Waypoint> points;
		
		private Waypoint targetPoint;
		private Waypoint StartPoint;
		private Waypoint EndPoint;

		private bool direction = true;

		#endregion
		#region Editor API
		void OnDrawGizmosSelected()
		{
			if(points.Count == transform.childCount)
			{
				Gizmos.color = Color.blue;
				Waypoint prevPoint = points[points.Count -1];
				foreach(Waypoint point in points)
				{
					Gizmos.DrawLine(point.transform.position, prevPoint.transform.position);
					prevPoint = point;
				}
			}
			else
			Refresh();
		}
		#endregion
		#region Unity
		void Start() 
		{
			Refresh();
		}
		#endregion

		#region API
		public Waypoint TargetPoint
		{
			get
			{
				return targetPoint;
			}
		}
		
		public void SetNextPoint()
		{
			if(wpType != WaypointType.PingPong )
			{
				if (points.IndexOf(targetPoint) == points.Count - 1)
				{
					targetPoint = points[0];
				}
				// if next point is the endpoint
				if(EndPoint == points[points.IndexOf(targetPoint) + 1])
				{
					if (wpType == WaypointType.None)
						targetPoint = EndPoint;
					else if (wpType == WaypointType.ClosedLoop || wpType == WaypointType.Loop)
						targetPoint = StartPoint;
				}
			}
			else
			{
				if(direction)
				{				
					if(EndPoint == points[points.IndexOf(targetPoint) + 1])
					{
						direction = !direction;
						if(points.IndexOf(targetPoint) - 1 == -1)
							targetPoint = points[points.Count - 1];
					}
				}else
				{

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
			
			targetPoint = StartPoint;
		}
		#endregion		
	}
}
