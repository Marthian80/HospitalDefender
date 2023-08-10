using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacteriaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] bacteria = new GameObject[4];
    [SerializeField][Range(0, 50)] private int poolSize = 5;
    [SerializeField][Range(0.1f, 30f)] private float spawnTimer = 1f;

    private GameObject[] bacteriaPool;

    private void Awake()
    {
        PopulatePool();
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private void PopulatePool()
    {
        bacteriaPool = new GameObject[poolSize];

        for (int i = 0; i < bacteriaPool.Length; i++)
        {
            bacteriaPool[i] = Instantiate(bacteria[Random.Range(0, bacteria.Length -1)], transform.position, Quaternion.identity);
            bacteriaPool[i].SetActive(false);
        }
    }

    private void EnableObjectInPool()
    {
        foreach (var gameObject in bacteriaPool)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                return;
            }
        }
    }    

    IEnumerator SpawnEnemy()
    {
        while (Application.isPlaying)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
