using CMD.DTO.Appointments;
using System.Collections.Generic;

namespace CMD.Business.Appointments
{
    public interface IAppointmentManager
    {
        ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId, PaginationParams pagination);
        int GetAppointmentCount();
        AppointmentConfirmationDTO AddAppointment(AppointmentFormDTO appointmentForm);
        ICollection<PatientDTOForPatientSearch> GetPatients(int doctorId);
        ICollection<string> GetIssues();
        AppointmentCommentDTO GetAppointmentComment(int appointmentId);
        bool UpdateAppointmentComment(int appointmentId, AppointmentCommentDTO appointmentComment);   
    }
}
