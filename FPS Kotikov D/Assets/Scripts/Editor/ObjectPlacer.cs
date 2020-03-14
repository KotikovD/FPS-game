using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace FPS_Kotikov_D.Editor
{

    public class ObjectPlacer : EditorWindow
    {


        #region Fields

        private int _howTotalObjects;
        List<DataBlocks> _dataBlocks = new List<DataBlocks>();
        List<GameObject> _objects = new List<GameObject>();
        private int _minDistance = 5;
        private int _maxDistance = 50;

        #endregion


        #region Window&GUIMetodths

        [MenuItem("MyEditorScripts/Object Placer")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ObjectPlacer));
        }

        void OnGUI()
        {
            GUILayout.Space(20);
            _howTotalObjects = EditorGUILayout.IntSlider("How total objects?",_howTotalObjects, 1, 10);
            _minDistance = EditorGUILayout.IntSlider("Min Distance", _minDistance, 1, 100);
            _maxDistance = EditorGUILayout.IntSlider("Max Distance", _maxDistance, 1, 100);
            GUILayout.Space(20);


            for (int i = 0; i < _howTotalObjects; i++)
            {
                _dataBlocks.Add(new DataBlocks());
                _dataBlocks[i].CreateBlock();
            }


            GUILayout.Space(10);
            if (GUILayout.Button("Create objects"))
            {
                foreach (DataBlocks block in _dataBlocks)
                {
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



            if (GUILayout.Button("Remove objects"))
            {
                foreach (GameObject go in _objects)
                {
                    DestroyImmediate(go);
                }
            }

        }

        #endregion


    }
}


