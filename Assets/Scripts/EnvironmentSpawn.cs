using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawn : MonoBehaviour
{
    public List<Transform> assets = new List<Transform>();

    public Transform player;
    public float distanceToActivate;
    public float distanceToDeactivate;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            assets.Add(child);
            child.gameObject.SetActive(false);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        foreach (Transform asset in assets)      
        {
            if(asset != null)
            {
                if (asset.position.z - player.position.z < distanceToActivate)
                {
                    asset.gameObject.SetActive(true);
                }
                if (player.position.z - asset.position.z > distanceToDeactivate)
                {
                    asset.gameObject.SetActive(false);
                }
            }         
        }
    }
}
