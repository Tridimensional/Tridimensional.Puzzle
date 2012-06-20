using UnityEngine;
using System.Collections;

public class DragFree : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	private Vector3 cruurentRay = Vector3.zero;
	
	
	//////////////////////////////////////////
	private Vector3 mouseDownVector3 = Vector3.zero;
	private Vector3 mouseDownObjectPosition = Vector3.zero;
	private Vector3 lastInductionPoint = Vector3.zero;
	private float oAngle = 0;
	private bool isMouseDown = false;
	
	// Update is called once per frame
	void Update () 
	{
		//when left mouse down
		if (Input.GetMouseButtonDown(0)) 
		{
			cruurentRay = Input.mousePosition;
			Ray rayForDray = Camera.mainCamera.ScreenPointToRay(cruurentRay);
			RaycastHit hit;
			if(Physics.Raycast(rayForDray,out hit,20f,1000))
			{
				isMouseDown = true;
				mouseDownVector3 = hit.point;
				mouseDownObjectPosition = transform.position;
				
				//caculate the angle from induction point to center point
				Vector3 oldPiecePointVect3 = (mouseDownObjectPosition - mouseDownVector3);
				oAngle = Mathf.Atan2(oldPiecePointVect3.y , oldPiecePointVect3.x);
			}
			else
			{
				//print(transform.lossyScale );
				//Debug.Log(Mathf.Sin(-Mathf.PI/2));
				//Debug.Log(Mathf.DeltaAngle(370,-10));
				Debug.Log("no hit");
				//Debug.Log(Mathf.Atan2(-1,1)/Mathf.PI * 180);
			}
			
		}
		
		// When left mouse up.
		if(Input.GetMouseButtonUp(0))
		{
			Debug.Log("up");
			mouseDownVector3 = Vector3.zero;
			mouseDownObjectPosition = Vector3.zero;
			lastInductionPoint = Vector3.zero;
			isMouseDown = false;
		}
		
		
		
		if(cruurentRay != Vector3.zero)
		{
			Ray ray = Camera.mainCamera.ScreenPointToRay(cruurentRay);
			Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
		}
		
		SetPiecePosition();
		//transform.Rotate(Vector3.forward * Time.deltaTime);
		//Debug.Log(Camera.mainCamera);
	}
	
	// caculate & set the position of piece
	private void SetPiecePosition()
	{
		if(isMouseDown)
		{
			// find the cast point of hidden plane
			cruurentRay = Input.mousePosition;
			Ray rayForDray = Camera.mainCamera.ScreenPointToRay(cruurentRay);
			RaycastHit hit;
			if(Physics.Raycast(rayForDray,out hit,20f,1<<10))
			{
				
				if(lastInductionPoint == Vector3.zero) 
				{
					lastInductionPoint = hit.point;
					return;
				}
				
				if(Vector3.Equals(lastInductionPoint,hit.point))
				{
					return;
				}
				
				Vector3 thisMoveVector = hit.point - lastInductionPoint;
				Vector3 objectMoseToCenterVector = mouseDownObjectPosition - mouseDownVector3;
				float thisMoveAngle = (Mathf.Atan2(thisMoveVector.y , thisMoveVector.x));
				float angelDistanceNum = Mathf.DeltaAngle(180 * thisMoveAngle / Mathf.PI, 180 * oAngle/ Mathf.PI);
				
				
				float rotAngle = 0;
				//
				if(objectMoseToCenterVector.magnitude > transform.lossyScale.x/5)
				{
					rotAngle = (Mathf.Sin(angelDistanceNum/180 * Mathf.PI)) * thisMoveVector.magnitude * 1000 * Time.deltaTime;
				}
				
				transform.Rotate(Vector3.forward * rotAngle);
				oAngle += rotAngle/180 * Mathf.PI;
				
				
				
				float newYPosi = hit.point.y + objectMoseToCenterVector.magnitude * Mathf.Sin(oAngle);
				float newXPosi = hit.point.x + objectMoseToCenterVector.magnitude * Mathf.Cos(oAngle);
				transform.position = new Vector3(newXPosi, newYPosi, transform.position.z);
				//transform.position = new Vector3(0, 0, 0);
				//Debug.Log(thisMoveVector.magnitude);
				
				//Debug.Log(rotAngle);
				
				//Vector3 newPosition = hit.point -( mouseDownVector3 - mouseDownObjectPosition);
				//transform.position = newPosition;
				
				
				lastInductionPoint = hit.point;
			}
		}
	}
}










