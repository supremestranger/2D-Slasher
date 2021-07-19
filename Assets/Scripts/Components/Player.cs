using UnityEngine;

namespace Components
{
    public struct Player
    {
        public Rigidbody2D rigidBody;
        public float speed;
        public Transform playerTranfsorm;
        public float jumpForce;
        public float rollSpeed;
        public float rollDuration;
    }
}