using Microsoft.EntityFrameworkCore;
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
        public string NodeId { get; set; }
        public int? x { get; set; }
        public int? y { get; set; }
    }
}
