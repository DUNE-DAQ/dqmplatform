using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DuneDaqMonitoringPlatform.Models
{
    public class UserSSO
    {
      [JsonProperty("sub")]
      public string Id { get; set; }

      [JsonProperty("email")]
      public string Email { get; set; }

      [JsonProperty("preferred_username")]
      public string Username { get; set; }

      [JsonProperty("name")]
      public string FullName { get; set; }

      [JsonProperty("given_name")]
      public string FirstName { get; set; }

      [JsonProperty("family_name")]
      public string LastName { get; set; }

      [JsonProperty("updated_at")]
      public DateTime UpdatedAt { get; set; }

      [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
      [JsonProperty("Password")]
      public string Password { get; set; }
    }
}