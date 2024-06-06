using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float maxEnemies = 1.0f;
    [SerializeField] private float spawnRate = 1.0f;
    [SerializeField] private float spawnRadius = 1.0f;

    private int currentEnemies;
    private float spawnTimer;

    private void Start()
    {
        currentEnemies = 0;
        spawnTimer = 0.0f;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (currentEnemies < maxEnemies && spawnTimer >= spawnRate)
        {
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            Vector3 spawnPosition = new(transform.position.x + randomPosition.x, transform.position.y, transform.position.z + randomPosition.z);
            Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
            enemy.OnEnemyKilled += (object sender, System.EventArgs e) => currentEnemies--;
            currentEnemies++;
            spawnTimer = 0.0f;
        }
    }
}
