using CMD.DTO.Appointments;
using CMD.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMD.Repository.Appointments
{
    public interface IAppointmentRepository
    {
        #region Subham
        ICollection<Appointment> GetAllAppointment(int doctorId);
        int AppointmentCount(int doctorId);
        int AppointmentCount(int doctorId, string status);
        Appointment CreateAppointment(Appointment appointment);
        bool CheckDate(DateTime date, TimeSpan time, int doctorId);
        ICollection<string> GetIssues();
        ICollection<Patient> GetPatients(int doctorId);
        PatientDetail CreatePatientDetial(int patientId);
        Doctor GetDoctor(int docId);
        Issue GetIssue(string issueName);
        bool DoctorAppointmentValidate(int appointmentId, int doctorId);
        bool AcceptApppointment(int appointmentId);
        bool RejectApppointment(int appointmentId);

        #endregion

        #region KCS Kishore
        List<int> GetIdsAssociatedWithAppointment(int appointmentId);
        IQueryable<AppointmentStatus> GetAppointmentSummary(int doctorId);
        Patient GetPatient(int patientId);
        Doctor GetDoctorIncludingContactDetail(int doctorId);


        #endregion

        #region Praveen
        string GetComment(int appointmentId);
        bool EditComment(int appointmentId, string comment);
        #endregion

        #region Gagana
        Doctor GetDoctorWithContactDetails(int id);
        void EditDoctor(Doctor doctor);
        #endregion

        #region Supriya
        FeedBack GetFeedback(int id);
        #endregion

        #region Akash
        //Doctor GetDoctor(int id);

        //void EditDoctor(Doctor doctor);

        // Same Code 
        #endregion

        #region BalaKrishna

        Recommedation AddRecommendtaion(Recommedation reco);
        bool RemoveRecommendation(int id);
        List<DoctorInfoDTO> GetDoctors();

        #endregion

        #region Sakshi + Saba 
        ICollection<Prescription> GetPrescriptions(int PateintDetailId);
        bool DeletePrescription(int PateintDetailId, int PrescriptionId);


        Prescription AddPrescription(int id, Prescription prescription);
        Prescription UpdatePrescription(Prescription prescription);
        Medicine GetMedicine(string name);
        ICollection<Medicine> GetAllMedicine();

        #endregion

        #region Abhishek + Venkat
        TestReport AddTest(Test test, int appointmentId);
        List<Test> GetAllTests();
        TestReport DeleteTest(int appointmnetId, int testReportId);
        ICollection<TestReport> GetRecommendedTests(int appointmentId);
        List<TestReport> GetTestReports();

        #endregion

        #region Pankaj
        List<Vital> getAllVitals();
        Vital getVitalById(int id);
        Vital updateVital(Vital v);
        #endregion

    }
}
