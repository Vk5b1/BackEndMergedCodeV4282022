using System.ComponentModel.DataAnnotations.Schema;

namespace CMD.Model.Appointments
{
    public class Recommedation
    {
        public int RecommedationId { get; set; }
        [ForeignKey("PatientDetail")]
        public int PatientDetialId { get; set; }

        public PatientDetail PatientDetail { get; set; }

        [ForeignKey("RecommendedDoctor")]
        public int RecommendedDoctorId { get; set; }

        public Doctor RecommendedDoctor { get; set; }
    }
}
