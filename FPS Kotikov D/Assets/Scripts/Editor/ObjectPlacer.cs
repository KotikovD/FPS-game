using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace FPS_Kotikov_D.Editor
{
    public class ObjectPlacer : EditorWindow
    {


        #region Fields

        const int MINVALUE = 1;
        const int MAXVALUE = 100;
        const int MAXOBJECTS = 10;

        private static int _howTotalObjects = 1;
        private int _minDistance = 5;
        private int _maxDistance = 50;
        private List<DataBlocks> _dataBlocks = new List<DataBlocks>();
        private List<GameObject> _objects = new List<GameObject>();
        private List<bool> _objectsGroups = new List<bool>();

        #endregion


        #region Window&GUIMetodths

        [MenuItem("MyEditorScripts/Object Placer")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ObjectPlacer));
        }

        private void OnGUI()
        {

            GUILayout.Space(20);
            _howTotalObjects = EditorGUILayout.IntSlider("How total objects?", _howTotalObjects, MINVALUE, MAXOBJECTS);
            _minDistance = EditorGUILayout.IntSlider("Min Distance", _minDistance, MINVALUE, MAXVALUE);
            _maxDistance = EditorGUILayout.IntSlider("Max Distance", _maxDistance, MINVALUE, MAXVALUE);
            GUILayout.Space(20);

            for (int i = 0; i < _howTotalObjects; i++)
            {
                _dataBlocks.Add(new DataBlocks());
                _dataBlocks[i].CreateBlock();
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Create objects"))
                CreateObjects();

            GUILayout.Space(10);
            if (GUILayout.Button("Remove all objects"))
                RemoveAllObjects();

            GUILayout.Space(20);
            for (int z = 0; z < _howTotalObjects; z++)
            {
                if (_objectsGroups.Count < _howTotalObjects)
                    _objectsGroups.Add(false);

                var name = _dataBlocks[z].BaseNameForObjects == null ? "Empty object name" : _dataBlocks[z].BaseNameForObjects;
                _objectsGroups[z] = EditorGUILayout.Toggle($"{name}", _objectsGroups[z]);
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Remove selected groups objects"))
                RemoveGroup();

        }

        #endregion


        #region Metodths


        private void CreateObjects()
        {
            for (int a = 0; a < _howTotalObjects; a++)
            {
                var block = _dataBlocks[a];
                for (int i = 0; i < block.HowEachObjects;)
                {
                    var dis = Random.Range(_minDistance, _maxDistance);
                    var randomPoint = Random.insideUnitSphere * dis;

                    if (NavMesh.SamplePosition(randomPoint, out var hit, dis, NavMesh.AllAreas))
                    {
                        var obj = Instantiate(block.ObjectForPlace, hit.position, Quaternion.identity) as GameObject;
                        obj.name = $"{block.BaseNameForObjects} {i}";
                        _objects.Add(obj);
                        i++;
                    }
                }
            }
        }

        private void RemoveAllObjects()
        {
            foreach (GameObject obj in _objects)
            {
                DestroyImmediate(obj);
            }
        }

        private void RemoveGroup()
        {
            for (int i = 0; i < _howTotalObjects; i++)
            {
                if (_objectsGroups[i] == true)
                    for (int a = 0; a < _dataBlocks[i].HowEachObjects; a++)
                    {
                        DestroyImmediate(GameObject.Find($"{_dataBlocks[i].BaseNameForObjects} {a}"));
                    }
            }
        }

        #endregion


    }
}


