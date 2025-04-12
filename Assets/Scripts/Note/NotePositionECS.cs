using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

namespace PERM.Player
{
    public partial struct NotePositionSystem : ISystem
    {
        private Timer Timer;

        public void OnCreate(ref SystemState state)
        {
            // 获取 Timer 组件的引用
            Timer = GameSet.Instance.Timer;
        }

        public void OnDestroy(ref SystemState state)
        {
            // 销毁系统时的逻辑
        }

        public void OnUpdate(ref SystemState state)
        {
            float currentTime = Timer.GetElapsedTime();

            // 遍历所有具有 NoteECS 和 LocalTransform 组件的实体
            foreach (var (note, transform) in SystemAPI.Query<RefRW<NoteECS>, RefRW<LocalTransform>>())
            {
                // 计算新的位置
                float newY = note.ValueRO.startFloor + note.ValueRO.speed * (currentTime - note.ValueRO.startTime);

                // 更新位置
                transform.ValueRW.Position = new float3(note.ValueRO.positionX, newY, 0);
            }
        }
    }
}
