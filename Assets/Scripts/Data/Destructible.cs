using Charly.Common.Utils;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Destructible : IComponentData
    {
        public Mask DestroyedByAny;
        public Mask DestroyedByAll;
        public bool BeingDestroyed;

        public readonly bool IsDestroyedBy(Mask destroyer)
        {
            return MaskUtils.HasAllFlags((int)destroyer, (int)DestroyedByAll) ||
                   MaskUtils.ContainsAtLeastOneFlag((int)destroyer, (int)DestroyedByAny);
        }
    }
}