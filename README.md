# OCR API – Bangla & English Text Extraction

A lightweight OCR (Optical Character Recognition) API built with **ASP.NET Core** for extracting both **Bangla** and **English** text from images. This API uses **Tesseract OCR** with advanced image preprocessing techniques to improve accuracy on low-quality scans and photocopies.

## 🚀 Features

- 📷 **Image Upload API** (via multipart/form-data)
- 🌐 Supports **Bangla (ben)** and **English (eng)** OCR
- 🖼️ **Preprocessing Enhancements**: Grayscale, Contrast Boost, Sharpening, Thresholding
- ⚙️ Easy to deploy on **IIS**, **Kestrel**, or **Docker**
- ✅ Tested with Swagger & Postman

---

## 🛠️ Installation

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/reton2008/OcrEngBangla.git
cd your-repo-name
```

### 2️⃣ Install Required Dependencies

#### .NET and Visual C++ Runtime:

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual C++ Redistributable (x64)](https://learn.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist) *(Required for Tesseract on Windows)*

#### NuGet Packages:

Run the following commands in the project directory or use Visual Studio NuGet Package Manager to install these dependencies:

```bash
dotnet add package Tesseract
```

```bash
dotnet add package SixLabors.ImageSharp
```

```bash
dotnet add package Swashbuckle.AspNetCore
```

```bash
dotnet add package SixLabors.ImageSharp.Drawing
```

```bash
dotnet add package SixLabors.ImageSharp.Processing
```

---

## 📁 Configuration

### 📚 `tessdata` Folder

- Place trained language data files (`eng.traineddata`, `ben.traineddata`) in a folder named `` in the project root.
- Download traineddata files from:
  - [https://github.com/tesseract-ocr/tessdata](https://github.com/tesseract-ocr/tessdata)

**Directory Example:**

```
your-project-root/
├── tessdata/
│   ├── eng.traineddata
│   └── ben.traineddata
├── Controllers/
├── Program.cs
```

---

## ▶️ Running the Project

### Using Visual Studio

1. Open the solution in Visual Studio 2022.
2. Set the startup project.
3. Run → API available at: `https://localhost:{port}/swagger`

### Using CLI

```bash
dotnet build
dotnet run
```

---

## 📬 Usage (API Endpoint)

| Method | Endpoint                | Description                 |
| ------ | ----------------------- | --------------------------- |
| POST   | `/api/ocr/extract-text` | Upload image & extract text |

#### Parameters:

- `image` (file) → Image file (JPEG, PNG, etc.)
- `language` (string) → Languages for OCR (default: `eng+ben`)

**Example (Postman):**

- Method: `POST`
- URL: `https://localhost:{port}/api/ocr/extract-text`
- Body: `form-data`
  - `image`: (choose file)
  - `language`: `eng+ben`

---

## 📦 Deployment Notes

- **IIS**: Place `tessdata` folder in the **root directory** of your deployed app.
- **Ensure read/write permissions** on upload directories.

---

## 📄 License

MIT License

---

## 👫 Need Help?

For any issues or feature requests, please open an issue or discussion here on GitHub.

