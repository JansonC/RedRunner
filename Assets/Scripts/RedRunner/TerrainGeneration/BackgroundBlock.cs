using UnityEngine;

namespace RedRunner.TerrainGeneration
{
    public class BackgroundBlock : Block
    {
        [SerializeField] protected float m_MinWidth = 1f;
        [SerializeField] protected float m_MaxWidth = 10f;

        public virtual float MinWidth => m_MinWidth;

        public virtual float MaxWidth => m_MaxWidth;

        public override float Width
        {
            get => base.Width;
            set => m_Width = value;
        }

        protected virtual void Start()
        {
        }
    }
}
