using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] float activationRadius = 10f; 
    public float ActivationRadius => activationRadius;

    public Transform[] SpawnPositions; 
}