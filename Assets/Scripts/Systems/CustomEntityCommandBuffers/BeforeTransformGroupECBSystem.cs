using Unity.Entities;
using Unity.Transforms;

namespace Charly.Systems.CustomEntityCommandBuffers
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class BeforeTransformGroupECBSystem : EntityCommandBufferSystem
    {
        
    }
}