using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMD.Model.Appointments
{
    public class PatientDetail
    {
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public ICollection<VitalReading> VitalReadings { get; set; }
        public ICollection<MedicalProblem> MedicalProblems { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
        public ICollection<TestReport> TestReports { get; set; }
    }
}
