using Assets.Scripts.AnimalScriptLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {

    public class StartSimulation : MonoBehaviour
    {
        public List<GameObject> elephantObjects;
        public List<GameObject> miceObject;


        public List<Mouse> mouseList;
        public List<Elephant> elephantList;

        void Start()
        {
            elephantObjects = GridManager.elephants;
            miceObject = GridManager.mice;

            elephantList = new List<Elephant>();
        }

        public void startSimulation()
        {
            foreach (GameObject eleObj in elephantObjects)
            {
                Debug.Log(el.name);
            }

            foreach (GameObject miObj in miceObject)
            {
                Debug.Log(el.name);
            }
        }
    }
}
