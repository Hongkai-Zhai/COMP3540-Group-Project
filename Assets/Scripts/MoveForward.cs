using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 40.0f;
    public float maxLifetime = 3f; 

    private void Start()
    {
        
        Destroy(gameObject, maxLifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("Bullet collided with: " + collision.gameObject.name);
        Destroy(gameObject);
    }
}