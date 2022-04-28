using CMD.Business.Appointments.CustomExceptions;
using CMD.DTO.Appointments;
using System.Collections.Generic;
using System.Linq;
using System;
using CMD.Model;
using CMD.Repository.Appointments;

namespace CMD.Business.Appointments
{
    public class AppointmentManager : IAppointmentManager
    {
        protected IAppointmentRepository repo;

        public AppointmentManager(IAppointmentRepository repo)
        {
            this.repo = repo;
        }
        #region Subham
        public AppointmentConfirmationDTO AddAppointment(AppointmentFormDTO appointmentForm)
        {

                if (appointmentForm.AppointmentDate == default)
                    throw new DateNullException();

                if (appointmentForm.AppointmentDate < DateTime.Now || (appointmentForm.AppointmentDate == DateTime.Now.Date && appointmentForm.AppointmentTime < DateTime.Now.TimeOfDay))
                    throw new PastDateException();

                //if(appointmentForm.PatientId == null || appointmentForm.DoctorId == null || appointmentForm.Issue == "") 
                //    throw new NullReferenceException();

                if (repo.CheckDate(appointmentForm.AppointmentDate, appointmentForm.AppointmentTime, (int)appointmentForm.DoctorId))
                    throw new SameAppointmentTimingException();

                Appointment appointment = new Appointment()
                {
                    PatientDetail = repo.CreatePatientDetial((int)appointmentForm.PatientId),
                    AppointmentDate = appointmentForm.AppointmentDate,
                    AppointmentTime = appointmentForm.AppointmentTime,
                    Issue = repo.GetIssue(appointmentForm.Issue),
                    Reason = appointmentForm.Reason,
                    Doctor = repo.GetDoctor((int)appointmentForm.DoctorId),
                };

                Appointment a = repo.CreateAppointment(appointment);

                var aform = new AppointmentConfirmationDTO()
                {
                    AppointmentId = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status.ToString(),
                    Reason = a.Reason,
                    IssueName = a.Issue.Name,
                    PatientName = a.PatientDetail.Patient.Name,
                    PatientDOB = a.PatientDetail.Patient.DOB,
                    DoctorName = a.Doctor.Name,
                    DoctorSpeciality = a.Doctor.Speciality,
                };
                return aform;
            
        }

        public ICollection<string> GetIssues()
        {
            return repo.GetIssues();
        }

        public ICollection<PatientDTOForPatientSearch> GetPatients(int doctorId)
        {
            ICollection<Patient> patients = repo.GetPatients(doctorId);

            if(patients == null)
            {
                return null;
            }

            ICollection<PatientDTOForPatientSearch> result = new List<PatientDTOForPatientSearch>();
            foreach(Patient patient in patients)
            {
                result.Add(new PatientDTOForPatientSearch
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    PhoneNumber = patient.ContactDetail.PhoneNumber
                });
            }
            return result;
        }

        public ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId, string status, PaginationParams pagination)
        {
            ICollection<Appointment> appointments = repo.GetAllAppointment(doctorId).Where(a => a.Status.ToString().ToLower().Equals(status.ToLower())).Skip((pagination.Page - 1) * pagination.ItemsPerPage).Take(pagination.ItemsPerPage).ToList();

            if(appointments == null) return null;

            ICollection<AppointmentBasicInfoDTO> result = new List<AppointmentBasicInfoDTO>();
            foreach (var appointment in appointments)
            {
                result.Add(new AppointmentBasicInfoDTO()
                {
                    AppointmentId = appointment.Id,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    AppointmentStatus = appointment.Status.ToString(),
                    PatientName = appointment.PatientDetail.Patient.Name,
                    PatientPicture = appointment.PatientDetail.Patient.PatientPicture,
                    PatientDOB = appointment.PatientDetail.Patient.DOB,
                    Issue = appointment.Issue.Name
                });
            }
            return result;
        }

        public ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId, PaginationParams pagination)
        {
            ICollection<Appointment> appointments = repo.GetAllAppointment(doctorId).Skip((pagination.Page - 1) * pagination.ItemsPerPage).Take(pagination.ItemsPerPage).ToList();
            ICollection<AppointmentBasicInfoDTO> result = new List<AppointmentBasicInfoDTO>();
            foreach (var appointment in appointments)
            {
                result.Add(new AppointmentBasicInfoDTO()
                {
                    AppointmentId = appointment.Id,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    AppointmentStatus = appointment.Status.ToString(),
                    PatientName = appointment.PatientDetail.Patient.Name ,
                    PatientPicture = appointment.PatientDetail.Patient.PatientPicture,
                    PatientDOB = appointment.PatientDetail.Patient.DOB,
                    Issue = appointment.Issue.Name
                });
            }
            return result;
        }
        public int GetAppointmentCount(int doctorId)
        {
            return repo.AppointmentCount(doctorId);
        }
        public int GetAppointmentCountBasedOnStatus(int doctorId, string status)
        {
            return repo.AppointmentCount(doctorId, status);
        }

        public bool ChangeAppointmentStatus(AppointmentStatusDTO statusDTO, int doctorId)
        {
            var result = false;
            if(statusDTO.Status == "Confirmed")
            {
                result = repo.AcceptApppointment(statusDTO.AppointmentId);
            }
            else if(statusDTO.Status == "Cancelled")
            {
                result = repo.RejectApppointment(statusDTO.AppointmentId);
            }
            return result;
        }

        #endregion

        #region kishore

        public IdsListViewDetailsDTO GetIdsAssociatedWithAppointment(int appointmentId)
        {
            IdsListViewDetailsDTO idsListViewDetailsDTO = new IdsListViewDetailsDTO();
            var temp = repo.GetIdsAssociatedWithAppointment(appointmentId);
            idsListViewDetailsDTO.AppointmentId = temp[0];
            idsListViewDetailsDTO.PatientId = temp[1];
            idsListViewDetailsDTO.DoctorId = temp[2];
            return idsListViewDetailsDTO;
        }

        public Dictionary<string,int> DashboardSummary(int doctorId)
        {
            var statuses = repo.GetAppointmentSummary(doctorId).Select(y => y.ToString()).ToList();
            int[] summary = { 0, 0, 0 };//accepted,total,cancelled
            foreach (var str in statuses)
            {
                if (str.Equals("Confirmed"))
                {
                    summary[0]++;
                }
                else if (str.Equals("Cancelled"))
                {
                    summary[2]++;
                }
                else
                {
                    summary[1]++;
                }
            }
            summary[1] = summary.Sum();
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
            keyValuePairs.Add("Accepted", summary[0]);
            keyValuePairs.Add("Total", summary[1]);
            keyValuePairs.Add("Cancelled", summary[2]);
            return keyValuePairs;
        }

        public DoctorCardDTO GetDoctorCard(int doctorId)
        {
            Doctor doctor = repo.GetDoctor(doctorId);
            DoctorCardDTO DoctorCard = new DoctorCardDTO();
            DoctorCard.Id = doctor.Id;
            DoctorCard.DoctorPicture = doctor.DoctorPicture;
            DoctorCard.Name = doctor.Name;
            DoctorCard.PhoneNumber = doctor.ContactDetail.PhoneNumber;
            DoctorCard.Mail = doctor.ContactDetail.Email;
            DoctorCard.PracticeLocation = doctor.PracticeLocation;
            DoctorCard.NPINumber = doctor.NPINumber;
            DoctorCard.SpecialityName = doctor.Speciality;
            //ICollection<Speciality> specialities = doctor.Specialities;
            //specialities.
            return DoctorCard;
        }
        public PatientCardDTO GetPatientCard(int patientId)
        {
            Patient patient = repo.GetPatient(patientId);
            //Patient patient { get; set;}
            PatientCardDTO PatientCard = new PatientCardDTO();
            PatientCard.Id = patient.Id;
            PatientCard.PatientPicture = patient.PatientPicture;
            PatientCard.Name = patient.Name;
            PatientCard.PhoneNumber = patient.ContactDetail.PhoneNumber;
            PatientCard.Mail = patient.ContactDetail.Email;
            PatientCard.DOB = patient.DOB.Date.ToString();
            PatientCard.Age = DateTime.Today.Year - patient.DOB.Year;
            PatientCard.BloodGroup = patient.BloodGroup.ToString();
            PatientCard.Gender = patient.Gender.ToString();
            return PatientCard;
        }

        #endregion

        #region Praveen

        public AppointmentCommentDTO GetAppointmentComment(int appointmentId)
        {
            return new AppointmentCommentDTO { Id = appointmentId, Comment = repo.GetComment(appointmentId) };
        }

        public bool UpdateAppointmentComment(int appointmentId, AppointmentCommentDTO appointmentComment)
        {
            return repo.EditComment(appointmentId, appointmentComment.Comment);
        }

        #endregion

        #region Gagana
        public void EditDoctor(DoctorProfileDTO doctorsDTO)
        {

            Doctor doctor = new Doctor
            {
                Id = (int)doctorsDTO.id,
                Name = doctorsDTO.doctor_name,
                Speciality = doctorsDTO.doctor_speciality,
                NPINumber = doctorsDTO.doctor_npi_no,
                PracticeLocation = doctorsDTO.doctor_practice_location,
                DoctorPicture = doctorsDTO.doctor_profile_image,

                ContactDetail = new ContactDetail
                {
                    Id = doctorsDTO.ContactDetails.Id,
                    Email = doctorsDTO.ContactDetails.doctor_email_id,
                    PhoneNumber = doctorsDTO.ContactDetails.doctor_phone_number,
                }
            };

            repo.EditDoctor(doctor);
        }

        public DoctorProfileDTO GetDoctorsWithContact(int id)
        {
            var doctor = repo.GetDoctor(id);
            if( doctor == null){
                return null;
            }
            DoctorProfileDTO doctorsDTO = new DoctorProfileDTO
            {
                id = doctor.Id,
                doctor_name = doctor.Name,
                doctor_speciality = doctor.Speciality,
                doctor_npi_no = doctor.NPINumber,
                doctor_practice_location = doctor.PracticeLocation,
                doctor_profile_image = doctor.DoctorPicture,
                ContactDetails = new ContactDetailDTO
                {
                    Id = doctor.ContactDetail.Id,
                    doctor_email_id = doctor.ContactDetail.Email,
                    doctor_phone_number = doctor.ContactDetail.PhoneNumber
                }
            };
            return doctorsDTO;
        }



        #endregion

        #region Supriya
        public FeedBack GetFeedback(int id)
        {
            var result = repo.GetFeedback(id);
            return result;
        }

        #endregion

        #region Akash

        public void EditDoctor(DoctorDTO doctorDTO)
        {
            Doctor doctor = new Doctor();
            doctor.Id = doctorDTO.id;
            doctor.Name = doctorDTO.doctor_name;
            doctor.DoctorPicture = doctorDTO.doctor_profile_image;
            repo.EditDoctor(doctor);

        }

        public DoctorDTO GetDoctors(int id)
        {
            Doctor doctor = repo.GetDoctor(id);
            DoctorDTO doctorDTO = new DoctorDTO(); 
            doctorDTO.id = doctor.Id;
            doctorDTO.doctor_name = doctor.Name;
            doctorDTO.doctor_profile_image = doctor.DoctorPicture;
            return doctorDTO;

        }
        #endregion

        #region Bala


        public bool RemoveRecommendation(int id)
        {
            return repo.RemoveRecommendation(id);
        }
        public List<DoctorInfoDTO> GetDoctors()
        {
            return repo.GetDoctors();
        }

        public Recommedation AddRecommendtaion(Recommedation reco)
        {
            return repo.AddRecommendtaion(reco);
        }



        #endregion

        #region Sakshi + Saba
        public bool DeletePrescription(int PatientDetailId, int PrescriptionId)
        {
            return repo.DeletePrescription(PatientDetailId, PrescriptionId);
        }

        public ICollection<PrescriptionDTO> GetPrescriptions(int PatientDetailId)
        {
            ICollection<Prescription> prescriptions = repo.GetPrescriptions(PatientDetailId);
            ICollection<PrescriptionDTO> prescriptionDTOs = new List<PrescriptionDTO>();
            foreach (Prescription p in prescriptions)
            {
                PrescriptionDTO temp = new PrescriptionDTO()
                {
                    Id = p.Id,
                    Medicine = p.Medicine.Name,
                    Intake = p.Intake == Intake.BeforeFood,
                    Span = p.Span,
                    TimeOfDay = p.TimeOfDay,
                    AdditionalComment = p.AdditionalComment
                };
                prescriptionDTOs.Add(temp);

            }
            return prescriptionDTOs;
        }

        //Add and View Prescription

        public PrescriptionDTO AddPrescription(int PatientDetailId, PrescriptionDTO prescriptionId)
        {
            Prescription prescriptions = new Prescription
            {
                Medicine = repo.GetMedicine(prescriptionId.Medicine),
                Span = prescriptionId.Span,
                Intake = prescriptionId.Intake ? Intake.AfterFood : Intake.BeforeFood,
                AdditionalComment = prescriptionId.AdditionalComment,
                TimeOfDay = prescriptionId.TimeOfDay,

            };

            try
            {
                var pre = repo.AddPrescription(PatientDetailId, prescriptions);
                return new PrescriptionDTO
                {
                    Id = pre.Id,
                    Span = pre.Span,
                    TimeOfDay = pre.TimeOfDay,
                    AdditionalComment = pre.AdditionalComment,
                    Intake = pre.Intake == Intake.BeforeFood,
                    Medicine = pre.Medicine.Name,
                };

            }
            catch (Exception)
            {

                throw new Exception("Medicine record not found");
            }
        }

        public PrescriptionDTO UpdatePrescription(PrescriptionDTO prescriptionId)
        {
            Prescription prescriptions = new Prescription
            {
                Id = prescriptionId.Id,
                Medicine = repo.GetMedicine(prescriptionId.Medicine),
                Span = prescriptionId.Span,
                Intake = prescriptionId.Intake ? Intake.AfterFood : Intake.BeforeFood,
                AdditionalComment = prescriptionId.AdditionalComment,
                TimeOfDay = prescriptionId.TimeOfDay,

            };
            var pre = repo.UpdatePrescription(prescriptions);
            return new PrescriptionDTO
            {
                Id = pre.Id,
                Span = pre.Span,
                TimeOfDay = pre.TimeOfDay,
                AdditionalComment = pre.AdditionalComment,
                Intake = pre.Intake == Intake.BeforeFood,
                Medicine = pre.Medicine.Name,
            };
        }

        public ICollection<Medicine> GetAllMedicine()
        {
            return repo.GetAllMedicine();
        }




        #endregion

        #region Abhishek + Venkat
        public TestReport AddTest(Test test, int appointmentId)
        {
            try
            {
                return this.repo.AddTest(test, appointmentId);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid data" + e.Message);
            }
        }

        public List<Test> GetAllTests()
        {
            return this.repo.GetAllTests();
        }

        public List<Test> GetRecommendedTests(int appointmentId)
        {
            return this.repo.GetRecommendedTests(appointmentId).Select(t => t.Test).ToList();
        }

        public TestReport DeleteTest(int appointmentId, int testReportId)
        {

            return this.repo.DeleteTest(appointmentId, testReportId);

        }

        public List<TestReportDTO> GetTestReports()
        {

            var testReports = this.repo.GetTestReports();
            return ToTestReportDTOList(testReports);
        }

        public List<TestReportDTO> ToTestReportDTOList(List<TestReport> testReports)
        {
            if (testReports == null)
            {
                return null;
            }
            List<TestReportDTO> testReportDTOs = new List<TestReportDTO>();
            foreach (TestReport report in testReports)
            {
                testReportDTOs.Add(ToTestReportDTO(report));
            }
            return testReportDTOs;
        }

        public TestReportDTO ToTestReportDTO(TestReport testReport)
        {
            if (testReport == null)
            {
                return null;
            }
            TestReportDTO testReportDTO = new TestReportDTO
            {
                Id = testReport.Id,
                TestId = testReport.TestId,
            };
            return testReportDTO;
        }

        #endregion

        #region Pankaj
        public List<VitalDTO> GetAllVitalsDTO()
        {
            List<VitalDTO> list = new List<VitalDTO>();
            List<Vital> vital = repo.getAllVitals();
            foreach (Vital v in vital)
            {
                VitalDTO temp = new VitalDTO();
                temp.id = v.Id;
                temp.diabetes = v.Diabetes;
                temp.ecg = v.ECG;
                temp.temperature = v.Temperature;
                temp.respiration_rate = v.RespirationRate;
                list.Add(temp);
            }
            return list;
        }


        public VitalDTO GetVitalDTOByVitalId(int vital_id)
        {
            VitalDTO vitaldto = new VitalDTO();
            Vital vital = repo.getVitalById(vital_id);
            if (vital != null)
            {
                vitaldto.id = vital.Id;
                vitaldto.diabetes = vital.Diabetes;
                vitaldto.ecg = vital.ECG;
                vitaldto.temperature = vital.Temperature;
                vitaldto.respiration_rate = vital.RespirationRate;
            }
            return vitaldto;
        }

        public VitalDTO UpdateVital(VitalDTO v)
        {
            Vital vital = new Vital()
            {
                Id = v.id,
                Diabetes = v.diabetes,
                Temperature = v.temperature,
                ECG = v.ecg,
                RespirationRate = v.respiration_rate
            };

            Vital vital1 = repo.updateVital(vital);

            v.ecg = vital1.ECG;
            v.respiration_rate = vital1.RespirationRate;
            v.temperature = vital1.Temperature;
            v.diabetes = vital1.Diabetes;

            return v;
        }
        #endregion

    }
}
