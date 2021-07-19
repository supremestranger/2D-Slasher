using Leopotam.EcsLite;
using UnityEngine;

namespace Views
{
    public class EnemyView : MonoBehaviour
    {
        public EcsPackedEntity entity;
        public Animator animator;
        public int damage;
        public float attackDuration;
        public Transform attackPoint;
        public int health;
    }
}