﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Domain.Calendars
{
    public class CalendarId : SaaSOvation.Common.Domain.Identity
    {
        public CalendarId() { }

        public CalendarId(string id)
            : base(id)
        {
        }
    }
}
