using CMD.DTO.Appointments;
using System.Collections.Generic;

namespace CMD.Business.Appointments
{
    public interface IAppointmentManager
    {
        ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId, PaginationParams pagination);
        int GetAppointmentCount(int doctorId);
        AppointmentConfirmationDTO AddAppointment(AppointmentFormDTO appointmentForm);
        ICollection<PatientDTOForPatientSearch> GetPatients(int doctorId);
        ICollection<string> GetIssues();
        bool ChangeAppointmentStatus(AppointmentStatusDTO statusDTO, int doctorId);
    }
}
