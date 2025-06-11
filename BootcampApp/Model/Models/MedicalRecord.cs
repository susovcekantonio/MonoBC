using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class MedicalRecord
    {
        public Guid Id { get; set; }
        public string Treatment { get; set; }
        public Guid PatientId { get; set; }
    }
}
