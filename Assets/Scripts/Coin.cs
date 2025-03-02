using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed;
    public int scoreValue = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Coin collected by player!");
            Destroy(gameObject);
            GameManager.Instance.AddScore();
            
        }

    }
    private void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed * Time.fixedDeltaTime, 0);
    }
}
