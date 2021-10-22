using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AnimalScriptLogic
{
    abstract class Animal : MonoBehaviour
    {
        private static readonly object _objLock = new object();

        protected List<Mouse> mouseList = new List<Mouse>();
        protected List<Elephant> elephantList;

        protected int strikeDistance;

    }
}
