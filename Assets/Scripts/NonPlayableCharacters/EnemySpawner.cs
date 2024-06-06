using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemies = 1;
    [SerializeField] private float spawnRate = 1.0f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform LookAt;

    private int currentEnemies;
    private float spawnTimer;

    private void Start()
    {
        currentEnemies = 0;
        spawnTimer = 0.0f;
    }

    private void Update()
    {
        if (currentEnemies >= maxEnemies) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnRate)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Quaternion spawnRotation = Quaternion.LookRotation(LookAt.position - spawnPoint.position);
            Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnRotation).GetComponent<Enemy>();
            enemy.OnEnemyKilled += (object sender, System.EventArgs e) => currentEnemies--;
            currentEnemies++;
            spawnTimer = 0.0f;
        }
    }
}
