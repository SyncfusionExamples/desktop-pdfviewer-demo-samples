using Syncfusion.Pdf;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace HostedPdfViewer
{
    /// <summary>
    /// Interaction logic for WPFPdfViewer.xaml
    /// </summary>
    public partial class WPFPdfViewer : UserControl
    {
        string filePath = string.Empty;
        public WPFPdfViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when the user control is loaded.
        /// </summary>
        /// <param name="sender">User control</param>
        /// <param name="e">Routed Event arguments</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Load a PDF file in the PDF viewer
#if !NETCOREAPP
            filePath = "../../Data/F#.pdf";
#else
            filePath = "../../../Data/F#.pdf";
#endif
            byte[] pdfBytes = File.ReadAllBytes(filePath);
            var stream = new MemoryStream(pdfBytes);
            var document = new PdfLoadedDocument(stream);
            pdfViewer.Load(document);
        }

        private void ExportAnnotations_Click(object sender, RoutedEventArgs e)
        {
            pdfViewer.ExportAnnotations("../../../Data/annotations.xfdf", AnnotationDataFormat.XFdf);
        }

        private void ImportAnnotations_Click(object sender, RoutedEventArgs e)
        {
            PdfLoadedDocument document = new PdfLoadedDocument(filePath);
            foreach (PdfLoadedPage page in document.Pages)
            {
                for (int i = page.Annotations.Count - 1; i >= 0; i--)
                {
                    if (!(page.Annotations[i] is PdfLoadedUriAnnotation or PdfLoadedDocumentLinkAnnotation or PdfLoadedFileLinkAnnotation or PdfLoadedTextWebLinkAnnotation or PdfLoadedWidgetAnnotation))
                    {
                        page.Annotations.RemoveAt(i);
                    }
                }
            }
            document.ImportAnnotations("../../../Data/annotations.xfdf", AnnotationDataFormat.XFdf);
            document.FileStructure.IncrementalUpdate = false;
            document.Save("../../../Data/UpdatedDocument.pdf");

            byte[] pdfBytes = File.ReadAllBytes("../../../Data/UpdatedDocument.pdf");
            var document1 = new PdfLoadedDocument(pdfBytes);
            pdfViewer.Load(document1);
        }
    }
}
