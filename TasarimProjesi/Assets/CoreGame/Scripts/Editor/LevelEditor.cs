using RunTime.Datas.UnityObjects;
using System;
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
        private GUIStyle _titleLabelStyle;
        private GUIStyle _lowSeperatorLabelStyle;
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

            Seperator();

            #region Object Features

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Object Features", _titleLabelStyle);

            if (GUILayout.Button("Clear Object Features", new GUIStyle(GUI.skin.button) { fixedWidth = 150, margin = new RectOffset(0, 20, 7, 10) }))
            {
                //ClearObjectFeatures();
            }
            EditorGUILayout.EndHorizontal();

            try
            {
                LowSeperator();

                #region Texture Array
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Get Objects"))
                {
                    //FixTextures(_objectString);
                }
                if (GUILayout.Button("Get Blocks"))
                {
                    //FixTextures(_blockString);
                }
                EditorGUILayout.EndHorizontal();
                #endregion


                //_selectedTexture = EditorGUILayout.IntSlider("Selected Object", _selectedTexture, 0, _editorTextures.ToArray().Length - 1);

                //_ = (Texture)EditorGUILayout.ObjectField(_editorTextures[_selectedTexture] != null ? _editorTextures[_selectedTexture].name : "Empty", _editorTextures[_selectedTexture], typeof(Texture), false);
            }
            catch (Exception)
            {
                //if (isPressedBlocks)
                //{
                //    FixTextures(_blockString);
                //}
                //else
                //{
                //    FixTextures(_objectString);
                //}
                //EditorGUILayout.LabelField("------------------", _middleLabelStyle);
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
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField(_separator, _seperatorLabelStyle);
            EditorGUILayout.Space(10);

        }
        private void LowSeperator()
        {
            EditorGUILayout.LabelField(_separator, _lowSeperatorLabelStyle);
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
            _seperatorLabelStyle.normal.textColor = Color.cyan;
            _seperatorLabelStyle.fontSize = 18;
            _seperatorLabelStyle.alignment = TextAnchor.MiddleLeft;
            _seperatorLabelStyle.fontStyle = FontStyle.Bold;

            _lowSeperatorLabelStyle = new GUIStyle();
            _lowSeperatorLabelStyle.normal.textColor = Color.gray;
            _lowSeperatorLabelStyle.fontSize = 18;
            _lowSeperatorLabelStyle.alignment = TextAnchor.MiddleLeft;

            _titleLabelStyle = new GUIStyle();
            _titleLabelStyle.normal.textColor = Color.white;
            _titleLabelStyle.fontSize = 18;
            _titleLabelStyle.alignment = TextAnchor.MiddleCenter;
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