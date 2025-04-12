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

            // 初始化实体池
            EntityArchetype noteArchetype = entityManager.CreateArchetype(
                typeof(NoteECS),
                typeof(LocalTransform),
                typeof(LocalToWorld)
            );
            InitializeEntityPool(100, noteArchetype); // 假设池大小为 100
        }

        private void Update()
        {
            float currentFloor = EventHandler.LineFloor;
            float currentTime = EventHandler.LineTime;
            float maxFloor = currentFloor + LoadRange;

            // 左下标搜索
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

            // 右下标搜索
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

            // 遍历范围内的 Note 并生成 ECS 实体
            for (int i = LeftIndex; i <= RightIndex; i++)
            {
                var note = LineInfo.notes[i];
                if (note.endTime >= currentTime && !note.isCreated)
                {
                    CreateNoteEntity(note);
                    note.isCreated = true; // 设置标志位
                }
            }
        }

        private void CreateNoteEntity(Note note)
        {
            // 从实体池中获取实体
            Entity noteEntity = GetEntityFromPool();

            // 设置 NoteECS 组件数据
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

            // 设置实体的初始位置
            entityManager.SetComponentData(noteEntity, new LocalTransform
            {
                Position = new float3(note.positionX, note.startFloor, 0),
                Rotation = quaternion.identity,
                Scale = 1f
            });

            // 添加 RenderMeshUnmanaged 组件
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
                entityManager.SetEnabled(entity, false); // 禁用实体
                entityPool.Add(entity);
            }
        }

        private Entity GetEntityFromPool()
        {
            if (poolIndex >= entityPool.Count)
            {
                // 动态扩展实体池
                ExpandEntityPool(50); // 每次扩展 50 个实体
            }

            Entity entity = entityPool[poolIndex];
            entityManager.SetEnabled(entity, true); // 启用实体
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
                entityManager.SetEnabled(entity, false); // 禁用实体
                entityPool.Add(entity);
            }
        }

        private void RecycleEntity(Entity entity)
        {
            entityManager.SetEnabled(entity, false); // 禁用实体
        }
    }
}