////-----------------------------------------------------------------------
//// Copyright © 2019 Tobii Pro AB. All rights reserved.
////-----------------------------------------------------------------------

//using UnityEngine;

//namespace Tobii.Research.Unity
//{
//    public class GazeTrail : GazeTrailBase
//    {
//        /// <summary>
//        /// Get <see cref="GazeTrail"/> instance. This is assigned
//        /// in Awake(), so call earliest in Start().
//        /// </summary>
//        public static GazeTrail Instance { get; private set; }

//        private FindEyeTracker _eyeTracker;

//        //protected override void OnAwake()
//        //{
//        //    base.OnAwake();
//        //    Instance = this;
//        //}

//        //protected override void OnStart()
//        //{
//        //    base.OnStart();
//        //    _eyeTracker = FindEyeTracker.Instance;
//        //}

//        protected override bool GetRay(out Ray ray)
//        {
//            if (_eyeTracker == null)
//            {
//                return base.GetRay(out ray);
//            }

//            var data = _eyeTracker.LatestGazeData;
//            ray = data.CombinedGazeRayScreen;
//            return data.CombinedGazeRayScreenValid;
//        }

//        protected override bool HasEyeTracker
//        {
//            get
//            {
//                return _eyeTracker != null;
//            }
//        }
//    }
//}