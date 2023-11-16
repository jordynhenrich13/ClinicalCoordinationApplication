using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace ClinicalCoordinationApplication.Model
{
    public class ClinicalButton
    {
        public string Name { get; set; }
        public Type ClinicalPageType { get; set; }

        public ClinicalButton(string name, Type clinicalPageType)
        {
            Name = name;
            ClinicalPageType = clinicalPageType;
        }
    }
}