using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanAirshipNamedUserReconciler.Serializers
{
    public class AssociationSerializer
    {
        public string channel_id { get; set; }
        public string device_type { get; set; }
        public string named_user_id { get; set; }
    }
}
