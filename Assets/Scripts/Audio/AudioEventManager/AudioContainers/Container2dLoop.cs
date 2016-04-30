using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Container2dLoop : MonoBehaviour 
{
	[Header("PARAMETERS")]
	[Range (0,15)]
	public float fadeInTime;
	[Range (0,15)]
	public float fadeOutTime;
	public bool randomStartPosition;

	private bool hasPlayedOnce;
	private bool fadingIn;
	private bool fadingOut;
	private float targetVolume;
	private AudioSource source;


	// Sets the initial conditions for this container to play properly
	void Awake()
	{
		source = gameObject.GetComponent<AudioSource> ();
		if (source.playOnAwake == true)
			Debug.LogError ("PlayOnAwake is active on Container2DLoop " + gameObject.name);
		if (source.clip == null)
			Debug.LogError ("No AudioClip is set on Container2dLoop " + gameObject.name);
		if (source.loop == false)
			source.loop = true;
		
		targetVolume = source.volume;
		source.volume = 0f;

	}

	// Plays the AudioSource with specifics start behaviours
	// On first play, if "RandomStartPosition" is ticked, the source can start from anywhere in the file. If not, it starts from the beginning
	// If the sound has already played, it restarts from where it stoped
	public void Play () 
	{
		if (!hasPlayedOnce ) 
		{
			if (!randomStartPosition) 
			{
				source.Play ();
				fadingIn = true;
				fadingOut = false;
			}
			else
			{
				source.timeSamples = Random.Range(0, source.clip.samples);
				source.Play();
				fadingIn = true;
				fadingOut = false;
			}
			hasPlayedOnce = true;
		}
		else
		{
			source.UnPause();
			fadingIn = true;
			fadingOut = false;
		}
	}

	// Pauses the AudioSource
	public void Stop()
	{
		StartCoroutine(WaitForFadeOut(fadeOutTime * source.volume));
		fadingOut = true;
		fadingIn = false;
	}

	// Delays the Stop method execution to let the fadeOut happen
	IEnumerator WaitForFadeOut(float time)
	{
		yield return new WaitForSeconds(time);
		if (fadingOut && source.volume == 0f)
		{
			source.Pause();
			fadingOut = false;
		}
	}

	// Checks if the container is currently playing
	public bool isPlaying()
	{
		bool result = source.isPlaying;
		return result;
	}
	
	// Handles the fades in & out value tweenings
	void Update () 
	{
		if (fadingIn && source.volume < targetVolume)
		{
			source.volume += Time.deltaTime * ((1/fadeInTime) * targetVolume);
		}

		if (fadingOut && source.volume > 0f)
		{
			source.volume -= Time.deltaTime * ((1/fadeOutTime) * targetVolume);
		}

		/*TEST KEYS
		if (Input.GetKeyDown(KeyCode.A))
			Play();

		if (Input.GetKeyDown(KeyCode.Z))
			Stop();

		if (Input.GetKeyDown(KeyCode.E))
			Debug.Log(isPlaying());
		*/
	}
}
