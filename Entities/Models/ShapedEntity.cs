using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ShapedEntity
    {
        public ShapedEntity()
        {
            Entity = new ExpandoObject();
        }
        public Guid Id { get; set; }
        public ExpandoObject Entity { get; set; }
    }
}
