using CMD.Model.Appointments.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMD.Repository.Appointments
{
    public class AppointmentRepository : IAppointmentRepository
    {
        protected CMDAppointmentContext db = new CMDAppointmentContext();

        public Appointment CreateAppointment(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public ICollection<Appointment> GetAllAppointment(int doctorId)
        {
            return db.Appointments.Where(a => a.Doctor.Id == doctorId).ToList();
        }
    }
}
