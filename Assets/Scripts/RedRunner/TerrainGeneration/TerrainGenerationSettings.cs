using UnityEngine;

namespace RedRunner.TerrainGeneration
{
    [CreateAssetMenu(menuName = "Create Terrain Generator Settings")]
    public class TerrainGenerationSettings : ScriptableObject
    {
        [SerializeField] protected float m_LevelLength = 200f;
        [SerializeField] protected int m_StartBlocksCount = 1;
        [SerializeField] protected int m_MiddleBlocksCount = -1;
        [SerializeField] protected int m_EndBlocksCount = 1;
        [SerializeField] protected Block[] m_StartBlocks;
        [SerializeField] protected Block[] m_MiddleBlocks;
        [SerializeField] protected Block[] m_EndBlocks;
        [SerializeField] protected BackgroundLayer[] m_BackgroundLayers;

        public float LevelLength => m_LevelLength;

        public int StartBlocksCount => m_StartBlocksCount;

        public int MiddleBlocksCount => m_MiddleBlocksCount;

        public int EndBlocksCount => m_EndBlocksCount;

        public Block[] StartBlocks => m_StartBlocks;

        public Block[] MiddleBlocks => m_MiddleBlocks;

        public Block[] EndBlocks => m_EndBlocks;

        public BackgroundLayer[] BackgroundLayers => m_BackgroundLayers;
    }
}
