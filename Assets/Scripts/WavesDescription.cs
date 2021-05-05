using UnityEngine;

[CreateAssetMenu(menuName = "Waves")]
public class WavesDescription : ScriptableObject
{
    [SerializeField] public Wave[] waves;
}