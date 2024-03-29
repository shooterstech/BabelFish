﻿using System;
using System.Collections.Generic;
using System.Text;
using Amazon.Util;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class EventComposite {

        public EventComposite() {
            this.Children = new List<EventComposite>();
            this.Parent = null;
            this.EventType = EventtType.NONE;
        }

        public string EventName { get; private set; }  

        public EventtType EventType { get; private set; }

        public List<EventComposite> Children { get; private set; }

        public string EventAppellation {
            get {
                if (EventStyleMapping != null)
                    return EventStyleMapping.EventAppellation;
                return null;
            }
        }

        public string StageAppellation { get {
                if (StageStyleMapping != null)
                    return StageStyleMapping.StageAppellation;
                return null;
            }
        }

        public EventStyleMapping EventStyleMapping { get; private set; }

        public StageStyleMapping StageStyleMapping { get; private set; }

        public EventComposite Parent { get; private set; }

        public string ScoreFormat { get; private set; }

        public bool HasChildren {
            get {
                return Children.Count > 0;
            }
        }

        public bool HasParent {
            get {
                return Parent != null;
            }
        }

        /// <summary>
        /// Returns a list of all descendants that are Singulars
        /// </summary>
        public List<EventComposite> GetAllSingulars() {
            return GetEvents(false,false,false,false,false,true);
        }
        public List<EventComposite> GetEvents(bool none = true, bool @event = true, bool stage = true, bool series = true, bool @string = true, bool singular = true) {
            //NONE, EVENT, STAGE, SERIES, STRING, SINGULAR
            List<EventComposite> descendants = new List<EventComposite>();

            if (this.EventType == EventtType.NONE && none) {
                descendants.Add(this);
            }
            if (this.EventType == EventtType.EVENT && @event) {
                descendants.Add(this);
            }
            if (this.EventType == EventtType.STAGE && stage) {
                descendants.Add(this);
            }
            if (this.EventType == EventtType.SERIES && series) {
                descendants.Add(this);
            }
            if (this.EventType == EventtType.STRING && @string) {
                descendants.Add(this);
            }
            if (this.EventType == EventtType.SINGULAR && singular) {
                descendants.Add(this);
            }
            foreach (var child in Children) {
                descendants.AddRange( child.GetEvents(none, @event, stage, series, @string, singular ) );
            }

            return descendants;
        }
        public EventComposite FindEventComposite( string eventName ) {

            if ( this.EventName == eventName ) return this;

            foreach (var child in Children) {
                var foo = child.FindEventComposite( eventName );
                if (foo != null) return foo;
            }

            return null;
        }

        public override string ToString() {
            return $"{EventName} of {EventType}";
        }

        private static void GenerateChildStage(Event gEvent, int stageValue){
            if (gEvent.Children is JArray)
                return;

            if (gEvent.Children.ContainsKey("String")){
                int strValue = (int)gEvent.Children["String"];
                if (strValue <= 0) return; // nothing to do
                int startVal = strValue * (stageValue - 1) + 1;
                int endVal = startVal - 1 + strValue;

                gEvent.Children["String"] = -1;
                gEvent.Children["Values"] = String.Format("{0}..{1}", startVal, endVal);
            }
        }

        private static void GenerateEvents(CourseOfFire cof){
            List<Event> generatedEventList = new List<Event>();
            foreach( Event t in cof.Events ) {
                if (!string.IsNullOrEmpty( t.Values ) ){ //needs generating
                    ValueSeries vs = new ValueSeries((string)t.Values);
                    var valueList = vs.GetAsList();

                    foreach (int i in valueList) { //i is stageValue
                        Event newEvent = t.Clone();
                        newEvent.EventName = newEvent.EventName.Replace("{}", i.ToString());
                        newEvent.Values = "";
                        //makes the "K1" "K2" "K3" (...) events
                        GenerateChildStage(newEvent, i);

                        generatedEventList.Add(newEvent);
                    }
                }
                else {
                    generatedEventList.Add(t);
                }
            }

            cof.Events = generatedEventList;
        }

        private static void GenerateSingulars(CourseOfFire cof ) {
            List<Singular> generatedSingulars = new List<Singular>();
            foreach(  Singular s in cof.Singulars ) {
                if (!string.IsNullOrEmpty( s.Values) ) { //Needs generating
                    ValueSeries vs = new ValueSeries((string) s.Values);
                    var valueList = vs.GetAsList();
                    foreach( int i in valueList ) {
                        Singular newSingular = s.Clone();
                        s.EventName = s.EventName.Replace("{}", i.ToString());
                        s.Values = "";

                        generatedSingulars.Add(newSingular);
                    }
                } else {
                    generatedSingulars.Add( s );
                }
            }

            cof.Singulars = generatedSingulars;
        }

        public static EventComposite GrowEventTree( CourseOfFire cofRef ) {

            CourseOfFire cof = cofRef.Clone(); //dont modify passed in cof
            GenerateEvents(cof);

            EventComposite top;
            Event topLevelEvent = null;

            //Identify the top level event
            //needed for the SINGLE EVENT WE HAVE
            foreach( Event t in cof.Events ) {
                if ( t.EventType == EventtType.EVENT ) {
                    topLevelEvent = t;
                    break;
                }
            }
            if (topLevelEvent == null)
                throw new ScoposException( "Could not identify a top level event, with Event Type == Event." );
            //add the event to the event tree, this is the root.
            top = new EventComposite() {
                EventName = topLevelEvent.EventName,
				EventStyleMapping = topLevelEvent.EventStyleMapping,
                ScoreFormat = topLevelEvent.ScoreFormat
            };
            //this should be where we go through 
            GrowChildren( cof, top, 0 );

            return top;
        }

        private static void GrowChildren( CourseOfFire cof, EventComposite parent, int level ) {

            if (level > 10)
                throw new ScoposException( "Recursion level exceeded expectations." );
            level++;

            foreach ( Event t in cof.Events ) {
                if (t.EventName == parent.EventName ) {
                    parent.EventType = t.EventType;
					parent.ScoreFormat = t.ScoreFormat;
                    if (t.EventType == EventtType.STAGE)
                        parent.StageStyleMapping = t.StageStyleMapping;
					foreach ( var childName in t.GetChildrenEventNames() ) {
                        EventComposite child;
                        if (t.EventType == EventtType.STAGE) {
                            child = new EventComposite() {
                                EventName = childName,
                                Parent = parent
                            };
                        } else {
                            child = new EventComposite() {
                                EventName = childName,
                                Parent = parent
                            };
                        }
                        parent.Children.Add( child );

                        GrowChildren( cof, child, level );
                    }
                    return;
                }
            }

            //If we get here, then the parent must be a Singular ... or at least *should* be a Singular
            foreach( Singular s in cof.Singulars ) {
				parent.EventType = EventtType.SINGULAR;
                parent.ScoreFormat = s.ScoreFormat;
                return;
			}

            //We shouldn't get here if the cof is defined correctly.
            parent.EventType = EventtType.SINGULAR;
            parent.ScoreFormat = "Shot";
        }

    }
}
