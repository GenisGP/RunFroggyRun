using UnityEngine;

public class Fly : MonoBehaviour
{
    public ParticleSystem eatenParticle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerManager.numberOfCoins += 1;
            eatenParticle.Play();
            gameObject.SetActive(false);  
        }
    }
}
