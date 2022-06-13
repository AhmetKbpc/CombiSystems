using System.ComponentModel.DataAnnotations;

namespace CombiSystems.Web.ViewModels
{
    public class AppointmentViewModel
    {
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        [StringLength(500)]
        public string Description { get; set; }


        [Required(ErrorMessage = "Adress alanı gereklidir")]
        [Display(Name = "Adress")]
        public string AppointmentAddress { get; set; }

        public List<Core.Entities.Appointment> Appointments{ get; set; }
    }
}
