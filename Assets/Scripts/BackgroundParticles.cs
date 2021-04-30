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
            case GameState.Play when particles.isPaused:
                particles.Play();
                break;
            case GameState.Pause when particles.isPlaying:
                particles.Pause();
                break;
        }
    }
}
