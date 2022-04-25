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
            db.Appointments.Add(appointment);

            db.SaveChanges();

            return appointment;
        }
        public PatientDetail CreatePatientDetial(int patientId)
        {
            PatientDetail patientDetail = new PatientDetail();
            patientDetail.Patient = db.Patients.Where(p => p.Id == patientId).First();
            
            if(patientDetail.Patient == null)
            {
                throw new KeyNotFoundException();
            }
            db.PatientDetails.Add(patientDetail);
            db.SaveChanges();
            return patientDetail;
        }
        public Doctor GetDoctor(int docId)
        {
            return db.Doctors.Find(docId);
        }

        public Issue GetIssue(string issueName)
        {
            return db.Issues.Where(i => i.Name == issueName).Any() ? db.Issues.Where(i => i.Name == issueName).First() : AddNewIssue(new Issue { Name = issueName});
        }

        private Issue AddNewIssue(Issue issue)
        {
            db.Issues.Add(issue);
            db.SaveChanges();
            return issue;   
        }

        public ICollection<Appointment> GetAllAppointment(int doctorId)
        {
            return db.Appointments
                .Include(path: a => a.Doctor)
                .Include(path: a => a.PatientDetail.Patient)
                .Include(path: a => a.Issue)
                .Where(a => a.Doctor.Id == doctorId).ToList();
        }

        public ICollection<Issue> GetIssues()
        {
            return db.Issues.ToList();
        }

        public ICollection<Patient> GetRecommededPatients(int doctorId)
        {
            return db.Recommedations.Where(r => r.RecommendedDoctorId == doctorId).Select(r => r.PatientDetail.Patient).Include(p => p.ContactDetail).ToList();
        }

        #region Praveen Code

        public string GetComment(int appointmentId)
        {
            return db.Appointments.Where(a => a.Id == appointmentId).Select(a => a.Comment).FirstOrDefault();
        }

        public bool EditComment(int appointmentId, string comment)
        {
            var appointment = db.Appointments.Find(appointmentId);
            appointment.Comment = comment;
            db.Entry(appointment).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        #endregion
    }
}
