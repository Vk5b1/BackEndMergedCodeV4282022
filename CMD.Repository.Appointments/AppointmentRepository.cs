using CMD.DTO.Appointments;
using CMD.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMD.Repository.Appointments
{
    public class AppointmentRepository : IAppointmentRepository
    {
        protected CMDAppointmentContext db = new CMDAppointmentContext();

        #region Subham
        public Appointment CreateAppointment(Appointment appointment)
        {
            db.Appointments.Add(appointment);
            var questions = db.Questions.ToList();

            if (questions.Count > 0)
            {
                foreach (var item in questions)
                {
                    appointment.FeedBack.Rating.Add(new QuestionRating { Question = item });
                }
            }

            db.SaveChanges();

            return appointment;
        }

        public PatientDetail CreatePatientDetial(int patientId)
        {
            PatientDetail patientDetail = new PatientDetail
            {
                Patient = db.Patients.Where(p => p.Id == patientId).First()
            };

            if (patientDetail.Patient == null)
            {
                throw new KeyNotFoundException();
            }
            db.PatientDetails.Add(patientDetail);
            db.SaveChanges();
            return patientDetail;
        }

        public bool CheckDate(DateTime date, TimeSpan time, int doctorId)
        {
            var flag = db.Appointments.Where(a => a.Doctor.Id == doctorId).Any(a => a.AppointmentDate == date && a.AppointmentTime == time);

            return flag;
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

        public int AppointmentCount(int doctorId)
        {
            return db.Appointments.Where(a => a.Doctor.Id == doctorId).Count();
        }

        public int AppointmentCount(int doctorId, string status)
        {
            return db.Appointments.Where(a => a.Doctor.Id == doctorId && a.Status.ToString().ToLower().Equals(status.ToLower())).Count();
        }

        public ICollection<Appointment> GetAllAppointment(int doctorId)
        {
            var allAppointments = db.Appointments
                .Include(path: a => a.Doctor)
                .Include(path: a => a.PatientDetail.Patient)
                .Include(path: a => a.Issue)
                .Where(a => a.Doctor.Id == doctorId).OrderBy(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime).ToList();
            return allAppointments;
        }

        public ICollection<string> GetIssues()
        {
            return db.Issues.Select(i => i.Name).ToList();
        }

        public ICollection<Patient> GetPatients(int doctorId)
        {
            return db.Patients.ToList();
        }

        public bool AcceptApppointment(int appointmentId)
        {
            var appointment = db.Appointments.Find(appointmentId);

            appointment.Status = AppointmentStatus.Confirmed;
            return db.SaveChanges() > 0;
        }

        public bool RejectApppointment(int appointmentId)
        {
            var appointment = db.Appointments.Find(appointmentId);

            appointment.Status = AppointmentStatus.Cancelled;
            return db.SaveChanges() > 0;
        }

        public bool DoctorAppointmentValidate(int appointmentId, int doctorId)
        {
            return db.Appointments.Find(appointmentId).Doctor.Id == doctorId;
        }

        #endregion

        #region kcs kishore
        public List<int> GetIdsAssociatedWithAppointment(int appointmentId)
        {
            return db.Appointments.Where(x => x.Id == appointmentId).Select(x => new List<int>
            {
                x.Id,
                x.PatientDetail.Patient.Id,
                x.Doctor.Id
            }).FirstOrDefault();
        }

        public IQueryable<AppointmentStatus> GetAppointmentSummary(int doctorId)
        {
            return db.Appointments.Where(a => a.Doctor.Id == doctorId).Select(x => x.Status).AsQueryable();
        }

        public Patient GetPatient(int patientId)
        {
            var result = db.Patients.Where(a=>a.Id==patientId).
                Include("ContactDetail").FirstOrDefault();
            return result;
        }

        public Doctor GetDoctorIncludingContactDetail(int doctorId)
        {
            var result = db.Doctors.Where(a => a.Id == doctorId).
                Include("ContactDetail").FirstOrDefault();

            return result;
        }
        #endregion

        #region Praveen 

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

        #region Gagana
         public void EditDoctor(Doctor doctor)
        {

            db.Entry(doctor).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }



        public Doctor GetDoctorWithContactDetails(int id)
        {
            //var p = db.DoctorProfile.Find(id);
            //return p;
            var doctorInfo = db.Doctors.Include(d => d.ContactDetail)
            .Where(p => p.Id == id).FirstOrDefault();
            return doctorInfo;
        }

        #endregion

        #region Supriya
        public FeedBack GetFeedback(int id)
        {
            var result = db.Appointments.Where(a => a.Id == id).Select(a => a.FeedBack).SelectMany(f => f.Rating).Include(r => r.Question).ToList();
            var feedback = db.Appointments.Where(a => a.Id == id).Select(a => a.FeedBack).FirstOrDefault();
            if (feedback == null)
            {
                return null;
            }
            var x = new FeedBack
            {
                Id = feedback.Id,
                AdditionalComment = feedback.AdditionalComment,
                Rating = result
            };


            return x;
        }
        #endregion

        #region Akash
        //public Doctor GetDoctor(int id)
        //{
        //    Doctor p = null;
        //    p = db.Doctors.Find(id);
        //    return p;
        //}
        //public void EditDoctor(Doctor doctor)
        //{
        //    //var p = db.Doctors.Find(doctor.DoctorId)
        //    db.Entry(doctor).State = System.Data.Entity.EntityState.Modified;
        //    db.SaveChanges();
        //}
        #endregion

        #region Bala


        public Recommedation AddRecommendtaion(Recommedation reco)
        {
            Doctor doctor = db.Doctors.Find(reco.DoctorId);
            reco.RecommendedDoctor = doctor;
            Appointment appointment = db.Appointments.Find(reco.AppointmentId);
            reco = db.Recommedations.Add(reco);



            appointment.Recommedations.Add(reco);



            db.Appointments.Append(appointment);
            db.SaveChanges();
            return reco;
        }

        public bool RemoveRecommendation(int id)
        {
            var r = db.Recommedations.Find(id);
            if (r == null) return false;
            db.Recommedations.Remove(r);
            return db.SaveChanges() > 0;



        }

        public List<DoctorInfoDTO> GetDoctors()
        {
            List<DoctorInfoDTO> list = new List<DoctorInfoDTO>();
            List<Doctor> doctors = db.Doctors.ToList();
            foreach (Doctor doctor in doctors)
            {
                DoctorInfoDTO temp = new DoctorInfoDTO();
                temp.Id = doctor.Id;
                temp.Name = doctor.Name;
                list.Add(temp);
            }
            return list;
        }



        #endregion


        #region Sakshi + Saba 
        public bool DeletePrescription(int PateintDetailId, int PrescriptionId)
        {
            PatientDetail patientDetail = db.PatientDetails.Include("Prescriptions").Where(p => p.Id == PateintDetailId).FirstOrDefault();
            Prescription pres = null;
            foreach (Prescription p in patientDetail.Prescriptions)
            {
                if (p.Id == PrescriptionId)
                {
                    pres = p;
                    break;
                }
            }
            if (pres != null)
            {
                patientDetail.Prescriptions.Remove(pres);
                db.Prescriptions.Remove(pres);
                db.PatientDetails.Append(patientDetail);
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public ICollection<Prescription> GetPrescriptions(int PateintDetailId)
        {
            ICollection<Prescription> prescriptions1 = new List<Prescription>();
            ICollection<Prescription> prescriptions = db.PatientDetails.Include("Prescriptions").Where(p => p.Id == PateintDetailId).Select(p => p.Prescriptions).FirstOrDefault();
            foreach (Prescription p in prescriptions)
            {
                //p.Medicine=db.Medicines.Find(p.Medicine.Id);
                Prescription temp = db.Prescriptions.Include("Medicine").Where(s => s.Id == p.Id).FirstOrDefault();
                temp.Medicine = db.Medicines.Find(temp.Medicine.Id);
                prescriptions1.Add(temp);
            }
            return prescriptions1;
        }


        //Add and Update Prescription

        public Prescription AddPrescription(int PateintDetailId, Prescription prescriptionId)
        {
            var p = db.PatientDetails.Find(PateintDetailId);

            p.Prescriptions.Add(prescriptionId);

            db.SaveChanges();
            return prescriptionId;
        }

        public Medicine GetMedicine(string name)
        {
            var result = db.Medicines.Where(m => m.Name == name).FirstOrDefault();
            return result;
        }

        public ICollection<Medicine> GetAllMedicine()
        {
            return db.Medicines.ToList();
        }

        public Prescription UpdatePrescription(Prescription prescriptionId)
        {
            db.Entry(prescriptionId).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return prescriptionId;
        }




        #endregion

        #region Abhishek + Venkat
        public TestReport AddTest(Test test, int appointmentId)
        {

            //if (!db.Tests.Any(checkTest => checkTest.Id == test.Id))
            //{
            //    test = CreateTest(test);
            //}

            var tr = db.TestReports.Add(new TestReport());
            tr.TestId = test.Id;
            var appointment = db.Appointments.Include("PatientDetail").Where(p => p.Id == appointmentId).FirstOrDefault();
            appointment.PatientDetail.TestReports.Add(tr);
            db.SaveChanges();
            return tr;
        }


        public TestReport DeleteTest(int appointmnetId, int testReportId)
        {

            var appointment = db.Appointments.Find(appointmnetId);
            if (appointment == null)
            {
                return null;
            }
            var result = db.TestReports.Find(testReportId);
            if (result == null)
            {
                return null;
            }
            db.TestReports.Remove(result);
            db.SaveChanges();
            return result;

        }

        public List<Test> GetAllTests()
        {
            return db.Tests.ToList();
        }

        public ICollection<TestReport> GetRecommendedTests(int appointmentId)
        {
            var testReports = db.Appointments.Where(p => p.Id == appointmentId).Select(p => p.PatientDetail).SelectMany(t => t.TestReports).Include(d => d.Test).ToList();
            return testReports;
        }

        public List<TestReport> GetTestReports()
        {
            var testreports = db.TestReports.ToList();
            return testreports;
        }

        //public Test CreateTest(Test test)
        //{
        //    return db.Tests.Add(test);
        //}



        #endregion


        #region Pankaj
        public List<Vital> getAllVitals()
        {
            return db.Vitals.ToList();
        }

        public Vital getVitalById(int id)
        {
            var item = db.Vitals.Find(id);
            return item;
        }

        public Vital updateVital(Vital v)
        {

            db.Entry(v).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return v;
        }
        #endregion
    }
}
