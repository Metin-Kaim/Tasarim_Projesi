using RunTime.Abstracts.Entities;
using RunTime.Signals;
using UnityEngine;

namespace RunTime.Handlers
{
    public class TileHandler : MonoBehaviour, IEntity
    {
        [SerializeField] private int _id;
        [SerializeField] private int _row;
        [SerializeField] private int _column;

        public bool IsChecked;
        public AbsEntity CurrentEntity;

        SpriteRenderer _spriteRenderer;

        public bool IsBomb { get; set; }
        public bool IsRocket { get; set; }
        public int Id { get => _id; set => _id = value; }
        public int Row { get => _row; set => _row = value; }
        public int Column { get => _column; set => _column = value; }
        public SpriteRenderer SpriteRenderer { get => _spriteRenderer; set => _spriteRenderer = value; }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetFeatures(int id, int row, int col)
        {
            Id = id;
            Row = row;
            Column = col;
        }

        protected virtual void OnMouseDown()
        {
            if (InputSignals.Instance.onGetStateOfTouch.Invoke())
            {
                InputSignals.Instance.onDisableTouch?.Invoke();
                CurrentEntity.CallToCheck();
            }
        }
    }
}