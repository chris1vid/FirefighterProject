#pragma strict
var menuPause:Animator; // menu to deploy when in pause
var AudioCUnPause:AudioClip;
var AudioCPause:AudioClip;
var AudioPause:Audio.AudioMixerSnapshot;
var AudioUnpause:Audio.AudioMixerSnapshot;
@script RequireComponent(AudioSource);
function Start () {
	menuPause.updateMode = AnimatorUpdateMode.UnscaledTime;
}

function Update () {

	if (Input.GetButtonDown("Cancel") ){
		if (Time.timeScale == 1){
			AudioPause.TransitionTo(0.01);
			GetComponent.<AudioSource>().PlayOneShot(AudioCPause, 1.0F);
			menuPause.SetBool("pause", true);
			Time.timeScale = 0;
		}else {
			menuPause.SetBool("pause", false);
			Time.timeScale = 1;
			AudioUnpause.TransitionTo(0.01);
			GetComponent.<AudioSource>().PlayOneShot(AudioCUnPause, 1.0F);
		}
	}
	


}