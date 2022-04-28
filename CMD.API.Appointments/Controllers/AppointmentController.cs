using CMD.Business.Appointments;
using CMD.DTO.Appointments;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http;
using System.Net;
using CMD.Business.Appointments.CustomExceptions;
using System;
using CMD.Model;

namespace CMD.API.Appointments.Controllers
{
    [RoutePrefix("api/cmd")]
    public class AppointmentController : ApiController
    {
        protected IAppointmentManager manager;

        public AppointmentController(IAppointmentManager manager)
        {
            this.manager = manager;
        }

        #region Subham

        // GET: api/appointment/allappointments/{doctorId}
        [HttpGet]
        [Route("appointment/allappointments/{doctorId}")]
        [ResponseType(typeof(AppointmentBasicInfoDTO))]
        public IHttpActionResult GetAllAppointment(int doctorId, [FromUri] PaginationParams parameters)
        {
            ICollection<AppointmentBasicInfoDTO> appointments = manager.GetAllAppointment(doctorId, parameters);

            if (appointments.Count() == 0)
            {
                return Ok("No appointment");
            }

            var totalAppointmentCount = manager.GetAppointmentCount(doctorId);

            var paginationMetaData = new PaginationMetaData(totalAppointmentCount, parameters.Page, parameters.ItemsPerPage, appointments);

            var response = Request.CreateResponse(HttpStatusCode.OK, paginationMetaData);
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("appointment/allappointments/{status}/{doctorId}")]
        [ResponseType(typeof(AppointmentBasicInfoDTO))]
        public IHttpActionResult GetAllAppointmentBasedOnStatus(int doctorId, string status, [FromUri] PaginationParams parameters)
        {
            ICollection<AppointmentBasicInfoDTO> appointments = manager.GetAllAppointment(doctorId, status, parameters);

            if (appointments.Count() == 0)
            {
                return Ok("No appointment");
            }

            var totalAppointmentCount = manager.GetAppointmentCountBasedOnStatus(doctorId, status);

            var paginationMetaData = new PaginationMetaData(totalAppointmentCount, parameters.Page, parameters.ItemsPerPage, appointments);

            var response = Request.CreateResponse(HttpStatusCode.OK, paginationMetaData);
            return ResponseMessage(response);
        }

        // GET: api/appointment/recommendedpatient/{doctorId}
        [HttpGet]
        [Route("appointment/patients/{doctorId}")]
        [ResponseType(typeof(PatientDTOForPatientSearch))]
        public IHttpActionResult GetAllPatient(int doctorId)
        {
            ICollection<PatientDTOForPatientSearch> patientsDTO = manager.GetPatients(doctorId);
            if (patientsDTO.Count() == 0)
            {
                return NotFound();
            }
            return Ok(patientsDTO);
        }

        // GET: api/appointment/issues
        [HttpGet]
        [Route("appointment/issues")]
        [ResponseType(typeof(IssueDTO))]
        public IHttpActionResult GetAllIssues()
        {
            var issues = manager.GetIssues();
            if (issues.Count() == 0)
            {
                return NotFound();
            }
            return Ok(issues);
        }

        // PUT: api/appointment/acceptappointment/{appointmentId}/doctorId/{doctorId}
        [HttpPut]
        [Route("appointment/changestatus/doctorId/{doctorId}")]
        public IHttpActionResult ChangeStatus([FromBody] AppointmentStatusDTO appDTO, int doctorId)
        {
            var result = manager.ChangeAppointmentStatus(appDTO, doctorId);
            var response = Request.CreateResponse(result ? HttpStatusCode.NoContent : HttpStatusCode.PreconditionFailed);
            return ResponseMessage(response);
        }

        // POST: api/cmd/appointment/create
        [HttpPost]
        [Route("appointment/create")]
        [ResponseType(typeof(AppointmentFormDTO))]
        public IHttpActionResult AddAppointment(AppointmentFormDTO jsonData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AppointmentFormDTO appointmentForm = jsonData;

            AppointmentConfirmationDTO result;
            try
            {
                result = manager.AddAppointment(appointmentForm);
            }
            catch (DateNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PastDateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SameAppointmentTimingException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Created($"api/appointment/{result.AppointmentId}", result);
        }
        #endregion

        #region Kishore
        // GET: api/appointment/allappointments/{doctorId}
        [HttpGet]
        [Route("getIds/{appointmentId}")]
        [ResponseType(typeof(int[]))]
        public IHttpActionResult GetIdsAssociatedWithAppointment(int appointmentId)
        {
            return Ok(manager.GetIdsAssociatedWithAppointment(appointmentId));
        }

        // DoctorCard 

        // GET: api/appointment/allappointments/{doctorId}
        [HttpGet]
        [Route("doctorcard/{doctorId}")]
        [ResponseType(typeof(DoctorCardDTO))]
        public IHttpActionResult GetDoctorCard(int doctorId)
        {
            DoctorCardDTO cardDetails = manager.GetDoctorCard(doctorId);
            return Ok(cardDetails);
        }

        // PatientCard

        [HttpGet]
        [Route("patientcard/{patientId}")]
        [ResponseType(typeof(PatientCardDTO))]
        public IHttpActionResult GetPatientCard(int patientId)
        {
            PatientCardDTO cardDetails = manager.GetPatientCard(patientId);
            return Ok(cardDetails);
        }
        

        [HttpGet]
        [Route("dashboardsummary/{doctorId}")]
        [ResponseType(typeof(Dictionary<string, int>))]
        public IHttpActionResult DashboardSummary(int doctorId)
        {
            Dictionary<string, int> summary = manager.DashboardSummary(doctorId);
            return Ok(summary);
        }

        #endregion

        #region Praveen

        // GET: api/appointment/comment/{appointmentId}
        [HttpGet]
        [Route("comment/{appointmentId}")]
        [ResponseType(typeof(AppointmentCommentDTO))]
        public IHttpActionResult GetComment(int appointmentId)
        {
            var a = manager.GetAppointmentComment(appointmentId);
            return Ok(a);
        }


        // PUT: api/appointment/comment/{appointmentId}
        [HttpPut]
        [Route("comment/{appointmentId}")]
        [ResponseType(typeof(AppointmentCommentDTO))]
        public IHttpActionResult EditComment(int appointmentId, AppointmentCommentDTO comment)
        {
            var a = manager.UpdateAppointmentComment(appointmentId, comment);
            if (!a)
            {
                return BadRequest();
            }
            return Ok(comment);
        }
        #endregion

        #region Gagana
        [HttpGet]
        [Route("doctorprofile/{id}")]
        [ResponseType(typeof(DoctorProfileDTO))]
        public IHttpActionResult Get(int id)
        {

            DoctorProfileDTO doctor = manager.GetDoctorsWithContact(id);
            if (doctor == null)
            {
                var msg = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Your search ID is not found {0}", id)),
                    ReasonPhrase = "Doctor not found"
                };
                throw new HttpResponseException(msg);
            }
            return Ok(doctor);
        }



        // POST: api/Doctor
        [HttpPut]
        [Route("doctorprofile")]
        [ResponseType(typeof(DoctorProfileDTO))]
        public IHttpActionResult Put(DoctorProfileDTO doctor)
        {
            if (doctor == null)
            {
                var msg = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Your search ID is not found {0}", doctor)),
                    ReasonPhrase = "Doctor not found"
                };
                throw new HttpResponseException(msg);



            }
            manager.EditDoctor(doctor);
            return Ok(doctor);
        }



        #endregion

        #region Supriya
        [HttpGet]
        [Route("GetFeedback/{id}")]
        [ResponseType(typeof(FeedBack))]
        public IHttpActionResult GetFeedback(int id)
        {
            var result = manager.GetFeedback(id);
            if (result == null)
            {
                var msg = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Your feedback ID is not found {0}", id)),
                    ReasonPhrase = "Appointment Id not found"
                };
                throw new HttpResponseException(msg);
            }
            return Ok(result);

        }

        #endregion

        #region Akash
        [HttpGet]
        [Route("doctor/profile/{id}")]
        [ResponseType(typeof(DoctorDTO))]
        public IHttpActionResult GetDoctorProfilePicture(int id)
        {
            DoctorDTO doctor = manager.GetDoctors(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }

        [HttpPut]
        [Route("doctor/profile")]
        public void Put(DoctorDTO doctorDTO)
        {
            manager.EditDoctor(doctorDTO);
            return;
        }

        #endregion

        #region Bala

        [Route("Recommendation")]
        [HttpPost]
        [ResponseType(typeof(Recommedation))]
        public IHttpActionResult AddRecommendation(Recommedation recommendation)
        {
            var reco = manager.AddRecommendtaion(recommendation);

            return Created($"api/recommendation/{reco.RecommedationId}", reco);
        }


        [Route("Recommendation/{id}")]
        [HttpDelete]
        public IHttpActionResult Remove(int id)
        {
            if (!manager.RemoveRecommendation(id))
                return BadRequest();
            return Ok();

        }

        [Route("doctor")]
        [HttpGet]
        [ResponseType(typeof(List<DoctorInfoDTO>))]
        public List<DoctorInfoDTO> GetDoctorNames()
        {
            return manager.GetDoctors();
        }


        #endregion

        #region Sakshi + Saba

        [Route("PatientDetail/{PatientDetailId}")]
        [HttpGet]
        [ResponseType(typeof(ICollection<PrescriptionDTO>))]
        public IHttpActionResult GetPrescription(int PatientDetailId)
        {
            ICollection<PrescriptionDTO> prescriptionsDTO = manager.GetPrescriptions(PatientDetailId);
            if (prescriptionsDTO == null)
            {
                return NotFound();
            }
            return Ok(prescriptionsDTO);
        }


        [Route("PatientDetailId/{PatientDetailId}/PrescriptionId/{PrescriptionId}")]
        [HttpDelete]
        public IHttpActionResult DeletePrescription(int PatientDetailId, int PrescriptionId)
        {
            manager.DeletePrescription(PatientDetailId, PrescriptionId);
            return Ok();
            //int count = Prescription.DeletePrescription(PatientDetailId);
        }



        // POST and PUT methods And Get



        [HttpGet]
        [Route("medicine")]
        [ResponseType(typeof(ICollection<Medicine>))]
        public IHttpActionResult GetAllMedicines()
        {
            ICollection<Medicine> medicines = manager.GetAllMedicine();
            return Ok(medicines);
        }



        [HttpPost]
        [Route("AddPrescription/{patientDetailId}")]
        [ResponseType(typeof(PrescriptionDTO))]
        public IHttpActionResult AddPrescription(int patientDetailId, PrescriptionDTO prescriptionDto)
        {



            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }




            var p = manager.AddPrescription(patientDetailId, prescriptionDto);

            return Ok(p);



        }



        [HttpPut]
        [Route("PrescriptionEdit/{id}")]
        [ResponseType(typeof(PrescriptionDTO))]
        public IHttpActionResult Edit(int id, PrescriptionDTO prescriptionDto)
        {
            if (id != prescriptionDto.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            var p = manager.UpdatePrescription(prescriptionDto);



            return Ok(p);
        }




        #endregion

        #region Abhishek + Venkat

        [Route("AddTest/{appointmnetId}")]
        [HttpPost]
        [ResponseType(typeof(TestReport))]
        public IHttpActionResult AddTest(int appointmnetId, Test test)
        {
            if (test == null)
                return BadRequest("Invalid input");
            if (appointmnetId < 0)
            {
                return BadRequest("Invalid input");
            }
            var testReport = manager.AddTest(test, appointmnetId);

            return Ok(testReport);
        }

        [HttpGet]
        [Route("GetTest")]
        [ResponseType(typeof(List<Test>))]
        public IHttpActionResult GetTests()
        {
            var tests = manager.GetAllTests();
            return Ok(tests);
        }

        [Route("GetRecommendedTest/{appointmentId}")]
        [HttpGet]
        [ResponseType(typeof(List<Test>))]
        public IHttpActionResult GetRecommendedTest(int appointmentId)
        {
            var tests = manager.GetRecommendedTests(appointmentId);
            return Ok(tests);
        }

        [Route("RemoveTest/testReportid/{testReportId}/appointmentid/{appointmentId}")]
        [HttpDelete]
        [ResponseType(typeof(TestReport))]
        public IHttpActionResult RemoveTest(int testReportId, int appointmentId)
        {
            var result = manager.DeleteTest(appointmentId, testReportId);
            return Ok(result);
        }

        [Route("GetTestReports")]
        [HttpGet]
        [ResponseType(typeof(List<TestReportDTO>))]
        public IHttpActionResult GetTestReports()
        {
            var testReport = manager.GetTestReports();
            return Ok(testReport);
        }

        #endregion

        #region Pankaj

        [Route("GetAllVitals")]
        [HttpGet]
        [ResponseType(typeof(List<VitalDTO>))]
        public IHttpActionResult Get()
        {
            return Ok(manager.GetAllVitalsDTO());
        }

        [Route("GetAllVitals/{id}")]
        [HttpGet]
        [ResponseType(typeof(VitalDTO))]
        // GET: api/Vital/5
        public IHttpActionResult GetAllVitals(int id)
        {
            return Ok(manager.GetVitalDTOByVitalId(id));
        }

        //Action method for editing
        // PUT: api/Vital/id
        [Route("UpdateVital")]
        [HttpPut]
        [ResponseType(typeof(VitalDTO))]
        public IHttpActionResult Put(int id, VitalDTO vital)
        {
            if (id != vital.id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest("Invalid Input");

            VitalDTO v = manager.UpdateVital(vital);

            return Ok(v);
        }
        #endregion
    }
}
