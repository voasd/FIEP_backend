﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.DTO
{
    public class EventDTO
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime TimeOccur { get; set; }
        public string EventImageUrl { get; set; }
        public int Follower { get; set; }
        public GroupDTO Group { get; set; }
        public string Location { get; set; }
    }
}
