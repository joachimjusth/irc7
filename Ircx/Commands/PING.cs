﻿using Core.Ircx.Objects;

namespace Core.Ircx.Commands;

internal class PING : Command
{
    public PING(CommandCode Code) : base(Code)
    {
        MinParamCount = 1;
        DataType = CommandDataType.None;
        ForceFloodCheck = true;
    }

    public new COM_RESULT Execute(Frame Frame)
    {
        Frame.User.Send(Raws.Create(Frame.Server, Client: Frame.User, Raw: Raws.RPL_PONG));
        return COM_RESULT.COM_SUCCESS;
    }
}