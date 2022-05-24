using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.Interfaces
{
    /// <summary>
    /// Interface describing classes that can have a penalty applied. 
    /// </summary>
    public interface IPenalty
    {

        /// <summary>
        /// Adds the passed in penalty to the list of Penalties
        /// </summary>
        /// <param name="penalty"></param>
        void AddPenalty(Penalty penalty);

        /// <summary>
        /// Returns a boolean indicating if the passed in Peanlty is already applied. 
        /// </summary>
        /// <param name="penalty"></param>
        /// <returns></returns>
        bool HasPenalty(Penalty penalty);

        /// <summary>
        /// Returns a boolean indicating if the passed in Peanlty ID exists. 
        /// </summary>
        /// <param name="penaltyID"></param>
        /// <returns></returns>
        bool HasPenalty(string penaltyID);

        /// <summary>
        /// Remove the passed in Penalty. If this penalty is not part of the Peanlties list, no action is applied.
        /// </summary>
        /// <param name="penalty"></param>
        void RemovePenalty(Penalty penalty);

        /// <summary>
        /// Removes the penalty that has the passed in penaltyID
        /// </summary>
        /// <param name="penaltyID"></param>
        void RemovePenalty(string penaltyID);

        /// <summary>
        /// Returns the list of Penalties. Same as calling .Penalties
        /// </summary>
        /// <returns></returns>
        List<Penalty> GetPenalties();

        /// <summary>
        /// Replaces the existing list of penalties with the passed in list. Same as calling .Penalties = value
        /// </summary>
        /// <param name="penalties"></param>
        void SetPenalties(List<Penalty> penalties);

        /// <summary>
        /// Get set the list of Penalties. Generally avoid using this function (except for serilaization).
        /// </summary>
        List<Penalty> Penalties { get; set; }

        /// <summary>
        /// Returns the sum of all penalty points.
        /// </summary>
        /// <returns></returns>
        float GetSumPenaltyPoints();
    }
}