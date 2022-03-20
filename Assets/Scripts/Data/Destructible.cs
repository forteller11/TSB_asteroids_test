using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Destructible : IComponentData
    {
        public Mask DestroyedBy;
        public bool BeingDestroyed;
    }
}