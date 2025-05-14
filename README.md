# Sensing4USensor

A robust Windows Forms application built to manage, analyze, and visualize time-series sensor data collected from remote devices. The application supports multiple datasets, interactive filtering, data export, and dynamic UI adjustments for efficient sensor data monitoring.

---

## 📌 Project Overview

**Sensing4USensor** is designed for technicians and data analysts who require a lightweight, offline-friendly interface to process `.csv` and `.bin` sensor files. It includes a searchable and color-coded data grid, range filters, and dataset labeling capabilities.

---

## 🛠️ Technologies Used

- **.NET 6.0 / Windows Forms**
- **C#**
- **DocFX** for API documentation
- **Visual Studio 2022**
- **Singleton Pattern** (used in `SensorFileManager`)
- **Custom Model Binding** (`SensorData`, `ColorCategory`)
- **FlowLayoutPanel for dynamic dataset switching**

---

## 🧪 Testing

The application was tested using the following strategies:

- **Unit Testing (manual)** for:
  - File parsing (`ReadGridCsv`, `ReadPythonDetailedBinary`)
  - Data transformation (`ToDailyHourlyArray`)
  - Singleton instance validation
- **UI Testing**:
  - Dataset loading and display
  - Label renaming and color filters
  - Edge cases (e.g., invalid ranges, empty files)
- **Debugging Tools**:
  - Visual Studio Debugger
  - Trace code with `MessageBox` feedback for critical actions

A test singleton method (`TestSingletonInstance`) confirms the file manager returns a consistent instance.

---

## 📦 Features

- ✅ Load multiple `.csv` or `.bin` files
- ✅ Automatically structure data into daily/hourly 2D array
- ✅ Color-coded cells by data range: `Low`, `Acceptable`, `High`
- ✅ Label renaming for datasets with dynamic UI update
- ✅ Search by date and sort results with yellow highlighting
- ✅ Export dataset to `.bin` format
- ✅ Dataset switching through dynamically generated buttons
- ✅ User feedback messages for all major actions

---

## 🚀 Getting Started

### Prerequisites

- Visual Studio 2022 or later
- .NET 9.0 SDK

### Steps to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/Brumertz/Sensor4UI
