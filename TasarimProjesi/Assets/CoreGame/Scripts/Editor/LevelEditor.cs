using RunTime.Datas.UnityObjects;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LevelEditor : EditorWindow
    {
        bool _panelIsOpened = false;

        int _row, _col;
        private Texture[] _cellsTextures;
        private GUIStyle _cellStyle;
        private Vector2 _scrollPosition = Vector2.zero;
        private int _selectedCell;
        private readonly string _separator = new('-', 50);
        private int _selectedLevel;
        private string[] _levelNames;
        private int _levelCount;
        private GUIStyle _seperatorLabelStyle;
        private int _backupSelectedLevel;
        private CD_Level _level;
        private readonly List<Texture> _editorTextures = new();


        [MenuItem("Tools/Level Editor")]
        static void ShowWindow()
        {
            GetWindow<LevelEditor>("Grid Level Editor");
        }

        private void OnGUI()
        {
            if (!_panelIsOpened) // Just once
            {
                _panelIsOpened = true;

                GridSize gridSize = Resources.Load<CD_Grid>("GridDatas/GridSize").GridSize;
                _row = gridSize.row;
                _col = gridSize.column;

                _cellsTextures = new Texture[_row * _col];

                AdjustCell();
                GetLevels();
                AdjustLabelsStyles();

                //Debug.Log($"row: {_row}, col: {_col}");
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition); // Scroll Begin


            _selectedCell = GUILayout.SelectionGrid(-1, _cellsTextures, _col, _cellStyle); // Select any cell

            Seperator();

            #region Select Level From DropDown
            // Created dropdown to select level
            _selectedLevel = EditorGUILayout.Popup(_selectedLevel, _levelNames);
            EditorGUILayout.LabelField("Selected Level: " + _levelNames[_selectedLevel]);

            if (GUILayout.Button("Refresh Levels"))
            {
                GetLevels();
            }
            #endregion

            EditorGUILayout.EndScrollView(); // Scroll End

            #region Pressed Any Cell
            if (_selectedCell != -1) // if pressed any cell
            {
                HandleOnClickedAnyCell();
            }
            #endregion

            #region Selected Any level
            if (_selectedLevel != _backupSelectedLevel) // if selected any level
            {
                _backupSelectedLevel = _selectedLevel;
                //ClearArea();

                if (_selectedLevel > 0)
                {
                    _level = GetLevel();

                    if (_level.LevelEntities.ObjectsList.Count == _row * _col)
                    {
                        //LoadMainCellTexturesFromLevels();
                    }
                    else // calculate level's data's count
                    {
                        int value = (_row * _col) - _level.LevelEntities.ObjectsList.Count;

                        #region if count of level's data is less or more than grid size then fix it
                        if (value > 0) // increase level's data
                        {
                            for (int i = 0; i < value; i++)
                            {
                                _level.LevelEntities.ObjectsList.Add(new());
                            }
                        }
                        else // decrease level's data
                        {
                            for (int i = 0; i < -value; i++)
                            {
                                _level.LevelEntities.ObjectsList.RemoveAt(_level.LevelEntities.ObjectsList.Count - 1);
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }

        private void AdjustCell()
        {
            _cellStyle = new(GUI.skin.button)
            {
                fixedWidth = 30, // Width of buttons
                fixedHeight = 30 // Height of buttons
            };
        }
        private void Seperator()
        {
            EditorGUILayout.LabelField(_separator, _seperatorLabelStyle);
        }
        private void GetLevels()
        {
            _levelCount = Resources.LoadAll<CD_Level>("Levels").Length + 1; // Added "choose level" member

            _levelNames = new string[_levelCount];
            _levelNames[0] = "Choose Level";

            for (int i = 1; i < _levelCount; i++)
            {
                _levelNames[i] = $"Level {i}";
            }
        }
        public void AdjustLabelsStyles()
        {
            _seperatorLabelStyle = new GUIStyle();
            _seperatorLabelStyle.normal.textColor = Color.white;
            _seperatorLabelStyle.fontSize = 18;
            _seperatorLabelStyle.alignment = TextAnchor.MiddleLeft;
            _seperatorLabelStyle.fontStyle = FontStyle.Bold;
        }
        private void HandleOnClickedAnyCell()
        {
            Debug.Log(_selectedCell);
        }
        private CD_Level GetLevel()
        {
            return Resources.Load<CD_Level>($"Levels/Level {_selectedLevel}");
        }
    }
}