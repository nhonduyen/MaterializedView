using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MaterializedViews.API.Models
{
    [Table("Event", Schema = "Source")]
    public class Event
    {
        public int WidgetID { get; set; }
        public int EventTypeID { get; set; }
        public int TripID { get; set; }
        public DateTime EventDate { get; set; }
    }
}
