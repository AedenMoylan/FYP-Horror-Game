using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

namespace Tobii.Research.Unity
{
    public class FindEyeTracker : MonoBehaviour
    {

        /// The IEyeTracker instance
        private IEyeTracker eyeTracker = null;

        /// Max queue size for gaze data
        private const int maxGazeDataQueueSize = 20;


        // instance used in FindGazeTrail to get the current connected eye tracker
        public static FindEyeTracker Instance;


        // will get set to the first connected eyeTracker
        private IEyeTracker FoundEyeTracker;

        private bool isEyeTrackerConnected = false;

        /// Get the newest gaze data. if any new data arrives, it will be processed before being returned
        public IGazeData LatestGazeData
        {
            get
            {
                // if unprocessed data exists
                if (unprocessedGazeData.Count > 0)
                {
                    ProcessGazeEvents();
                }

                return newestGazeData;
            }
        }

        // queue of gaze data before it is processed. Has a locked max size
        private LockedQueue<GazeDataEventArgs> unprocessedGazeData = new LockedQueue<GazeDataEventArgs>(maxCount: maxGazeDataQueueSize);


        /// Holds unprocessed gaze data. GazeData is a premade tobii script that processes eye tracking data
        private IGazeData newestGazeData = new GazeData();

        private void Start()
        {
            Instance = this;
        }

        private void Update()
        {
            if (isEyeTrackerConnected == false)
            {
                connectEyeTracker();
            }
            ProcessGazeEvents();
        }

        private void connectEyeTracker()
        {
            // eyeTrackers contains all connected eye trackers
            EyeTrackerCollection eyeTrackers = EyeTrackingOperations.FindAllEyeTrackers();

            if (eyeTrackers != null)
            {
                // goes through eyeTrackers to get first eye tracker connected
                foreach (IEyeTracker eyeTrackerEntry in eyeTrackers)
                {
                    FoundEyeTracker = eyeTrackerEntry;
                }
            }

            if (eyeTracker == null && FoundEyeTracker != null)
            {
                eyeTracker = FoundEyeTracker;
                eyeTracker.GazeDataReceived += GazeDataReceivedCallback;
                Debug.Log("Connected to Eye Tracker: " + eyeTracker.SerialNumber);
            }
        }

        private void ProcessGazeEvents()
        {
            const int maxIterations = 20;

            IGazeData gazeData = newestGazeData;

            for (int i = 0; i < maxIterations; i++)
            {
                GazeDataEventArgs originalGaze = unprocessedGazeData.Next;

                if (originalGaze == null)
                {
                    break;
                }

                // GazeData script processes gaze data
                gazeData = new GazeData(originalGaze);
            }

            // throws debug message if there are still items to be processed
            if (unprocessedGazeData.Count > 0)
            {
                Debug.LogWarning("Queue not fully emptied: " + unprocessedGazeData.Count + " things left");
            }

            newestGazeData = gazeData;
        }

        // allows eye tracker to receive Gaze Data
        private void GazeDataReceivedCallback(object sender, GazeDataEventArgs eventArgs)
        {
            unprocessedGazeData.Next = eventArgs;
        }


    }
}