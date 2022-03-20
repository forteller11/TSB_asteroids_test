using Charly.Data;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Charly.Authoring
{
    [DisallowMultipleComponent]
    public class RandomStateAuthoring: MonoBehaviour, IConvertGameObjectToEntity
    {
        public bool OverrideInitialSeed = false;
        public int CustomSeed;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new RandomState(GetStartingSeed()));
        }

        public int GetStartingSeed()
        {
            return OverrideInitialSeed ? 
                CustomSeed : 
                GetInstanceID();
        }

        private void OnValidate()
        {
            if (CustomSeed == 0)
                CustomSeed = GetInstanceID();
        }
    }

    [CustomEditor(typeof(RandomStateAuthoring))]
    public class RandomStateAuthoringEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var authoring = (RandomStateAuthoring) target;

            GUIContent overrideInitialSeedContent = new GUIContent(
                "Override Initial Seed",
                "Otherwise the initial seed will be equal to the objects InstanceID");
            authoring.OverrideInitialSeed = EditorGUILayout.Toggle(overrideInitialSeedContent, authoring.OverrideInitialSeed);
            
            if (authoring.OverrideInitialSeed)
            {
                authoring.CustomSeed = EditorGUILayout.IntField("Custom Seed",(int)authoring.CustomSeed);
            }
        }
    }
}