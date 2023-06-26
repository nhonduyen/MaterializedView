using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WorkerService.Models
{
    [Table("WidgetLatestState", Schema = "Dest")]
    public class WidgetLatestState
    {
        [Key, NotNull]
        public int WidgetID { get; set; }
        [NotNull]
        public int LastTripID { get; set; }

        public DateTime LastEventDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
    }
}
