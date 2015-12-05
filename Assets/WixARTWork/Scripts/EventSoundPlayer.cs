using UnityEngine;
using System.Collections;

public class EventSoundPlayer : MonoBehaviour {

    [SerializeField]
    SoundTable[] soundList;
    Hashtable hashSoundList = new Hashtable();

    AudioSource audioSource;

    void Start() {
        for(int i = 0; i < soundList.Length; i++) {
            hashSoundList.Add(soundList[i].soundName, soundList[i].clip);
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(string key) {
        audioSource.PlayOneShot(hashSoundList[key] as AudioClip);
    }


    [System.Serializable]
    class SoundTable {
        public string soundName;
        public AudioClip clip;
    }
}
