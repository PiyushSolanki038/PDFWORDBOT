# Solankii - AI Design Assistant for UI/UX Teams

![Solankii Logo](https://img.shields.io/badge/Solankii-AI%20Design%20Assistant-purple)
![.NET 8](https://img.shields.io/badge/.NET-8.0-blue)
![License](https://img.shields.io/badge/License-MIT-green)

Solankii is a comprehensive AI-powered design assistant that helps UI/UX teams create exceptional user experiences through intelligent component generation, design suggestions, and collaborative workflows.

## 🚀 Features

### 🤖 AI-Powered Component Generation
- Generate modern, accessible UI components from natural language descriptions
- Support for React, Vue, Angular, and vanilla HTML/CSS
- Automatic accessibility compliance and responsive design
- Smart code optimization and best practices

### 💡 Intelligent Design Suggestions
- AI-driven recommendations for usability improvements
- Accessibility analysis and suggestions
- Performance optimization recommendations
- Visual design and layout suggestions

### 👥 Collaborative Design Reviews
- Real-time collaboration tools
- AI-powered feedback and scoring
- Version control and change tracking
- Automated review workflows

### 🎨 Design System Management
- Centralized component libraries
- Consistent design tokens and styles
- Brand guideline enforcement
- Multi-framework export capabilities

### 📊 Analytics & Insights
- Component performance metrics
- Usability scoring and analysis
- Accessibility compliance reports
- Design iteration tracking

## 🏗️ Architecture

Solankii follows a clean architecture pattern with the following layers:

```
Solankii/
├── src/
│   ├── Solankii.Core/           # Domain entities and interfaces
│   ├── Solankii.Infrastructure/ # Data access and external services
│   ├── Solankii.API/           # REST API endpoints
│   └── Solankii.Web/           # Web interface
├── docs/                       # Documentation
└── tests/                      # Unit and integration tests
```

### Technology Stack

- **Backend**: .NET 8, Entity Framework Core, ASP.NET Core
- **Database**: SQL Server (LocalDB for development)
- **AI Integration**: OpenAI GPT-4 API
- **Frontend**: Razor Pages, Tailwind CSS, JavaScript
- **API**: RESTful API with Swagger documentation

## 🛠️ Installation & Setup

### Prerequisites

- .NET 8 SDK
- SQL Server LocalDB (or SQL Server Express)
- OpenAI API key

### Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/solankii.git
   cd solankii
   ```

2. **Configure the application**
   ```bash
   # Copy and edit the configuration file
   cp src/Solankii.API/appsettings.json src/Solankii.API/appsettings.Development.json
   ```

3. **Update the configuration**
   Edit `src/Solankii.API/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SolankiiDb;Trusted_Connection=true;MultipleActiveResultSets=true"
     },
     "OpenAI": {
       "ApiKey": "your-openai-api-key-here"
     }
   }
   ```

4. **Build and run the application**
   ```bash
   # Build the solution
   dotnet build

   # Run the API
   cd src/Solankii.API
   dotnet run

   # Run the Web interface (in another terminal)
   cd src/Solankii.Web
   dotnet run
   ```

5. **Access the application**
   - API: http://localhost:5000
   - Swagger UI: http://localhost:5000/swagger
   - Web Interface: http://localhost:5001

## 📚 API Documentation

### Projects Endpoints

- `GET /api/projects` - Get all projects
- `GET /api/projects/{id}` - Get project by ID
- `POST /api/projects` - Create new project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Delete project
- `POST /api/projects/{id}/suggestions` - Generate AI suggestions

### Components Endpoints

- `GET /api/components` - Get components by project or type
- `GET /api/components/{id}` - Get component by ID
- `POST /api/components` - Create new component
- `POST /api/components/generate` - Generate component with AI
- `PUT /api/components/{id}` - Update component
- `DELETE /api/components/{id}` - Delete component
- `POST /api/components/{id}/export` - Export component code
- `POST /api/components/{id}/analyze` - Analyze component metrics

## 🎯 Usage Examples

### Creating a New Project

```csharp
var project = new DesignProject
{
    Name = "E-commerce Dashboard",
    Description = "Modern dashboard for e-commerce analytics",
    Type = ProjectType.Dashboard,
    Status = ProjectStatus.Draft,
    CreatedBy = "designer@company.com"
};

var createdProject = await projectService.CreateProjectAsync(project);
```

### Generating a Component with AI

```csharp
var request = new GenerateComponentRequest
{
    Description = "A modern card component with hover effects and responsive design",
    Type = ComponentType.Card,
    Style = new ComponentStyle
    {
        PrimaryColor = "#3B82F6",
        BorderRadius = "12px",
        FontFamily = "Inter, sans-serif"
    },
    ProjectId = projectId
};

var component = await componentService.GenerateComponentAsync(request);
```

### Getting AI Design Suggestions

```csharp
var suggestions = await aiService.GenerateDesignSuggestionsAsync(project, component);
foreach (var suggestion in suggestions)
{
    Console.WriteLine($"Suggestion: {suggestion.Title}");
    Console.WriteLine($"Priority: {suggestion.Priority}");
    Console.WriteLine($"Type: {suggestion.Type}");
}
```

## 🔧 Configuration

### Database Configuration

The application uses Entity Framework Core with SQL Server. You can configure the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=SolankiiDb;Trusted_Connection=true;"
  }
}
```

### OpenAI Configuration

Configure your OpenAI API settings:

```json
{
  "OpenAI": {
    "ApiKey": "your-openai-api-key",
    "Endpoint": "https://api.openai.com/v1/chat/completions"
  }
}
```

### CORS Configuration

Configure CORS for cross-origin requests:

```json
{
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "http://localhost:4200"
    ]
  }
}
```

## 🧪 Testing

Run the test suite:

```bash
dotnet test
```

### Test Categories

- **Unit Tests**: Core business logic and services
- **Integration Tests**: API endpoints and database operations
- **AI Service Tests**: OpenAI integration and response parsing

## 📦 Deployment

### Docker Deployment

1. **Build the Docker image**
   ```bash
   docker build -t solankii .
   ```

2. **Run the container**
   ```bash
   docker run -p 5000:5000 -p 5001:5001 solankii
   ```

### Azure Deployment

1. **Create Azure resources**
   ```bash
   az group create --name solankii-rg --location eastus
   az appservice plan create --name solankii-plan --resource-group solankii-rg
   az webapp create --name solankii-app --resource-group solankii-rg --plan solankii-plan
   ```

2. **Deploy the application**
   ```bash
   az webapp deployment source config-zip --resource-group solankii-rg --name solankii-app --src ./publish.zip
   ```

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details.

### Development Setup

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Make your changes and add tests
4. Commit your changes: `git commit -m 'Add amazing feature'`
5. Push to the branch: `git push origin feature/amazing-feature`
6. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- **Documentation**: [docs/](docs/)
- **Issues**: [GitHub Issues](https://github.com/yourusername/solankii/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/solankii/discussions)
- **Email**: support@solankii.com

## 🙏 Acknowledgments

- OpenAI for providing the GPT-4 API
- The .NET community for excellent tooling and libraries
- All contributors who help improve Solankii

## 📈 Roadmap

- [ ] Real-time collaboration features
- [ ] Advanced AI model support (Claude, Gemini)
- [ ] Figma integration
- [ ] Mobile app companion
- [ ] Advanced analytics dashboard
- [ ] Plugin system for custom AI models
- [ ] Multi-language support
- [ ] Enterprise features (SSO, RBAC)

---

**Made with ❤️ by the Solankii Team** 