﻿using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FortniteDotNet.Models.Fortnite.Calendar
{
    public class TimelineChannel
    {
        [JsonProperty("states")]
        public List<ChannelState> States { get; set; }

        [JsonProperty("cacheExpire")]
        public DateTime CacheExpire { get; set; }
    }
}