using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20.0f;
    public float lifetime = 1f;

    public float damage = 10f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        checkCollision();
    }

    void checkCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime))
        {
            string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            if (layerName == "Enemy")
            {
                hit.collider.GetComponent<Enemy>().ReceiveDamage(damage);
                print("Hit Enemy");
            }
            Destroy(gameObject);
        }
    }
}