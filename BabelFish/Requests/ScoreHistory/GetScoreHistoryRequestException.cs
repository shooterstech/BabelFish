using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.Requests;

namespace ShootersTech.Requests.ScoreHistory {
    public class GetScoreHistoryRequestException : RequestException {
        public GetScoreHistoryRequestException()
            : base( "Something bad happened!" ) {
        }
        public GetScoreHistoryRequestException( string message )
            : base( message ) {
        }
        public GetScoreHistoryRequestException( string message, Exception inner )
            : base( message, inner ) {
        }
    }
}
