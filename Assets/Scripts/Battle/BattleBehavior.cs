using Project.Game;
using Project.Input;

using Unity.VisualScripting;

using UnityEngine;

namespace Project.Battle
{
    public class BattleBehavior : MonoBehaviour
    {
        public enum AnimationType
        {
            None,
            Idle,
            Damaged,
            Attacking,
        }

        private SpriteRenderer m_renderer;
        private BoxCollider2D m_collider;

        private AnimationType m_animation;

        private IEntity m_entity;
        private bool m_upsizing;

        public IEntity Entity
        {
            get
            {
                return this.m_entity;
            }
            set
            {
                this.m_entity = value;

                if (this.m_renderer != null)
                {
                    this.m_renderer.sprite = value?.Sprite;

                    // #TODO collider scaling
                }
            }
        }

        public int Index;

        public float AnimationSpeed = 0.13f; // 0.2 for player

        public float MaximumScale = 1.00f; // 1.25 for player

        public float MinimumScale = 0.90f; // 1.10 for player

        public float DefaultScale = 1.00f; // same as max scale typically

        public Vector2 UnitPosition; // unit screen scale position

        public Vector2 TopMostPoint; // for text animations

        public bool NoneIsIdleAnimation;

        private void Update()
        {
            switch (this.m_animation)
            {
                case AnimationType.None:
                    if (this.NoneIsIdleAnimation)
                    {
                        this.AnimateIdle();
                    }
                    break;

                case AnimationType.Idle:
                    this.AnimateIdle();
                    break;

                case AnimationType.Damaged:
                    this.AnimateDamaged();
                    break;

                case AnimationType.Attacking:
                    this.AnimateAttacking();
                    break;
            }
        }

        private void ResetScaling()
        {
            this.transform.localScale = new Vector3(this.DefaultScale, this.DefaultScale, 1.0f);

            this.m_upsizing = false;
        }

        private void AnimateIdle()
        {
            var transform = this.transform;

            var oldScale = transform.localScale;

            var position = transform.position;

            float newScale;

            if (this.m_upsizing)
            {
                newScale = oldScale.y + this.AnimationSpeed * Time.deltaTime;

                if (newScale >= this.MaximumScale)
                {
                    newScale = this.MaximumScale;

                    this.m_upsizing = false;
                }
            }
            else
            {
                newScale = oldScale.y - this.AnimationSpeed * Time.deltaTime;

                if (newScale <= this.MinimumScale)
                {
                    newScale = this.MinimumScale;

                    this.m_upsizing = true;
                }
            }

            transform.localScale = new Vector3(oldScale.x, newScale, oldScale.z);

            position.y += newScale - oldScale.y;

            transform.position = position;
        }

        private void AnimateDamaged()
        {

        }

        private void AnimateAttacking()
        {

        }

        public void Initialize(string name, Vector2 position, float scale)
        {
            this.gameObject.name = name;

            this.m_renderer = this.gameObject.GetComponent<SpriteRenderer>();
            this.m_collider = this.gameObject.GetComponent<BoxCollider2D>();

            this.m_renderer.sortingOrder = 5;
            this.m_renderer.sprite = this.Entity?.Sprite;

            this.DefaultScale = scale;
            this.UnitPosition = position;

            var worldPosition = ScreenManager.Instance.UnitScreenPointToWorldPosition(position);
            var localScale = new Vector3(scale, scale, 1.0f);
            var boundsSize = this.m_renderer.bounds.size;

            this.transform.position = worldPosition;
            this.transform.localScale = localScale;

            this.m_collider.size = new Vector2(boundsSize.x, boundsSize.y);

            this.TopMostPoint = ScreenManager.Instance.WorldPositionToUnitScreenPoint(worldPosition + new Vector2(0.0f, scale));

            // #TODO effect children
        }

        public void PlayAnimation(AnimationType type)
        {
            this.m_animation = type;

            this.ResetScaling();
        }
    }
}
