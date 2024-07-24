using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    public Food _foodPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var b = Instantiate(_foodPrefab);
            b.transform.position = RandomSpawn();
        }    
    }

    Vector3 RandomSpawn()
    {
        float spawnX = Random.Range(GameManager.instance.boundWidth  / 2, -GameManager.instance.boundWidth  / 2);
        float spanwZ = Random.Range(GameManager.instance.boundHeight / 2, -GameManager.instance.boundHeight / 2);
        Vector3 randomSpawn = new Vector3(spawnX, 0, spanwZ);
        return randomSpawn;
    }
}
