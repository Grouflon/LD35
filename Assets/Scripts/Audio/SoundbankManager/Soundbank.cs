using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Soundbank : MonoBehaviour 
{
	public new string name;
	public List<AudioClip> audioClips;
	public bool isLoaded = false;

	private int loadedClipSuccesCount = 0;
	private int cycleCount = 0;

	// Loads the soundbank
	public void LoadSoundbank()
	{
		if (!isLoaded) 
		{
			foreach (AudioClip clip in audioClips) 
			{
				StartCoroutine (LoadAudioClip (clip));
			}
		}
		else 
		{
			Debug.LogError ("The Soundbank " + name + " your're trying to load is already loaded");
		}
	}

	// Handles the loading logic and waits for the files to be actually loaded before callback
	IEnumerator LoadAudioClip (AudioClip clip)
	{
		
		clip.LoadAudioData ();
		while (clip.loadState == AudioDataLoadState.Loading || clip.loadState == AudioDataLoadState.Loading) 
		{
			yield return null;
		}
			
		if (clip.loadState == AudioDataLoadState.Loaded) 
		{				
			loadedClipSuccesCount++;
			cycleCount++;
		}
		else 
		{
			cycleCount++;
		}
	
		if (cycleCount == audioClips.Count) 
		{
			isLoaded = CheckLoad ();

			if (isLoaded) 
			{
				Debug.Log ("Soundbank " + name + " is loaded");
			} 
			else 
			{
				Debug.LogError ("Loading soundbank " + name + " has failed");
			}
		}				
	}
		
	// Unloads the soundbank
	public void UnloadSoundbank () 
	{
		if (isLoaded) 
		{
			foreach (AudioClip clip in audioClips) 
			{
				StartCoroutine (UnloadAudioClip (clip));
			}
		} 
		else 
		{
			Debug.LogError ("The Soundbank " + name + " your're trying to unload is already unloaded");
		}
	}

	// Handles the unloading logic and waits for the files to be actually unloaded before callback
	IEnumerator UnloadAudioClip (AudioClip clip)
	{
		clip.UnloadAudioData ();
		while (clip.loadState == AudioDataLoadState.Loaded || clip.loadState == AudioDataLoadState.Loading) 
		{
			yield return null;
		}

		if (clip.loadState == AudioDataLoadState.Unloaded) 
		{
			loadedClipSuccesCount--;
			cycleCount--;
		} 
		else 
		{
			cycleCount--;
		}

		if (cycleCount == 0) 
		{
			isLoaded = CheckLoad ();

			if (!isLoaded) 
			{
				Debug.Log ("Soundbank " + name + " has been unloaded");
			} 
			else 
			{
				Debug.LogError ("Unloading soundbank " + name + " has failed");
			}
		}				
	} 


	// Checks if the bank has been loaded or unloaded
	bool CheckLoad()
	{
		bool bankIsLoaded = cycleCount == loadedClipSuccesCount && loadedClipSuccesCount == audioClips.Count;
		return bankIsLoaded;
	}

	// Test Keys
	/*void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			LoadSoundbank ();	
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			UnloadSoundbank ();	
		}
	}*/
}
