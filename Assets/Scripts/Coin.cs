using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed;
    public int scoreValue = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }

    }
    private void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed * Time.fixedDeltaTime, 0);
    }
}
