using CMD.Model.Appointments;
using System.Collections.Generic;

namespace CMD.Repository.Appointments
{
    public interface IAppointmentRepository
    {
        ICollection<Appointment> GetAllAppointment(int doctorId);
        int AppointmentCount();
        Appointment CreateAppointment(Appointment appointment);
        ICollection<string> GetIssues();
        ICollection<Patient> GetPatients(int doctorId);
        PatientDetail CreatePatientDetial(int patientId);
        Doctor GetDoctor(int docId);
        Issue GetIssue(string issueName);
        string GetComment(int appointmentId);
        bool EditComment(int appointmentId, string comment);
        
    }
}
