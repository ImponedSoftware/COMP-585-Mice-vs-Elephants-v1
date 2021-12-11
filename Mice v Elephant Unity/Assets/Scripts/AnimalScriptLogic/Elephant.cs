using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using UnityEngine;


namespace Assets.Scripts.AnimalScriptLogic
{
    class Elephant : Animal
    {
        private bool running;
        private bool elephantTurnFlag;

        public Elephant(List<Mouse> mouseList, List<Elephant> elephantList, object _objLock, int elephantsAvailable, int RowBound, int ColoumBound, int StrikeDistance, Point point) : base(mouseList, elephantList, _objLock, elephantsAvailable, RowBound, ColoumBound, StrikeDistance, point)
        {
            /*  do
              { // look if you can put a flag bool and don't need to check twice;
                  if (isOccupied())
                  {
                      point = getRandomPoint();
                  }
              } while (isOccupied());*/

        }
        protected override void Run()
        {
            running = true;
            elephantTurnFlag = true;

            while (running)
            {
                //Debug.Log(this.point + " BEginnign EL");
                Barrier();
                //Debug.Log("WERE fuckign runngin");

                if (running)
                {
                    if (elephantTurnFlag)
                    {
                        MoveAround();
                        /*
                                                if (isOccupied())
                                                {
                                                    Debug.Log("INNNN");
                                                    point = moveInRandomAdijantSquare();
                                                    Debug.Log(point + " s");
                                                } while (isOccupied()) ;*/
                    }
                }

                elephantTurnFlag = !elephantTurnFlag;
                //Print this or this is where we call the sprite to move to
                Thread.Sleep(1000);
                //Debug.Log(this.point + " El");

                if (running)
                {
                    SyncCurrentPosToScenePos(this);
                }
                else
                {
                    //Debug.Log($"{ mouseList.Count + elephantListCount} {(Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == mouseList.Count + elephantListCount)} {allSqawnedObjects}");
                    Debug.Log("Not call");

                

                    foreach(Mouse mice in mouseList)
                    {
                        Debug.Log("Mice thread alive: " + mice.thread.IsAlive);
                    }


                    foreach (Elephant elephant in elephantList)
                    {
                        Debug.Log("Ele thread alive: " + elephant.thread.IsAlive);
                    }
                }
              
            }

        }

        protected override void Barrier()
        {
            Monitor.Enter(_objLock);
            {
               // Debug.Log("Elphant111");
                if (amountOfMiceOnSquare() >= 2)
                {
                    isElephantEaten();
                    running = false;
                    //Debug.Log(running + " running");
                    // Debug.Log("EL " + elephantList.Count);
                    this.point.X = -1;
                    this.point.Y = -1;
                    elephantListCount -= 1;
                    //Debug.Log("EL " + elephantListCount + " " + ((Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == mouseList.Count + elephantListCount)));
                    if ((Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == mouseList.Count + elephantListCount))
                    {
                        Monitor.PulseAll(_objLock);
                        Interlocked.Increment(ref roundTurn);
                        Interlocked.Exchange(ref allSqawnedObjects, 0);
                    }

                }

                Interlocked.Increment(ref allSqawnedObjects);

                //Debug.Log(allSqawnedObjects);
                //Debug.Log((Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == mouseList.Count + elephantList.Count) + " pghkljoghkj");
                //Debug.Log((Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == mouseList.Count + elephantList.Count));
                if ((Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == mouseList.Count + elephantListCount))
                {
                    Monitor.PulseAll(_objLock);
                    Interlocked.Increment(ref roundTurn);
                    Interlocked.Exchange(ref allSqawnedObjects, 0);
                }
                else
                {
                    bool interrupted = false;
                    try
                    {
                        while (!interrupted)
                        {
                            //Debug.Log(this.thread.ManagedThreadId);
                            Monitor.Wait(_objLock);
                            interrupted = true;
                        }
                    }
                    catch (ThreadInterruptedException err)
                    {
                        TextWriter errorWriter = Console.Error;
                        errorWriter.WriteLine(err.Message);
                        return;
                    }
                }
            }
            Monitor.Exit(_objLock);
        }

        protected override void MoveAround()
        {
            int amountOfMiceOnMySquare = amountOfMiceOnSquare();

            if (amountOfMiceOnMySquare == 1)
            {
                
                mouseShotAway();
                point = moveInRandomAdijantSquare();
            }
            else if (checkTheNearestMouse() < StrikeDistance)
            {
                point = moveFartherAwayFromMouse(this.point, closestMouseToMe().point);
            }
            else
            {
                //Debug.Log(point + " INNN");
                point = moveInRandomAdijantSquare();
                //Debug.Log(point + " OUt");
            }
            //Debug.Log("OMG ITS WORKING");
            this.CheckBounds();
        }

        private void isElephantEaten()
        {
            Monitor.Enter(elephantList);// MAYBE lock on the this
            {
                Interlocked.Decrement(ref elephantsAvailable);
              
                //elephantList[elephantList.IndexOf(this)].thread.Join();
                Debug.Log("THIS IS IN EATENEL:" +(Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == mouseList.Count + elephantList.Count) + " AllS " + allSqawnedObjects);
                if (Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == ((mouseList.Count + elephantListCount) - 1) || (Interlocked.CompareExchange(ref elephantsAvailable, 0, 0) <= 0))
                {
                    Debug.Log("This El is eaten inside to release");
                    Monitor.PulseAll(_objLock);
                    Interlocked.Increment(ref roundTurn);
                    Interlocked.Exchange(ref allSqawnedObjects, 0);
                   
                }

                int index = elephantList.IndexOf(this);
                Debug.Log("This El is eaten " + index);
       

                StartSimulation.syncContext.Post(_ =>
                {
                    //Debug.Log(elephantList.IndexOf(this));//
                    GridManager.DestoryElephant(index);
                }, null);
               // GridManager.DestoryElephant(elephantList.IndexOf(this));               
                //elephantList.Remove(this);
                //lephantList.Remove(this);

                Debug.Log(elephantsAvailable);
                // want to problemley remove the specfic gameobject
            }
            Monitor.Exit(elephantList);
        }

        private void mouseShotAway()
        {
            System.Random rand = new System.Random();

            int randIndcies = rand.Next(0, (checkCellIndcies.GetLength(0)));

            int xDirection = checkCellIndcies[randIndcies, 0];
            int yDirection = checkCellIndcies[randIndcies, 1];


            lock (mouseList)
            {
                foreach (Mouse mice in mouseList)
                {
                    if (mice.point.Equals(this.point))
                    {
                        mice.point = new Point((mice.point.X + (StrikeDistance * xDirection) * 2), (mice.point.Y + (StrikeDistance * yDirection) * 2));
                        mice.CheckBounds();
                    }
                }
            }
        }

        private Mouse closestMouseToMe()
        {
            int closestMouse = int.MaxValue;
            int getTheDistanceOfMouse = 0;

            Mouse returnCloestMouse = null;

            lock (mouseList)
            {

                foreach (Mouse mice in mouseList)
                {

                    getTheDistanceOfMouse = getDistanceBetweenTwoPoints(this.point, mice.point);

                    if (getTheDistanceOfMouse < closestMouse)
                    {
                        closestMouse = getTheDistanceOfMouse;
                        returnCloestMouse = mice;
                    }
                }
            }

            return returnCloestMouse;
        }

        private Point moveFartherAwayFromMouse(Point minimizeClosestDistance, Point pointInReference)
        {
            int farthestDistanceAwayFromObject;
            int farthestPointInReferenceDistance = int.MinValue;

            Point checkTempPoint;
            Point returnFarthestDistanceToObject;


            Debug.Log($"{minimizeClosestDistance} {pointInReference}");

            for (int i = 0; i < (checkCellIndcies.GetLength(0) - 1); i++)
            {
                checkTempPoint = new Point(minimizeClosestDistance.X, minimizeClosestDistance.Y);
                checkTempPoint.X += checkCellIndcies[i, 0];
                checkTempPoint.Y += checkCellIndcies[i, 1];

                farthestDistanceAwayFromObject = getDistanceBetweenTwoPoints(checkTempPoint, pointInReference);



                if (farthestDistanceAwayFromObject > farthestPointInReferenceDistance)
                {
                    farthestPointInReferenceDistance = farthestDistanceAwayFromObject;
                    returnFarthestDistanceToObject = new Point(checkTempPoint.X, checkTempPoint.Y);
                }
            }
            return returnFarthestDistanceToObject;
        }


        private int amountOfMiceOnSquare()
        {
            lock (mouseList)
            {
                int amountOfMiceOnSquare = 0;

                foreach (Mouse mice in mouseList)
                {
                    if (mice.point.Equals(this.point))
                        amountOfMiceOnSquare++;
                }
                return amountOfMiceOnSquare;
            }

        }
        private bool isOccupied()
        {
            lock (elephantList)// need to be converted to collection syncorniszed list for thread safety
            {
                foreach (Elephant elephant in elephantList)
                {
                    if (this.point.Equals(elephant.point) && (this.thread.ManagedThreadId != elephant.thread.ManagedThreadId))
                        return true;
                }
            }
            return false;

        }

        protected override bool IsMouse()
        {
            return false;
        }
    }
}
