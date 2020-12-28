using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;

    private Animator anim;

    public PlayerController playerController;

    public GameObject mesh;
    public ParticleSystem dieParticle;

    public Sound[] sounds;
    private AudioSource aSource;

    private void Start()
    {
        anim = GetComponent<Animator>();
        aSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        //Para que no rote en el eje Y
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        
        
        if (PlayerManager.gameOver)
        {
            aSource.Stop();
        }
    }

    public void Die()
    {
        PlaySound("Die");

        mesh.SetActive(false);
        dieParticle.Play();
    }

    public void PlaySound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                aSource.clip = s.clip;
                aSource.loop = s.loop;
                aSource.volume = s.volume;
                aSource.outputAudioMixerGroup = s.mixerGroup;
                aSource.Play();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {            
            if (PlayerController.isSliding == true)
            {
                Die();
                GetComponent<BoxCollider>().enabled = false;
            }else if(PlayerController.isSliding == false)
            {
               anim.SetBool("Attack", true);

                PlaySound("Attack");

                StartCoroutine(playerController.Dead());
            }         
        }
    }
}
