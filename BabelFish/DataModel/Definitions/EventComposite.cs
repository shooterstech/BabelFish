using System;
using System.Collections.Concurrent;
using System.Text;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// An abstract term that represents both Events from a COURSE OF FIRE and Singulars from a COURSE OF FIRE. 
    /// Event Composites can have one parent Event Composite and multiple children Event Composites.
    /// <para>Use the GrowEventTree() method to determine the Event Tree from a COURSE OF FIRE.</para>
    /// </summary>
    public class EventComposite : IEquatable<EventComposite> {

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

        /// <summary>
        /// For an event composite, almost always "SUM"
        /// For a singularity, which doesn't have children it will be an empty string ""
        /// </summary>
        public EventCalculation Calculation { get; private set; } = EventCalculation.NONE;

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

        /// <summary>
        /// Returns a list of EvenComposites, that are children of this, and that have an EventType equal to one of the values passed in.
        /// </summary>
        /// <param name="none">No EventType listed.</param>
        /// <param name="event">EventType == EVENT</param>
        /// <param name="stage">EventType == STAGE</param>
        /// <param name="series">EventType == SERIES</param>
        /// <param name="string">EventType == STRING</param>
        /// <param name="singular">EventType == SINGULAR</param>
        /// <returns></returns>
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

        /// <summary>
        /// Searches the children of this Event Composite, for one with the passed in eventName and returns it.
        /// Returns null, if it could not be found.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public EventComposite FindEventComposite( string eventName ) {

            if ( this.EventName == eventName ) return this;

            foreach (var child in Children) {
                var foo = child.FindEventComposite( eventName );
                if (foo != null) return foo;
            }

            return null;
        }

        /// <inheritdoc/>
        public override string ToString() {
            return $"{EventName} of {EventType}";
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            //Within a COF definition EventName shold be unique and we could use it to singularly
            //However, if we wanted it to me more universally unique, we need to include toher member fields ... of which is no guarantee
            int hash = this.EventName.GetHashCode();
            hash ^= this.EventType.GetHashCode();
            foreach (var child in this.Children) 
                hash ^= child.GetHashCode();
            hash ^= this.ScoreFormat.GetHashCode();
            hash ^= this.Calculation.GetHashCode();

            return hash;
        }

        private static ConcurrentDictionary<string, EventComposite> eventCompositeCache = new ConcurrentDictionary<string, EventComposite> ();

        /// <summary>
        /// Generates the Event tree as defined by the passed in Course of Fire definition. 
        /// the EventComposite that is passed back, is the top level Event in the tree.
        /// </summary>
        /// <param name="cofRef"></param>
        /// <returns></returns>
        /// <exception cref="ScoposException"></exception>
        public static EventComposite GrowEventTree( CourseOfFire cofRef ) {

            //Because growing the event tree can take time, and its an operation that's often repeated, we will try
            //and store cache copies of it and return the cached value first, before growing a new one.
            EventComposite ec;
            if ( eventCompositeCache.TryGetValue( cofRef.SetName, out ec ) ) { 
                return ec; 
            }

            //Dont modify passed in cof
            //And don't clone the whole COF, which can be SLOW
            //Besure to exclude Events that are known to be outside the event tree.
            var listOfEvents = new List<Event>();
            foreach( var origEvent in cofRef.Events ) {
                foreach( var cloneEvent in origEvent.GetCompiledEvents() ) {
                    if ( ! cloneEvent.ExternalToEventTree ) {
                        listOfEvents.Add( cloneEvent );
                    }
                }
            }

            EventComposite top;
            Event topLevelEvent = null;

            //Identify the top level event
            //needed for the SINGLE EVENT WE HAVE
            foreach( Event t in listOfEvents) {
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
                ScoreFormat = topLevelEvent.ScoreFormat,
                Calculation = topLevelEvent.Calculation
            };
            //this should be where we go through 
            GrowChildren( listOfEvents, cofRef.Singulars, top, 0 );

            //Store in cache, so we can use later
            eventCompositeCache.TryAdd( cofRef.SetName, top );

            return top;
        }

        private static void GrowChildren( List<Event> listOfEvents, List<Singular> singulars, EventComposite parent, int level ) {

            if (level > 10)
                throw new ScoposException( "Recursion level exceeded expectations." );
            level++;

            foreach ( Event t in listOfEvents) {
                if (t.EventName == parent.EventName ) {
                    parent.EventType = t.EventType;
					parent.ScoreFormat = t.ScoreFormat;
                    parent.Calculation = t.Calculation;

                    if (t.EventType == EventtType.STAGE)
                        parent.StageStyleMapping = t.StageStyleMapping;
					foreach ( var childName in t.Children ) {
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

                        GrowChildren( listOfEvents, singulars, child, level );
                    }
                    return;
                }
            }

            //If we get here, then the parent must be a Singular ... or at least *should* be a Singular
            foreach( Singular s in singulars ) {
				parent.EventType = EventtType.SINGULAR;
                parent.ScoreFormat = s.ScoreFormat;
                parent.Calculation = EventCalculation.NONE;
                return;
			}

            //We shouldn't get here if the cof is defined correctly.
            parent.EventType = EventtType.SINGULAR;
            parent.ScoreFormat = "Shots";
        }
        
        /// <inheritdoc/>
        public bool Equals( EventComposite other ) {
            return this.GetHashCode() == other.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals( object obj ) {
            if ( obj == null || obj is not EventComposite ) 
                return false;

            return Equals( obj as EventComposite );
        }
    }
}
