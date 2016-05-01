using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class ContainerGranularPlayer : MonoBehaviour {
    [Header ("REFERENCES")]
    public new string name;
    public AudioClip[] clips;
    public GameObject audioSourcePrefab;
    private GameObject audioListenerObject;

    [Header ("PARAMETERS")]
    [Range (0,15)]
    public float fadeInTime;
    [Range (0,15)]
    public float fadeOutTime;
    [Range(0f,1f)]
    public float minVolume = 1f;
    [Range(0f,1f)]
    public float maxVolume = 1f;
    [Range(0.01f,3f)]
    public float minPitch = 1f;
    [Range(0.01f,3f)]
    public float maxPitch = 1f;

    [Header ("GRANULAR ENGINE")]
    [Range (0, 60)]
    public float minInitialDelay;
    [Range (0, 60)]
    public float maxInitialDelay;
    [Range (0, 60)]
    public float minSpawnTime;
    [Range (0, 60)]
    public float maxSpawnTime;
    public bool enableSpacializedAudio;
    [Range (0, 1)]
    public float spawnDistanceRandomize;


    [Header ("PLAYBACK PROPERTIES")]
    [Range (1,20)]
    public int avoidRepeatLast = 1;
    [Range (1,20)]
    public int maxPolyphony = 1;
    public bool allowRuntimeAdjustment = true;

    private List<int> noRepetitionAllowed;
    private List<AudioSource> sourcesPool;
    private List<float> targetVolumes;
    private int playCounter = 0;
    private bool isStopped = true;
    private bool fadingIn;
    private bool fadingOut;
    private bool restarting;
    private float volumeMultiplier = 0f;


    // Initialized parameters
    void Awake()
    {
        noRepetitionAllowed  = new List<int>();
        sourcesPool = new List<AudioSource> ();
        targetVolumes = new List<float>();

        for (int i = 0; i < maxPolyphony; i++) 
        {
            InstantiateGameObjects ();
            initializeTargetVolumes();
        }

        for (int i = 0; i < avoidRepeatLast; i++) 
        {
            noRepetitionAllowed.Add (0);
        }
    }

    // Searches for the AudioListener in the scene if enableSpacializedAudio is off.
    void Start()
    {
        // Search for the AudioListenerObject on Startup
        if (!enableSpacializedAudio)
        {
            audioListenerObject = GameObject.Find("AudioListener");
            if (audioListenerObject == null)
            {
                Debug.LogError("No AudioListener gameObject found. Please make sure your AudioListener component is on his own gameObject named AudioListener");
            }
        }
    }

    //Handles the fades logic & tweening values
    void Update()
    {
        // Completly limit the range of volumeMultiplier
        if (volumeMultiplier > 1f)
            volumeMultiplier = 1f;

        if (volumeMultiplier < 0f)
            volumeMultiplier = 0f;

        // Sets the conditions for fadingIn, fadinOut and restarting
        if (!isStopped && volumeMultiplier == 1f)
        {
            fadingIn = false;
            restarting = false;
        }

        if (!isStopped && volumeMultiplier == 0f)
            fadingOut = false;

        // Sets the conditions for FadeIn and FadeOut to happen and value tweening
        if (fadingIn && volumeMultiplier < 1f)
            volumeMultiplier += Time.deltaTime * (1 / fadeInTime);

        if (fadingOut && volumeMultiplier > 0f)
            volumeMultiplier -= Time.deltaTime * (1 / fadeOutTime);

        // Apply the tweened volumes to the audiosources
        if (fadingIn || fadingOut)
        {
            foreach (AudioSource source in sourcesPool)
            {
                if (source.gameObject.activeInHierarchy)
                {
                    source.volume = targetVolumes[sourcesPool.IndexOf(source)] * volumeMultiplier;
                }
            }
        }
    }

    // Triggered on every Play call when the fades are happening. It resets a list of all the targetVolume of the pooled AudioSources
    void initializeTargetVolumes()
    {
        targetVolumes.Clear();
        foreach (AudioSource source in sourcesPool)
        {
            targetVolumes.Add(source.volume);
        }
    }

    // Gets a random delay time. The random logic is different if it's the first time a sound is triggered.
    float GetRandomTime()
    {
        float result = 0f;
        if (playCounter == 0)
        {
            result = Random.Range(minInitialDelay, maxInitialDelay);
            return result;
        }
        else
        {
            result = Random.Range(minSpawnTime, maxSpawnTime);
            return result;
        }
    }

    // Handle the logic of assigning parameter to the choosen AudioSource and plays  it.
    IEnumerator PlayAudioSource()
    {
        yield return new WaitForSeconds(GetRandomTime());
        if (!isStopped)
        {
            AudioSource source = GetPooledAudioSource();
            if (source != null)
            {
                
                source.gameObject.SetActive(true);

                for (int i = (noRepetitionAllowed.Count - 1); i > 0; i--)
                { 
                    noRepetitionAllowed[i] = noRepetitionAllowed[(i - 1)]; 
                }

                if (spawnDistanceRandomize != 0f)
                {
                    source.gameObject.transform.position = GetRandomizedPosition(source.maxDistance);
                }

                source.volume = Random.Range(minVolume, maxVolume);

                if (fadingIn || fadingOut)
                {
                    initializeTargetVolumes();
                }

                source.pitch = Random.Range(minPitch, maxPitch);
                source.clip = clips[RandomRangeNoRepeat(0, clips.Length)];
                source.Play();
                playCounter++;
                StartCoroutine(DisableSourceDelayed(source, source.clip.length * (1 / source.pitch)));
                StartCoroutine(PlayAudioSource());
            }
        }
    }

    // Disables the pooled gameObject when the sound has finished to play.
    IEnumerator DisableSourceDelayed(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.gameObject.transform.position = gameObject.transform.position;
        source.gameObject.SetActive(false);
    }

    // Stops the granular engine
    IEnumerator StopGranulator(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!restarting)
        {
            isStopped = true;
            foreach (AudioSource source in sourcesPool)
            {
                source.Stop();
                source.gameObject.SetActive(false);
                source.gameObject.transform.position = gameObject.transform.position;
                playCounter = 0;
            }   
        }
    }

    // Instantiate the corresponding gameObject and gets references to his AudioSource. 
    // Stores thoses references in a list.
    // Returns the instantiated GameObject if needed. 
    GameObject InstantiateGameObjects()
    {
        GameObject go = (GameObject)Instantiate(audioSourcePrefab, transform.position, transform.rotation);
        go.transform.parent = gameObject.transform;
        go.name = "POOLED : ContainerGranularPlayer : " + name;
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

    // Returns a random Vector3 based on the RandomizeSpawnDistancBy variable
    Vector3 GetRandomizedPosition(float maxDistance)
    {
        float randomnessUnits = maxDistance * spawnDistanceRandomize;
        Vector3 position = new Vector3(0, 0, 0);
        if (!enableSpacializedAudio && audioListenerObject != null)
        {
            position.x = audioListenerObject.transform.position.x + (Random.Range(-randomnessUnits, randomnessUnits));
            position.y = audioListenerObject.transform.position.y + (Random.Range(-randomnessUnits, randomnessUnits));
            position.z = audioListenerObject.transform.position.z + (Random.Range(-randomnessUnits, randomnessUnits));
            return position;
        }
        else
        {
            position.x = gameObject.transform.position.x + (Random.Range(-randomnessUnits, randomnessUnits));
            position.y = gameObject.transform.position.y + (Random.Range(-randomnessUnits, randomnessUnits));
            position.z = gameObject.transform.position.z + (Random.Range(-randomnessUnits, randomnessUnits));
            return position;
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

    // Starts the granular engine.
    public void Play()
    {
        isStopped = false;
        if (fadingOut)
        {
            restarting = true;
        }
        fadingOut = false;
        fadingIn = true;
        StartCoroutine (PlayAudioSource());
    }

    // Stops the granular engine.
    public void Stop()
    {
        fadingOut = true;
        StartCoroutine(StopGranulator(fadeOutTime));           
    }
}
