﻿using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using FortniteDotNet.Enums.Party;
using FortniteDotNet.Models.Accounts;
using FortniteDotNet.Services;

namespace FortniteDotNet.Models.Party
{
    public class PartyInfo
    {
        [JsonProperty("config")]
        public Dictionary<string, object> Config { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("applicants")]
        public List<string> Applicants { get; set; }
        
        [JsonProperty("members")]
        public List<PartyMember> Members { get; set; }

        [JsonProperty("meta")]
        public Dictionary<string, string> Meta { get; set; }

        [JsonProperty("invites")]
        public List<PartyInvite> Invites { get; set; }

        [JsonProperty("revision")]
        public int Revision { get; set; }

        [JsonProperty("intentions")]
        public object[] Intentions { get; set; }

        public async Task UpdatePrivacy(OAuthSession oAuthSession, PartyPrivacy partyPrivacy)
        {
            Dictionary<string, object> updated = new();
            List<string> deleted = new();

            if (Meta.ContainsKey("Default:PrivacySettings_j"))
            {
                updated["Default:PrivacySettings_j"] = Meta["Default:PrivacySettings_j"] = JsonConvert.SerializeObject(new PrivacySettings
                {
                    PartyType = partyPrivacy.PartyType,
                    OnlyLeaderFriendsCanJoin =  partyPrivacy.OnlyLeaderFriendsCanJoin,
                    PartyInviteRestriction = partyPrivacy.InviteRestriction
                });
            }

            updated["urn:epic:cfg:presence-perm_s"] = Meta["urn:epic:cfg:presence-perm_s"] = partyPrivacy.PresencePermission;
            updated["urn:epic:cfg:accepting-members_b"] = Meta["urn:epic:cfg:accepting-members_b"] = partyPrivacy.AcceptingMembers.ToString();
            updated["urn:epic:cfg:invite-perm_s"] = Meta["urn:epic:cfg:invite-perm_s"] = partyPrivacy.InvitePermission;

            if (partyPrivacy.PartyType == "Private")
            {
                deleted.Add("urn:epic:cfg:not-accepting-members");
                updated["urn:epic:cfg:not-accepting-members-reason_i"] = Meta["urn:epic:cfg:not-accepting-members-reason_i"] = "7";
                Config["discoverability"] = "INVITED_ONLY";
                Config["joinability"] = "INVITE_AND_FORMER";
            }
            else
            {
                deleted.Add("urn:epic:cfg:not-accepting-members-reason_i");
                Config["discoverability"] = "ALL";
                Config["joinability"] = "OPEN";
            }

            foreach (var deletedMeta in deleted)
                Meta.Remove(deletedMeta);

            await PartyService.UpdateParty(oAuthSession, this, updated, deleted);
        }

        public void UpdateParty(int revision, Dictionary<string, object> config, Dictionary<string, object> updated = null, List<string> deleted = null)
        {
            if (revision > Revision)
                Revision = revision;
            
            foreach (var deletedMeta in deleted)
                Meta.Remove(deletedMeta);

            foreach (var updatedMeta in updated)
                Meta[updatedMeta.Key] = updatedMeta.Value.ToString();

            Config["joinability"] = config["party_privacy_type"];
            Config["maxSize"] = config["max_number_of_members"];
            Config["subType"] = config["party_sub_type"];
            Config["type"] = config["party_type"];
            Config["inviteTtl"] = config["invite_ttl_seconds"];
            
            if (config.ContainsKey("discoverability"))
                Config["discoverability"] = config["discoverability"];
        }
    }
}