# 📦 PCAP Flow Extractor (Avalonia MVVM Application)
### Convert raw PCAP packet captures into structured network flow datasets for machine learning & analysis.


## 📝 Overview

**PCAP Flow Extractor** is a cross-platform desktop application built with **.NET 8** and **Avalonia UI (MVVM)**.  
It loads PCAP/PCAPNG files, parses packets using **SharpPcap + PacketDotNet**, aggregates them into **network flows**, and exports a clean, machine-learning-ready **CSV dataset**.

This tool is the foundation for advanced topics such as:

- 🔎 Network anomaly detection (ML-based)  
- 📊 Traffic visualization  
- 🔐 Cybersecurity analytics  
- 📡 Protocol behavior analysis  
- 🧠 Network research datasets  


## 🚀 Features

### ✔ Load PCAP / PCAPNG files
Supports captures from Wireshark, tcpdump, tshark, etc.

### ✔ Parse packets with SharpPcap + PacketDotNet
Extracts:
- Source / Destination IP  
- Ports  
- Protocol (TCP/UDP/IGMP)  
- Packet size  
- Timestamps  

### ✔ Automatic Flow Aggregation (5-tuple)
Flows are grouped by:

```
SrcIp, DstIp, SrcPort, DstPort, Protocol
```

### ✔ Millisecond-Precision Timestamps
All exported timestamps use:

```
yyyy-MM-dd HH:mm:ss.fff
```


### ✔ CSV Export
Final dataset includes:
- FlowId  
- StartTime / EndTime  
- DurationSeconds  
- PacketCount  
- TotalBytes  
- BytesPerSecond  
- PacketsPerSecond  

### ✔ Clean MVVM Architecture
- Full separation between Views, ViewModels, and Services  
- UI independent business logic  
- Fully testable components  


## 🏗 Tech Stack

| Component | Technology |
|----------|------------|
| UI | Avalonia UI (.NET 8) |
| Architecture | MVVM |
| PCAP Parsing | SharpPcap, PacketDotNet |
| CSV Writing | CsvHelper |
| Language | C# |
| Platforms | Windows, Linux, macOS |



## 📂 Project Structure

```
PcapFlowExtractor/
│
├── Models/
│ ├── FlowRecord.cs
│ └── FlowKey.cs
│
├── Services/
│ ├── IPcapFlowService.cs
│ ├── PcapFlowService.cs
│ ├── IFileDialogService.cs
│ └── FileDialogService.cs
│
├── ViewModels/
│ └── MainWindowViewModel.cs
│
├── Views/
│ ├── MainWindow.axaml
│ └── MainWindow.axaml.cs
│
├── Utils/
│ └── AsyncCommand.cs
│
├── App.axaml
└── App.axaml.cs

```


## 🖥 UI Preview

![alt text](Images/pic.png "Title")


## 🔧 How It Works

### **1️⃣ Load PCAP File**
User selects a `.pcap` or `.pcapng` file via the Avalonia file browser.

### **2️⃣ Parse Packets**
Each packet is decoded using PacketDotNet:
- Extract IP header  
- Extract TCP/UDP header  
- Read timestamps (with ms precision)

### **3️⃣ Build Flows**
Packets are grouped into flows based on:

```
(SrcIp, DstIp, SrcPort, DstPort, Protocol)
```


### **4️⃣ Compute Flow Statistics**
Each flow stores:
- StartTime  
- EndTime  
- DurationSeconds  
- PacketCount  
- TotalBytes  
- Throughput (BytesPerSecond)  
- Rate (PacketsPerSecond)  

### **5️⃣ Export to CSV**
Produces a structured dataset ready for:
- Machine learning  
- Python EDA  
- Security analysis  
- BI tools  



## 📊 Sample Output (CSV)

| FlowId | SrcIp | DstIp | SrcPort | DstPort | Protocol | StartTime | EndTime | DurationSeconds | PacketCount | TotalBytes |
|--------|--------|--------|----------|----------|-----------|----------------------------|----------------------------|------------------|-------------|-------------|
| 1 | 192.168.1.10 | 8.8.8.8 | 44532 | 53 | UDP | 2023-10-03 09:29:15.123 | 2023-10-03 09:29:16.284 | 1.161 | 8 | 512 |

---

## 📚 Completed Work

### ✔ Implemented full MVVM architecture  
### ✔ Integrated SharpPcap + PacketDotNet  
### ✔ Built flow aggregation logic (5-tuple)  
### ✔ Added high-precision timestamp support  
### ✔ Implemented CSV export with derived metrics  
### ✔ Connected processing pipeline to UI  

---

## 🛠 Roadmap

### 🔹 Short-Term Goals
- Add flow preview table (DataGrid)  
- Add protocol/IP/port filtering  
- Add error handling for malformed packets  

### 🔹 Mid-Term Goals
- Add TCP flag counters (SYN, FIN, RST)  
- Determine flow direction (incoming/outgoing)  
- Enrich flows with GeoIP (optional)  

### 🔹 Long-Term (ML Phase)
- Create Jupyter notebook for EDA  
- Implement ML anomaly detection (IsolationForest, LOF, One-Class SVM)  
- Build a Streamlit or Avalonia dashboard showing anomalies  


## 🤝 Contributing

Contributions, issues, and feature requests are welcome.


## 📜 License
MIT License © 2025 Your Gihan Chathuranga

## ⭐ Support

If you find this project useful, consider giving it a **star** ⭐ on GitHub!  
Machine learning and anomaly detection features will be added soon.

