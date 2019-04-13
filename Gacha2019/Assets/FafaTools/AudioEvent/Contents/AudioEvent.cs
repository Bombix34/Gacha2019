using UnityEngine;

public abstract class AudioEvent : ScriptableObject {

	public bool loop;
	public AudioClip[] clips;
	public abstract void Play(AudioSource source);
}
