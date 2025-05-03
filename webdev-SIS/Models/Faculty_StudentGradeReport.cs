namespace webdev_SIS.Models
{
    public class Faculty_StudentGradeReport
    {
        public string? LRN { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? GradeLevel { get; set; }

        public List<SubjectGrade>? SubjectGrades { get; set; }

        public string? Status { get; set; }
    }
    public class SubjectGrade
    {
        public string? SubjectName { get; set; }

        public List<double?> QuarterGrades { get; set; } = new List<double?>();

        public double? FinalGrade { get; set; }

        public string? Remarks { get; set; }
    }
}
