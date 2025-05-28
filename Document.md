# File Sharing API Documentation

## Overview
This project is built using .NET Core Web API (.NET 9) and provides a secure file sharing system with password protection and expiration functionality.

## API Endpoints

### File Controller
Base path: `api/File`

#### 1. Upload Files
- **Endpoint**: `POST /api/File/Upload`
- **Description**: Upload files (images and videos) to the server
- **Request**: Multipart form data
- **Response**: List of uploaded file names
- **Supported File Types**: Images and Videos
- **Recommended Improvements**:
  - Add file size limits
  - Implement file type validation
  - Add virus scanning
  - Support chunked upload for large files

### Form Controller
Base path: `api/Form`

#### 1. Create Form
- **Endpoint**: `POST /api/Form`
- **Description**: Create a new form with optional password protection
- **Request Body**:
```json
{
    "isPassword": boolean,
    "password": "string (optional)",
    "description": "string",
    "expiredDays": number,
    "fileNames": ["string"]
}
```
- **Response**: Form ID
- **Features**:
  - Password protection (optional)
  - Expiration date
  - Multiple file association

#### 2. Get Form Information
- **Endpoint**: `GET /api/Form/{id}`
- **Description**: Retrieve form metadata
- **Response**:
```json
{
    "isPassword": boolean,
    "description": "string"
}
```
- **Security**: Does not expose sensitive information

#### 3. Validate Password and Access Files
- **Endpoint**: `POST /api/Form/{id}/validate`
- **Description**: Validate form password and retrieve file access
- **Request Body**:
```json
{
    "password": "string"
}
```
- **Response**: List of accessible files
- **Security Features**:
  - Password validation
  - Expiration check
  - File existence verification

## Security Features
- Password protection for sensitive files
- Automatic file expiration
- Secure file access validation
- No direct file path exposure

## Recommended Improvements
1. **Authentication & Authorization**:
   - Add JWT authentication
   - Implement role-based access control
   - Add rate limiting

2. **File Management**:
   - Implement file compression
   - Add thumbnail generation for images
   - Support file preview
   - Implement file deletion after expiration

3. **Security Enhancements**:
   - Add HTTPS enforcement
   - Implement audit logging
   - Add IP-based access control
   - Support 2FA for sensitive files

4. **Performance Optimizations**:
   - Add caching layer
   - Implement async file operations
   - Add file streaming support
   - Database persistence for form data

5. **User Experience**:
   - Add email notifications
   - Support bulk file operations
   - Add file sharing links
   - Implement download progress tracking


```C#
 public class CreateFormRequest
    {
        public bool IsPassword { get; set; }
        public string? Password { get; set; }
        public string Description { get; set; }
        public int ExpiredDays { get; set; }
        public List<string> FileNames { get; set; }
    }
```

```C#
public class FormResponse
    {
        public bool IsPassword { get; set; }
        public string Description { get; set; }
    }
```

```C#
public class ValidatePasswordRequest
    {
        public string Password { get; set; }
    }

```
