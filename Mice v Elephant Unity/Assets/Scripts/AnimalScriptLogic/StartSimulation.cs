using Assets.Scripts.AnimalScriptLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.AnimalScriptLogic
{
    class StartSimulation : MonoBehaviour
    {
        public List<GameObject> elephantObjects;
        public List<GameObject> miceObject;

        public object _objLock = new object();


        public List<Mouse> mouseList = new List<Mouse>();
        public List<Elephant> elephantList=  new List<Elephant>();

        public int strikeDistance;

        public int numberOElephants;
        public int numberOfMouse;

        public int rowBound;
        public int coloumBound;

        public static SynchronizationContext syncContext;

        public GameObject startButton;

        public GameObject scatterButton;
        void Start()
        {
            startButton = GameObject.Find("StartSimulation");
            scatterButton = GameObject.Find("PlayButton");

            numberOElephants = InputInGameData.numberOfElephants;
            numberOfMouse = InputInGameData.numberOfMice;

            strikeDistance = InputInGameData.strikeDistance;

            syncContext = SynchronizationContext.Current;
        }

        public void startSimulation()
        {
            Debug.Log(GridManager.elephants.Count);
            Debug.Log(GridManager.mice.Count);

            startButton.SetActive(false);
            scatterButton.SetActive(false);

            Monitor.Enter(_objLock);
            {

                foreach (GameObject eleObj in GridManager.elephants)
                {

                    Vector3 pos = eleObj.transform.position;

                    int x = (int)pos.x;
                    int y = -(int)pos.y;

                    elephantList.Add(new Elephant(mouseList, elephantList, _objLock, numberOElephants, rowBound, coloumBound, strikeDistance, new Point(x, y)));
                    //Debug.Log(elephantList.Count + " lds;lfk;sdlfk");
                }

                foreach (GameObject miObj in GridManager.mice)
                {
                    Vector3 pos = miObj.transform.position;

                    int x = (int)pos.x;
                    int y = -(int)pos.y;

                    mouseList.Add(new Mouse(mouseList, elephantList, _objLock, numberOElephants, rowBound, coloumBound, strikeDistance, new Point(x, y)));
                    //Debug.Log(mouseList.Count + "moomomom");
                    /* if (x >= GridManager.gameObjRef.GetLength(0) - 1)
                     {
                         x = GridManager.gameObjRef.GetLength(0) - 1;
                     }

                     if (y >= GridManager.gameObjRef.GetLength(1) - 1)
                     {
                         y = GridManager.gameObjRef.GetLength(1) - 1;
                     }

     *//*
                     Vector3 fe = (GridManager.gameObjRef[x,y]);
                     eleObj.transform.position = new Vector3(fe.x, fe.y, fe.z);

                     Vector3 po1s = eleObj.transform.position;
                     Debug.Log((int)po1s.x + " : " + -(int)po1s.y);*/
                }

             
            }
            Monitor.Exit(_objLock);
            Debug.Log(mouseList.Count + elephantList.Count + " " +_objLock);
        }
    }
}
