using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    [DataContract]
    public class DataPointServicesBase
    {
      

        public DataPointServicesBase() { }
            public DataPointServicesBase(string label, double? y)
            {
                this.Label = label;
                this.Y = y;
            }

           
            [DataMember(Name = "label")]
            public string Label = "";

          
            [DataMember(Name = "y")]
            public Nullable<double> Y = null;
        }
    
}
