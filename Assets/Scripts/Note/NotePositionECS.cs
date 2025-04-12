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
            // ��ȡ Timer ���������
            Timer = GameSet.Instance.Timer;
        }

        public void OnDestroy(ref SystemState state)
        {
            // ����ϵͳʱ���߼�
        }

        public void OnUpdate(ref SystemState state)
        {
            float currentTime = Timer.GetElapsedTime();

            // �������о��� NoteECS �� LocalTransform �����ʵ��
            foreach (var (note, transform) in SystemAPI.Query<RefRW<NoteECS>, RefRW<LocalTransform>>())
            {
                // �����µ�λ��
                float newY = note.ValueRO.startFloor + note.ValueRO.speed * (currentTime - note.ValueRO.startTime);

                // ����λ��
                transform.ValueRW.Position = new float3(note.ValueRO.positionX, newY, 0);
            }
        }
    }
}
