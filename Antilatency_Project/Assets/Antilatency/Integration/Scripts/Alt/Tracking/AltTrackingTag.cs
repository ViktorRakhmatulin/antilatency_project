// Copyright 2019, ALT LLC. All Rights Reserved.
// This file is part of Antilatency SDK.
// It is subject to the license terms in the LICENSE file found in the top-level directory
// of this distribution and at http://www.antilatency.com/eula
// You may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections;
using System.Collections.Generic;
using Antilatency.DeviceNetwork;
using UnityEngine;
using System.Net.Sockets;

namespace Antilatency.Integration {

    /// <summary>
    /// The sample component that starts the tracking task on an Alt connected to a socket (bracer, tag) tagged with <paramref name="Tag"/>.
    /// </summary>
    public class AltTrackingTag : AltTracking {
        public string IP = "127.0.0.1"; //
        public int Port = 20;
        public byte[] dane;
        public Socket client;
        public float min_velocity;
        public string SocketTag;
        public Vector3 PlacementOffset;
        public Vector3 PlacementRotation;
        public Vector3 position;
        public Vector3 velocity;
        private Quaternion rotation;
        /// <returns>The first idle tracking node connected to the socket is tagged with <paramref name="Tag"/>.</returns>
        protected override NodeHandle GetAvailableTrackingNode() {
            return GetFirstIdleTrackerNode();
            //return GetFirstIdleTrackerNodeBySocketTag(SocketTag);
        }

        /// <returns>The pose that is created from PlacementOffset and PlacementRotation.</returns>
        protected override Pose GetPlacement() {
            return new Pose(PlacementOffset, Quaternion.Euler(PlacementRotation));
        }

        /// <summary>
        /// Apply tracking data to a component's GameObject transform.
        /// </summary>

        void Start()
        {
            Debug.Log("Start function");
           client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//
           client.Connect(IP, Port);//connecting port with ip address 
        }

        protected override void Update() {
            base.Update();

            Antilatency.Alt.Tracking.State trackingState;

            if (!GetTrackingState(out trackingState)) {
                return;
            }
            min_velocity = 0.1f;
            transform.localPosition = trackingState.pose.position;
            transform.localRotation = trackingState.pose.rotation;


            /*
            if (min_velocity < trackingState.velocity.sqrMagnitude)
            {
                Debug.Log("min_vel:" + min_velocity + " current vel: " + trackingState.velocity.sqrMagnitude);
            }
            Debug.Log("stability" + trackingState.stability.stage.ToString());
            
    */
            position = trackingState.pose.position;
          //  Debug.Log(position[0] +" " +  position[1] + " " + position[2]);
            velocity = trackingState.velocity;
            rotation = trackingState.pose.rotation;
            Debug.Log(velocity.ToString());
            string position_str = position[0] + " " + position[1] + " " + position[2];
            string stability_str = trackingState.stability.stage.ToString();
            string velocity_str = velocity[0] + " " + velocity[1] + " " + velocity[2];
            string rotation_str = rotation[0] + " " + rotation[1] + " " + rotation[2] + " " + rotation[3];

            dane = System.Text.Encoding.ASCII.GetBytes(
                "[" + SocketTag + ";" + trackingState.stability.stage.ToString() + ";" 
                + position[0] + " " + position[1] + " " +  position[2] + ";" +
                velocity[0] + " " + velocity[1] + " " + velocity[2] + "]"
                );

            client.Send(dane);

            //Debug.Log("velocity" + trackingState.velocity.sqrMagnitude.ToString());
            

            //Debug.Log("position" + transform.localPosition.ToString());
            //Debug.Log("angular velocity" + trackingState.localAngularVelocity.ToString());


        }
    }
}