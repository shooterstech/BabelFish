﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Athena.Range
{
    public class RangeConfiguration
    {

        private readonly Dictionary<string, string> thingNameToNicknameLookup = new Dictionary<string, string>();
        private readonly Dictionary<string, string> nicknameToThingNameLookup = new Dictionary<string, string>();

        public RangeConfiguration()
        {

            FiringLines = new List<FiringLine>();
            TargetLines = new List<TargetLine>();
            Groups = new List<Group>();
            FiringPoints = new List<FiringPoint>();
            Displays = new List<Display>();
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {

            //1. Check and make sure all the Displays, Targets, and Monitors have Nicknames
            //2. Add entries to the thing name to nick name lookups

            var index = 1;
            foreach (var display in Displays)
            {
                if (string.IsNullOrEmpty(display.Nickname))
                {
                    display.Nickname = $"Display {index}";
                }

                thingNameToNicknameLookup[display.DisplayStateAddress] = display.Nickname;
                nicknameToThingNameLookup[display.Nickname] = display.DisplayStateAddress;
                index++;
            }

            foreach (var fp in FiringPoints)
            {
                foreach (var target in fp.Targets)
                {
                    if (string.IsNullOrEmpty(target.Nickname))
                    {
                        target.Nickname = $"Target on FP {fp.FiringPointNumber}";
                    }

                    thingNameToNicknameLookup[target.TargetStateAddress] = target.Nickname;
                    nicknameToThingNameLookup[target.Nickname] = target.TargetStateAddress;
                }

                foreach (var monitor in fp.Monitors)
                {
                    if (string.IsNullOrEmpty(monitor.Nickname))
                    {
                        monitor.Nickname = $"Monitor on FP {fp.FiringPointNumber}";
                    }

                    thingNameToNicknameLookup[monitor.MonitorStateAddress] = monitor.Nickname;
                    nicknameToThingNameLookup[monitor.Nickname] = monitor.MonitorStateAddress;
                }
            }

            if (string.IsNullOrEmpty(NetworkManager.Nickname))
            {
                NetworkManager.Nickname = "Network Manager";
                thingNameToNicknameLookup[NetworkManager.GGGName] = NetworkManager.Nickname;
                nicknameToThingNameLookup[NetworkManager.Nickname] = NetworkManager.GGGName;
            }
        }

        /// <summary>
        /// The human readable name given to the range
        /// </summary>
        [JsonPropertyOrder( 1 )]
        public string RangeName { get; set; }

        /// <summary>
        /// Unique identifier within the Orion Acccount given to this range. Usually denoted with a single character, 'A', 'B', 'C', etc
        /// </summary>
        [JsonPropertyOrder( 2 )]
        public string RangeID { get; set; }

        /// <summary>
        /// The Orion Account Owner, formatted as OriionAccout000001
        /// </summary>
        [JsonPropertyOrder( 3 )]
        public string Owner { get; set; }

        /// <summary>
        /// A list of FiringLines that this Range has. Most ranges will only have one. As a counter example though, a high power range may have three, 200yd., 300yd. and 600yd. 
        /// </summary>
        [JsonPropertyOrder( 4 )]
        public List<FiringLine> FiringLines { get; set; }

        /// <summary>
        /// A list of TargetLines that this range has. Most ranges will only have one. As a counter example though, a Bullseye pistol range would have two, 25yd. and 50yd.
        /// </summary>
        [JsonPropertyOrder( 5 )]
        public List<TargetLine> TargetLines { get; set; }

        /// <summary>
        /// A list of Target Groups. Typically only used in an ISSF 25m pistol range.
        /// </summary>
        [JsonPropertyOrder( 6 )]
        public List<Group> Groups { get; set; }


        [JsonPropertyOrder( 7 )] 
        public NetworkManager NetworkManager { get; set; }

        /// <summary>
        /// A list of Firing Points 
        /// </summary>
        [JsonPropertyOrder( 10 )]
        public List<FiringPoint> FiringPoints { get; set; }

        [JsonPropertyOrder( 11 )] 
        public List<Display> Displays { get; set; }

        /// <summary>
        /// Helper function to get the state address (aka thing name) for the passed in nickname.
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public string GetStateAddress(string nickname)
        {
            return nicknameToThingNameLookup[nickname];
        }

        /// <summary>
        /// Helper function to get the state address (aka thing name) for the passed in nickname.
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="stateAddress"></param>
        /// <returns></returns>
        public bool TryGetStateAddress(string nickname, out string stateAddress)
        {
            try
            {
                stateAddress = nicknameToThingNameLookup[nickname];
                return true;
            }
            catch (Exception)
            {
                stateAddress = "";
                return false;
            }
        }

        /// <summary>
        /// Helper function to return the nickname of the passed in state address (aka thing name)
        /// </summary>
        /// <param name="stateAddress"></param>
        /// <returns></returns>
        public string GetNickname(string stateAddress)
        {
            return thingNameToNicknameLookup[stateAddress];
        }

        /// <summary>
        /// Helper function to return the nickname of the passed in state address (aka thing name)
        /// </summary>
        /// <param name="stateAddress"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public bool TryGetNickname(string stateAddress, out string nickname)
        {
            try
            {
                nickname = thingNameToNicknameLookup[stateAddress];
                return true;
            }
            catch (Exception)
            {
                nickname = "";
                return false;
            }
        }

        /// <summary>
        /// Helper function to return the FiringPoint object for the passed in firingPointNumber.
        /// </summary>
        /// <param name="firingPointNumber"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">If the passed in firingPointNumber is not known.</exception>
        public FiringPoint GetFiringPoint(string firingPointNumber)
        {
            foreach (var fp in FiringPoints)
                if (fp.FiringPointNumber == firingPointNumber)
                    return fp;

            throw new IndexOutOfRangeException($"Firing Point firingPointNumber is not known.");
        }

        /// <summary>
        /// Helper function to return the FiringPoint object for the passed in firingPointNumber.
        /// </summary>
        /// <param name="firingPointNumber"></param>
        /// <param name="firingPoint"></param>
        /// <returns></returns>
        public bool TryGetFiringPoint(string firingPointNumber, out FiringPoint firingPoint)
        {
            try
            {
                firingPoint = GetFiringPoint(firingPointNumber);
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                firingPoint = null;
                return false;
            }
        }
    }
}