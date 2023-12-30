using DG.Tweening;
using RunTime.Enums;
using RunTime.Handlers;
using RunTime.Signals;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Abstracts.Entities
{
    public abstract class AbsEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private int _id;
        [SerializeField] private int _row;
        [SerializeField] private int _column;
        [SerializeField] private EntitiesEnum _entityType;
        [SerializeField] private TileHandler _currentTile;

        protected Vector2 gridSize;
        protected TileHandler[,] tileHandlersArray;

        private SpriteRenderer _spriteRenderer;
        private Tweener _moveTweener;

        public static List<List<int>> _chosenCandies = new();
        public int Id { get => _id; set => _id = value; }
        public int Row { get => _row; set => _row = value; }
        public int Column { get => _column; set => _column = value; }
        public EntitiesEnum EntityType => _entityType;
        public TileHandler CurrentTile { get => _currentTile; set => _currentTile = value; }
        public Tweener MoveTweener { get => _moveTweener; set => _moveTweener = value; }

        public virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void Start()
        {
            gridSize = GridSignals.Instance.onGetGridSize.Invoke();
            tileHandlersArray = GridSignals.Instance.onGetTileHandlers.Invoke();
        }

        public void SetFeatures(int id, int row, int col)
        {
            Id = id;
            Row = row;
            Column = col;
        }

        public void AdjustStateOfEntity(bool state)
        {
            _spriteRenderer.enabled = state;
        }

        protected virtual void CheckForMatches(int x, int y, List<List<int>> chosenCandies)
        {

            if (y <= gridSize.y - 1 && y >= 0)//check for bounds
            {
                if (x <= gridSize.x - 1 && x >= 0)//check for bounds
                {
                    TileHandler otherTile = tileHandlersArray[x, y];
                    if (otherTile != null)
                    {
                        if (otherTile.IsChecked != true)
                        {
                            otherTile.IsChecked = true;//bir sonraki sekerin degerleri degistiriliyor.

                            chosenCandies.Add(new List<int> { x, y }); // eslesen her sekeri listeye ekleme.
                            if (otherTile.CurrentEntity.EntityType == EntitiesEnum.Bomb || otherTile.CurrentEntity.EntityType == EntitiesEnum.Rocket)
                                otherTile.CurrentEntity.CheckOtherDirections(x, y, chosenCandies);
                        }
                    }
                }
            }
        }

        public void CallToCheck() //Herhangi bir sekilde sekerleri kontrol etmek ve ayni turde olan sekerler uzerinde islem yapmak icin gerekli adimlari bulunduran method
        {
            //Listeyi temizle
            _chosenCandies.Clear();

            CurrentTile.IsChecked = true;//secilen sekeri isaretle


            CheckOtherDirections(Row, Column, _chosenCandies);

            _chosenCandies.Add(new List<int> { Row, Column }); // secilen ilk sekerin koordinatlarýný al

            float time = 0;
            bool areGoalsDone = false;
            bool isMoveCountDone = false;
            if (_chosenCandies.Count > 1)
            {
                areGoalsDone = (bool)UISignals.Instance.onCheckGoals?.Invoke(_chosenCandies);

                isMoveCountDone = (bool)UISignals.Instance.onAdjustMoveCount?.Invoke(-1, areGoalsDone);

                foreach (var item in _chosenCandies)
                {
                    TileHandler currentTile = tileHandlersArray[item[0], item[1]];
                    currentTile.CurrentEntity.AdjustStateOfEntity(false);
                    currentTile.CurrentEntity.transform.SetParent(null);
                    currentTile.CurrentEntity = null;
                    currentTile.SpriteRenderer.color = Color.white;
                }

                CheckLastTileForSpecialObject();

                List<List<int>> tempCandies = new();
                tempCandies.AddRange(_chosenCandies);

                IsCheckedCleaner();

                FallUpperCandies(tempCandies);
                SpawnNewCandies(tempCandies);
                time = .2f;
                GridSignals.Instance.onScanGrid?.Invoke();
            }
            if (!areGoalsDone && !isMoveCountDone)
                InputSignals.Instance.onEnableTouch?.Invoke(time);
        }

        private void CheckLastTileForSpecialObject()
        {
            //if (EntityType == EntitiesEnum.Rocket || EntityType == EntitiesEnum.Bomb) return;

            TileHandler lastTile = tileHandlersArray[_chosenCandies[^1][0], _chosenCandies[^1][1]];

            if (lastTile.IsBomb)
            {
                SpawnSpecificEntity(lastTile, EntitiesEnum.Bomb);
            }
            else if (lastTile.IsRocket)
            {
                SpawnSpecificEntity(lastTile, EntitiesEnum.Rocket);
            }
        }

        private static void SpawnSpecificEntity(TileHandler lastTile, EntitiesEnum entityEnum)
        {
            GridSignals.Instance.onSpawnTheEntity?.Invoke(lastTile, entityEnum);
            _chosenCandies.Remove(_chosenCandies[^1]);
        }

        public List<List<int>> CheckForCombos()
        {
            _chosenCandies.Clear();

            CurrentTile.IsChecked = true;//secilen sekeri isaretle


            CheckOtherDirections(Row, Column, _chosenCandies);

            _chosenCandies.Add(new List<int> { Row, Column }); // secilen ilk sekerin koordinatlarýný al

            return _chosenCandies;
        }

        public abstract void CheckOtherDirections(int x, int y, List<List<int>> chosenCandies);
        protected void FallUpperCandies(List<List<int>> chosenCandies)
        {
            foreach (var candies in chosenCandies)
            {
                int sayac = 0;
                //Patlatýlan sekerin ustune yeni bir seker gelmis fakat candies henuz bu sekere erisememis ise bu sekeri pas gec. Mesela ust uste 2 seker olsun. Alttaki seker 1, ustteki seker 2 olsun. Eger 1. sekere once basildiysa 1. seker ustte bulunan tum sekerleri asagi indirecek. Boylelike hem kendi yeri hem de 2. sekerin yeri dolabilecek. Eger 2. sekerin yeri 1. seker araciligiyla dolmussa 2. seker tekrar ustundeki sekerleri dusurme islemi yapmayacak. "!!! Alttaki if icin gecerlidir. !!!"
                if (tileHandlersArray[candies[0], candies[1]].CurrentEntity == null)
                {
                    for (int i = 0; i < candies[0]; i++)// secilen sekerin ustundeki tum sekerlere tek tek erisim
                    {
                        AbsEntity upperCandy = tileHandlersArray[candies[0] - (i + 1), candies[1]].CurrentEntity;//bir ust sekere erisim.
                        if (upperCandy != null)
                        {
                            if (upperCandy._moveTweener != null && upperCandy._moveTweener.IsPlaying())
                            {
                                upperCandy._moveTweener.Complete();
                            }

                            tileHandlersArray[candies[0] - (i + 1), candies[1]].CurrentEntity = null;

                            tileHandlersArray[upperCandy.Row + 1 + sayac, candies[1]].CurrentEntity = upperCandy;
                            upperCandy.CurrentTile = tileHandlersArray[upperCandy.Row + 1 + sayac, candies[1]];

                            upperCandy.Row = upperCandy.Row + 1 + sayac;
                            upperCandy.transform.SetParent(upperCandy.CurrentTile.transform);


                            upperCandy._moveTweener = upperCandy.transform.DOLocalMove(Vector3.zero, .4f).OnComplete(() => upperCandy._moveTweener = null);
                        }
                        else
                        {
                            sayac++;
                        }
                    }
                }
            }
        }

        protected void SpawnNewCandies(List<List<int>> chosenCandies)
        {
            foreach (var candies in chosenCandies)
            {
                for (int i = 0; i < candies[0] + 1; i++)// secilen sekerin ustundeki tum sekerlere tek tek erisim
                {
                    AbsEntity upperCandy = tileHandlersArray[candies[0] - i, candies[1]].CurrentEntity;//bir ust sekere erisim. !Once kendi yerine bakiyor. 

                    if (upperCandy == null)
                    {
                        GridSignals.Instance.onSpawnNewEntity?.Invoke(tileHandlersArray[candies[0] - i, candies[1]]);
                        break;
                    }
                }
            }
        }
        public void IsCheckedCleaner()
        {
            for (int y = 0; y < gridSize.x; y++)
            {
                for (int x = 0; x < gridSize.y; x++)
                {
                    tileHandlersArray[x, y].IsChecked = false;
                }
            }
        }
    }
}