using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFish : MonoBehaviour
{
    public PlayerController playerController;
    public float jumpForce;

    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Para resetar la animación del salto
            anim.SetBool("hasToJump", true);
            anim.SetBool("isGrounded", true);

            //Se hace saltar al player cambiando la fuerza de salto y luego asignando la fuerza de salto original para dejar de saltar tanto
            playerController.jumpForce *= jumpForce;
            playerController.hasToJump = true;

            
        }
    }
}
