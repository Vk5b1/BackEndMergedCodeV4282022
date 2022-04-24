using System.Collections.Generic;

namespace CMD.Model.Appointments.Entities
{
    public class PatientDetail
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public ICollection<VitalReading> VitalReadings { get; set; }
        public ICollection<MedicalProblem> MedicalProblems { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
        public ICollection<TestReport> TestReports { get; set; }
    }
}
