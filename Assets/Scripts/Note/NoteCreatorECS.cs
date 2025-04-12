using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using System.Collections.Generic;

namespace PERM.Player
{
    public class NoteCreatorECS : MonoBehaviour
    {
        [SerializeField] private LineInfo LineInfo;
        [SerializeField] private EventHandler EventHandler;
        [SerializeField] private float LoadRange = 0.5f;
        [SerializeField] private Mesh noteMesh;
        [SerializeField] private Material noteMaterial;

        private EntityManager entityManager;
        private int poolIndex = 0;
        private int LeftIndex = 0;
        private int RightIndex = 0;

        public List<Entity> entityPool = new List<Entity>();

        private void Start()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            // ��ʼ��ʵ���
            EntityArchetype noteArchetype = entityManager.CreateArchetype(
                typeof(NoteECS),
                typeof(LocalTransform),
                typeof(LocalToWorld)
            );
            InitializeEntityPool(100, noteArchetype); // ����ش�СΪ 100
        }

        private void Update()
        {
            float currentFloor = EventHandler.LineFloor;
            float currentTime = EventHandler.LineTime;
            float maxFloor = currentFloor + LoadRange;

            // ���±�����
            while (currentFloor > LineInfo.notes[LeftIndex].startFloor)
            {
                LeftIndex++;
                if (currentFloor <= LineInfo.notes[LeftIndex].startFloor)
                {
                    break;
                }
            }
            while (currentFloor <= LineInfo.notes[LeftIndex].startFloor)
            {
                if (LeftIndex > 0 && currentFloor > LineInfo.notes[LeftIndex - 1].startFloor)
                {
                    break;
                }
                LeftIndex--;
            }

            // ���±�����
            while (maxFloor > LineInfo.notes[RightIndex].startFloor)
            {
                if (RightIndex + 1 < LineInfo.notes.Count && maxFloor <= LineInfo.notes[RightIndex + 1].startFloor)
                {
                    break;
                }
                RightIndex++;
            }
            while (maxFloor <= LineInfo.notes[RightIndex].startFloor)
            {
                if (RightIndex > 0 && maxFloor > LineInfo.notes[RightIndex - 1].startFloor)
                {
                    break;
                }
                RightIndex--;
            }

            // ������Χ�ڵ� Note ������ ECS ʵ��
            for (int i = LeftIndex; i <= RightIndex; i++)
            {
                var note = LineInfo.notes[i];
                if (note.endTime >= currentTime && !note.isCreated)
                {
                    CreateNoteEntity(note);
                    note.isCreated = true; // ���ñ�־λ
                }
            }
        }

        private void CreateNoteEntity(Note note)
        {
            // ��ʵ����л�ȡʵ��
            Entity noteEntity = GetEntityFromPool();

            // ���� NoteECS �������
            entityManager.SetComponentData(noteEntity, new NoteECS
            {
                id = note.id,
                direction = note.direction,
                type = note.type,
                startTime = note.startTime,
                endTime = note.endTime,
                positionX = note.positionX,
                speed = note.speed,
                startFloor = note.startFloor,
                endFloor = note.endFloor
            });

            // ����ʵ��ĳ�ʼλ��
            entityManager.SetComponentData(noteEntity, new LocalTransform
            {
                Position = new float3(note.positionX, note.startFloor, 0),
                Rotation = quaternion.identity,
                Scale = 1f
            });

            // ��� RenderMeshUnmanaged ���
            entityManager.AddComponentData(noteEntity, new RenderMeshUnmanaged
            {
                mesh = noteMesh,
                materialForSubMesh = noteMaterial
            });
        }

        private void InitializeEntityPool(int poolSize, EntityArchetype archetype)
        {
            for (int i = 0; i < poolSize; i++)
            {
                Entity entity = entityManager.CreateEntity(archetype);
                entityManager.SetEnabled(entity, false); // ����ʵ��
                entityPool.Add(entity);
            }
        }

        private Entity GetEntityFromPool()
        {
            if (poolIndex >= entityPool.Count)
            {
                // ��̬��չʵ���
                ExpandEntityPool(50); // ÿ����չ 50 ��ʵ��
            }

            Entity entity = entityPool[poolIndex];
            entityManager.SetEnabled(entity, true); // ����ʵ��
            poolIndex++;
            return entity;
        }

        private void ExpandEntityPool(int additionalSize)
        {
            EntityArchetype noteArchetype = entityManager.CreateArchetype(
                typeof(NoteECS),
                typeof(LocalTransform),
                typeof(LocalToWorld)
            );

            for (int i = 0; i < additionalSize; i++)
            {
                Entity entity = entityManager.CreateEntity(noteArchetype);
                entityManager.SetEnabled(entity, false); // ����ʵ��
                entityPool.Add(entity);
            }
        }

        private void RecycleEntity(Entity entity)
        {
            entityManager.SetEnabled(entity, false); // ����ʵ��
        }
    }
}