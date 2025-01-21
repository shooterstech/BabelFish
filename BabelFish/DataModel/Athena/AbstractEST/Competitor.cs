﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class Competitor
    {

        public Competitor()
        {

        }

        public string DisplayName { get; set; }

        public string Country { get; set; }

        public string Club { get; set; }

        public string UserID { get; set; }

        public string CompetitorNumber { get; set; }

        public string ResultCOFID { get; set; }

        public string MatchID { get; set; }

        public string Relay { get; set; }

        public VisibilityOption Privacy { get; set; }
    }
}