﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irc.Constants;

namespace Irc.Extensions.Props.Channel
{
    internal class Onpart : PropRule
    {
        // The ONPART channel property contains a string that is sent (via NOTICE) to a user after they have parted from the channel.
        // The channel name is displayed as the sender of the message. Only the user parting the channel will see message.
        // Multiple lines can be generated by embedding '\n' in the string. The ONPART property is limited to 255 characters.

        public Onpart() : base(ExtendedResources.ChannelPropOnPart, EnumChannelAccessLevel.ChatHost, EnumChannelAccessLevel.ChatHost, Resources.ChannelPropOnpartRegex, string.Empty, false)
        {

        }
    }
}
