using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EveSettingsSaviour.Models.ESI;

namespace EveSettingsSaviour.Helpers
{
    public static class ESI
    {
        static readonly HttpClient client = new HttpClient();
        public static async Task<Character> GetCharacter(int id, string server = "tranquility")
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://esi.evetech.net/latest/characters/{id}/?datasource={server}").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var character = JsonSerializer.Deserialize<Character>(responseBody);
                character.Id = id;
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                //Console.WriteLine(responseBody);

                return character;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
        }
    }
}
