using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;


namespace Assets.Scripts.AnimalScriptLogic
{
    class Elephant : Animal
    {
        private bool running;
        private bool elephantTurnFlag;

        public Elephant(List<Mouse> mouseList, List<Elephant> elephantList, object _objLock, int elephantsAvailable, int RowBound, int ColoumBound, int StrikeDistance) : base(mouseList, elephantList, _objLock, elephantsAvailable, RowBound, ColoumBound, StrikeDistance)
        {
            do
            { // look if you can put a flag bool and don't need to check twice;
                if (isOccupied())
                {
                    point = getRandomPoint();
                }
            } while (isOccupied());

        }
        protected override void Run()
        {
            running = true;
            elephantTurnFlag = true;

            while (running)
            {
                Barrier();

                if (running)
                {
                    if (elephantTurnFlag)
                    {
                        MoveAround();

                        if (isOccupied())
                        {
                            point = moveInRandomAdijantSquare();

                        } while (isOccupied()) ;
                    }
                }

                elephantTurnFlag = !elephantTurnFlag;
                //Print this or this is where we call the sprite to move to
                Thread.Sleep(2000);
                SyncCurrentPosToScenePos(this);
            }
            //
        }

        protected override void Barrier()
        {
            lock (_objLock)
            {
                if (amountOfMiceOnSquare() >= 2)
                {
                    isElephantEaten();
                    running = false;
                    return;
                }
            }
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
                point = moveInRandomAdijantSquare();
            }

            CheckBounds();
        }

        private void isElephantEaten()
        {
            lock (elephantList) // MAYBE lock on the this
            {
                Interlocked.Decrement(ref elephantsAvailable);

                if (Interlocked.CompareExchange(ref allSqawnedObjects, 0, 0) == ((mouseList.Count + elephantList.Count) - 1) || (Interlocked.CompareExchange(ref elephantsAvailable, 0, 0) <= 0))
                {
                    Monitor.PulseAll(_objLock);
                    Interlocked.Increment(ref roundTurn);
                    Interlocked.Exchange(ref allSqawnedObjects, 0);
                }

                elephantList.Remove(this);
                // want to problemley remove the specfic gameobject
            }
        }

        private void mouseShotAway()
        {
            System.Random rand = new System.Random();

            int randIndcies = rand.Next(0, checkCellIndcies.Length + 1);

            int xDirection = checkCellIndcies[randIndcies, 0];
            int yDirection = checkCellIndcies[randIndcies, 1];

            lock (mouseList)
            {
                foreach (Mouse mice in mouseList)
                {
                    if (mice.point.Equals(this.point))
                    {
                        mice.point = new Point((mice.point.X + (StrikeDistance * 2 * xDirection)), (mice.point.Y + (StrikeDistance * 2 * yDirection)));
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




            for (int i = 0; i < checkCellIndcies.Length; i++)
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
