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
    }
}
