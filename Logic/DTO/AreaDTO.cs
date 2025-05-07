﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DTO
{
    public class AreaDTO
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public int? Sum { get; set; }

        public int Index { get; set; }

        public string Description { get; set; }

        public bool? IsMaaser { get; set; }
        
        public bool? IsActive { get; set; }

        public bool? IndexOn { get; set; }

        public bool? IsConnected  { get; set; }=false;
    }
}
