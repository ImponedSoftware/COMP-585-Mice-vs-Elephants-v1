using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Threading.Tasks;

namespace Assets.Scripts.AnimalScriptLogic
{
    class Mouse : Animal
    {
        public Mouse(List<Mouse> mouseList, List<Elephant> elephantList, object _objLock, int elephantsAvailable, int RowBound, int ColoumBound, int StrikeDistance) : base(mouseList, elephantList, _objLock, elephantsAvailable, RowBound, ColoumBound, StrikeDistance)
        {
      
        }


        protected override void Run()
        {
            while((Volatile.Read(ref elephantsAvailable) > 0))
            {
                Barrier();

                if ((Volatile.Read(ref elephantsAvailable) > 0))
                    break;

                MoveAround();
                //print or set the mouse sprite at the spefic location on the graph.
                Thread.Sleep(2000);
                SyncCurrentPosToScenePos(this);
            }
            // This is where mouse ternminates.
        }
  


        protected override void MoveAround()
        {
            Elephant closestElphant = getClosestElephant();
            bool elephantInStrikingDistance = false;
            int closestElephantDisatnce;

            if(closestElphant != null)
            {
                closestElephantDisatnce = getDistanceBetweenTwoPoints(this.point, closestElphant.point);
                elephantInStrikingDistance = closestElephantDisatnce <= StrikeDistance;
            }

            bool mouseInStrikingDistance = (checkTheNearestMouse() <= (StrikeDistance * 2));

            if (elephantInStrikingDistance == false)
                point = moveInRandomAdijantSquare();
            else if (mouseInStrikingDistance == false && elephantInStrikingDistance == true)
                return;
            else
            {
                if (iAmOnAnElepantGridSpace())
                    return;
                point = moveCloserToObject(point, closestElphant.point);
            }
            CheckBounds();
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
                    if (this.point.Equals(elephant.point))
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
