﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions.MaxAgeRangeBadRequestExceptions
{
    public class MaxAgeRangeBadRequestException : BadRequestException
    {
        public MaxAgeRangeBadRequestException() : 
            base("Max age can't be less than min age.")
        {
        }
    }
}
