using UnityEngine;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine.ParticleSystemJobs;


public struct CubeInput : IInputComponentData
{
    public int Horizontal;
    public int Vertical;
}

[DisallowMultipleComponent]
public class CubeInputAuthoring : MonoBehaviour
{
    class Baking : Baker<CubeInputAuthoring>
    {
        public override void Bake(CubeInputAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<CubeInput>(entity);
        }
    }
}

[UpdateInGroup(typeof(GhostInputSystemGroup))]
public partial struct SampleCubeInput : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        bool left = Input.GetKey("left");
        bool right = Input.GetKey("right");
        bool down = Input.GetKey("down");
        bool up = Input.GetKey("up");

        foreach (var playerInput in SystemAPI.Query<RefRW<CubeInput>>().WithAll<GhostOwnerIsLocal>())
        {
            playerInput.ValueRW = default;
            if (left)
                playerInput.ValueRW.Horizontal -= 1;
            if (right)
                playerInput.ValueRW.Horizontal += 1;
            if (down)
                playerInput.ValueRW.Vertical -= 1;
            if (up)
                playerInput.ValueRW.Vertical += 1;
        }
    }
}
