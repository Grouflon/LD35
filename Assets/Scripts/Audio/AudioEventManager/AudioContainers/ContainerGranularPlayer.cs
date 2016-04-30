using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class ContainerGranularPlayer : MonoBehaviour {
    [Header ("REFERENCES")]
    public AudioClip[] clips;
    public GameObject audioSourcePrefab;
    public GameObject audioListenerObject;
    public bool findListener;

    [Header ("PARAMETERS")]
    [Range(0f,1f)]
    public float minVolume = 1f;
    [Range(0f,1f)]
    public float maxVolume = 1f;
    [Range(0.01f,3f)]
    public float minPitch = 1f;
    [Range(0.01f,3f)]
    public float maxPitch = 1f;
    [Range(0f,22000f)]
    public float minLPF= 22000f;
    [Range(0f,22000f)]
    public float maxLPF = 22000f;

    [Header ("GRANULAR ENGINE")]
    [Range (0, 60)]
    public float minInitialDelay;
    [Range (0, 60)]
    public float maxInitialDelay;
    [Range (0, 60)]
    public float minSpawnTime;
    [Range (0, 60)]
    public float maxSpawnTime;

    [Header ("PLAYBACK PROPERTIES")]
    [Range (1,20)]
    public int avoidRepeatLast;
    [Range (1,20)]
    public int maxPolyphony;
    public bool allowRuntimeAdjustment = true;

    private List<int> noRepetitionAllowed;
    private List<AudioSource> sourcesPool;

    // Initialized parameters
    void Awake()
    {
        noRepetitionAllowed  = new List<int>();
        sourcesPool = new List<AudioSource> ();

        for (int i = 0; i < maxPolyphony; i++) 
        {
            InstantiateGameObjects ();
        }

        for (int i = 0; i < avoidRepeatLast; i++) 
        {
            noRepetitionAllowed.Add (0);
        }

        if (findListener)
        {
            audioListenerObject = null;
        }
    }

    // Search for the AudioListenerObject on Startup
    void Start()
    {
        if (findListener)
        {
            audioListenerObject = GameObject.Find("AudioListener");
            if (audioListenerObject == null)
            {
                Debug.LogError("No AudioListener gameObject found. Please make sure your AudioListener component is on his own gameObject named AudioListener");
            }
        }
    }

    // Handle the logic of assigning parameter to the choosen AudioSource and plays  it.
    public void PlayAudioSouce()
    {
        AudioSource source = GetPooledAudioSource();

        if (source != null)
        {
            source.gameObject.SetActive(true);
            for (int i = (noRepetitionAllowed.Count - 1); i > 0; i--)
            { 
                noRepetitionAllowed[i] = noRepetitionAllowed[(i - 1)]; 
            }
        
            source.volume = Random.Range(minVolume, maxVolume);
            source.pitch = Random.Range(minPitch, maxPitch);
            source.clip = clips[RandomRangeNoRepeat(0, clips.Length)];
            source.Play();

            StartCoroutine(DisableSourceDelayed(source, source.clip.length * (1 / source.pitch)));
        }
    }

    // Disables the pooled gameObject when the sound has finished to play.
    IEnumerator DisableSourceDelayed(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.gameObject.SetActive(false);
    }

    // Instantiate the corresponding gameObject and gets references to his AudioSource. 
    // Stores thoses references in a list.
    // Returns the instantiated GameObject if needed. 
    GameObject InstantiateGameObjects()
    {
        GameObject go = (GameObject)Instantiate(audioSourcePrefab, transform.position, transform.rotation);
        go.transform.parent = gameObject.transform;
        go.name = "POOLED : ContainerGranularPlayer : " + gameObject.name;
        go.SetActive(false);

        AudioSource source = go.GetComponent<AudioSource> ();
        sourcesPool.Add (source);

        return go;
    }

    // Gets a free to play AudioSource.
    // If allowed, instantiates a new instance if needed and returns it.
    AudioSource GetPooledAudioSource()
    {
        foreach (AudioSource source in sourcesPool)
        {
            if (!source.gameObject.activeInHierarchy)
            {
                source.gameObject.SetActive(true);
                return source;
            }
        }

        if (!allowRuntimeAdjustment)
        {
            Debug.LogError ("Not Enought polyphony available for ContainerRandomPlayer " + gameObject.name);
            return null;
        }
        else
        {
            GameObject go = InstantiateGameObjects();
            go.SetActive(true);
            AudioSource source = go.GetComponent<AudioSource>();
            return source;
        }
    }

    // Picks a random number within a range, 
    int RandomRangeNoRepeat(int min, int max)
    {
        int val = Random.Range(min, max);
        while (noRepetitionAllowed.Contains(val))
        {
            val = Random.Range (min, max);
        }
        noRepetitionAllowed[0] = val;
        return val;
    }
}
