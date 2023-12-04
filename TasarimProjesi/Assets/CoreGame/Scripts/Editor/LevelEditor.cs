using RunTime.Datas.UnityObjects;
using RunTime.Enums;
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
        private int _selectedTexture;
        private CD_TexturesAndModels _texturesAndModels;
        private readonly List<Texture> _objectsTextures = new();
        private readonly List<string> _objectsNames = new();


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
                FixTextures();

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

            LowSeperator();

            _selectedTexture = EditorGUILayout.IntSlider("Selected Object", _selectedTexture, 0, _objectsTextures.ToArray().Length - 1);

            _ = (Texture)EditorGUILayout.ObjectField(_objectsTextures[_selectedTexture] != null ? _objectsNames[_selectedTexture] : "Empty", _objectsTextures[_selectedTexture], typeof(Sprite), false);


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
                ClearArea();

                if (_selectedLevel > 0)
                {
                    _level = GetLevel();

                    if (_level.LevelEntities.ObjectsList.Count == _row * _col)
                    {
                        LoadMainCellTexturesFromLevels();
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

        private void ClearArea()
        {
            for (int i = 0; i < _cellsTextures.Length; i++)
            {
                _cellsTextures[i] = null;
            }
        }
        private void AllClear()
        {
            //ClearArea();
            //_level.Reset();
        }

        private void LoadMainCellTexturesFromLevels()
        {
            for (int i = 0; i < _row * _col; i++)
            {
                if (_level.LevelEntities.ObjectsList[i] != 0)
                {
                    for (int j = 0; j < _texturesAndModels.ObjectDatas.Length; j++)
                    {
                        if (_texturesAndModels.ObjectDatas[j].ObjectType == _level.LevelEntities.ObjectsList[i])
                        {
                            _cellsTextures[i] = _texturesAndModels.ObjectDatas[j].TextureData;
                            break;
                        }
                    }
                }
            }
        }

        private void AdjustCell()
        {
            _cellStyle = new(GUI.skin.button)
            {
                fixedWidth = 40, // Width of buttons
                fixedHeight = 40 // Height of buttons
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
            _cellsTextures[_selectedCell] = _objectsTextures[_selectedTexture];

            if (_selectedLevel <= 0) return; // if selected level zero (choose level)

            if (_objectsTextures[_selectedTexture] != null) // any object selected
            {
                _level.LevelEntities.ObjectsList[_selectedCell] = ((ObjectsEnum[])Enum.GetValues(typeof(ObjectsEnum)))[_selectedTexture];
            }
            else
            {
                _level.LevelEntities.ObjectsList[_selectedCell] = 0;
            }
        }
        private CD_Level GetLevel()
        {
            return Resources.Load<CD_Level>($"Levels/Level {_selectedLevel}");
        }
        private void FixTextures()
        {
            _texturesAndModels = Resources.Load<CD_TexturesAndModels>("TexturesAndModels/TexturesAndModels");

            if (_objectsTextures.Count > 0)
            {
                _objectsTextures.Clear();
            }

            int length = _texturesAndModels.ObjectDatas.Length;

            for (int i = 0; i < length; i++)
            {
                _objectsTextures.Add(_texturesAndModels.ObjectDatas[i].TextureData);
                _objectsNames.Add(_texturesAndModels.ObjectDatas[i].ObjectType.ToString());
            }
        }
    }
}