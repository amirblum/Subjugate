using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour {

	public AudioClip jumpSound;
	public AudioClip deathSound;
	public AudioClip controlSound;
	public AudioClip endSound;
	private  bool jumpSoundPlaying;

	// Use this for initialization
	void Start () {
		jumpSoundPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (jumpSoundPlaying) jumpSoundPlaying = false;
	}

	public void playJumpSound() {
		if (!jumpSoundPlaying) {
			jumpSoundPlaying = true;
			audio.PlayOneShot(jumpSound);
		}
	}

	public void playDeathSound() {
		audio.PlayOneShot (deathSound);
	}

	public void playControlSound() {
		audio.PlayOneShot (controlSound);
	}

	public void playEndSound() {
		audio.PlayOneShot (endSound);
	}

}
