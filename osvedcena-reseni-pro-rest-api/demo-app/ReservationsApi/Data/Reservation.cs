using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationsApi.Data;

public class Reservation
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(6)")]
    public string Apid { get; set; }
    public DateTime Date { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; }
}