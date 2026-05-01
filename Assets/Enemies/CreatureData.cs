using UnityEngine;

[CreateAssetMenu(fileName = "NuevaCriatura", menuName = "New Creature")]
public class CreatureData : ScriptableObject
{
    public int maxHealth;
    public int damage;
    public float speed;
    public string enemyType;
}
