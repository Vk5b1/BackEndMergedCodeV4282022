using CMD.DTO.Appointments;
using System.Collections.Generic;

namespace CMD.Business.Appointments
{
    public interface IAppointmentManager
    {
        ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId);
        AppointmentConfirmationDTO AddAppointment(AppointmentFormDTO appointmentForm);
        ICollection<PatientDTOForPatientSearch> GetRecommendedPatients(int doctorId);
        ICollection<IssueDTO> GetIssues();
        AppointmentCommentDTO GetAppointmentComment(int appointmentId);
        bool UpdateAppointmentComment(int appointmentId, AppointmentCommentDTO appointmentComment);   
    }
}
