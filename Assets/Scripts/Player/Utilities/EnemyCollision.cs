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
        if (psm.IsLaunched) return;

        Debug.Log("LAUNCH");

        psm.IsLaunched = true;

        Vector2 oldVel = psm.Rb.velocity;
        psm.Rb.velocity = new Vector2();
        Vector2 launchDirection = -oldVel.normalized;

        psm.Rb.AddForce(launchDirection * psm.BounceForce, ForceMode2D.Impulse);
    }
    private void KillPlayer() { Debug.Log("DEAD"); psm.Die(); }
}
