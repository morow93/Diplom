﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Entities
{
    public class Like
    {
        public Int32 LikeId { get; set; }
        public Int32 Vote { get; set; }

        public String UserId { get; set; }
        public Int32 TripId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
