using UnityEngine;

public class BackgroundParticles : MonoBehaviour
{
    private ParticleSystem particles;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particles.Play();
    }

    private void Update()
    {
        switch (GameManager.GameState)
        {
            case GameState.Start:
                break;
            case GameState.Play:
                if (particles.isPaused) particles.Play();
                break;
            case GameState.Pause:
                if (particles.isPlaying) particles.Pause();
                break;
            case GameState.GameOver:
                break;
            case GameState.Restart:
                break;
        }
    }
}
