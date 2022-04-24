using CMD.Model.Appointments.Entities;
using System.Collections.Generic;

namespace CMD.Repository.Appointments
{
    public interface IAppointmentRepository
    {
        ICollection<Appointment> GetAllAppointment(int doctorId);
        Appointment CreateAppointment(Appointment appointment);
    }
}
