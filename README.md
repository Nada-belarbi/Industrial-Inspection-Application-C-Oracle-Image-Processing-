# Ressuage Inspection Manager (C# / Oracle)

## Overview
This project is a C# Windows Forms application developed to manage industrial ressuage (non-destructive testing) inspections.

It provides a complete workflow for operators to:
- Manage serial numbers (SN)
- Record inspection indications (type, size, defect)
- Capture and associate images
- Track inspection history
- Store and retrieve data from an Oracle database

---

## Technologies Used
- C# (.NET Framework)
- Windows Forms
- Oracle Database
- Oracle Managed Data Access
- File System (image storage)
- Custom logging system

---

## Features
- SN (Serial Number) tracking and validation
- Inspection indication management
- Image capture and visualization
- Real-time data display (DataGridView)
- Oracle database integration
- Logging (trace, errors, history)

---

## Configuration

### App.config
Example configuration (sanitized):

```xml
<appSettings>
  <add key="photo_deportee_directory" value="PATH_TO_MINI_LAMP_PHOTOS"/>
  <add key="photo_lampe_directory" value="PATH_TO_LAMP_PHOTOS"/>
  <add key="telecharger_photo_minilampe_directory" value="DOWNLOAD_PATH_MINI"/>
  <add key="telecharger_photo_lampe_directory" value="DOWNLOAD_PATH_LAMP"/>
  <add key="archivage_photo" value="ARCHIVE_PHOTO_PATH"/>
  <add key="archivage_pdf" value="ARCHIVE_PDF_PATH"/>
  <add key="device" value="CAMERA_DEVICE_NAME"/>
  <add key="Help_File" value="HELP_FILE_PATH"/>
</appSettings>
```
## Database

This application relies on an Oracle database.

Expected features:

- Tables for SN, inspection indications, and defects

- Relationships between SN and inspection data

- Storage of image paths and metadata

⚠️ Database scripts are not included for security reasons.
You must adapt the schema to your environment.

## How It Works

1.User selects or scans a Serial Number (SN)

2.Application checks if SN is valid

3.User records inspection data:

  - Indication type

  - Size or class

  - Defect type

4.Images can be captured and linked to the indication

5.Data is stored in Oracle

6.Recap table displays all recorded information

## Logging

The application includes a logging system:

- Trace logs (queries, workflow)

- Error logs

- History logs

Logs help debugging and ensure traceability.

## Limitations

- Requires Oracle database

- Depends on local file system for images

- Designed for industrial environment (not standalone demo)

- Camera integration may require specific hardware
