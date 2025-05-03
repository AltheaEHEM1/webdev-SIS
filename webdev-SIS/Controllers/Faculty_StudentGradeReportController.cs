using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using System.IO;

namespace system_SIS.Controllers
{
    public class FacultyStudentGradeReportController : Controller
    {
        public IActionResult GeneratePdfGrade()
        {
            var student = new
            {
                LRN = "563786535809",
                Name = "Cuerdo, Joshua",
                Age = "15",
                Sex = "Male",
                Grade = "7",
                Section = "Araling Panlipunan",
                SchoolYear = "2023-2024",
                Subjects = new[] {
                    new { Name = "Filipino", QuarterGrades = new[] { 82, 81, 93, 92 }, FinalGrade = 87, Remarks = "Passed" },
                    new { Name = "English", QuarterGrades = new[] { 86, 91, 91, 93 }, FinalGrade = 90, Remarks = "Passed" },
                    new { Name = "Mathematics", QuarterGrades = new[] { 85, 90, 92, 94 }, FinalGrade = 90, Remarks = "Passed" },
                    new { Name = "Science", QuarterGrades = new[] { 88, 89, 87, 89 }, FinalGrade = 88, Remarks = "Passed" },
                    new { Name = "Araling Panlipunan (AP)", QuarterGrades = new[] { 86, 90, 87, 88 }, FinalGrade = 88, Remarks = "Passed" },
                    new { Name = "Edukasyon sa Pagpapakatao (EsP)", QuarterGrades = new[] { 82, 90, 91, 92 }, FinalGrade = 89, Remarks = "Passed" },
                    new { Name = "Technology and Livelihood Education (TLE)", QuarterGrades = new[] { 87, 90, 91, 92 }, FinalGrade = 90, Remarks = "Passed" },
                    new { Name = "MAPEH", QuarterGrades = new[] { 90, 91, 92, 93 }, FinalGrade = 92, Remarks = "Passed" }
                },
                GeneralAverage = 89
            };

            // Create PDF document
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            var graphics = XGraphics.FromPdfPage(page);
            var titleFont = new XFont("Arial", 16, XFontStyle.Bold);
            var boldFont = new XFont("Arial", 12, XFontStyle.Bold);
            var font = new XFont("Arial", 10, XFontStyle.Regular);

            // Add Logo
            string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
            double logoHeight = 0;
            if (System.IO.File.Exists(logoPath))
            {
                var logo = XImage.FromFile(logoPath);
                var logoWidth = 100;
                logoHeight = (logo.PixelHeight / (double)logo.PixelWidth) * logoWidth;
                double logoX = (page.Width - logoWidth) / 2; // Center the logo horizontally
                double logoY = 20; // Position the logo at the top
                graphics.DrawImage(logo, logoX, logoY, logoWidth, logoHeight);
            }

            // Header
            double headerY = 30 + logoHeight; // Adjust header Y to move below the logo
            graphics.DrawString("Laurelcrest High School", titleFont, XBrushes.Black,
                new XRect(0, headerY, page.Width, 30), XStringFormats.TopCenter);
            graphics.DrawString("LEARNER'S PROGRESS REPORT CARD", font, XBrushes.Black,
                new XRect(0, headerY + 40, page.Width, 20), XStringFormats.TopCenter);

            // Student Info
            double infoY = headerY + 90;
            graphics.DrawString($"Name: {student.Name}", boldFont, XBrushes.Black, new XPoint(40, infoY));
            graphics.DrawString($"Age: {student.Age}", boldFont, XBrushes.Black, new XPoint(300, infoY));
            graphics.DrawString($"Grade: {student.Grade}", boldFont, XBrushes.Black, new XPoint(40, infoY + 20));
            graphics.DrawString($"Sex: {student.Sex}", boldFont, XBrushes.Black, new XPoint(300, infoY + 20));
            graphics.DrawString($"School Year: {student.SchoolYear}", boldFont, XBrushes.Black, new XPoint(40, infoY + 40));
            graphics.DrawString($"Section: {student.Section}", boldFont, XBrushes.Black, new XPoint(300, infoY + 40));
            graphics.DrawString($"LRN: {student.LRN}", boldFont, XBrushes.Black, new XPoint(40, infoY + 60));

            // Grades Table
            double tableY = infoY + 100;
            double[] columnWidths = { 150, 40, 40, 40, 40, 80, 100 }; // Column widths
            double tableX = 40;

            // Draw Table Header
            var headerHeight = 30;
            graphics.DrawRectangle(XPens.Black, tableX, tableY, columnWidths.Sum(), headerHeight);
            var headerTitles = new[] { "Learning Areas", "1st", "2nd", "3rd", "4th", "Final Grade", "Remarks" };

            double xPosition = tableX;
            for (int i = 0; i < headerTitles.Length; i++)
            {
                graphics.DrawRectangle(XPens.Black, xPosition, tableY, columnWidths[i], headerHeight);
                graphics.DrawString(headerTitles[i], boldFont, XBrushes.Black,
                    new XRect(xPosition, tableY, columnWidths[i], headerHeight), XStringFormats.Center);
                xPosition += columnWidths[i];
            }

            // Draw Table Rows
            double currentY = tableY + headerHeight;
            foreach (var subject in student.Subjects)
            {
                double rowHeight = 30;
                double rowX = tableX;

                // Learning Areas
                var learningAreaFormatter = new XTextFormatter(graphics);
                var learningAreaRect = new XRect(rowX + 5, currentY + 5, columnWidths[0] - 10, rowHeight - 10); // Padding
                learningAreaFormatter.DrawString(subject.Name, font, XBrushes.Black, learningAreaRect);
                rowX += columnWidths[0];

                // Quarter Grades
                for (int i = 0; i < subject.QuarterGrades.Length; i++)
                {
                    graphics.DrawString(subject.QuarterGrades[i].ToString(), font, XBrushes.Black,
                        new XRect(rowX, currentY, columnWidths[i + 1], rowHeight), XStringFormats.Center);
                    rowX += columnWidths[i + 1];
                }

                // Final Grade
                graphics.DrawString(subject.FinalGrade.ToString(), font, XBrushes.Black,
                    new XRect(rowX, currentY, columnWidths[5], rowHeight), XStringFormats.Center);
                rowX += columnWidths[5];

                // Remarks
                graphics.DrawString(subject.Remarks, font, XBrushes.Black,
                    new XRect(rowX, currentY, columnWidths[6], rowHeight), XStringFormats.Center);

                // Draw Row Border
                graphics.DrawRectangle(XPens.Black, tableX, currentY, columnWidths.Sum(), rowHeight);
                currentY += rowHeight;
            }

            // Footer
            double footerY = page.Height - 40; // Positioning the footer 40 units above the bottom of the page
            string footerText = "Generated by Laurelcrest High School - Confidential";

            graphics.DrawString(footerText, font, XBrushes.Black,
                new XRect(0, footerY, page.Width, 20), XStringFormats.Center);



            // Save PDF
            using (var memoryStream = new MemoryStream())
            {
                pdf.Save(memoryStream);
                return File(memoryStream.ToArray(), "application/pdf", "grade_report.pdf");
            }
        }
    }
}
