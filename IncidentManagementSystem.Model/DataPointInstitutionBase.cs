﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    [DataContract]
    public class DataPointInstitutionBase
    {

        public DataPointInstitutionBase() { }
            public DataPointInstitutionBase(string label, double? y)
            {
                this.Label = label;
                this.Y = y;
            }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "label")]
            public string Label = "";

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public Nullable<double> Y = null;
        }
    
}
