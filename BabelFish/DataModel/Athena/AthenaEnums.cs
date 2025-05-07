using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena {

    public enum DisplayEntityType {
        AthleteDisplay,
        ImageDisplay,
        ResultList,
        SquaddingList
    }

    public enum ESTUnitCommand {
        /*
         * In order to get an Enum to serialize / deserialize to values with spaces have to do a couple of things.
         * First use the EnumMember(Value = "   ") attribute. This is what does the serialzing / deserialzing.
         * In order to get the Descriptions in code, have to use the Description attribute in conjunction with the
         * ExtensionMethod .Description() (Located in the ExtensionMethods.cs class).
        */
        /// <summary>
        /// Command to perform a soft reboot, which is to start and stop the process.
        /// </summary>
        [Description( "Soft Reboot" )][EnumMember( Value = "Soft Reboot" )] SOFTREBOOT,

        /// <summary>
        /// Command to perform a hard reboot, which is to start and stop the operating system.
        /// </summary>
        [Description( "Hard Reboot" )][EnumMember( Value = "Hard Reboot" )] HARDREBOOT,

        /// <summary>
        /// Command to perform a shutdown of the operating system.
        /// </summary>
        [Description( "Shutdown" )][EnumMember( Value = "Shutdown" )] SHUTDOWN,

        /// <summary>
        /// Command to turn simulation off on the EST Unit.
        /// </summary>
        [Description( "SimulationOff" )][EnumMember( Value = "SimulationOff" )] SIMULATIONOFF,

        /// <summary>
        /// Command to perform a self diagnostic test.
        /// </summary>
        [Description( "SelfDiagnostic" )][EnumMember( Value = "SelfDiagnostic" )] SELFDIAGNOSTIC,

        /// <summary>
        /// Command to rotate the Display View.
        /// </summary>
        [Description( "Rotate Display" )][EnumMember( Value = "Rotate Display" )] ROTATEDISPLAY,

        /// <summary>
        /// Command to request the target's current Result COF data packet.
        /// </summary>
        [Description( "Request Result COF" )][EnumMember( Value = "Request Result COF" )] REQUESTRESULTCOF,

        /// <summary>
        /// Command to request the target send all shots for the specified match or resutl COF
        /// </summary>
        [Description( "Request Shots" )][EnumMember( Value = "Request Shots" )] REQUESTSHOTS,

        /// <summary>
        /// Command to request the target update one or more shots
        /// </summary>
        [Description( "Update Shots" )][EnumMember( Value = "Update Shots" )] UPDATESHOTS,

        /// <summary>
        /// Command to request the est units immediatly save their state files.
        /// </summary>
        [Description( "SaveStateFile" )][EnumMember( Value = "SaveStateFile" )] SAVESTATEFILE
    }

    public enum ReplaceVariableOptions { ResultList, ResultLists };

}
