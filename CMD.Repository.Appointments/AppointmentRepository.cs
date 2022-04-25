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
        public Issue AddNewIssue(Issue issue)
        {
            return db.Issues.Add(issue);
        }
        public Issue GetIssue(int issueId)
        {
            return db.Issues.Find(issueId);
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
    }
}
