using System;
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

        public string EventAppellation { get; private set; }

        public string StageAppellation { get; private set; }

        public EventComposite Parent { get; private set; }

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
                if (t.Values != "" && t.Values != null){ //needs generating
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
                EventAppellation = topLevelEvent.EventStyleMapping.EventAppellation
                //StageAppellation = topLevelEvent.StageStyleMapping.StageAppellation
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
                    foreach( var childName in t.GetChildrenEventNames() ) {
                        EventComposite child;
                        if (t.EventType == EventtType.STAGE) {
                            child = new EventComposite() {
                                EventName = childName,
                                Parent = parent,
                                StageAppellation = parent.StageAppellation,
                                EventAppellation = parent.EventAppellation
                            };
                        } else {
                            child = new EventComposite() {
                                EventName = childName,
                                Parent = parent,
                                EventAppellation = parent.EventAppellation
                            };
                        }
                        parent.Children.Add( child );

                        GrowChildren( cof, child, level );
                    }
                    return;
                }
            }

            //If we get here, then the parent must be a Singular ... or at least *should* be a Singular
            parent.EventType = EventtType.SINGULAR;
        }

    }
}
