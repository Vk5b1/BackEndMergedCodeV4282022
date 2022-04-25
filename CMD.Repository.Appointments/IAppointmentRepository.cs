using CMD.Model.Appointments;
using System.Collections.Generic;

namespace CMD.Repository.Appointments
{
    public interface IAppointmentRepository
    {
        ICollection<Appointment> GetAllAppointment(int doctorId);
        Appointment CreateAppointment(Appointment appointment);
        ICollection<Issue> GetIssues();
        ICollection<Patient> GetRecommededPatients(int doctorId);
        PatientDetail CreatePatientDetial(int patientId);
        Doctor GetDoctor(int docId);
        Issue GetIssue(string issueName);
        string GetComment(int appointmentId);
        bool EditComment(int appointmentId, string comment);
        
    }
}
