using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public GameObject wave;

    public Animator transition;

    private AudioSource audioSource;

    public AudioSource music;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Play()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        wave.SetActive(true);

        transition.SetTrigger("WaveUp");
        
        music.Stop();
        audioSource.Play();

        yield return new WaitForSeconds(1.3f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);

    }
}
