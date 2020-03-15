#pragma strict
var particles:GameObject;
var startTime:float = 0;
var repeatTime:float = 10;

function Start () {
	InvokeRepeating("spawn", startTime, repeatTime);
}

function spawn(){
	var temp = Instantiate(particles, transform.position, transform.rotation);
	temp.transform.parent = null;
	Destroy(temp,repeatTime);
}