using CMD.DTO.Appointments;
using CMD.Model;
using System.Collections.Generic;

namespace CMD.Business.Appointments
{
    public interface IAppointmentManager
    {
        #region Subham

        ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId, PaginationParams pagination);
        ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId, string status, PaginationParams pagination);
        int GetAppointmentCount(int doctorId);
        int GetAppointmentCountBasedOnStatus(int doctorId, string status);
        AppointmentConfirmationDTO AddAppointment(AppointmentFormDTO appointmentForm);
        ICollection<PatientDTOForPatientSearch> GetPatients(int doctorId);
        ICollection<string> GetIssues();
        bool ChangeAppointmentStatus(AppointmentStatusDTO statusDTO, int doctorId);

        #endregion

        #region KCS Kishore
        IdsListViewDetailsDTO GetIdsAssociatedWithAppointment(int appointmentId);

        Dictionary<string,int> DashboardSummary(int doctorId);

        DoctorCardDTO GetDoctorCard(int doctorId);
        PatientCardDTO GetPatientCard(int patientId);

        #endregion

        #region Praveen

        AppointmentCommentDTO GetAppointmentComment(int appointmentId);
        bool UpdateAppointmentComment(int appointmentId, AppointmentCommentDTO appointmentComment);

        #endregion

        #region Gagana
         
        DoctorProfileDTO GetDoctorsWithContact(int id);
        void EditDoctor(DoctorProfileDTO doctorsDTO);

        #endregion

        #region Supriya
        FeedBack GetFeedback(int id);

        #endregion

        #region Akash

        DoctorDTO GetDoctors(int id);
        void EditDoctor(DoctorDTO doctorDTO);

        #endregion

        #region BalaKrishna

        bool RemoveRecommendation(int id);
        List<DoctorInfoDTO> GetDoctors();
        Recommedation AddRecommendtaion(Recommedation reco);

        #endregion

        #region Sakshi + Saba

        ICollection<PrescriptionDTO> GetPrescriptions(int PatientDetailId);
        bool DeletePrescription(int PateintDetailId, int PrescriptionId);


        PrescriptionDTO AddPrescription(int id, PrescriptionDTO prescriptionDto);
        PrescriptionDTO UpdatePrescription(PrescriptionDTO prescriptionDto);
        ICollection<Medicine> GetAllMedicine();



        #endregion


        #region Abhishek + Venkat
        TestReport AddTest(Test test, int appointmentId);
        List<Test> GetAllTests();
        List<Test> GetRecommendedTests(int appointmentId);
        TestReport DeleteTest(int testReportId, int appointmentId);
        List<TestReportDTO> GetTestReports();

        #endregion

        #region Pankaj
        List<VitalDTO> GetAllVitalsDTO();
        VitalDTO GetVitalDTOByVitalId(int vital_id);
        VitalDTO UpdateVital(VitalDTO v);
        #endregion

    }
}
