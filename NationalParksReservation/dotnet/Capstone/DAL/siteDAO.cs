﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public class SiteDAO : ISiteDAO
    {
        private string connectionString;
        public SiteDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
    }
}
