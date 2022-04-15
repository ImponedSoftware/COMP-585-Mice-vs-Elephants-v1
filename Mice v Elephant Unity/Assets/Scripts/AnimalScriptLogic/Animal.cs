using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using System;
using UnityEngine;
using System.IO;

namespace Assets.Scripts.AnimalScriptLogic
{
    abstract class Animal
    {
        public Point point;

        public object _objLock;

        public Thread thread;

        public List<Mouse> mouseList;
        public List<Elephant> elephantList;

        protected static System.Random rand = new System.Random();

        protected int RowBound;
        protected int ColoumBound;

        protected readonly int StrikeDistance;
        protected static int elephantListCount;

        protected static int elephantsAvailable;
        protected static int roundTurn = 0;
        protected static int allSqawnedObjects = 0;

        protected static int[,] checkCellIndcies = new int[,] { { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, -0 } };

        public Animal(List<Mouse> mouseList, List<Elephant> elephantList, object _objLock, int elephantsAvailables, int RowBound, int ColoumBound, int StrikeDistance, Point point)
        {

            this.mouseList = mouseList;
            this.elephantList = elephantList;

            this._objLock = _objLock;

            elephantsAvailable = elephantsAvailables;

            this.RowBound = RowBound;
            this.ColoumBound = ColoumBound;
            this.StrikeDistance = StrikeDistance;
            elephantListCount = elephantList.Count;
            this.point = point;

            //Debug.Log(mouseList.Count + elephantList.Count);

            this.thread = new Thread(new ThreadStart(this.Run));
            thread.Start();

            //  this.endThreat();

        }

        protected abstract void Run();

        protected abstract void MoveAround();

        protected abstract bool IsMouse();

        // This function is important for all the threads to be the same point during the run state. Such as each thread will be waiting to be realsesed to the next round until all the threads is eady to continue.
        protected virtual void Barrier()
        {
            Monitor.Enter(_objLock);
            {
                //Debug.Log("Mouse");
                Interlocked.Increment(ref allSqawnedObjects);
                //Debug.Log(mouseList.Count + elephantList.Count);
                Debug.Log((Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == mouseList.Count + elephantListCount));
                if ((Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == mouseList.Count + elephantListCount) || (Interlocked.CompareExchange(ref elephantsAvailable, 0, 0)) <= 0)
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
                            Debug.Log("Curr thread: " + this.thread.ManagedThreadId);
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


        protected void SyncCurrentPosToScenePos(Elephant animal)
        {
            Monitor.Enter(elephantList);
            {
                animal.CheckBounds();
                int x = animal.point.X;
                int y = animal.point.Y;
                int currentElephantIndex = elephantList.IndexOf(animal);



                //GridManager.SyncCurrentPosToScenePos(x, y, currentElephantIndex);

                StartSimulation.syncContext.Post(_ =>
                {

                    GridManager.SetSimulationRoundText(roundTurn);
                    GridManager.SyncCurrentPosToScenePos(x, y, currentElephantIndex);

                }, null);
            }
            Monitor.Exit(elephantList);
            //GridManager.elephants[currentElephantIndex].transform.position = new Vector3(x * GridManager.tileSpace, y * -GridManager.tileSpace, 1); 
        }

        protected void SyncCurrentPosToScenePos(Mouse animal)
        {
            Monitor.Enter(mouseList);
            {
                animal.CheckBounds();
                int x = animal.point.X;
                int y = animal.point.Y;
                int currentElephantIndex = mouseList.IndexOf(animal);

                //GridManager.SyncCurrentPosToScenePosMice(x, y, currentElephantIndex);

                if (x != -1 && y != -1)
                {
                    StartSimulation.syncContext.Post(_ =>
                    {
                        // This code here will run on the main thread
                        //Debug.Log("Hello from main thread! from mice" + currentElephantIndex + " " + x + " " + y + " " + this.thread.ManagedThreadId);

                        GridManager.SetSimulationRoundText(roundTurn);
                        GridManager.SyncCurrentPosToScenePosMice(x, y, currentElephantIndex);

                    }, null);
                }
            }
            Monitor.Exit(mouseList);
            //GridManager.mice[currentMouseIndex].transform.position = new Vector3(x * GridManager.tileSpace, y * -GridManager.tileSpace, 1);
        }

        protected int getDistanceBetweenTwoPoints(Point pointOne, Point pointTwo)
        {
            if (isDiagonal(pointOne, pointTwo))
                return getDiagonalDistance(pointOne, pointTwo);
            return distanceFormula(pointOne, pointTwo);
        }

        private int distanceFormula(Point pointOne, Point pointTwo)
        {
            return (int)Math.Sqrt((Math.Pow((pointTwo.X - pointOne.X), 2)) + (Math.Pow((pointTwo.Y - pointOne.Y), 2)));
        }

        protected bool isDiagonal(Point pointOne, Point pointTwo)
        {
            return (Math.Abs(pointOne.X - pointTwo.X)) == (Math.Abs(pointOne.Y - pointTwo.Y));
        }

        private int getDiagonalDistance(Point pointOne, Point pointTwo)
        {
            return Math.Max((Math.Abs(pointOne.X - pointTwo.X)), (Math.Abs(pointOne.Y - pointTwo.Y)));
        }

        protected int checkTheNearestMouse()
        {
            int closestMice = int.MaxValue;
            int getTheDistanceOfMice = 0;

            lock (mouseList) // need to be converted to collection syncorniszed list for thread safety
            {
                foreach (Mouse mice in mouseList)
                {
                    getTheDistanceOfMice = getDistanceBetweenTwoPoints(this.point, mice.point);
                    if ((getTheDistanceOfMice < closestMice) && (this.thread.ManagedThreadId != mice.thread.ManagedThreadId))
                        closestMice = getTheDistanceOfMice;
                }
            }
            return closestMice;
        }

        protected Point moveInRandomAdijantSquare()
        {
            //Debug.Log("OMG");
            lock (rand)
            {

                int xOffset = rand.Next(-1, 2);
                int yOffset = rand.Next(-1, 2);

                //Debug.Log("Xof: " + xOffset + " Yof: " + yOffset + "Mice Or El: " + this.thread.ManagedThreadId);

                Point result = new Point(this.point.X + xOffset, this.point.Y + yOffset);
                //if(((result.X >= 0) && (result.Y >= 0)) && ((result.X <= ColoumBound) && (result.Y <= RowBound)))
                return result;
                //return this.point;
            }
        }

        protected Point moveCloserToObject(Point maximizeCloestDistance, Point pointInReference)
        {
            Point tempPoint;
            Point returnClosestPointInReferneceDistance;

            int closestDistanceToObject;
            int closestPointInReferneceDistance = int.MaxValue;


            if (this.point.X == -1 && this.point.Y == -1)
            {
                Debug.Log($"this object is 'dead'");
                return this.point;
            }

            for (int i = 0; i < (checkCellIndcies.GetLength(0) - 1); i++)
            {
                tempPoint = new Point(maximizeCloestDistance.X, maximizeCloestDistance.Y);

                Debug.Log(i + " " + (checkCellIndcies.GetLength(0) - 1));

                tempPoint.X += checkCellIndcies[i, 0];
                tempPoint.Y += checkCellIndcies[i, 1];

                closestDistanceToObject = getDistanceBetweenTwoPoints(tempPoint, pointInReference);

                if (closestDistanceToObject == 0)
                {
                    closestPointInReferneceDistance = closestDistanceToObject;
                    returnClosestPointInReferneceDistance = new Point(tempPoint.X, tempPoint.Y);
                    return returnClosestPointInReferneceDistance;
                }

                else if (closestDistanceToObject < closestPointInReferneceDistance)
                {
                    closestPointInReferneceDistance = closestDistanceToObject;
                    returnClosestPointInReferneceDistance = new Point(tempPoint.X, tempPoint.Y);
                }
            }
            return returnClosestPointInReferneceDistance;
        }

        // If point goes out of bounds, then set it to the respected value;
        public void CheckBounds()
        {
            Debug.Log("IN CHECK: " + this.thread.ManagedThreadId + " " + this.point.X + " " + this.point.Y);
            if (this.point.X == -1 && this.point.Y == -1)
            {
                Debug.Log($"this object is 'dead'");
            }
            else
            {
                if (this.point.X > RowBound - 1)
                {
                    this.point.X = RowBound - 1;
                }
                if (this.point.X < 0)
                {
                    this.point.X = 0;
                }
                if (this.point.Y > ColoumBound - 1)
                {
                    this.point.Y = ColoumBound - 1;
                }
                if (this.point.Y < 0)
                {
                    this.point.Y = 0;
                }

                Debug.Log("OUT CHECK: " + this.thread.ManagedThreadId + " " + this.point.X + " " + this.point.Y);
            }
        }

        //Overide To String????
    }
}
