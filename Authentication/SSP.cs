﻿using System;

namespace Core.Authentication;
// Lazy implementation of my own idea of SSP

public abstract class SSP
{
    public enum state
    {
        SSP_UNSUPPORTED = -2,
        SSP_UNKNOWN = -1,
        SSP_FAILED = -3,
        SSP_OK = 0,
        SSP_INIT = 1,
        SSP_SEC = 2,
        SSP_EXT = 3,
        SSP_CREDENTIALS = 4,
        SSP_AUTHENTICATED = 5
    }

    public const ulong SIGNATURE = 0x1;
    public static string DOMAIN;
    public static string SupportedPackages;
    public bool guest;
    protected bool IsAuthenticated;
    public ulong memberIdLow, memberIdHigh;
    public string NicknameMask = @"[(\x00-\x2F)(\x3A-\x40)]{1}|^Admin|^Sysop|^Guide";

    public uint
        server_sequence; //1 = Init, 2 = Server Reply, 3 = Client Response, 4 = Waiting for additional information

    public uint server_version; //1 = Not supported, 2 = 42, 3 = 45
    public SSPCredentials UserCredentials;
    public byte[] uuid;
    public bool Authenticated => IsAuthenticated;
    public virtual ulong Signature => 0;

    public abstract string CreateSecurityChallenge(state stage);
    public abstract state InitializeSecurityContext(string data, string ip);
    public abstract state AcceptSecurityContext(string data, string ip);
    public abstract SSP Create();
    public abstract string GetDomain();
    public abstract string GetNickMask();


    public static SSP GetPackage(string Name)
    {
        try
        {
            var type = Type.GetType("Core.Authentication.Package." + Name);
            var ssp = (SSP) Activator.CreateInstance(type);
            return ssp;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public static void EnumerateSupportPackages()
    {
        // To do later as it seems hard to enumerate in .NET Core
        SupportedPackages = "NTLM,GateKeeper,ANON";
    }
}