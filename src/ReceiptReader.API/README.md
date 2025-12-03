# 🧾 ReceiptReader: OCR-Powered Data Extraction API

## Project Status

[![GitHub last commit](https://img.shields.io/github/last-commit/Timiman1/ReceiptReader)](https://github.com/Timiman1/ReceiptReader/commits/main)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE.md)

---

## 💡 Overview

The **ReceiptReader** is a **C# ASP.NET Core Web API** solution for automating the extraction and structuring of financial data from receipts (images or PDFs).

It uses advanced OCR services to parse raw text and maps it to a structured domain model. This allows for reliable persistence and analysis. This service acts as the backbone for expense tracking.

### Key Features

* **Raw Text Extraction:** Utilizes external OCR providers (Azure AI Document Intelligence, Tesseract) to convert receipts into readable text.
* **Domain Mapping:** Transforms raw text into structured objects (Vendor Name, Total Amount, Line Items, etc.) via specialized parsing logic (WIP).
* **Persistence:** Stores extracted data and analysis logs using **Entity Framework Core (EF Core)**.
* **Caching:** Implements a caching layer (via `AnalysisLog`) to avoid re-processing identical receipts.

---

## 🛠️ Technology Stack

| Category | Technology | Purpose |
| :--- | :--- | :--- |
| **Backend** | **ASP.NET Core 10+** | Core API framework for building RESTful services. |
| **Language** | **C#** | Primary programming language. |
| **Database** | **SQLite** (Development) | Lightweight, file-based database used for local development and testing. |
| **ORM** | **Entity Framework Core** | Data access, migration management, and database interaction. |
| **External Services** | **Azure AI Document Intelligence** | Primary cloud-based OCR service for high-quality text extraction. |
| **Architecture** | **Clean/Layered Architecture** | Separation of concerns (API, Application, Domain, Infrastructure). |

---

## 🚀 Getting Started

These instructions will get a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

1.  **.NET 10 SDK** or newer installed.
2.  A local **Microsoft SQL Server** instance or PostgreSQL instance (for production deployment).
3.  An **Azure AI Services account** with the Document Intelligence endpoint and API Key.

### Installation

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/Timiman1/ReceiptReader.git](https://github.com/Timiman1/ReceiptReader.git)
    cd ReceiptReader
    ```

2.  **Restore Dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Set User Secrets (Crucial):**
    You must configure your Azure AI API key using the .NET User Secrets Manager.
    ```bash
    dotnet user-secrets set "RawTextExtractionProviders:AzureAIDocumentIntelligence:ApiKey" "YOUR_AZURE_API_KEY_HERE"
    ```
    *Ensure you also set the `Endpoint` in `appsettings.Development.json`.*

4.  **Run Migrations:**
    Create the local SQLite database file based on the EF Core migration history.
    ```bash
    dotnet ef database update --project ReceiptReader.Infrastructure
    ```

### Running the Application

Start the Web API from the root directory:
```bash
dotnet run --project ReceiptReader.Api
