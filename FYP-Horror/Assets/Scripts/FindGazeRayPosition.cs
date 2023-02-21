using System.Net.Sockets;
using UnityEngine;

namespace Tobii.Research.Unity
{
    public class FindGazeRayPosition : MonoBehaviour
    {
        // used to point the flashlight towards the ray hit point
        public FlashLightDirection flashLightDirection;

        private FindEyeTracker eyeTracker;

        public GameObject gun;

        // used to tell if an eye tracker has been connected or not
        private bool hasTrackerBeenFound = false;

        /// Get the latest hit object.
        public Transform LatestHitObject;

        private void Update()
        {
            // if an eye tracker has been found connect it
            if (hasTrackerBeenFound == false)
            {
                eyeTracker = FindEyeTracker.Instance;

                if (eyeTracker != null)
                {
                    hasTrackerBeenFound = true;
                }
            }

            // shoot the ray
            if (hasTrackerBeenFound == true)
            {
                shootRay();
                detectWhenEyeIsClosed();
            }
        }
        protected bool GetRay(out Ray ray)
        {
            // returns default ray if eye tracker isn't connected
            if (eyeTracker == null)
            {
                ray = default(Ray);
                return false;
            }

            IGazeData gazeData = eyeTracker.LatestGazeData;
            ray = gazeData.CombinedGazeRayScreen;
            return gazeData.CombinedGazeRayScreenValid;
        }

        private void shootRay()
        {
            Ray ray;
            // returns whether the combined gaze is valid. also sets ray to this data
            var valid = GetRay(out ray);
            if (valid)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // finds hit object and points flashlight towards ray point
                    LatestHitObject = hit.transform;
                    flashLightDirection.testRay = hit;
                }
                else
                {
                    LatestHitObject = null;
                }
            }
        }

        private void detectWhenEyeIsClosed()
        {
            IGazeData gazeData = eyeTracker.LatestGazeData;
            if (gazeData.Left.GazePointValid == true && gazeData.Right.GazePointValid == false ||
                gazeData.Left.GazePointValid == false && gazeData.Right.GazePointValid == true)
            {
                gun.GetComponent<GunScript>().aimGun();
            }
            else
            {
                gun.GetComponent<GunScript>().resetGun();
            }
        }
    }
}