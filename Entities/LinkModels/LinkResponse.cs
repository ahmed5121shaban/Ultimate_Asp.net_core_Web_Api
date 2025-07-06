using Entities.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public class LinkResponse
    {
        public bool HasLinks { get; set; }
        public List<ExpandoObject> ShapedEntities { get; set; }
        public LinkCollectionWrapper<ExpandoObject> LinkedEntities { get; set; }
        public LinkResponse()
        {
            LinkedEntities = new LinkCollectionWrapper<ExpandoObject>();
            ShapedEntities = new List<ExpandoObject>();
        }
    }
}
