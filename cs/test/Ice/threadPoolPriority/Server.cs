// **********************************************************************
//
// Copyright (c) 2003-2012 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************

using System;
using System.Diagnostics;
using System.Reflection;

[assembly: CLSCompliant(true)]

[assembly: AssemblyTitle("IceTest")]
[assembly: AssemblyDescription("Ice test")]
[assembly: AssemblyCompany("ZeroC, Inc.")]

public class Server
{
    private static int run(string[] args, Ice.Communicator communicator)
    {
        //
        // We don't want connection warnings because of the timeout test.
        //
        communicator.getProperties().setProperty("Ice.Warn.Connections", "0");
        communicator.getProperties().setProperty("TestAdapter.Endpoints", "default -p 12010 -t 10000:udp");
        communicator.getProperties().setProperty("Ice.ThreadPool.Server.ThreadPriority", "AboveNormal");

        Ice.ObjectAdapter adapter = communicator.createObjectAdapter("TestAdapter");
        adapter.add(new PriorityI(), communicator.stringToIdentity("test"));
        adapter.activate();

        communicator.waitForShutdown();
        return 0;
    }

    public static int Main(string[] args)
    {
        int status = 0;
        Ice.Communicator communicator = null;

#if !COMPACT
        Debug.Listeners.Add(new ConsoleTraceListener());
#endif

        try
        {
            Ice.InitializationData initData = new Ice.InitializationData();
            initData.properties = Ice.Util.createProperties(ref args);
            //
            // It's possible to have batch oneway requests dispatched
            // after the adapter is deactivated due to thread
            // scheduling so we suppress this warning.
            //
            initData.properties.setProperty("Ice.Warn.Dispatch", "0");
            communicator = Ice.Util.initialize(ref args, initData);
            status = run(args, communicator);
        }
        catch(System.Exception ex)
        {
            Console.Error.WriteLine(ex);
            status = 1;
        }

        if(communicator != null)
        {
            try
            {
                communicator.destroy();
            }
            catch(Ice.LocalException ex)
            {
                Console.Error.WriteLine(ex);
                status = 1;
            }
        }

        return status;
    }
}
