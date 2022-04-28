﻿using CMD.Business.Appointments;
using CMD.DTO.Appointments;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using CMD.Business.Appointments.CustomExceptions;
using System;

namespace CMD.API.Appointments.Controllers
{
    [RoutePrefix("api/appointment")]    
    public class AppointmentController : ApiController
    {
        protected IAppointmentManager appointmentManager;
        
        public AppointmentController(IAppointmentManager appointmentManager) 
        {
            this.appointmentManager = appointmentManager;
        }

        // GET: api/appointment/allappointments/{doctorId}
        [HttpGet]
        [Route("allappointments/{doctorId}")]
        [ResponseType(typeof(AppointmentBasicInfoDTO))]
        public IHttpActionResult GetAllAppointment(int doctorId, [FromUri]PaginationParams parameters)
        {
            ICollection<AppointmentBasicInfoDTO> appointments = appointmentManager.GetAllAppointment(doctorId, parameters);

            if(appointments.Count() == 0)
            {
                return Ok("No appointment");
            }

            var totalAppointmentCount = appointmentManager.GetAppointmentCount(doctorId);

            var paginationMetaData = new PaginationMetaData(totalAppointmentCount, parameters.Page, parameters.ItemsPerPage, appointments);
            
            var response = Request.CreateResponse(HttpStatusCode.OK, paginationMetaData);
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("allappointments/{status}/{doctorId}")]
        [ResponseType(typeof(AppointmentBasicInfoDTO))]
        public IHttpActionResult GetAllAppointmentBasedOnStatus(int doctorId, string status, [FromUri] PaginationParams parameters)
        {
            ICollection<AppointmentBasicInfoDTO> appointments = appointmentManager.GetAllAppointment(doctorId, status,parameters);

            if (appointments.Count() == 0)
            {
                return Ok("No appointment");
            }

            var totalAppointmentCount = appointmentManager.GetAppointmentCountBasedOnStatus(doctorId, status);

            var paginationMetaData = new PaginationMetaData(totalAppointmentCount, parameters.Page, parameters.ItemsPerPage, appointments);
            
            var response = Request.CreateResponse(HttpStatusCode.OK, paginationMetaData);
            return ResponseMessage(response);
        }

        // GET: api/appointment/recommendedpatient/{doctorId}
        [HttpGet]
        [Route("patients/{doctorId}")]
        [ResponseType(typeof(PatientDTOForPatientSearch))]
        public IHttpActionResult GetAllPatient(int doctorId)
        {
            ICollection<PatientDTOForPatientSearch> patientsDTO = appointmentManager.GetPatients(doctorId);
            if(patientsDTO.Count() == 0)
            {
                return NotFound();
            }
            return Ok(patientsDTO);
        }

        // GET: api/appointment/issues
        [HttpGet]
        [Route("issues")]
        [ResponseType(typeof(IssueDTO))]
        public IHttpActionResult GetAllIssues()
        {
            var isssues = appointmentManager.GetIssues();
            if(isssues.Count() == 0)
            {
                return NotFound();
            }
            return Ok(isssues);
        }

        // PUT: api/appointment/acceptappointment/{appointmentId}/doctorId/{doctorId}
        [HttpPut]
        [Route("changestatus/doctorId/{doctorId}")]
        public IHttpActionResult ChangeStatus([FromBody]AppointmentStatusDTO appDTO, int doctorId)
        {
            var result = appointmentManager.ChangeAppointmentStatus(appDTO, doctorId);
            var response = Request.CreateResponse(result ?HttpStatusCode.NoContent:HttpStatusCode.PreconditionFailed);
            return ResponseMessage(response);
        }

        // POST: api/appointment/create
        [HttpPost]
        [Route("create")]
        [ResponseType(typeof(AppointmentFormDTO))]
        public IHttpActionResult AddAppointment(AppointmentFormDTO jsonData)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AppointmentConfirmationDTO result = null;

            AppointmentFormDTO appointmentForm = jsonData;
            try
            {
                result = appointmentManager.AddAppointment(appointmentForm);
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
            catch(SameAppointmentTimingException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Created($"api/appointment/{result.AppointmentId}", result);
        }

        #region kishore code
        // GET: api/appointment/allappointments/{doctorId}
        [HttpGet]
        [Route("getIds/{appointmentId}")]
        [ResponseType(typeof(int[]))]
        public IHttpActionResult GetIdsAssociatedWithAppointment(int appointmentId)
        {
            return Ok(appointmentManager.GetIdsAssociatedWithAppointment(appointmentId));
        }
        #endregion

        #region Praveen Code

        // GET: api/appointment/comment/{appointmentId}
        [HttpGet]
        [Route("comment/{appointmentId}")]
        [ResponseType(typeof(AppointmentCommentDTO))]
        public IHttpActionResult GetComment(int appointmentId)
        {
            var a = appointmentManager.GetAppointmentComment(appointmentId);
            return Ok(a);
        }


        // PUT: api/appointment/comment/{appointmentId}
        [HttpPut]
        [Route("comment/{appointmentId}")]
        [ResponseType(typeof(AppointmentCommentDTO))]
        public IHttpActionResult EditComment(int appointmentId, AppointmentCommentDTO comment)
        {
            var a = appointmentManager.UpdateAppointmentComment(appointmentId, comment);
            if (!a)
            {
                return BadRequest();
            }
            return Ok(comment);
        }
        #endregion
    }
}
