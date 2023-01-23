using System;
using System.Web;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Runtime.Authentication {

    public static class AwsSigner {
        /////////////
        // https://docs.aws.amazon.com/general/latest/gr/signing_aws_api_requests.html
        // https://docs.aws.amazon.com/general/latest/gr/signature-version-4.html
        // https://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html
        // You can use temporary security credentials provided by the AWS Security Token Service(AWS STS)
        //  to sign a request.The process is the same as using long-term credentials, but when you add
        //  signing information to the query string you must add an additional query parameter for the security token.
        //  The parameter name is X-Amz-Security-Token, and the parameter's value is the URI-encoded session token
        //  (the string you received from AWS STS when you obtained temporary security credentials).
        /////////////

        /// <summary>
        /// RichW's custom code (URL requires privs to Shooter's Tech GitHub)
        /// https://github.com/shooterstech/Genesis/blob/36be2492dc137d03b1e74c06c14ddbdcf71ce6b7/Genesis.Prime/Services/MatchSearchService.cs
        /// </summary>
        /// <returns>HttpResponseMessage object to parse</returns>
        [Obsolete( "Use https://github.com/FantasticFiasco/aws-signature-version-4 instead." )]
        public static void SignRequest( HttpRequestMessage msg, UserAuthentication user ) {

            user.GenerateIAMCredentials();

            string awsRegion = "us-east-1";
            string awsServiceName = "execute-api";
            string awsSecretKey = user.SecretKey;
            DateTimeOffset utcNowSaved = DateTimeOffset.UtcNow;
            string amzLongDate = utcNowSaved.ToString( "yyyyMMddTHHmmssZ" );
            string amzShortDate = utcNowSaved.ToString( "yyyyMMdd" );
            string url = msg.RequestUri.AbsoluteUri;

            //msg.Headers.Host = msg.RequestUri.Host;
            msg.Headers.Add( "x-amz-date", amzLongDate );

            //The following line should be ignored, if we're using permanent IAM credntials. However BabelFish was written with the idea of using only tempoary credentials.
            msg.Headers.Add( "X-Amz-Security-Token", user.SessionToken );

            if (msg.Content == null)
                msg.Content = new StringContent( "" );
            var payloadHash = Hash( msg.Content.ReadAsByteArrayAsync().Result );
            msg.Headers.Add( "x-amz-content-sha256", payloadHash );

            // Create Canonical Request
            var canonicalRequest = new StringBuilder();
            var method = msg.Method.ToString();
            canonicalRequest.AppendLine( msg.Method.ToString() );
            canonicalRequest.AppendLine( string.Join( "/", msg.RequestUri.AbsolutePath.Split( '/' ).Select( Uri.EscapeDataString ) ) );
            canonicalRequest.AppendLine( GetCanonicalQueryParams( msg ) ); // Query params to do.
            List<string> headersToBeSigned = new List<string>();
            foreach (var header in msg.Headers.OrderBy( a => a.Key.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase )) {
                canonicalRequest.Append( header.Key.ToLowerInvariant() );
                canonicalRequest.Append( ":" );
                canonicalRequest.Append( string.Join( ",", header.Value.Select( s => s.Trim() ) ) );
                canonicalRequest.AppendLine();
                headersToBeSigned.Add( header.Key.ToLowerInvariant() );
            }
            canonicalRequest.AppendLine();
            var signedHeaders = string.Join( ";", headersToBeSigned );
            canonicalRequest.AppendLine( signedHeaders );
            canonicalRequest.Append( payloadHash );
            // String to sign
            string stringToSign = $"AWS4-HMAC-SHA256\n{amzLongDate}\n{amzShortDate}/{awsRegion}/{awsServiceName}/aws4_request\n{Hash( Encoding.UTF8.GetBytes( canonicalRequest.ToString() ) )}";
            var dateKey = HmacSha256( Encoding.UTF8.GetBytes( $"AWS4{awsSecretKey}" ), amzShortDate );
            var dateRegionKey = HmacSha256( dateKey, awsRegion );
            var dateRegionServiceKey = HmacSha256( dateRegionKey, awsServiceName );
            var signingKey = HmacSha256( dateRegionServiceKey, "aws4_request" );
            var signature = ToHexString( HmacSha256( signingKey, stringToSign ) );
            var credentialScope = $"{amzShortDate}/{awsRegion}/{awsServiceName}/aws4_request";
            msg.Headers.TryAddWithoutValidation( "Authorization",
                $"AWS4-HMAC-SHA256 Credential={user.AccessKey}/{credentialScope}, SignedHeaders={signedHeaders}, Signature={signature}" );
        }

        private static string GetCanonicalQueryParams( HttpRequestMessage request ) {
            var values = new SortedDictionary<string, string>();

            //AKS-20220324 Refactor for multiple values per key
            var querystring = HttpUtility.ParseQueryString( request.RequestUri.Query );
            foreach (var key in querystring.AllKeys) {
                if (key == null) { // Handles keys without values
                    values.Add( Uri.EscapeDataString( querystring[key] ), $"{Uri.EscapeDataString( querystring[key] )}=" );
                } else {
                    foreach (var value in querystring.GetValues( key )) {
                        // Escape to upper case. Required.
                        values.Add( Uri.EscapeDataString( key ) + value.ToString(), $"{Uri.EscapeDataString( key )}={Uri.EscapeDataString( value )}" );
                    }
                }
            }
            // Put in order - this is important.
            var queryParams = values.Select( a => a.Value );
            return string.Join( "&", queryParams );
        }
        private static string Hash( byte[] bytesToHash ) {
            return ToHexString( SHA256.Create().ComputeHash( bytesToHash ) );
        }
        private static string ToHexString( IReadOnlyCollection<byte> array ) {
            var hex = new StringBuilder( array.Count * 2 );
            foreach (var b in array) {
                hex.AppendFormat( "{0:x2}", b );
            }
            return hex.ToString();
        }
        private static byte[] HmacSha256( byte[] key, string data ) {
            return new HMACSHA256( key ).ComputeHash( Encoding.UTF8.GetBytes( data ) );
        }
    }

}