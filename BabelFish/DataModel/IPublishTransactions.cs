﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel {

    /// <summary>
    /// Intended for use with publshing messages with lists on IoT. Definines an interface that allows messages
    /// to be broken up and published in smaller chuncks, to re-assembled by the receiver. 
    /// </summary>
    public interface IPublishTransactions {

        /// <summary>
        /// When a message with a list is too large, the list may be published through a series of 
        /// transactions. Each sub-message must have the same PublishTransactionId. It is the responsiblity
        /// of the publisher to ensure this and to break the original message up. The Receiver is
        /// responsible for re-assembling the messages.
        /// 
        /// An empty string value is assumed to mean that the original message was small enought in size that
        /// it didn't need to be broken up. 
        /// </summary>
        public string PublishTransactionId { get; set; }


    }
}
