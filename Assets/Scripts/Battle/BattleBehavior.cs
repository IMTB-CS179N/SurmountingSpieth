using Project.Game;
using Project.Input;

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

        private const float kOrthoDefault = 0.2f;

        private SpriteRenderer m_renderer;
        private BoxCollider2D m_collider;
        private float m_animationSpeed;
        private float m_maximumScale;
        private float m_minimumScale;
        private float m_defaultScale;
        private Vector2 m_unitPosition;
        private Vector2 m_topMostPoint;
        private AnimationType m_animation;
        private IEntity m_entity;
        private bool m_upsizing;
        private int m_index;

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

        public int Index
        {
            get => this.m_index;
            set => this.m_index = value;
        }

        public float AnimationSpeed
        {
            get => this.m_animationSpeed / (ScreenManager.Instance.OrthographicSize * kOrthoDefault);
            set => this.m_animationSpeed = value * (ScreenManager.Instance.OrthographicSize * kOrthoDefault);
        }

        public float MaximumScale
        {
            get => this.m_maximumScale / (ScreenManager.Instance.OrthographicSize * kOrthoDefault);
            set => this.m_maximumScale = value * (ScreenManager.Instance.OrthographicSize * kOrthoDefault);
        }

        public float MinimumScale
        {
            get => this.m_minimumScale / (ScreenManager.Instance.OrthographicSize * kOrthoDefault);
            set => this.m_minimumScale = value * (ScreenManager.Instance.OrthographicSize * kOrthoDefault);
        }

        public float DefaultScale
        {
            get => this.m_defaultScale / (ScreenManager.Instance.OrthographicSize * kOrthoDefault);
            set => this.m_defaultScale = value * (ScreenManager.Instance.OrthographicSize * kOrthoDefault);
        }

        public Vector2 UnitPosition
        {
            get => this.m_unitPosition;
            set => this.m_unitPosition = value;
        }

        public Vector2 TopMostPoint => this.m_topMostPoint;

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

        private void AnimateIdle()
        {
            var transform = this.transform;

            var oldScale = transform.localScale;

            var position = transform.position;

            float newScale;

            if (this.m_upsizing)
            {
                newScale = oldScale.y + this.m_animationSpeed * Time.deltaTime;

                if (newScale >= this.m_maximumScale)
                {
                    newScale = this.m_maximumScale;

                    this.m_upsizing = false;
                }
            }
            else
            {
                newScale = oldScale.y - this.m_animationSpeed * Time.deltaTime;

                if (newScale <= this.m_minimumScale)
                {
                    newScale = this.m_minimumScale;

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

        public void Initialize(string name)
        {
            this.gameObject.name = name;

            this.m_renderer = this.gameObject.GetComponent<SpriteRenderer>();
            this.m_collider = this.gameObject.GetComponent<BoxCollider2D>();

            this.m_renderer.sortingOrder = 5;
            this.m_renderer.sprite = this.Entity?.Sprite;

            this.RecalculateTransform();

            // #TODO effect children
        }

        public void PlayAnimation(AnimationType type)
        {
            this.m_animation = type;

            this.RecalculateTransform();
        }

        public void RecalculateTransform()
        {
            var worldPosition = ScreenManager.Instance.UnitScreenPointToWorldPosition(this.m_unitPosition);
            var localScale = new Vector3(this.m_defaultScale, this.m_defaultScale, 1.0f);
            var boundsSize = this.m_renderer.bounds.size;

            this.transform.position = worldPosition;
            this.transform.localScale = localScale;
            this.m_collider.size = new Vector2(boundsSize.x, boundsSize.y) / (ScreenManager.Instance.OrthographicSize * kOrthoDefault);

            this.m_topMostPoint = ScreenManager.Instance.WorldPositionToUnitScreenPoint(worldPosition + new Vector2(0.0f, this.m_defaultScale));

            this.m_upsizing = false;
        }
    }
}
