using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MaterializedViews.API.Models
{
    [Table("Event", Schema = "Source")]
    public class Event
    {
        [Key, Column(Order = 0), NotNull]
        public int WidgetID { get; set; }
        [Key, Column(Order = 1), NotNull]
        public int EventTypeID { get; set; }
        [Key, Column(Order = 2), NotNull]
        public int TripID { get; set; }
        [Key, Column(Order = 3)]
        public DateTime EventDate { get; set; }
    }
}
