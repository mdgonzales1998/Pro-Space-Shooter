using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private int _enemySpawn = 5;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private GameObject _enemyContainer;
    private bool _spawnning = true;

    public void StartSpawning()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_spawnning)
        {
            Vector3 enemy_location = new Vector3(Random.Range(-9.5f, 9.5f), 8, 0);
            GameObject enemy = Instantiate(_enemyPrefab, enemy_location, Quaternion.identity);
            enemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_spawnning)
        {
            Vector3 powerup_location = new Vector3(Random.Range(-9.5f, 9.5f), 8, 0);
            int powerUpIndex = Random.Range(0, 3);
            Instantiate(_powerups[powerUpIndex], powerup_location, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 8.0f));
        }
    }

    public void OnPlayerDeath()
    {
        _spawnning = false;
    }
}
