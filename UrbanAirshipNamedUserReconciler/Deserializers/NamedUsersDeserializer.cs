﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanAirshipNamedUserReconciler.Deserializers
{
    public class NamedUsersDeserializer
    {
        public List<NamedUser> NamedUsers { get; set; }

        public NamedUsersDeserializer()
        {
            NamedUsers = new List<NamedUser>();
        }
    }
}
