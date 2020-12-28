using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumping : MonoBehaviour
{
    public float jumpForce;
    public Vector3 jumpDirection = Vector3.up;
    public float jumpRate = 3f;
    private float nextTimeToJump;
    private bool grounded, attacking = false;
    private Animator anim;

    private Rigidbody rb;

    public PlayerController playerController;

    public Sound[] sounds;
    private AudioSource aSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        aSource = GetComponent<AudioSource>();
        /*nextTimeToJump = jumpRate;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.gameOver)
        {
            aSource.Stop();
        }
    }
    private void FixedUpdate()
    {
        if (grounded)
        {
            if (Time.timeSinceLevelLoad >= nextTimeToJump && !attacking)
            {
                rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            }
        }
    }
    public void Die()
    {
        PlaySound("Die");
        Destroy(gameObject);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;

            if (!attacking)
            {
                PlaySound("Ground");
            }
            
            anim.SetBool("isGrounded", true);
            anim.SetBool("isJumping", false);
            nextTimeToJump = Time.timeSinceLevelLoad + jumpRate;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
            
            PlaySound("Jump");
            
            anim.SetBool("isGrounded", false);
            anim.SetBool("isJumping", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            attacking = true;

            anim.SetBool("Attack", true);

            PlaySound("Attack");

            StartCoroutine(playerController.Dead());
        }
    }
}

