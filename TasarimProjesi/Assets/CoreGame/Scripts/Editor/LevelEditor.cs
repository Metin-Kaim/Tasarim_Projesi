using log4net.Core;
using RunTime.Datas.UnityObjects;
using RunTime.Datas.ValueObjects;
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
        private GUIStyle _sidetitleLabelStyle;
        private GUIStyle _lowSeperatorLabelStyle;
        private int _selectedTexture;
        private CD_TexturesAndModels _texturesAndModels;
        private readonly List<Texture> _objectsTextures = new();
        private readonly List<string> _objectsNames = new();
        private readonly List<EntitiesEnum> _objectsTypes = new();
        private bool _isStatic;
        private bool _candy1;
        private bool _candy2;
        private bool _candy3;
        private bool _candy4;
        private List<LevelGoals> _goalsList;


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
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition); // Scroll Begin


            _selectedCell = GUILayout.SelectionGrid(-1, _cellsTextures, _col, _cellStyle); // Select any cell

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear Grid"))
            {
                AllClear();
            }
            if (GUILayout.Button("Fill Grid"))
            {
                FillGrid();
            }
            EditorGUILayout.EndHorizontal();

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

            #region Level Features

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Level Features", _titleLabelStyle);

            if (GUILayout.Button("Clear Level Features", new GUIStyle(GUI.skin.button) { fixedWidth = 150, margin = new RectOffset(0, 20, 7, 10) }))
            {
                ClearLevelFeatures();
            }
            EditorGUILayout.EndHorizontal();

            LowSeperator();

            EditorGUILayout.BeginVertical();
            EditorGUI.BeginChangeCheck();
            _candy1 = GUILayout.Toggle(_candy1, "Candy 1");
            _candy2 = GUILayout.Toggle(_candy2, "Candy 2");
            _candy3 = GUILayout.Toggle(_candy3, "Candy 3");
            _candy4 = GUILayout.Toggle(_candy4, "Candy 4");
            if (EditorGUI.EndChangeCheck())
            {
                if (_selectedLevel > 0)
                {
                    _level.LevelFeatures.AllowedCandies.SetAllows(_candy1, _candy2, _candy3, _candy4);
                }
            }
            EditorGUILayout.EndVertical();

            LowSeperator();
            EditorGUILayout.LabelField("-> Goals", _sidetitleLabelStyle);

            if (_goalsList != null)
            {

                for (int i = 0; i < _goalsList.Count; i++)
                {
                    _goalsList[i].entityType = (EntitiesEnum)EditorGUILayout.EnumPopup("Obj Tipi", _goalsList[i].entityType);
                    _goalsList[i].entityCount = EditorGUILayout.IntSlider("Obj Sayısı", _goalsList[i].entityCount, 1, _row * _col);

                    if (GUILayout.Button("Sil", _cellStyle))
                    {
                        _goalsList.RemoveAt(i);
                        GUI.FocusControl(null); // Düzenleyici penceresinin düzgün çalışması için odaklanmayı sıfırla
                    }
                }

                // Yeni bir hedef ekleme butonu
                if (GUILayout.Button("Yeni Hedef Ekle"))
                {
                    _goalsList.Add(new LevelGoals());
                }

                // Hedefleri kaydetme butonu
                if (GUILayout.Button("Hedefleri Kaydet"))
                {
                    Kaydet();
                }
            }

            #endregion

            Seperator();

            #region Object Features

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Object Features", _titleLabelStyle);

            if (GUILayout.Button("Clear Object Features", new GUIStyle(GUI.skin.button) { fixedWidth = 150, margin = new RectOffset(0, 20, 7, 10) }))
            {
                ClearObjectFeatures();
            }
            EditorGUILayout.EndHorizontal();

            LowSeperator();

            try
            {
                _selectedTexture = EditorGUILayout.IntSlider("Selected Object", _selectedTexture, 0, _objectsTextures.ToArray().Length - 1);

                _ = (Texture)EditorGUILayout.ObjectField(_objectsTextures[_selectedTexture] != null ? _objectsNames[_selectedTexture] : "Empty", _objectsTextures[_selectedTexture], typeof(Sprite), false);

                _isStatic = EditorGUILayout.ToggleLeft(_isStatic ? "  Static" : "  UnStatic", _isStatic);

            }
            catch (Exception)
            {
                FixTextures();
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
                ClearArea();
                _goalsList = new();

                if (_selectedLevel > 0)
                {
                    _level = GetLevel();

                    if (_level.LevelFeatures.EntitiesList.Count == _row * _col)
                    {
                        LoadMainCellTexturesFromLevels();
                        LoadLevelFeaturesFromLevels();
                    }
                    else // calculate level's data's count
                    {
                        int value = (_row * _col) - _level.LevelFeatures.EntitiesList.Count;

                        #region if count of level's data is less or more than grid size then fix it
                        if (value > 0) // increase level's data
                        {
                            for (int i = 0; i < value; i++)
                            {
                                _level.LevelFeatures.EntitiesList.Add(new());
                            }
                        }
                        else // decrease level's data
                        {
                            for (int i = 0; i < -value; i++)
                            {
                                _level.LevelFeatures.EntitiesList.RemoveAt(_level.LevelFeatures.EntitiesList.Count - 1);
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }

        private void LoadLevelFeaturesFromLevels()
        {
            AllowedCandies allowedCandies = _level.LevelFeatures.AllowedCandies;
            _candy1 = allowedCandies.Candy1;
            _candy2 = allowedCandies.Candy2;
            _candy3 = allowedCandies.Candy3;
            _candy4 = allowedCandies.Candy4;

            //_goalsList.Clear();
            _goalsList.AddRange(_level.LevelFeatures.LevelGoals);

        }

        private void Kaydet()
        {
            if (_selectedLevel <= 0) return;

            _level.LevelFeatures.LevelGoals?.Clear();
            _level.LevelFeatures.LevelGoals.AddRange(_goalsList);
        }
        private void ClearLevelFeatures()
        {
            ClearAllows();
            _level.LevelFeatures.AllowedCandies.ClearAllows();
            _goalsList.Clear();
            Kaydet();
        }

        private void FillGrid()
        {
            for (int i = 0; i < _cellsTextures.Length; i++)
            {
                if (_cellsTextures[i] != null) continue;

                _cellsTextures[i] = _objectsTextures[1];

                if (_selectedLevel <= 0) continue; // if selected level zero (choose level)

                _level.LevelFeatures.EntitiesList[i].SetFeatures(_objectsTypes[1], false);
            }
        }

        private void ClearObjectFeatures()
        {
            _selectedTexture = 0;
            _isStatic = false;
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
            ClearArea();
            _level?.Reset();
        }

        private void LoadMainCellTexturesFromLevels()
        {
            for (int i = 0; i < _row * _col; i++)
            {
                if (_level.LevelFeatures.EntitiesList[i].EntityType != 0)
                {
                    for (int j = 0; j < _texturesAndModels.ObjectDatas.Length; j++)
                    {
                        if (_texturesAndModels.ObjectDatas[j].EntityType == _level.LevelFeatures.EntitiesList[i].EntityType)
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

            _sidetitleLabelStyle = new GUIStyle();
            _sidetitleLabelStyle.normal.textColor = Color.white;
            _sidetitleLabelStyle.fontSize = 15;
            _sidetitleLabelStyle.alignment = TextAnchor.MiddleLeft;
        }
        private void HandleOnClickedAnyCell()
        {
            _cellsTextures[_selectedCell] = _objectsTextures[_selectedTexture];

            if (_selectedLevel <= 0) return; // if selected level zero (choose level)

            if (_objectsTextures[_selectedTexture] != null) // any object selected
            {
                _level.LevelFeatures.EntitiesList[_selectedCell].SetFeatures(_objectsTypes[_selectedTexture], _isStatic);
            }
            else
            {
                _level.LevelFeatures.EntitiesList[_selectedCell].Reset();

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
                _objectsTypes.Clear();
                _objectsNames.Clear();
            }

            int length = _texturesAndModels.ObjectDatas.Length;

            for (int i = 0; i < length; i++)
            {
                _objectsTextures.Add(_texturesAndModels.ObjectDatas[i].TextureData);
                _objectsNames.Add(_texturesAndModels.ObjectDatas[i].EntityType.ToString());
                _objectsTypes.Add(_texturesAndModels.ObjectDatas[i].EntityType);

            }
        }
        private void ClearAllows()
        {
            _candy1 = false;
            _candy2 = false;
            _candy3 = false;
            _candy4 = false;
        }
    }
}
