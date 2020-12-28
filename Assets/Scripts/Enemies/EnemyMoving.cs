using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoving : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private NavMeshAgent agent;
    private Transform waypointsParent;
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;

    public GameObject mesh;
    public ParticleSystem dieParticle;

    public PlayerController playerController;
    private Animator anim;

    public Sound [] sounds;
    private AudioSource aSource;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        aSource = GetComponent<AudioSource>();

        anim.SetBool("isMoving", true);
        waypointsParent = transform.parent.Find("Waypoints");  //Buscamos en el objeto padre el objeto hijo (hermano) que contiene los waypoints
        foreach (Transform child in waypointsParent)    //Se añade cada waypoint a la lista de waypoints   
        {
            waypoints.Add(child);
        }
        targetWaypoint = waypoints[targetWaypointIndex];    //Se asigna el waypoint de destino
        agent.destination = targetWaypoint.position;        //Se asigna el destino al agente navmesh

        PlaySound("Standard");
    }

    // Update is called once per frame
    void Update()
    {
        //if (Vector3.Distance(transform.position, targetWaypoint.position) <= 0f)
        if (transform.position.x == targetWaypoint.position.x  && transform.position.z >= targetWaypoint.position.z )//Si ha llegado al destino (solo mira la x)
            {
            //Se pasa al siguiente waypoint de la lista y se asigna como destino
            targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Count;  
            targetWaypoint = waypoints[targetWaypointIndex];
            agent.destination = targetWaypoint.position;
        }

        if (PlayerManager.gameOver)
        {
            aSource.Stop();
        }
    }

    public void Die()
    {
        PlaySound("Die");

        mesh.SetActive(false);

        dieParticle.transform.position = transform.position;
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
            }
            else if (PlayerController.isSliding == false)
            {
                agent.isStopped = true;
                transform.LookAt(playerController.gameObject.transform);
                anim.SetBool("Attack", true);

                PlaySound("Attack");

                StartCoroutine(playerController.Dead());
            }
        }
    }
}
