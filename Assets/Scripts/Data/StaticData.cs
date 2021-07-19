using UnityEngine;

namespace Data
{
    [CreateAssetMenu]
    public class StaticData : ScriptableObject
    {
        public float playerSpeed;
        public float jumpForce;
        public float rollSpeed;
        public float rollDuration;
        public int damage;
        public float attackDuration;
        public int health;
    }
}