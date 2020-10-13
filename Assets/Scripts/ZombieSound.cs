using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ZombieSound :
    MonoBehaviour
{
    private AudioSource _audio;
	private void Start ()
    {
        var audio = GetComponent<AudioSource>();
        audio.time = Random.Range(0, audio.clip.length);
	}
}
