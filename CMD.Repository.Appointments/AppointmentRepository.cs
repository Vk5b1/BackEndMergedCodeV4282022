using CMD.Model.Appointments;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMD.Repository.Appointments
{
    public class AppointmentRepository : IAppointmentRepository
    {
        protected CMDAppointmentContext db = new CMDAppointmentContext();

        public Appointment CreateAppointment(Appointment appointment)
        {
            if(db.Issues.Any(item => item.Name == appointment.Issue.Name))
            {
                appointment.Issue = db.Issues.Where(item => item.Name == appointment.Issue.Name).FirstOrDefault();
            }
            else
            {
                appointment.Issue = db.Issues.Add(appointment.Issue);
            }

            appointment.Doctor = db.Doctors.Where(doc => doc.Id == appointment.Doctor.Id).FirstOrDefault();

            db.Appointments.Add(appointment);

            db.SaveChanges();

            db.Entry(appointment).State = System.Data.Entity.EntityState.Detached;

            return appointment;
        }

        public ICollection<Appointment> GetAllAppointment(int doctorId)
        {
            return db.Appointments.Where(a => a.Doctor.Id == doctorId).ToList();
        }

        public ICollection<Issue> GetIssues()
        {
            return db.Issues.ToList();
        }

        public ICollection<Patient> GetRecommededPatients(int doctorId)
        {
            return db.Recommedations.Where(r => r.RecommendedDoctorId == doctorId).Select(r => r.PatientDetail.Patient).Include(p => p.ContactDetail).ToList();
        }
    }
}
