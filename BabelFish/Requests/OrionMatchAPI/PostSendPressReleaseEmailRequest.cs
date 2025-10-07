using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class PostSendPressReleaseEmailRequest : Request {

        public PostSendPressReleaseEmailRequest( UserAuthentication credentials ) : base( "SendPressReleaseEmail", credentials ) {
            HttpMethod = HttpMethod.Post;
        }

        public string GameID { get; set; }

        public string LeagueID { get; set; }

        /// <summary>
        /// Resend must be true to send the press release email to the team's distribution list more than once.
        /// This is meant to prevent unintended spamming of users.
        /// </summary>
        public bool Resend { get; set; } = false;


        /// <summary>
        /// If set to true, no emails are sent. But the response mimicks the response as if they were.
        /// </summary>
        public bool TestOnly { get; set; } = false;

        /// <summary>
        /// The subject to use on the press release email. If null or an empty string a default
        /// subject is used instead. Typically "Press Release Game Recap: [name of game]
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// If true, the press release will be sent to the email address each team, in the game,
        /// have on their distribution list.
        /// <para>After the first time the press release is sent to the distribution list, it can 
        /// not be resent, unless Resend is also true. This is meant to prevent unintended spamming of 
        /// users.</para>
        /// </summary>
        public bool SendToDistribution { get; set; } = false;

        /// <summary>
        /// Email addresses to send to, in addition to the distribution lists for the teams.
        /// <para>When TestOnly is true, only send to these email addresses</para>
        /// </summary>
        public List<string> SendTo { get; set; } = new List<string>();

        /// <inheritdoc />
        public override string RelativePath {
            get {
                return $"/league/{LeagueID}/press-release";
            }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                //NOTE: This is a POST rest api, which means typically parameters are sent on PostParameters. I'm just choosing to send them on the query string instead.

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (string.IsNullOrEmpty( GameID )) {
                    throw new ArgumentNullException( $"The GameId (aka MatchId) must be set in order to request the press release to be sent" );
                } else { 
                    parameterList.Add( "game-id", new List<string> { GameID } );
                }

                if (!string.IsNullOrEmpty( Subject )) {
                    parameterList.Add( "subject", new List<string> { GameID } );
                }

                if (SendTo != null && SendTo.Count > 0) {
                    parameterList.Add( "send-to", SendTo );
                }

                if (SendToDistribution) {
                    parameterList.Add( "test-only", new List<string> { TestOnly.ToString() } );
                }

                if (Resend) {
                    parameterList.Add( "resend", new List<string> { Resend.ToString() } );
                }

                if ( TestOnly ) {
                    parameterList.Add( "test-only", new List<string> { TestOnly.ToString() } );
                }

                return parameterList;
            }
        }
    }

    public class SendPressReleaseEmailPostParameters {


    }
}
