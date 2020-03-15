using UnityEngine;
using System.Collections;

public class Fireworksound : MonoBehaviour {

	private ParticleSystem particle;
	public AudioSource audio;
	public AudioClip sfxLaunch;
	public AudioClip sfxDelayedExplosion;
	public float delayExplosion = 1.0f;
	public float delayFirework = 3.0f;
	private bool isDone = false;
	private bool isDone2 = false;
	private Transform posParticle;
	private ParticleSystem.Particle[] m_Particles;
	public Light SunLight;
	public float nightAngle = -10.7f;
	public float dayAngle = -7.58f;
	private Vector3 nightpos;
	private Vector3 dayPos;
	public float delayStart;

	void Start () {
		isDone = true;
		particle = gameObject.GetComponent<ParticleSystem>();
		nightpos = new Vector3(nightAngle, SunLight.transform.eulerAngles.y, SunLight.transform.eulerAngles.z);
		dayPos = new Vector3(dayAngle, SunLight.transform.eulerAngles.y, SunLight.transform.eulerAngles.z);
		Invoke ("Fire", delayStart);
	}
	void Update () {
		if (isDone == false) {
			isDone = true;
			isDone2 = true;
			audio.PlayOneShot(sfxLaunch);
			particle.Emit(1);
		}
		if(particle.isPlaying == false && isDone2 == true) {
			isDone2 = false;
			SunLight.transform.eulerAngles = dayPos;
			Invoke("Explosion", delayExplosion);	
			Invoke("Fire", delayFirework);
			Invoke("lightOff", 0.1f);
		}
	}
	void Fire() {
		isDone = false;
	}
	void lightOff() {
		SunLight.transform.eulerAngles = nightpos;
	}

	void Explosion() {
		audio.PlayOneShot(sfxDelayedExplosion);
	}
}
