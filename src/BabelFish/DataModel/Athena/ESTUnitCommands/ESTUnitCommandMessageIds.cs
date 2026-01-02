using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.ESTUnitCommands {
    public static class ESTUnitCommandMessageIds {

        //Value is the Message ID. Key is the Payload object
        private static Dictionary<string, ESTUnitCommandMessage> messages = new Dictionary<string, ESTUnitCommandMessage>(); 

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void LogCommandMessage( ESTUnitCommandPayload payload, string estUnitName) {
            var message = new ESTUnitCommandMessage() {
                Payload = payload,
                TimeSent = DateTime.Now,
                ESTUnitName = estUnitName
            };

            messages.Add( payload.MessageID, message );

            logger.Trace( message.ToString() );
        }

        public static ESTUnitCommandMessage GetMessageById( string id ) {
            return messages[id];
        }

        public static bool TryGetMessageById(string id, out ESTUnitCommandMessage message) {
            return messages.TryGetValue(id, out message );
        }
    }
}
