using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Configs/Player")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private float _walkSpeed = 5f;
    public float WalkSpeed => _walkSpeed;


}