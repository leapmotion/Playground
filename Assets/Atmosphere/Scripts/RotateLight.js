#pragma strict

var speed : float = 100.0;
private var lastMousePos : Vector2;
private var rotate : boolean = false;

function Update () 
{

	if(Input.GetMouseButtonDown(0)) rotate = true;
	if(Input.GetMouseButtonUp(0)) rotate = false;

	var delta : Vector2 = lastMousePos - Input.mousePosition;
	
	if(rotate)
	{
		transform.Rotate(Vector3(delta.y*Time.deltaTime*speed, delta.x*Time.deltaTime*speed, 0));
	}
	
	lastMousePos = Input.mousePosition;

}

