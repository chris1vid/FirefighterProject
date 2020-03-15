#pragma strict
var audioC:AudioClip;
@script RequireComponent(AudioSource);
function playSfx(){
	GetComponent.<AudioSource>().PlayOneShot(audioC, 1.0F);
}