using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public float zSpawn = 0;  //Donde se instanciará el prefab
    public float tileLength = 30f; //Longitud de cada prefab, en este caso todos miden 30
    public int numberOfTiles = 5; //Numero de elementos vistos en pantalla
    private List<GameObject> activeTile = new List<GameObject>(); //Lista de elementos activos en la escena

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        //Al empezar se spawnearán elementos, tantos como se hayan especificado en la variabble NumberOfTiles, en este caso 5
        for (int i = 0; i <= numberOfTiles; i++)
        {
            if(i == 0)
            {
                //El primero será siempre el primer elemento, la carretera vacía
                SpawnTile(0);
            }
            else
            {
                SpawnTile(Random.Range(0, tilePrefabs.Length - 1));
            }    
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Cuando el player pase del centro de un prefab al centro de otro se instanciará uno nuevo al final.
        //Ej: al principio se spawnean 6 elementos así que zSpawn es 30*6 = 180. Entonces queda posición en z > 180-(5*30) = 30
        if(player.transform.position.z > zSpawn - (numberOfTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length - 1));
            DeleteTile();
        }
    }

    public void SpawnTile(int tileIndex)
    {
        //Se instancia el prefab a una distancia de zSpawn del TileManager en (0,0,0)
        GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        //Se añade el gameobject a la lista de activos
        activeTile.Add(go);
        //Al principio será 30, luego 60, así que cada vez se irá augmentando la posición a partir de la longitud del prefab
        zSpawn += tileLength;
    }

    void DeleteTile()
    {
        //Elimina el objeto de la escena y de la lista de activos. Al ser una lista los índices de la posición de los objetos se actualizan automáticamente
        Destroy(activeTile[0]);
        activeTile.RemoveAt(0);
    }
}
