using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using UnityEngine;
using System;
using System.IO;

namespace Assets.Scripts.AnimalScriptLogic
{
    abstract class Animal : MonoBehaviour
    {
        public Point point;

        protected readonly object _objLock;

        protected Thread thread;

        public List<Mouse> mouseList;
        public List<Elephant> elephantList;

        protected readonly int RowBound;
        protected readonly int ColoumBound;
 
        protected readonly int StrikeDistance;

        protected int elephantsAvailable;
        protected static int roundTurn = 0;
        protected static int allSqawnedObjects = 0;

        protected readonly int[,] checkCellIndcies = new int[,] { {-1, -1}, { 0, -1}, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, -0} };

        public Animal(List<Mouse> mouseList, List<Elephant> elephantList, object _objLock, int elephantsAvailable, int RowBound, int ColoumBound, int StrikeDistance)
        {
            this.mouseList = mouseList;
            this.elephantList = elephantList;

            this._objLock = _objLock;

            this.elephantsAvailable = elephantsAvailable;

            this.RowBound = RowBound;
            this.ColoumBound = ColoumBound;
            this.StrikeDistance = StrikeDistance;

            this.point = getRandomPoint();

            this.thread = new Thread(new ThreadStart(this.Run));
            thread.Start();

        }

        protected abstract void Run();

        protected abstract void MoveAround();

        protected abstract bool IsMouse();

        // This function is important for all the threads to be the same point during the run state. Such as each thread will be waiting to be realsesed to the next round until all the threads is eady to continue.
        protected virtual void Barrier()
        {
            lock (_objLock)
            {
                Interlocked.Increment(ref allSqawnedObjects);

                if((Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == mouseList.Count + elephantList.Count) || (Interlocked.CompareExchange(ref elephantsAvailable, 0, 0)) <= 0)
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
                        do
                        {
                            Monitor.Wait(_objLock);
                            interrupted = true;
                        } while (interrupted);
                    }
                    catch (ThreadInterruptedException err)
                    {
                        TextWriter errorWriter = Console.Error;
                        errorWriter.WriteLine(err.Message);
                        return;
                    }
                }
            }
        }
        protected Point getRandomPoint()
        {
            System.Random rand = new System.Random();
            return new Point(rand.Next(RowBound, ColoumBound + 1)); //ColoumBound + 1 beacsue its RowBound <= value < ColoumBound, so we have to add a 1 to ColoumBound.
        }

        protected int getDistanceBetweenTwoPoints(Point pointOne, Point pointTwo)
        {
            if(isDiagonal(pointOne, pointTwo))
                return getDiagonalDistance(pointOne, pointTwo);
            return distanceFormula(pointOne, pointTwo);
        }

        private int distanceFormula(Point pointOne, Point pointTwo)
        {
            return (int) Math.Sqrt((Math.Pow((pointTwo.X - pointOne.X), 2)) + (Math.Pow((pointTwo.Y - pointOne.Y), 2)));
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
            System.Random rand = new System.Random();

            int xOffset = rand.Next(-1, 2);
            int yOffset = rand.Next(-1, 2);

            Point result = new Point(this.point.X + xOffset, this.point.Y + yOffset);

            if(((result.X >= 0) && (result.Y >= 0)) && ((result.X <= ColoumBound) && (result.Y <= RowBound)))
                return result;
            return this.point;
        }

        protected Point moveCloserToObject(Point maximizeCloestDistance, Point pointInReference)
        {
            Point tempPoint;
            Point returnClosestPointInReferneceDistance;

            int closestDistanceToObject;
            int closestPointInReferneceDistance = int.MaxValue;

           for(int i = 0; i < checkCellIndcies.Length; i++)
            {
                tempPoint = new Point(maximizeCloestDistance.X, maximizeCloestDistance.Y);

                tempPoint.X += checkCellIndcies[i, 0];
                tempPoint.Y += checkCellIndcies[i, 1];

                closestDistanceToObject = getDistanceBetweenTwoPoints(tempPoint, pointInReference);

                if(closestDistanceToObject == 0)
                {
                    closestPointInReferneceDistance = closestDistanceToObject;
                    returnClosestPointInReferneceDistance = new Point(tempPoint.X, tempPoint.Y);
                    return returnClosestPointInReferneceDistance;
                }

                else if(closestDistanceToObject < closestPointInReferneceDistance)
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
            if (this.point.X > ColoumBound)
                this.point.X = ColoumBound;
            if (this.point.X < 0)
                this.point.X = 0;
            if (this.point.Y > RowBound)
                this.point.Y = RowBound;
            if (this.point.Y < 0)
                this.point.Y = 0;
        }

        //Overide To String????
    }
}
