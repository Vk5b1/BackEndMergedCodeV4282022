using System.Collections.Generic;

namespace CMD.Model
{
    public class PatientDetail
    {
        public PatientDetail()
        {
            MedicalProblems = new HashSet<MedicalProblem>();
            Prescriptions = new HashSet<Prescription>();
            TestReports = new HashSet<TestReport>();
        }
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public Vital Vital { get; set; }
        public ICollection<MedicalProblem> MedicalProblems { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
        public ICollection<TestReport> TestReports { get; set; }
    }
}