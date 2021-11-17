using Assets.Scripts.AnimalScriptLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.AnimalScriptLogic
{
    class StartSimulation : MonoBehaviour
    {
        public List<GameObject> elephantObjects;
        public List<GameObject> miceObject;


        public List<Mouse> mouseList;
        public List<Elephant> elephantList;

        void Start()
        {
            elephantObjects = GridManager.elephants;
            miceObject = GridManager.mice;  
        }

        public void startSimulation()
        {
            foreach (GameObject eleObj in elephantObjects)
            {
               mouseList.Add(new Mouse(mouseList, elephantList,))
            }

            foreach (GameObject miObj in miceObject)
            {
                Debug.Log(el.name);
            }
        }
    }
}
