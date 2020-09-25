//-----------------------------------------------------------------------
// <copyright file="PawnManipulator.cs" company="Google LLC">
//
// Copyright 2019 Google LLC. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.ObjectManipulation
{
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System.Collections.Generic;

    /// <summary>
    /// Controls the placement of objects via a tap gesture.
    /// </summary>
    public class PawnManipulator : Manipulator
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;
        bool spawned =false;

        public Text objecttransform;
        public Text manipulatortransform;
        public Text manipulatorrotation;
        public Text objectrotation;

        public GameObject ManipulatorPrefab;
        public GameObject manipulator;

        private GameObject gameObject1;
        private GameObject gameObject2;

        bool active;

        public GameObject PawnPrefab1; //cube
        public GameObject PawnPrefab2; //sphere

        public Button m_Object1Button;
        public Button m_Object2Button;

         void Start()
        {
            objecttransform = objecttransform.GetComponent<Text>();
            manipulatortransform = manipulatortransform.GetComponent<Text>(); ;
        }
        void Update()
        {
            m_Object1Button.onClick.AddListener(onbutton1clicked); 
            m_Object2Button.onClick.AddListener(onbutton2clicked);
            
        }

        //void OnDestroy()
        //{
        //    m_Object1Button.onClick.RemoveListener(onbutton1clicked); ; 
        //    m_Object2Button.onClick.RemoveListener(onbutton2clicked);
        //}

        public void onbutton1clicked()
        {
                gameObject1.SetActive(true);
                gameObject2.SetActive(false);
        }

        public void onbutton2clicked()
        {
                gameObject2.SetActive(true);
                gameObject1.SetActive(false);
        }

        /// <summary>
        /// Returns true if the manipulation can be started for the given gesture.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        /// <returns>True if the manipulation can be started.</returns>
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            if (gesture.TargetObject == null)
            {
                return true;
            }

            return false;
        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDatacurrentPosition = new PointerEventData(EventSystem.current);
            eventDatacurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDatacurrentPosition, results);
            return results.Count > 0;
        }


        /// <summary>
        /// Function called when the manipulation is ended.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnEndManipulation(TapGesture gesture)
        {
            //var manipulator ;
            if (gesture.WasCancelled)
            {
                return;
            }

            // If gesture is targeting an existing object we are done.
            if (gesture.TargetObject != null)
            {
                return;
            }

            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;

            if (Frame.Raycast(
                gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
            {
                if (!IsPointerOverUIObject())
                {
                    // Use hit pose and camera pose to check if hittest is from the
                    // back of the plane, if it is, no need to create the anchor.
                    if ((hit.Trackable is DetectedPlane) &&
                        Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                            hit.Pose.rotation * Vector3.up) < 0)
                    {
                        Debug.Log("Hit at back of the current DetectedPlane");
                    }
                    else
                    {
                        if (!spawned)
                        {
                            // Instantiate game object at the hit pose.
                            gameObject1 = Instantiate(PawnPrefab1, hit.Pose.position, hit.Pose.rotation);
                            gameObject2 = Instantiate(PawnPrefab2, hit.Pose.position, hit.Pose.rotation);
                          
                            // Instantiate manipulator.
                            manipulator = Instantiate(ManipulatorPrefab, hit.Pose.position, hit.Pose.rotation);

                            // Make game object a child of the manipulator.
                            gameObject1.transform.parent = manipulator.transform;
                            gameObject2.transform.parent = manipulator.transform;

                            // Create an anchor to allow ARCore to track the hitpoint as understanding of
                            // the physical world evolves.
                            var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                            // Make manipulator a child of the anchor.
                            manipulator.transform.parent = anchor.transform;

                            // Select the placed object.
                            manipulator.GetComponent<Manipulator>().Select();
                            spawned = true;
                            gameObject2.SetActive(false);
                        }
                        else
                        {
                            if (gameObject1.activeInHierarchy)
                            {
                                gameObject1.transform.position = hit.Pose.position;
                                gameObject1.transform.parent = manipulator.transform;
                                manipulator.transform.position = new Vector3(gameObject2.transform.position.x, gameObject2.transform.position.y, gameObject2.transform.position.z);
                                manipulator.transform.rotation = hit.Pose.rotation;
                                objecttransform.text = "object transform is: " + gameObject1.transform.position.ToString();
                                objectrotation.text = "object rotation is: " + gameObject1.transform.rotation.ToString();
                                manipulatortransform.text = "manipulator transform is: "+manipulator.transform.position.ToString();
                                manipulatorrotation.text = "manipulator rotation is: " + manipulator.transform.rotation.ToString();
                            }

                            if (gameObject2.activeInHierarchy)
                            {
                                gameObject2.transform.position = hit.Pose.position;
                                gameObject2.transform.parent = manipulator.transform;
                                manipulator.transform.position = new Vector3(gameObject2.transform.position.x, gameObject2.transform.position.y, gameObject2.transform.position.z);
                                manipulator.transform.rotation = hit.Pose.rotation;
                                objecttransform.text = "object transform is: "+ gameObject2.transform.position.ToString();
                                objectrotation.text = "object rotation is: "+ gameObject2.transform.rotation.ToString();
                                manipulatortransform.text = "manipulator transform is: "+manipulator.transform.position.ToString();
                                manipulatorrotation.text = "manipulator rotation is: "+manipulator.transform.rotation.ToString();
                            }
                            
                        }
                    }
                }
            }
        }
    }
}
