using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public PitchVolumeAudio shootSound; //0
    public PitchVolumeAudio filetSound; //1
    public PitchVolumeAudio catchButterfly; //2
    public PitchVolumeAudio destructionHouse; //3
    public PitchVolumeAudio endLevel; //4

	
	public AudioSource[] audiosources; 

	void Start(){
		for(int i =0 ; i < audiosources.Length;i++){
			audiosources[i].loop=false;
		}
	}

	public void PlaySound(int sound)
    {
		switch(sound)
        {
			case 0:
				GetAudioSourceAvailable(shootSound);
				break;
			case 1:
				GetAudioSourceAvailable(filetSound);
				break;
            case 2:
                GetAudioSourceAvailable(catchButterfly);
                break;
            case 3:
                GetAudioSourceAvailable(destructionHouse);
                break;
            case 4:
                GetAudioSourceAvailable(endLevel);
                break;
        }
    }


	public void GetAudioSourceAvailable(AudioClip clip){
		for(int i =0 ; i < audiosources.Length;i++){
			if(!audiosources[i].isPlaying){
				audiosources[i].loop=false;
				audiosources[i].clip=clip;
				audiosources[i].Play();
				return;
			}
		}
	}

	public void GetAudioSourceAvailable(PitchVolumeAudio clip){
		for(int i =0 ; i < audiosources.Length;i++){
			if(!audiosources[i].isPlaying){
				clip.Play(audiosources[i]);
				return;
			}
		}
	}


	private static SoundManager s_Instance = null;

	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	public static SoundManager instance
	{
		get
		{
			if (s_Instance == null)
			{
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;
			}

			// If it is still null, create a new instance
			if (s_Instance == null)
			{
				GameObject obj = Instantiate(Resources.Load("SoundManager") as GameObject);
				s_Instance = obj.GetComponent<SoundManager>();
			}
			return s_Instance;
		}
	}
}