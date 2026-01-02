using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST {
    public class EventAssignments {

        /*
         * IMPORTANT NOTE
         * If a property value in EventAssignments gets updated, then the enum DisplayEventOptions
         * (found in DefinitionEnums.cs) also needs to be updated.
         */

        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string Default { get; set; } = "AthenaMatchInProgress";

        [G_NS.JsonProperty( Order = 10, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationPreEvent { get; set; } = "AthenaQualificationPreEvent";
    
        [G_NS.JsonProperty( Order = 11, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationPostEvent { get; set; } = "AthenaQualificationPostEvent";
    
        [G_NS.JsonProperty( Order = 12, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationCallToLine { get; set; } = "AthenaQualificationCallToLine";
    
        [G_NS.JsonProperty( Order = 13, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationRemoveEquipment { get; set; } = "AthenaQualificationPostEvent";
    
        [G_NS.JsonProperty( Order = 14, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationStart { get; set; } = "AthenaMatchInProgress";
    
        [G_NS.JsonProperty( Order = 15, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationStop { get; set; } = "AthenaOnlyResultList";
    
        [G_NS.JsonProperty( Order = 16, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationPreparationPeriodStart { get; set; } = "AthenaPreparationPeriod";
    
        [G_NS.JsonProperty( Order = 17, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationPreparationPeriodStop { get; set; } = "AthenaMatchInProgress";
    
        [G_NS.JsonProperty( Order = 18, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationSightersStart { get; set; } = "AthenaMatchAthleteDisplay";
    
        [G_NS.JsonProperty( Order = 19, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationSightersStop { get; set; } = "AthenaMatchAthleteDisplay";
    
        [G_NS.JsonProperty( Order = 20, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationStageStart { get; set; } = "AthenaMatchInProgress";
    
        [G_NS.JsonProperty( Order = 21, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationStageStop { get; set; } = "AthenaMatchInProgress";
    
        [G_NS.JsonProperty( Order = 22, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationTargetChangeStart { get; set; } = "AthenaMatchInProgress";
    
        [G_NS.JsonProperty( Order = 23, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationTargetChangeStop { get; set; } = "AthenaMatchInProgress";
    
        [G_NS.JsonProperty( Order = 24, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationChangeOverStart { get; set; } = "AthenaChangeOver";
    
        [G_NS.JsonProperty( Order = 25, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationChangeOverStop { get; set; } = "AthenaMatchInProgress";
    
        [G_NS.JsonProperty( Order = 26, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationExtraTimeStart { get; set; } = "AthenaMatchInProgress";
    
        [G_NS.JsonProperty( Order = 27, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationExtraTimeStop { get; set; } = "AthenaOnlyResultList";
    
        [G_NS.JsonProperty( Order = 28, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationUnscheduledCeaseFire { get; set; } = "AthenaCeaseFire";
    
        [G_NS.JsonProperty( Order = 29, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationBoatInImpactArea { get; set; } = "AthenaQualificationBoatInImpactArea";
    
        [G_NS.JsonProperty( Order = 30, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationAlibiStart { get; set; } = "AthenaMatchInProgress";
    
        [G_NS.JsonProperty( Order = 31, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string QualificationAlibiStop { get; set; } = "AthenaOnlyResultList";
    
        
        [G_NS.JsonProperty( Order = 51, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalStart { get; set; } = "AthenaFinalCallToLine";
    
        [G_NS.JsonProperty( Order = 52, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalStop { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 53, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalPreEvent { get; set; } = "AthenaFinalCallToLine";
    
        [G_NS.JsonProperty( Order = 54, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalPostEvent { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 55, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalCallToLine { get; set; } = "AthenaFinalCallToLine";
    
        [G_NS.JsonProperty( Order = 56, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalRemoveEquipment { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 57, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalPreparationPeriodStart { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 58, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalPreparationPeriodStop { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 59, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalSightersStart { get; set; } = "AthenaFinalAthleteDisplay";
    
        [G_NS.JsonProperty( Order = 60, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalSightersStop { get; set; } = "AthenaFinalAthleteDisplay";
    
        [G_NS.JsonProperty( Order = 61, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalAthleteIntroductionStart { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 62, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalAthleteIntroductionStop { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 63, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalStageStart { get; set; } = "AthenaFinalAthleteDisplay";
    
        [G_NS.JsonProperty( Order = 64, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalStageStop { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 65, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalCommentary { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 66, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalEliminationStageStart { get; set; } = "AthenaFinalAthleteDisplay";
    
        [G_NS.JsonProperty( Order = 67, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalEliminationStageStop { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 68, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalChangeOverStart { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 69, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalChangeOverStop { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 70, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalAthleteEliminated { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 71, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalThirdPlaceAnnounced { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 72, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalSecondPlaceAnnounced { get; set; } = "AthenaFinalResultList";
    
        [G_NS.JsonProperty( Order = 73, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string FinalFirstPlaceAnnounced { get; set; } = "AthenaFinalResultList";
    

        [G_NS.JsonProperty( Order = 91, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string SpecialEventOne { get; set; } = "AthenaOnlyResultList";
    
        [G_NS.JsonProperty( Order = 92, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string SpecialEventTwo { get; set; } = "AthenaOnlyResultList";
    
        [G_NS.JsonProperty( Order = 93, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string SpecialEventThree { get; set; } = "AthenaOnlyResultList";

        [G_NS.JsonProperty( Order = 94, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public string SpecialEventFour { get; set; } = "AthenaOnlyResultList";


        public void Merge( EventAssignments other ) {
            foreach (PropertyInfo prop in typeof( EventAssignments ).GetProperties()) {
                string value = (string)prop.GetValue( other );
                if (!string.IsNullOrEmpty( value )) // Only overwrite if not null or empty
                {
                    prop.SetValue( this, value );
                }
            }
        }

        /// <summary>
        /// Returns a list of view configuration names, representing the unique list of view configurations
        /// listed in this Event Assignments file.
        /// </summary>
        /// <returns></returns>
        public List<string> GetViewConfigurationNames() {
            List<string> viewConfigNames = new List<string>();

            foreach (PropertyInfo prop in typeof( EventAssignments ).GetProperties()) {
                string viewConfigName = (string)prop.GetValue( this );
                if (!viewConfigNames.Contains( viewConfigName ) ) 
                {
                    viewConfigNames.Add( viewConfigName );
                }
            }

            return viewConfigNames;
        }

    }
}
