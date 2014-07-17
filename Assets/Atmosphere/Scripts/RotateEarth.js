#pragma strict

var speed : float = 1.0;

function Update () 
{
	transform.Rotate(Vector3(0, Time.deltaTime * speed, 0));
}