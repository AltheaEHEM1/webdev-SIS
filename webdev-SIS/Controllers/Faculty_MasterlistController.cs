using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf; // Add this using directive
using PdfSharpCore.Drawing;
using webdev_SIS.Models; // Add this using directive

namespace system_SIS.Controllers
{
    public class Faculty_MasterlistController : Controller
    {
        public IActionResult GeneratePdf()
        {
            // Sample list of students (hardcoded data)
            var students = new List<Faculty_MasterlistStudents>
            {
                new Faculty_MasterlistStudents { LRN = "345673596432", Name = "John Doe", Gender = "Male" },
                new Faculty_MasterlistStudents { LRN = "249506493827", Name = "Jane Smith", Gender = "Female" }
            };

            // Create a new PDF document
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            var graphics = XGraphics.FromPdfPage(page);
            var font = new XFont("Verdana", 12, XFontStyle.Regular);
            var titleFont = new XFont("Verdana", 16, XFontStyle.Bold);
            var smallerTitleFont = new XFont("Verdana", 12, XFontStyle.Regular);  // Smaller font for Masterlist of Students

            // Draw the logo at the center of the page
            string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
            if (System.IO.File.Exists(logoPath))
            {
                var logo = XImage.FromFile(logoPath);
                var logoWidth = 100; // Set the desired width
                var logoHeight = (logo.PixelHeight / (double)logo.PixelWidth) * logoWidth; // Maintain aspect ratio
                graphics.DrawImage(logo, (page.Width - logoWidth) / 2, 30, logoWidth, logoHeight);  // Center the logo
            }

            // Draw school name below the logo and center it
            graphics.DrawString("Laurelcrest High School", titleFont, XBrushes.Black, new XPoint(page.Width / 2, 150), XStringFormats.Center);

            // Add spacing
            graphics.DrawString("", font, XBrushes.Black, new XPoint(page.Width / 2, 170), XStringFormats.Center);

            // Draw Masterlist of Students below the school name, centered, with smaller font
            graphics.DrawString("Masterlist of Students", smallerTitleFont, XBrushes.Black, new XPoint(page.Width / 2, 210), XStringFormats.Center);
            graphics.DrawString("Grade: 7-Mars", font, XBrushes.Black, new XPoint(page.Width / 2, 230), XStringFormats.Center);
            graphics.DrawString("Academic School Year: 2024-2025", font, XBrushes.Black, new XPoint(page.Width / 2, 250), XStringFormats.Center);

            // Table start position (centered)
            var tableWidth = 500; // Set the table width
            var tableStartX = (page.Width - tableWidth) / 2; // Center the table horizontally
            var tableStartY = 290; // Starting Y position for the table

            // Column widths
            var col1Width = 150;  // LRN column width
            var col2Width = 200;  // Name column width
            var col3Width = 150;  // Gender column width

            // Draw the table header with light blue background
            var headerFont = new XFont("Verdana", 12, XFontStyle.Bold);
            graphics.DrawRectangle(XBrushes.LightBlue, tableStartX, tableStartY, tableWidth, 30);
            graphics.DrawString("LRN", headerFont, XBrushes.Black, new XPoint(tableStartX + col1Width / 2, tableStartY + 15), XStringFormats.Center);  // LRN column
            graphics.DrawString("Name", headerFont, XBrushes.Black, new XPoint(tableStartX + col1Width + col2Width / 2, tableStartY + 15), XStringFormats.Center); // Name column
            graphics.DrawString("Gender", headerFont, XBrushes.Black, new XPoint(tableStartX + col1Width + col2Width + col3Width / 2, tableStartY + 15), XStringFormats.Center); // Gender column

            // Draw vertical lines between columns
            graphics.DrawLine(XPens.Black, tableStartX + col1Width, tableStartY, tableStartX + col1Width, tableStartY + 30);  // Between LRN and Name
            graphics.DrawLine(XPens.Black, tableStartX + col1Width + col2Width, tableStartY, tableStartX + col1Width + col2Width, tableStartY + 30);  // Between Name and Gender

            // Add a space below the table header
            var rowY = tableStartY + 30 + 10;  // Add 10px of space below the header

            // Draw the student data rows
            foreach (var student in students)
            {
                // LRN column
                graphics.DrawString(student.LRN, font, XBrushes.Black, new XPoint(tableStartX + col1Width / 2, rowY), XStringFormats.Center);
                // Name column
                graphics.DrawString(student.Name, font, XBrushes.Black, new XPoint(tableStartX + col1Width + col2Width / 2, rowY), XStringFormats.Center);
                // Gender column
                graphics.DrawString(student.Gender, font, XBrushes.Black, new XPoint(tableStartX + col1Width + col2Width + col3Width / 2, rowY), XStringFormats.Center);

                // Draw vertical lines between columns
                graphics.DrawLine(XPens.Black, tableStartX + col1Width, rowY, tableStartX + col1Width, rowY + 20);  // Between LRN and Name
                graphics.DrawLine(XPens.Black, tableStartX + col1Width + col2Width, rowY, tableStartX + col1Width + col2Width, rowY + 20);  // Between Name and Gender

                rowY += 20;  // Move down for the next row
            }

            // Draw the bottom border of the table
            graphics.DrawRectangle(XPens.Black, tableStartX, tableStartY, tableWidth, rowY - tableStartY);

            // Save the PDF to a memory stream
            using (var stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                stream.Position = 0; // Reset the stream position to the beginning
                // Return the PDF as a downloadable file
                return new FileContentResult(stream.ToArray(), "application/pdf")
                {
                    FileDownloadName = "FacultyMasterlist.pdf"
                };
            }
        }
    }
}
