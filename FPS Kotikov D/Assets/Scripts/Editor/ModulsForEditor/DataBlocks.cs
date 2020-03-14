using UnityEditor;
using UnityEngine;


namespace FPS_Kotikov_D.Editor
{
    public class DataBlocks
    {


        #region Fields

        private Object _objectForPlace;
        private int _howEachObjects;
        private string _baseNameForObjects;

        #endregion


        #region Properties

        public Object ObjectForPlace
        {
            get { return _objectForPlace; }
            set { _objectForPlace = value; }
        }

        public Transform ObjectPosition
        {
            get
            {
                var obj = (GameObject)_objectForPlace;
                return obj.transform;
            }

        }


        public int HowEachObjects
        {
            get { return _howEachObjects; }
            set { _howEachObjects = value; }
        }

        public string BaseNameForObjects
        {
            get { return _baseNameForObjects; }
            set { _baseNameForObjects = value; }
        }

        #endregion


        #region Metodths

        public void CreateBlock()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label($"Object {BaseNameForObjects}", EditorStyles.boldLabel);
            _objectForPlace = EditorGUILayout.ObjectField("Add object", _objectForPlace, typeof(Object), true);
            _baseNameForObjects = EditorGUILayout.TextField("Object name", BaseNameForObjects);
            _howEachObjects = EditorGUILayout.IntField("How curent objects?", HowEachObjects);
            GUILayout.EndVertical();
        }

        #endregion


    }
}