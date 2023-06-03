using System;
using System.Collections.Generic;
using System.Text;
using Amazon.Util;
using Scopos.BabelFish.Runtime;

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

            List<EventComposite> descendants = new List<EventComposite>();

            if (this.EventType == EventtType.SINGULAR) {
                descendants.Add( this );
            } else {
                foreach (var child in Children) {
                    descendants.AddRange( child.GetAllSingulars() );
                }
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

        public static EventComposite GrowEventTree( CourseOfFire cof ) {

            EventComposite top;
            Event topLevelEvent = null;

            //Identify the top level event
            foreach( Event t in cof.Events ) {
                if ( t.EventType == EventtType.EVENT ) {
                    topLevelEvent = t;
                    break;
                }
            }
            if (topLevelEvent == null)
                throw new ScoposException( "Could not identify a top level event, with Event Type == Event." );

            top = new EventComposite() {
                EventName = topLevelEvent.EventName
            };

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
                        EventComposite child = new EventComposite() {
                            EventName = childName,
                            Parent = parent
                        };
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
