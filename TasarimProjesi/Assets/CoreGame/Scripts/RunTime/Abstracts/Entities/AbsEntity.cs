using RunTime.Enums;
using UnityEngine;

namespace RunTime.Abstracts.Entities
{
    public abstract class AbsEntity : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private int _row;
        [SerializeField] private int _column;
        [SerializeField] private EntitiesEnum _objectType;

        public int Id { get => _id; set => _id = value; }
        public int Row { get => _row; set => _row = value; }
        public int Column { get => _column; set => _column = value; }
        public EntitiesEnum ObjectType => _objectType;

        public void SetFeatures(int id, int row, int column)
        {
            Id = id;
            Row = row;
            Column = column;
        }

        private void OnMouseDown()
        {
            OnTouch();
        }

        protected abstract void OnTouch();
    }
}