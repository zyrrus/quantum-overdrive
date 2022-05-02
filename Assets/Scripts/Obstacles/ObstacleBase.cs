using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    // Keep this one and delete the other, but see what depends on the other one first

    private Vector3 resetPoint;
    [SerializeField] private bool isBouncy;
    [SerializeField] private bool isDeadly;
    public bool IsBouncy { get => isBouncy; private set { isBouncy = value; } }
    public bool IsDeadly { get => isDeadly; private set { isDeadly = value; } }

    private void Start() => resetPoint = gameObject.transform.position;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            EnemyCollision pc = other.gameObject.GetComponent<EnemyCollision>();
            pc.OnHitPlayer(transform.position, true);
        }
    }

    public void ResetObstacle() => gameObject.transform.position = resetPoint;
}
