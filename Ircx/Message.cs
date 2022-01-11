﻿using System.Collections.Generic;
using Core.CSharpTools;

namespace Core.Ircx;

public class Message
{
    public static short whiteSpace = 0x20;

    public List<string> Data;

    // Temp stuff
    public int ParamOffset;

    public string rawData, Prefix, Command;

    // Temp stuff
    public Message(string incomingData)
    {
        rawData = incomingData;
        Parse();
    }

    public string GetNextParam()
    {
        if (ParamOffset < Data.Count)
            return Data[ParamOffset++];
        return null;
    }

    private void Parse()
    {
        if (rawData == null) return;
        if (rawData.Length == 0) return;

        var offset = 0;

        for (; offset < rawData.Length && rawData.ToByteArray()[offset] == whiteSpace; offset++) ; //trimming

        if (rawData.ToByteArray()[offset] == ':')
        {
            Prefix = GetWord(++offset);
            //if (Prefix == null) return;
            if (Prefix != null) offset += Prefix.Length + 1;
        }

        while (Command == null && offset < rawData.Length)
        {
            Command = GetWord(offset);
            offset++;
        }

        if (Command == null) return;
        Command = Command.ToUpper();


        offset += Command.Length;

        Data = GetParams(offset);
    }

    private string GetWord(int offset)
    {
        int c;
        for (c = offset; c < rawData.Length; c++)
            if (rawData.ToByteArray()[c] == whiteSpace)
            {
                if (c - offset <= 1)
                    return null;
                return StringBuilderExtensions.FromBytes(rawData.ToByteArray(), offset, c).ToString();
            }

        if (c == rawData.Length)
            return StringBuilderExtensions.FromBytes(rawData.ToByteArray(), offset, c).ToString();
        return null;
    }

    private List<string> GetParams(int offset)
    {
        if (rawData.ToByteArray().Length - offset <= 0) return null;

        var Data = new List<string>();

        if (rawData.ToByteArray()[offset] == 58 && offset + 1 < rawData.ToByteArray().Length)
        {
            Data.Add(StringBuilderExtensions.FromBytes(rawData.ToByteArray(), offset + 1, rawData.ToByteArray().Length)
                .ToString());
            return Data;
        }

        if (rawData.ToByteArray()[offset] == 58 && offset + 1 == rawData.ToByteArray().Length)
        {
            Data.Add(string.Empty);
        }
        else
        {
            for (var i = offset; i < rawData.ToByteArray().Length; i++)
                if (rawData.ToByteArray()[i] == whiteSpace)
                {
                    if (i - offset > 0)
                        Data.Add(StringBuilderExtensions.FromBytes(rawData.ToByteArray(), offset, i).ToString());

                    if (i + 1 < rawData.ToByteArray().Length)
                        if (rawData.ToByteArray()[i + 1] == 0x3A) //check if next parameter has a :
                        {
                            //if (i + 2 == rawData.bytes.Length) { Data.Add(Resources.Null); } // Fix for blank messages

                            offset = i + 2; // if so break and catch the rest and save as param
                            break;
                        }

                    offset = i + 1;
                }

            Data.Add(StringBuilderExtensions.FromBytes(rawData.ToByteArray(), offset, rawData.ToByteArray().Length)
                .ToString());
        }

        return Data;
    }
}