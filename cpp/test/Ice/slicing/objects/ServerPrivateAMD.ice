// **********************************************************************
//
// Copyright (c) 2003-2012 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************

#pragma once

#include <TestAMD.ice>

module Test
{

class SBSUnknownDerived extends SBase
{
    string sbsud;
};

class SUnknown
{
    string su;
};

class D2 extends B
{
    string sd2;
    B pd2;
};

class D4 extends B
{
    B p1;
    B p2;
};

exception UnknownDerivedException extends BaseException
{
    string sude;
    D2 pd2;
};

};
