using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public PlayerController player;

    public GameObject endParticle;

    public Animator anim;

    public AudioSource aSourceEnd, aSourceEndHit;

    public AudioManager aManager;

    private void Update()
    {
        if (PlayerManager.levelCompleted || PlayerManager.gameOver)
        {
            aSourceEnd.Stop();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        player.isEnding = true;
        player.gravity = 0;

        player.gameObject.SetActive(false);
        //Animación de escalar
        anim.SetBool("EndLevel", true);

        //Partículas del final
        Vector3 spawnPos = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z);
        endParticle.transform.position = spawnPos;
        endParticle.SetActive(true);
        //endParticle.Play();

        //Audio
        aSourceEndHit.Play();
        aManager.KeepPlayingSound("Music");

        yield return new WaitForSeconds(2f);

        PlayerManager.levelCompleted = true;
    }
}
