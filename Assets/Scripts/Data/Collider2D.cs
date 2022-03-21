using System;
using Unity.Entities;
using UnityEngine;

namespace Charly.Data
{
    public struct Collider2D : IComponentData
    {
        public ColliderType Type;
        public float Radius;

        public float GetApproximateRadius()
        {
            switch (Type)
            {
                case ColliderType.Inactive:
                    return 0;
                case ColliderType.Circle:
                    return Radius;
                default:
                    // Debug.LogError($"Argument Out of range at {nameof(Collider2D)} with {nameof(Type)}: {Type}"); //cannot debug.log with burst compile enabled?
                    throw new ArgumentOutOfRangeException();
                    return 0;
            }
        }
    }
    public enum ColliderType
    {
        Inactive=default,
        Circle,
    }
    
}