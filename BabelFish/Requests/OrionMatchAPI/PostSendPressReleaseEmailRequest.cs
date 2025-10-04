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

        public bool Resend { get; set; } = false;

        public bool TestOnly { get; set; } = false;

        public string Subject { get; set; } = string.Empty;

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

        public override StringContent PostParameters {
            get {
                StringBuilder serializedJSON = new StringBuilder();
                try {
                    SendPressReleaseEmailPostParameters foo = new SendPressReleaseEmailPostParameters();
                    return new StringContent( G_NS.JsonConvert.SerializeObject( foo, SerializerOptions.NewtonsoftJsonSerializer ), Encoding.UTF8, "application/json" );
                } catch (Exception ex) {
                    return new StringContent( "" );
                }
            }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrEmpty( GameID )) {
                    parameterList.Add( "game-id", new List<string> { GameID } );
                }

                parameterList.Add( "resend", new List<string> { Resend.ToString() } );

                if (SendTo != null && SendTo.Count > 0) {
                    parameterList.Add( "send-to", SendTo );
                }

                if ( TestOnly ) {
                    parameterList.Add( "test-only", new List<string> { TestOnly.ToString() } );
                }

                if (!string.IsNullOrEmpty( Subject )) {
                    parameterList.Add( "subject", new List<string> { GameID } );
                }

                return parameterList;
            }
        }
    }

    public class SendPressReleaseEmailPostParameters {


    }
}
