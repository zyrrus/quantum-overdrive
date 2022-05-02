using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerStateMachine psm;

    private void Awake() => psm = GetComponent<PlayerStateMachine>();

    public void OnHitPlayer(Vector2 obstaclePos, bool isObstacleBouncy)
    {
        Debug.Log("Player and Obstacle collision");
        if (isObstacleBouncy && psm.IsDashing)
            LaunchPlayer(obstaclePos);
        else KillPlayer();
    }

    private void LaunchPlayer(Vector2 obstaclePos)
    {
        Debug.Log("LAUNCH");

        Vector2 oldVel = psm.Rb.velocity;
        psm.Rb.velocity = new Vector2();

        psm.Rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }
    private void KillPlayer() { Debug.Log("DEAD"); }
}
