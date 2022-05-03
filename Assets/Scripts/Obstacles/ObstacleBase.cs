using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    // Keep this one and delete the other, but see what depends on the other one first

    private Vector3 resetPoint;

    [SerializeField] private bool isBouncy;
    [SerializeField] private bool isDeadly;
    [SerializeField] private bool isHit;

    public bool IsBouncy { get => isBouncy; private set { isBouncy = value; } }
    public bool IsDeadly { get => isDeadly; private set { isDeadly = value; } }
    public bool IsHit { get => isHit; set => isHit = value; }


    private void Start() => resetPoint = gameObject.transform.position;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isHit = true;
            EnemyCollision pc = other.gameObject.GetComponent<EnemyCollision>();
            pc.OnHitPlayer(transform.position, isBouncy);
        }
    }

    public void ResetObstacle() => gameObject.transform.position = resetPoint;
}
