using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private void Awake() => DontDestroyOnLoad(transform.gameObject);
}