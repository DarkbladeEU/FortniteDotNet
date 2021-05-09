﻿using Newtonsoft.Json;

namespace FortniteDotNet.Models.Party
{
    public class PrivacySettings
    {
        [JsonProperty("partyType")]
        public string PartyType { get; set; }
        
        [JsonProperty("partyInviteRestriction")]
        public string PartyInviteRestriction { get; set; }
        
        [JsonProperty("bOnlyLeaderFriendsCanJoin")]
        public bool OnlyLeaderFriendsCanJoin { get; set; }
    }
}