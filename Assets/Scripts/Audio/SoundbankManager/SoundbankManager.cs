using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundbankManager : MonoBehaviour {

	public static SoundbankManager instance = null;
	private Soundbank[] soundbanks;

	void Awake () 
	{
		//Singleton pattern
		if (instance == null) 
		{
			instance = this;
		} 
		else if (instance != null) 
		{
			Destroy (this.gameObject);
		}
		DontDestroyOnLoad (gameObject);

		// get references from all the soundbanks
		soundbanks = GetComponents<Soundbank> ();
	}

	// Loads the soundbank with a given name
	public void Load(string soundbankName)
	{
		Soundbank soundbank = GetSoundbankWithName (soundbankName);
		if (soundbank != null) 
		{
			soundbank.LoadSoundbank ();
		}
	}

	// Unloads the soundbank with a given name
	public void Unload(string soundbankName)
	{
		Soundbank soundbank = GetSoundbankWithName (soundbankName);
		if (soundbank != null)
		{
			soundbank.UnloadSoundbank ();
		}
	}

	// Checks if a soundbank with a given name is loaded
	public bool IsSoundbankLoaded(string soundbankName)
	{
		Soundbank soundbank = GetSoundbankWithName (soundbankName);
		if (soundbank != null) 
		{
			return soundbank.isLoaded;
		} 
		else 
		{
			Debug.LogError ("The Soundbank " + soundbankName + " your're trying to load does not exist.");
			return false;
		}
	
	}

	// Unloads all the curently loaded soundbanks
	public void UnloadAll()
	{
		foreach (Soundbank soundbank in soundbanks) 
		{
			if (soundbank.isLoaded) 
			{
				soundbank.UnloadSoundbank ();
			}				
		}
	}

	// Returns the reference of a soundbank with a given name
	Soundbank GetSoundbankWithName(string name)
	{
		for (int i = 0; i < soundbanks.Length; i++) 
		{
			if (soundbanks [i].name == name) 
			{
				return soundbanks [i];
			} 
		}

		Debug.LogError ("The Soundbank " + name + " your're trying to load does not exist.");
		return null;
	}
}
