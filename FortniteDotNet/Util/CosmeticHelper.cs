﻿using System;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using FortniteDotNet.Models;
using System.Threading.Tasks;

namespace FortniteDotNet.Util
{
    internal class CosmeticHelper
    {
        internal static async Task<Cosmetic> GetCosmeticByName(string name, string type)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("lang", "en");
            query.Add("searchLang", "en");
            query.Add("matchMethod", "starts");
            query.Add("name", name);
            query.Add("backendType", type);

            try
            {
                using var client = new WebClient();
                var response = await client.DownloadStringTaskAsync($"https://fortnite-api.com/v2/cosmetics/br/search?{query}").ConfigureAwait(false);
                return JsonConvert.DeserializeObject<FortniteAPIResponse<Cosmetic>>(response).Data;
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    return default;
                
                using var reader = new StreamReader(ex.Response.GetResponseStream());
                var body = await reader.ReadToEndAsync().ConfigureAwait(false);
                var error = JsonConvert.DeserializeObject<FortniteAPIResponse<Cosmetic>>(body).Error;
                throw new Exception(error);
            }
        }
    }
}