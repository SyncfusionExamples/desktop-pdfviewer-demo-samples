# Desktop PDF Viewer Demo Samples

A collection of Syncfusion PDF Viewer demo samples for desktop applications. The current sample, **HostedPdfViewer**, shows how to host the Syncfusion WPF PDF Viewer (`PdfViewerControl`) inside a Windows Forms application and how to export and import annotations in XFDF format.

## Description and Purpose

Many existing desktop applications are built with Windows Forms but need modern PDF viewing and annotation features. The Syncfusion WPF PDF Viewer can be used in those applications by placing it in a WPF `UserControl` and hosting that control in a Windows Forms form.

This sample demonstrates:

- Hosting the Syncfusion `PdfViewerControl` (WPF) in a Windows Forms app.
- Loading a PDF document into the viewer from a stream.
- Exporting the annotations added in the viewer to an XFDF file.
- Importing annotations from an XFDF file into a PDF document and showing the updated document in the viewer.

**Technologies:** .NET 10 (`net10.0-windows7.0`), Windows Forms, WPF, Syncfusion `Syncfusion.PdfViewer.WPF` NuGet package (version 35.1.42).

## Features

### 1. Host the Syncfusion WPF PDF Viewer in Windows Forms

The viewer is declared in a WPF `UserControl` (`WPFPdfViewer.xaml`), together with two buttons for exporting and importing annotations.

```xml
<PdfViewer:PdfViewerControl Grid.Row="1" Grid.ColumnSpan="2" x:Name="pdfViewer"/>
<Button Content="Export Annotations" Click="ExportAnnotations_Click" .../>
<Button Content="Import Annotations" Click="ImportAnnotations_Click" .../>
```

`Form1` creates an `ElementHost`, docks it inside a panel, and sets the WPF user control as its child:

```csharp
elementHost = new ElementHost();
elementHost.Dock = DockStyle.Fill;
panel1.Controls.Add(elementHost);

WPFPdfViewer pdfViewer = new WPFPdfViewer();
elementHost.Child = pdfViewer;
```

### 2. Load a PDF document

When the user control is loaded, the sample reads `Data/F#.pdf` into a `MemoryStream`, creates a `PdfLoadedDocument`, and loads it into the viewer.

```csharp
byte[] pdfBytes = File.ReadAllBytes(filePath);
var stream = new MemoryStream(pdfBytes);
var document = new PdfLoadedDocument(stream);
pdfViewer.Load(document);
```

### 3. Export annotations to XFDF

Annotations added in the viewer (for example free text, shapes, and highlights) are saved to `Data/annotations.xfdf` by clicking **Export Annotations**.

```csharp
pdfViewer.ExportAnnotations("../../../Data/annotations.xfdf", AnnotationDataFormat.XFdf);
```

### 4. Import annotations from XFDF

Clicking **Import Annotations** does the following:

1. Opens the source PDF as a `PdfLoadedDocument`.
2. Removes existing annotations from every page, but keeps links (URI, document link, file link, text web link) and form field widgets.
3. Imports the annotations from the XFDF file.
4. Saves the result as `Data/UpdatedDocument.pdf` and reloads it into the viewer.

```csharp
PdfLoadedDocument document = new PdfLoadedDocument(filePath);
foreach (PdfLoadedPage page in document.Pages)
{
    for (int i = page.Annotations.Count - 1; i >= 0; i--)
    {
        if (!(page.Annotations[i] is PdfLoadedUriAnnotation or PdfLoadedDocumentLinkAnnotation
              or PdfLoadedFileLinkAnnotation or PdfLoadedTextWebLinkAnnotation
              or PdfLoadedWidgetAnnotation))
        {
            page.Annotations.RemoveAt(i);
        }
    }
}
document.ImportAnnotations("../../../Data/annotations.xfdf", AnnotationDataFormat.XFdf);
document.FileStructure.IncrementalUpdate = false;
document.Save("../../../Data/UpdatedDocument.pdf");

byte[] pdfBytes = File.ReadAllBytes("../../../Data/UpdatedDocument.pdf");
pdfViewer.Load(new PdfLoadedDocument(pdfBytes));
```

## Folder Structure

```
desktop-pdfviewer-demo-samples
├── README.md
└── HostedPdfViewer
    ├── HostedPdfViewer_NET.sln        # Visual Studio solution
    ├── HostedPdfViewer_NET.csproj     # Project file (net10.0-windows7.0, WPF + WinForms, Syncfusion.PdfViewer.WPF)
    ├── Program.cs                     # Application entry point, starts Form1
    ├── Form1.cs / Form1.Designer.cs   # Windows Forms window hosting the WPF control via ElementHost
    ├── Form1.resx                     # Form resources
    ├── WPFPdfViewer.xaml              # WPF user control with PdfViewerControl and the Export/Import buttons
    ├── WPFPdfViewer.xaml.cs           # Load, export, and import annotation logic
    ├── App.config                     # Application configuration
    ├── packages.config                # NuGet package list
    ├── Properties                     # Assembly info, resources, and settings
    └── Data
        ├── F#.pdf                     # Sample PDF loaded on startup
        ├── annotations.xfdf           # Exported/imported annotation data
        └── UpdatedDocument.pdf        # PDF generated after importing annotations
```

## How to Run the Sample

### Prerequisites

- Windows OS
- Visual Studio 2022 (or later) with the **.NET desktop development** workload, and the .NET 10 SDK
- A Syncfusion license key. Get one from the [Syncfusion license page](https://help.syncfusion.com/common/essential-studio/licensing/overview), or start a [free trial](https://www.syncfusion.com/downloads).

### Steps

1. Clone the repository and open `HostedPdfViewer/HostedPdfViewer_NET.sln` in Visual Studio.
2. Restore the NuGet packages. The `Syncfusion.PdfViewer.WPF` package is restored automatically.
3. Register your Syncfusion license key at the start of `Main` in `Program.cs`, before any Syncfusion control is created:

   ```csharp
   Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");
   ```

4. Build and run the project (**F5**). The window opens maximized and shows `F#.pdf` in the viewer.
5. Add annotations using the viewer's annotation tools, then click **Export Annotations** to write them to `Data/annotations.xfdf`.
6. Click **Import Annotations** to apply the XFDF annotations to the original PDF. The viewer reloads the updated document, which is also saved as `Data/UpdatedDocument.pdf`.

> **Note:** The file paths in the sample are relative to the build output folder (`bin/Debug/net10.0-windows7.0`), so run the app from Visual Studio or keep the `Data` folder at the same relative location.

## Reference

- [Syncfusion WPF PDF Viewer documentation](https://help.syncfusion.com/wpf/pdf-viewer/overview)
