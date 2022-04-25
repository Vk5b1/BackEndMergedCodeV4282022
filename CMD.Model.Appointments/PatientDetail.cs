using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMD.Model.Appointments
{
    public class PatientDetail
    {
        public PatientDetail()
        {
            VitalReadings = new HashSet<VitalReading>();
            MedicalProblems = new HashSet<MedicalProblem>();
            Prescriptions = new HashSet<Prescription>();
            TestReports = new HashSet<TestReport>();
        }
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public ICollection<VitalReading> VitalReadings { get; set; }
        public ICollection<MedicalProblem> MedicalProblems { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
        public ICollection<TestReport> TestReports { get; set; }
    }
}
