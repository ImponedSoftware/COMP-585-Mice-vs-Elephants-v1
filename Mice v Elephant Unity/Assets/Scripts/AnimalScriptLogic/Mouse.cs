using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AnimalScriptLogic
{
    class Mouse : Animal
    {
        public Mouse(List<Mouse> mouseList, List<Elephant> elephantList, object _objLock, int elephantsAvailable, int RowBound, int ColoumBound, int StrikeDistance, Point point) : base(mouseList, elephantList, _objLock, elephantsAvailable, RowBound, ColoumBound, StrikeDistance, point)
        {
      
        }


        protected override void Run()
        {
           // Debug.Log("WERE fuckign runngin inside asdasdasd");
            while ((Interlocked.CompareExchange(ref elephantsAvailable, 0, 0)) > 0)
            {
                Barrier();
                Debug.Log(elephantsAvailable + " " + ((Interlocked.CompareExchange(ref elephantsAvailable, 0, 0)) <= 0));
                if ((Interlocked.CompareExchange(ref elephantsAvailable, 0, 0)) <= 0)
                {
                    Debug.Log("MOUSE IS BREAKING");

                    int index = mouseList.IndexOf(this);

                    StartSimulation.syncContext.Post(_ =>
                    {
                        //Debug.Log(elephantList.IndexOf(this));//
                        GridManager.DestoryMice(index);
                    }, null);

                    break;
                }

                MoveAround();
                //print or set the mouse sprite at the spefic location on the graph.
                Thread.Sleep(1000);
                //Debug.Log(this.point + " MMOO");
                SyncCurrentPosToScenePos(this);
            }
            // This is where mouse ternminates.
            int miceindex = mouseList.IndexOf(this);
            StartSimulation.syncContext.Post(_ =>
            {
                //Debug.Log(elephantList.IndexOf(this));//
                GridManager.DestoryMice(miceindex);
            }, null);
        }
  


        protected override void MoveAround()
        {
            Elephant closestElphant = getClosestElephant();
            bool elephantInStrikingDistance = false;
            int closestElephantDisatnce;


           // Debug.Log("closestElphant: " + closestElphant.point);
            if (closestElphant.point.X == -1 && closestElphant.point.Y == -1)
            {
               // Debug.Log("MOVE AWAY FROM DEAD ELEPANT");
                point = moveInRandomAdijantSquare();
                return;
            } else
            {
                if (closestElphant != null)
                {
                    closestElephantDisatnce = getDistanceBetweenTwoPoints(this.point, closestElphant.point);
                    elephantInStrikingDistance = closestElephantDisatnce <= StrikeDistance;

                }

                bool mouseInStrikingDistance = (checkTheNearestMouse() <= (StrikeDistance * 2));


               // Debug.Log(mouseInStrikingDistance + " " + elephantInStrikingDistance);
                if (elephantInStrikingDistance == false)
                    point = moveInRandomAdijantSquare();
                else if (mouseInStrikingDistance == false && elephantInStrikingDistance == true)
                    return; // If thier is no mouse to back the current mouse, then the mouse frezzes. His scared!!!
                else
                {
                    if (iAmOnAnElepantGridSpace())
                        return;
                    point = moveCloserToObject(point, closestElphant.point);
                }

                this.CheckBounds();

                // Debug.Log("MOUSE OMG");
            }
        }

        private Elephant getClosestElephant()
        {
            Elephant holdClosestElephantObject = null;
            int holdNearestDistance = int.MaxValue;
            int getDistance = int.MaxValue;

            lock (elephantList) // check if list is thread safe.
            {
                foreach(Elephant elephant in elephantList)
                {
                    getDistance = getDistanceBetweenTwoPoints(this.point, elephant.point);
                    
                    if(getDistance < holdNearestDistance)
                    {
                        holdClosestElephantObject = elephant;
                        holdNearestDistance = getDistance;
                    }
                }
            }
            return holdClosestElephantObject;
        }

        private bool iAmOnAnElepantGridSpace()
        {
            lock (elephantList)
            {
                foreach(Elephant elephant in elephantList)
                {

                    if (this.point.X == -1 && this.point.Y == -1)
                    {
                        Debug.Log($"this object is 'dead'");
                        return false;
                    }
                    else if (this.point.Equals(elephant.point))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected override bool IsMouse()
        {
            return true;
        }


    }
}
