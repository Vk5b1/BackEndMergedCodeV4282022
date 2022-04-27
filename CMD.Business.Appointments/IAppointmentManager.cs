using CMD.DTO.Appointments;
using System.Collections.Generic;

namespace CMD.Business.Appointments
{
    public interface IAppointmentManager
    {
        ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId, PaginationParams pagination);
        ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId, string status, PaginationParams pagination);
        int GetAppointmentCount(int doctorId);
        int GetAppointmentCountBasedOnStatus(int doctorId, string status);
        AppointmentConfirmationDTO AddAppointment(AppointmentFormDTO appointmentForm);
        ICollection<PatientDTOForPatientSearch> GetPatients(int doctorId);
        ICollection<string> GetIssues();
        bool ChangeAppointmentStatus(AppointmentStatusDTO statusDTO, int doctorId);
        IdsListViewDetailsDTO GetIdsAssociatedWithAppointment(int appointmentId);
    }
}
