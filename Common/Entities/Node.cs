using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    [PrimaryKey (nameof(NodeId))]
    public class Node
    {
        [JsonProperty (PropertyName = "id")]
        public string NodeId { get; set; }
        [JsonProperty(PropertyName = "label")]
        public string Name { get; set; }
        public int? x { get; set; }
        public int? y { get; set; }
        [JsonProperty(PropertyName = "schoolId")]
        public string? SchoolId { get; set; }
    }
}
