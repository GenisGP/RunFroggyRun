using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public ParticleSystem splash;

    public CameraController camController;

    public PlayerController playerController;

    public AudioManager aManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.isDrowing = true;  //El jugador se está ahogando

            //Audio
            aManager.PlaySound("WaterSplash");

            //Partículas de salpicadura
            splash = Instantiate(splash, other.transform.position, Quaternion.Euler(90f, 0f, 0f));
            splash.Play();

            camController.cameraFollowing = false;  //La cámara deja de seguir al jugador

            StartCoroutine(playerController.Drown());   //Se inicia la corrutina de ahogarse
        }
    }
}
