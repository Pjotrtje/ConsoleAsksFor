﻿using System;

namespace ConsoleAsksFor;

internal interface IDateTimeProvider
{
    DateTime Now { get; }
}