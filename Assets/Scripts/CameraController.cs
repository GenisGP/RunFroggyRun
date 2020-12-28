using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float offsetX;  //Para que la cámara no se mueva tanta distancia al moverse el personaje horizontalmente

    public PlayerController playerCtrl;

    public bool cameraFollowing = true;

    // Start is called before the first frame update
    void Start()
    {
        //La separación será igual a su posición - la posición del player
        //offset = transform.position - target.position;

        //Al empezar la cámara estará en la mismo posición que el personaje pero desplazada un offset (10) hacia atrás
        transform.position = new Vector3(target.transform.position.x, offset.y + target.transform.position.y, offset.z + target.position.z);
    }
    private void Update()
    {
        
        switch(playerCtrl.desiredLane)
        {
            case 0:
                offsetX = -1f;
                break;
            case 1:
                offsetX = 0f;
                break;
            case 2:
                offsetX = 1f;
                break;
        }
       
    }
    private void LateUpdate()
    {
        if (cameraFollowing)
        {
            //La nueva posición de la cámara tendrá siempre la misma X e Y pero variará en Z según la distancia con el player
            Vector3 newPosition = new Vector3(target.transform.position.x - offsetX, offset.y + target.transform.position.y, offset.z + target.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, 10 * Time.deltaTime);
        }
    }
}
