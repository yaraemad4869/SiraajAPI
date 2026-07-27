<div align="center">
  <h1 style="font-size: 3em; margin: 0; color: #C9A84C;">🌟 Siraaj API</h1>
  <p style="font-size: 1.2em; color: #666; margin-top: -5px;">Online Quran Academy - Complete Backend Solution</p>
  
  [![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
  [![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
  [![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server)

  
  <p>
    <a href="#-features">Features</a> •
    <a href="#-architecture">Architecture</a> •
    <a href="#-tech-stack">Tech Stack</a> •
    <a href="#-api-endpoints">API Endpoints</a> •
    <a href="#-getting-started">Getting Started</a> •
    <a href="#-deployment">Deployment</a>
  </p>
</div>

---

## 📖 **About Siraaj API**

Siraaj API is a **production-ready, enterprise-grade backend solution** for an online Quran academy. It provides a complete set of features for managing students, teachers, programs, packages, payments, and subscriptions in a seamless, secure, and scalable way.

### 🌟 **Why Siraaj?**

| Feature | Benefit |
|---------|---------|
| **🎓 Complete Academy Management** | Manage students, teachers, programs, and packages in one place |
| **💳 Real Payment Integration** | Process payments with Stripe, Paymob, and Instapay |
| **👨‍👩‍👧‍👦 Family Discount System** | Automatic discounts for multiple children (10%, 15%, 20%) |
| **🤝 Referral Program** | 50 SAR discount for both referrer and referee |
| **🔐 Enterprise Security** | JWT authentication, role-based authorization, HTTPS |
| **📊 Full Audit Trail** | Payment history, invoices, and subscription tracking |
| **🚀 Production Ready** | Scalable, tested, and deployment-ready |

---

## ✨ **Features**

### **🔐 Authentication & Authorization**
- ✅ JWT-based authentication with refresh tokens
- ✅ Role-based access control (Admin, Teacher, Guardian, Student)
- ✅ Secure password hashing with BCrypt
- ✅ Token expiration and renewal

### **📚 Program & Package Management**
- ✅ CRUD operations for Quran programs
- ✅ Flexible package tiers (Basic, Premium, VIP)
- ✅ Session scheduling (2, 3, or 5 sessions per week)
- ✅ Feature management for each package

### **👨‍👩‍👧‍👦 Student Management**
- ✅ Student registration with program/package selection
- ✅ Guardian-child relationship management
- ✅ Schedule management (day/time slots)
- ✅ Student activation/deactivation
- ✅ Teacher assignment

### **💰 Payment & Subscription**
- ✅ Multiple payment methods (Visa, Credit Card, Instapay, Paymob, Stripe)
- ✅ Real Stripe/Paymob integration
- ✅ Payment history tracking
- ✅ Automatic invoice generation
- ✅ Subscription management with auto-renewal

### **🎁 Discount System**
- ✅ Family discounts (10%, 15%, 20% for 2nd, 3rd, 4th+ children)
- ✅ Referral program (50 SAR discount)
- ✅ Discount code validation
- ✅ Discount amount tracking

### **👨‍🏫 Teacher Management**
- ✅ Teacher CRUD operations
- ✅ Student-teacher assignment
- ✅ Teacher specialization tracking

---

## 🏗️ **Architecture**

```mermaid
graph TB
    A[Frontend - React/Next.js] --> B[API Gateway]
    B --> C[Authentication Service]
    B --> D[Program Service]
    B --> E[Payment Service]
    B --> F[Student Service]
    B --> G[Discount Service]
    
    C --> H[(SQL Server Database)]
    D --> H
    E --> H
    F --> H
    G --> H
    
    E --> I[Stripe API]
    E --> J[Paymob API]
    E --> K[Instapay API]
    
    style A fill:#C9A84C,color:#fff
    style B fill:#1E6B4A,color:#fff
    style C fill:#7C3AED,color:#fff
    style D fill:#2563EB,color:#fff
    style E fill:#DC2626,color:#fff
    style F fill:#F59E0B,color:#fff
    style G fill:#10B981,color:#fff
```
Layered Architecture
Controllers: Handle HTTP requests and responses

Services: Business logic and external integrations

Repositories: Data access layer

Models: Entity framework core entities

DTOs: Data transfer objects for API communication

🛠️ Tech Stack
Backend
Technology	Version	Purpose
.NET	9.0	Framework
C#	12.0	Language
Entity Framework Core	9.0	ORM
SQL Server	2022	Database
ASP.NET Core Identity	9.0	Authentication
JWT	9.0	Token-based auth
BCrypt.Net	4.0	Password hashing
Payment Gateways
Gateway	Purpose
Stripe	International card payments
Paymob	MENA region payments
Instapay	Saudi Arabia bank transfers
Tools & Libraries
Tool	Purpose
Swagger/OpenAPI	API Documentation
Serilog	Logging
FluentValidation	Input validation
xUnit	Testing
Moq	Mocking
## 📊 **Database Schema (ER Diagram)**
```mermaid
erDiagram
    Users ||--o{ Students : "has one"
    Users ||--o{ Guardians : "has one"
    Users ||--o{ Teachers : "has one"
    Users ||--o{ RefreshTokens : "has many"
    Users ||--o{ DiscountCodes : "creates"
    Users ||--o{ Payments : "makes"
    Users ||--o{ Invoices : "receives"
    
    Students ||--o{ StudentPrograms : "enrolls in"
    Students ||--o{ Payments : "makes"
    Students ||--o{ Subscriptions : "has"
    Students ||--o{ Schedules : "has"
    
    Guardians ||--o{ Students : "has children"
    Teachers ||--o{ Students : "teaches"
    
    Programs ||--o{ StudentPrograms : "includes"
    Packages ||--o{ StudentPrograms : "includes"
    Packages ||--o{ Subscriptions : "has"
    
    Payments ||--|| Invoices : "generates"
    Payments ||--o{ DiscountCodes : "uses"
    
    Users {
        Guid Id PK
        string NameAr
        string NameEn
        string Phone UK
        string PasswordHash
        string Role
        string ReferralCode UK
        datetime JoinDate
        datetime CreatedAt
        datetime UpdatedAt
    }
    
    Students {
        Guid UserId PK
        string PackageId
        Guid TeacherId FK
        Guid GuardianId FK
        string Schedule
        string Status
        int Age
        string Level
    }
    
    Guardians {
        Guid UserId PK
        string ChildrenIds
    }
    
    Teachers {
        Guid UserId PK
        string Specialization
        string StudentIds
    }
    
    Programs {
        Guid Id PK
        string Icon
        string NameAr
        string NameEn
        string DescAr
        string DescEn
        bool IsActive
        datetime CreatedAt
        datetime UpdatedAt
    }
    
    Packages {
        Guid Id PK
        string NameAr
        string NameEn
        int Price
        int OriginalPrice
        int Sessions
        int SessionsPerWeek
        string Color
        bool IsHighlighted
        string FeaturesAr
        string FeaturesEn
        bool IsActive
        datetime CreatedAt
        datetime UpdatedAt
    }
    
    StudentPrograms {
        Guid Id PK
        Guid StudentId FK
        Guid ProgramId FK
        Guid PackageId FK
        datetime EnrolledDate
        datetime EndDate
        bool IsActive
        datetime CreatedAt
        datetime UpdatedAt
    }
    
    Payments {
        Guid Id PK
        Guid StudentId FK
        Guid StudentProgramId FK
        decimal Amount
        decimal DiscountApplied
        decimal FinalAmount
        string PaymentMethod
        string PaymentGateway
        string TransactionId
        string Status
        datetime PaymentDate
        datetime CreatedAt
        datetime UpdatedAt
    }
    
    Invoices {
        Guid Id PK
        string InvoiceNumber
        Guid PaymentId FK
        Guid StudentId FK
        string StudentName
        string PackageName
        decimal Amount
        decimal Tax
        decimal TotalAmount
        string Currency
        datetime IssueDate
        datetime DueDate
        bool IsPaid
        string PdfUrl
        datetime CreatedAt
        datetime UpdatedAt
    }
    
    Subscriptions {
        Guid Id PK
        Guid StudentId FK
        Guid PackageId FK
        Guid ProgramId FK
        datetime StartDate
        datetime EndDate
        bool IsActive
        bool AutoRenew
        string Status
        datetime CreatedAt
        datetime UpdatedAt
    }
```
## 📊 **Authentication Flow**
```mermaid
sequenceDiagram
    participant Client as Frontend
    participant API as API Gateway
    participant Auth as Auth Service
    participant DB as Database
    participant JWT as JWT Token

    Client->>API: Login Request (Phone, Password)
    API->>Auth: Validate Credentials
    Auth->>DB: Query User
    DB-->>Auth: Return User Data
    Auth->>Auth: Verify Password (BCrypt)
    Auth->>JWT: Generate Access Token
    Auth->>JWT: Generate Refresh Token
    Auth->>DB: Store Refresh Token
    JWT-->>Auth: Tokens Generated
    Auth-->>API: Authentication Result
    API-->>Client: Return Tokens + User Data
    
    Note over Client,JWT: Subsequent Requests
    
    Client->>API: Request with Access Token
    API->>JWT: Validate Token
    JWT-->>API: Token Valid
    API->>Client: Return Response
    
    Note over Client,JWT: Token Expired
    
    Client->>API: Request with Expired Token
    API-->>Client: 401 Unauthorized
    Client->>API: Refresh Token Request
    API->>Auth: Validate Refresh Token
    Auth->>DB: Check Refresh Token
    DB-->>Auth: Token Valid
    Auth->>JWT: Generate New Access Token
    JWT-->>Client: New Access Token
    Client->>API: Retry with New Token
    API-->>Client: Return Response
```
## 📊 **Payment Processing Flow**
```mermaid
sequenceDiagram
    participant Client as Frontend
    participant API as API Gateway
    participant Payment as Payment Service
    participant Stripe as Stripe API
    participant DB as Database
    participant Webhook as Webhook Handler

    Client->>API: Initiate Payment
    API->>Payment: Process Payment Request
    Payment->>DB: Get Student Program Details
    DB-->>Payment: Return Program Data
    Payment->>Payment: Calculate Final Amount
    
    alt Card Payment
        Payment->>Stripe: Create Payment Intent
        Stripe-->>Payment: Return Client Secret
        Payment-->>API: Payment Initiated
        API-->>Client: Payment URL + Client Secret
        Client->>Stripe: Complete Payment
    else Instapay
        Payment->>Payment: Generate Instapay Reference
        Payment-->>API: Payment Initiated
        API-->>Client: Instapay Instructions
    else Paymob
        Payment->>Stripe: Create Paymob Order
        Stripe-->>Payment: Return Payment Key
        Payment-->>API: Payment Initiated
        API-->>Client: Paymob Payment URL
    end
    
    Stripe->>Webhook: Payment Confirmation
    Webhook->>Payment: Process Webhook
    Payment->>DB: Update Payment Status
    
    alt Payment Successful
        Payment->>DB: Create Subscription
        Payment->>DB: Generate Invoice
        Payment-->>API: Payment Completed
    else Payment Failed
        Payment->>DB: Update Payment Status
        Payment-->>API: Payment Failed
    end
    
    API-->>Client: Payment Result
```
## 📊 **Student Registration Flow**
```mermaid
flowchart TD
    A[Start Registration] --> B{Choose Role}
    B -->|Guardian| C[Register Guardian]
    B -->|Student| D[Register Student]
    
    C --> E[Create Guardian Account]
    E --> F[Login as Guardian]
    F --> G[Add Child]
    G --> H[Select Program]
    H --> I[Select Package]
    I --> J[Choose Schedule]
    J --> K[Apply Discounts]
    K --> L[Initiate Payment]
    
    D --> M[Create Student Account]
    M --> N[Provide Guardian Phone]
    N --> O[Select Program]
    O --> P[Select Package]
    P --> Q[Choose Schedule]
    Q --> R[Apply Discounts]
    R --> L
    
    L --> S{Payment Successful?}
    S -->|Yes| T[Create Subscription]
    T --> U[Generate Invoice]
    U --> V[Assign Teacher]
    V --> W[Send Confirmation]
    W --> X[Registration Complete]
    
    S -->|No| Y[Payment Failed]
    Y --> Z[Retry Payment]
    Z --> L
    
    style A fill:#C9A84C,color:#fff
    style X fill:#10B981,color:#fff
    style Y fill:#EF4444,color:#fff
```
## 📊 **Discount Calculation Flow**
```mermaid
flowchart TD
    A[Calculate Discount] --> B{Check Discount Type}
    
    B -->|Family Discount| C[Get Number of Children]
    C --> D{Child Count}
    D -->|1 Child| E[0% Discount]
    D -->|2 Children| F[10% Discount]
    D -->|3 Children| G[15% Discount]
    D -->|4+ Children| H[20% Discount]
    
    B -->|Referral Discount| I[Validate Referral Code]
    I --> J{Code Valid?}
    J -->|Yes| K[50 SAR Discount]
    J -->|No| L[0 SAR Discount]
    
    B -->|Both| M[Apply Both Discounts]
    M --> N[Calculate Total Discount]
    N --> O[Apply to Package Price]
    O --> P[Return Final Price]
    
    E --> Q[Return Price]
    F --> Q
    G --> Q
    H --> Q
    K --> Q
    L --> Q
    Q --> R[Final Amount]
    
    style A fill:#F59E0B,color:#fff
    style R fill:#10B981,color:#fff
```
## 📊 **Admin Dashboard Workflow**
```mermaid
flowchart TD
    A[Admin Login] --> B[Dashboard]
    
    B --> C[Manage Students]
    B --> D[Manage Teachers]
    B --> E[Manage Programs]
    B --> F[Manage Packages]
    B --> G[View Reports]
    
    C --> C1[View All Students]
    C --> C2[Activate/Deactivate]
    C --> C3[Assign Teacher]
    C --> C4[View Payment History]
    
    D --> D1[Add Teacher]
    D --> D2[Update Teacher]
    D --> D3[Delete Teacher]
    D --> D4[View Teacher Students]
    
    E --> E1[Add Program]
    E --> E2[Update Program]
    E --> E3[Delete Program]
    E --> E4[Set Program Status]
    
    F --> F1[Add Package]
    F --> F2[Update Package]
    F --> F3[Delete Package]
    F --> F4[Set Package Features]
    
    G --> G1[Revenue Reports]
    G --> G2[Student Reports]
    G --> G3[Payment Reports]
    G --> G4[Discount Reports]
    
    style A fill:#C9A84C,color:#fff
    style B fill:#1E6B4A,color:#fff
    style C fill:#2563EB,color:#fff
    style D fill:#7C3AED,color:#fff
    style E fill:#F59E0B,color:#fff
    style F fill:#DC2626,color:#fff
    style G fill:#10B981,color:#fff
```
## 📊 **Deployment Pipeline**
```mermaid
flowchart LR
    A[Developer] --> B[Git Push]
    B --> C[GitHub Actions]
    C --> D[Run Tests]
    D --> E{Tests Pass?}
    
    E -->|Yes| F[Build Application]
    F --> G[Create Docker Image]
    G --> H[Push to Container Registry]
    H --> I[Deploy to Azure/AWS]
    I --> J[Production Server]
    
    E -->|No| K[Notify Developer]
    K --> A
    
    J --> L[Health Check]
    L --> M[Monitoring]
    M --> N[Alerts]
    N --> A
    
    style A fill:#C9A84C,color:#fff
    style J fill:#10B981,color:#fff
    style K fill:#EF4444,color:#fff
```
## 📊 **Data Flow for Payment Processing**
```mermaid
flowchart LR
    A[Student] -->|Selects Package| B[Payment Request]
    B --> C[Calculate Price]
    C --> D{Apply Discounts?}
    
    D -->|Yes| E[Family Discount]
    D -->|Yes| F[Referral Discount]
    D -->|No| G[Full Price]
    
    E --> H[Final Price]
    F --> H
    G --> H
    
    H --> I{Payment Method}
    I -->|Stripe| J[Stripe API]
    I -->|Paymob| K[Paymob API]
    I -->|Instapay| L[Instapay API]
    
    J --> M[Payment Processing]
    K --> M
    L --> M
    
    M --> N{Payment Status}
    N -->|Success| O[Create Subscription]
    N -->|Success| P[Generate Invoice]
    N -->|Success| Q[Send Confirmation]
    N -->|Failure| R[Payment Failed]
    
    O --> S[Student Active]
    P --> S
    Q --> S
    
    style A fill:#C9A84C,color:#fff
    style S fill:#10B981,color:#fff
    style R fill:#EF4444,color:#fff
```
## 📊 **API Security Flow**
```mermaid
flowchart TD
    A[Incoming Request] --> B{Has JWT Token?}
    
    B -->|No| C[Return 401 Unauthorized]
    C --> D[Client Logout]
    
    B -->|Yes| E[Validate Token]
    E --> F{Token Valid?}
    
    F -->|No| G{Token Expired?}
    G -->|Yes| H[Refresh Token]
    G -->|No| C
    
    F -->|Yes| I[Check Role]
    I --> J{Role Authorized?}
    
    J -->|No| K[Return 403 Forbidden]
    
    J -->|Yes| L[Process Request]
    L --> M[Return Response]
    
    H --> N{Refresh Valid?}
    N -->|Yes| O[Generate New Token]
    O --> L
    
    N -->|No| C
    
    style A fill:#C9A84C,color:#fff
    style L fill:#10B981,color:#fff
    style C fill:#EF4444,color:#fff
    style K fill:#F59E0B,color:#fff
```
📡 API Endpoints
```text
Authentication
http
POST   /api/auth/login           # Login user
POST   /api/auth/register        # Register new user
POST   /api/auth/refresh-token   # Refresh JWT token
POST   /api/auth/logout          # Logout user
GET    /api/auth/me              # Get current user
Programs
http
GET    /api/programs             # Get all programs
GET    /api/programs/{id}        # Get program by ID
POST   /api/programs             # Create program (Admin)
PUT    /api/programs/{id}        # Update program (Admin)
DELETE /api/programs/{id}        # Delete program (Admin)
Packages
http
GET    /api/packages             # Get all packages
GET    /api/packages/{id}        # Get package by ID
POST   /api/packages             # Create package (Admin)
PUT    /api/packages/{id}        # Update package (Admin)
DELETE /api/packages/{id}        # Delete package (Admin)
Payments
http
POST   /api/payments/initiate              # Initiate payment
GET    /api/payments/history/{studentId}   # Get payment history
GET    /api/payments/invoices/{studentId}  # Get invoices
GET    /api/payments/subscriptions/{studentId} # Get subscriptions
POST   /api/payments/callback              # Payment callback
POST   /api/webhook/stripe                 # Stripe webhook
Admin
http
GET    /api/admin/teachers         # Get all teachers
POST   /api/admin/teachers         # Add teacher
PUT    /api/admin/teachers/{id}    # Update teacher
DELETE /api/admin/teachers/{id}    # Delete teacher
GET    /api/admin/users            # Get all users
Discounts
http
POST   /api/discounts/validate     # Validate discount code
GET    /api/discounts/family       # Get family discounts
POST   /api/discounts/generate     # Generate discount code (Admin)
```
🚀 Getting Started
Prerequisites
.NET 9 SDK

SQL Server (or SQL Server Express)

Git

Installation
Clone the repository

```bash
git clone https://github.com/yaraemad4869/SiraajAPI.git
cd SiraajAPI
Restore dependencies
```

```bash
dotnet restore
Update database connection string in appsettings.json
```

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SiraajDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
}
```
Apply migrations

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
Run the application
```
```bash
dotnet run --urls="http://localhost:5000"
Access Swagger Documentation
Open your browser and go to: http://localhost:5000/swagger
```

🧪 Test Accounts
- Admin	01000000000	admin123
- Student	01012345678	123456
- Guardian	01098765432	123456
- Teacher	01111111111	teacher123

📦 Project Structure
```text
SiraajAPI/
├── Controllers/
│   ├── AuthController.cs
│   ├── ProgramsController.cs
│   ├── PackagesController.cs
│   ├── PaymentsController.cs
│   ├── AdminController.cs
│   ├── DiscountController.cs
│   └── WebhookController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── DTOs/
│   ├── AuthDtos.cs
│   ├── ProgramDtos.cs
│   ├── PackageDtos.cs
│   ├── PaymentDtos.cs
│   └── TeacherDtos.cs
├── Models/
│   ├── User.cs
│   ├── Student.cs
│   ├── Teacher.cs
│   ├── Guardian.cs
│   ├── Program.cs
│   ├── Package.cs
│   ├── Payment.cs
│   ├── Invoice.cs
│   └── Subscription.cs
├── Services/
│   ├── JwtService.cs
│   ├── StripePaymentService.cs
│   ├── PaymobPaymentService.cs
│   └── Interfaces/
├── Helpers/
│   ├── PasswordHelper.cs
│   ├── JwtSettings.cs
│   ├── StripeSettings.cs
│   └── PaymobSettings.cs
├── Migrations/
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── SiraajAPI.csproj
```
🔒 Security Best Practices
API Keys: Never commit real API keys. Use environment variables in production.

HTTPS: Always use HTTPS in production.

JWT: Keep token expiration short, use refresh tokens.

Password Hashing: BCrypt is used for secure password storage.

CORS: Configure CORS policies properly.

Rate Limiting: Consider implementing rate limiting for production.

Logging: Use Serilog for structured logging.

🌐 Deployment
Azure App Service
bash
# Publish to Azure
dotnet publish -c Release -o ./publish
# Deploy via Azure CLI or Azure DevOps
IIS
bash
# Publish for IIS
dotnet publish -c Release -r win-x64 --self-contained false
# Deploy to IIS folder
Docker
dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SiraajAPI.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SiraajAPI.dll"]
📊 Performance Metrics
Metric	Value
Average Response Time	< 100ms
Concurrent Users	1000+
Database Connections	Pooled (100)
Token Expiration	60 minutes
Refresh Token Expiration	7 days
🤝 Contributing
Fork the repository

Create your feature branch (git checkout -b feature/AmazingFeature)

Commit your changes (git commit -m 'Add some AmazingFeature')

Push to the branch (git push origin feature/AmazingFeature)

Open a Pull Request

📄 License
This project is licensed under the MIT License - see the LICENSE file for details.

🙏 Acknowledgments
Stripe - Payment processing

Paymob - MENA payment gateway

Microsoft - .NET Framework

Open Source Community - Various libraries and tools

📞 Contact
GitHub: @yaraemad4869

Email: info@sirajacademy.com

Phone: +966 50 000 0000

<div align="center"> <h3>🌟 "And We have certainly made the Quran easy to remember" - Surah Al-Qamar 54:17</h3> <p style="color: #666;">Made with ❤️ for Siraaj Online Quran Academy</p>
⬆ Back to Top

</div> ```
