using System;
using System.Collections.Generic;
using System.Text;

namespace BookReadingEvents.Domain.Enums
{
    public enum EventType // (Enumeration -> a fixed list of allowed values) made this enum to reduce database inconsistencies and to make it easier to manage event types in the future and scale it if needed.
    {
        Public = 0, Private = 1 // we wrote 0 and 1 just for readability, but we can omit them and the compiler will assign them automatically starting from 0
    }
}
