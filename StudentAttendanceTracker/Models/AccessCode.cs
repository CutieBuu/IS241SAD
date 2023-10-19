using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceTracker.Models
{
    public class AccessCode
    {
        /// <summary>
        /// Primary key of the AccessCodes table
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key of the Course associated with this access code
        /// </summary>
        public int CourseID { get; set; }

        /// <summary>
        /// Course object of the course associated with this access code
        /// </summary>
        public Course Course { get; set; }

    }
}
