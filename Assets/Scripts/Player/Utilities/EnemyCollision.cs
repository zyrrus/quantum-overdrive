using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private PlayerStateMachine psm;

    private void Awake() => psm = GetComponent<PlayerStateMachine>();

    public void OnHitPlayer(Vector2 obstaclePos, bool isObstacleBouncy)
    {
        if (isObstacleBouncy && psm.IsDashing)
            LaunchPlayer(obstaclePos);
        else KillPlayer();
    }

    private void LaunchPlayer(Vector2 obstaclePos)
    {
        Debug.Log("LAUNCH");

        // Might need to set a 'beingLaunched' flag in psm

        Vector2 oldVel = psm.Rb.velocity;
        psm.Rb.velocity = new Vector2();

        Vector2 launchDirection = new Vector2(transform.position.x, transform.position.y) - obstaclePos;
        launchDirection.Normalize();

        psm.Rb.AddForce(launchDirection * 10, ForceMode2D.Impulse);
    }
    private void KillPlayer() { Debug.Log("DEAD"); psm.Die(); }
}
