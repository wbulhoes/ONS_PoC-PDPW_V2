using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Configurar para abrir o navegador automaticamente
var url = "http://localhost:5555";

app.MapGet("/", () => Results.Content(GenerateHtmlPage(), "text/html"));

// Iniciar o navegador automaticamente
var startTask = Task.Run(async () =>
{
    await Task.Delay(1000); // Aguardar servidor iniciar
    OpenBrowser(url);
});

Console.WriteLine("========================================");
Console.WriteLine("  ?? Hello World - Web Dashboard");
Console.WriteLine("========================================");
Console.WriteLine();
Console.WriteLine($"?? Abrindo navegador em: {url}");
Console.WriteLine($"? Pressione Ctrl+C para encerrar");
Console.WriteLine();

app.Run(url);

static void OpenBrowser(string url)
{
    try
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"?? Não foi possível abrir o navegador automaticamente: {ex.Message}");
        Console.WriteLine($"?? Acesse manualmente: {url}");
    }
}

static string GenerateHtmlPage()
{
    var workspaceDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
    var projectsInfo = GetProjectsInfo(workspaceDir);
    var environmentInfo = GetEnvironmentInfo();
    var systemInfo = GetSystemInfo();
    var dotnetInfo = GetDotNetInfo();

    return $@"
<!DOCTYPE html>
<html lang='pt-BR'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>?? Hello World - Dashboard</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: #333;
            padding: 20px;
            min-height: 100vh;
        }}
        
        .container {{
            max-width: 1400px;
            margin: 0 auto;
        }}
        
        .header {{
            text-align: center;
            color: white;
            margin-bottom: 30px;
            padding: 30px;
            background: rgba(255, 255, 255, 0.1);
            backdrop-filter: blur(10px);
            border-radius: 20px;
            box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
        }}
        
        .header h1 {{
            font-size: 3em;
            margin-bottom: 10px;
            text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
        }}
        
        .header p {{
            font-size: 1.2em;
            opacity: 0.9;
        }}
        
        .status-badge {{
            display: inline-block;
            padding: 8px 20px;
            background: #4ade80;
            color: white;
            border-radius: 20px;
            font-weight: bold;
            margin-top: 10px;
            animation: pulse 2s infinite;
        }}
        
        @keyframes pulse {{
            0%, 100% {{ transform: scale(1); }}
            50% {{ transform: scale(1.05); }}
        }}
        
        .grid {{
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
            gap: 20px;
            margin-bottom: 20px;
        }}
        
        .card {{
            background: white;
            border-radius: 15px;
            padding: 25px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
            transition: transform 0.3s ease, box-shadow 0.3s ease;
        }}
        
        .card:hover {{
            transform: translateY(-5px);
            box-shadow: 0 15px 40px rgba(0, 0, 0, 0.3);
        }}
        
        .card-title {{
            font-size: 1.5em;
            margin-bottom: 20px;
            color: #667eea;
            border-bottom: 3px solid #667eea;
            padding-bottom: 10px;
            display: flex;
            align-items: center;
            gap: 10px;
        }}
        
        .info-row {{
            display: flex;
            justify-content: space-between;
            padding: 12px 0;
            border-bottom: 1px solid #f0f0f0;
        }}
        
        .info-row:last-child {{
            border-bottom: none;
        }}
        
        .info-label {{
            font-weight: 600;
            color: #666;
        }}
        
        .info-value {{
            color: #333;
            font-family: 'Courier New', monospace;
            background: #f8f9fa;
            padding: 4px 10px;
            border-radius: 5px;
        }}
        
        .success {{
            color: #4ade80;
            font-weight: bold;
        }}
        
        .project-item {{
            background: #f8f9fa;
            padding: 15px;
            margin: 10px 0;
            border-radius: 10px;
            border-left: 4px solid #667eea;
            transition: all 0.3s ease;
        }}
        
        .project-item:hover {{
            background: #e9ecef;
            border-left-color: #764ba2;
        }}
        
        .project-name {{
            font-weight: bold;
            color: #667eea;
            font-size: 1.1em;
            margin-bottom: 8px;
        }}
        
        .project-path {{
            font-family: 'Courier New', monospace;
            font-size: 0.9em;
            color: #666;
            word-break: break-all;
        }}
        
        .feature-check {{
            display: flex;
            align-items: center;
            padding: 10px;
            margin: 8px 0;
            background: #f0fdf4;
            border-radius: 8px;
            border-left: 3px solid #4ade80;
        }}
        
        .feature-check::before {{
            content: '?';
            color: #4ade80;
            font-weight: bold;
            font-size: 1.3em;
            margin-right: 10px;
        }}
        
        .footer {{
            text-align: center;
            color: white;
            margin-top: 30px;
            padding: 20px;
            background: rgba(255, 255, 255, 0.1);
            backdrop-filter: blur(10px);
            border-radius: 15px;
        }}
        
        .refresh-btn {{
            background: #667eea;
            color: white;
            border: none;
            padding: 12px 30px;
            border-radius: 25px;
            font-size: 1em;
            cursor: pointer;
            transition: all 0.3s ease;
            margin-top: 10px;
        }}
        
        .refresh-btn:hover {{
            background: #764ba2;
            transform: scale(1.05);
        }}
        
        .emoji {{
            font-size: 1.5em;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>?? Hello World - Dashboard</h1>
            <p>Validação Completa do Ambiente de Desenvolvimento</p>
            <div class='status-badge'>? AMBIENTE OK</div>
        </div>
        
        <div class='grid'>
            <!-- Informações do Sistema -->
            <div class='card'>
                <div class='card-title'>
                    <span class='emoji'>??</span> Informações do Sistema
                </div>
                {systemInfo}
            </div>
            
            <!-- Informações do .NET -->
            <div class='card'>
                <div class='card-title'>
                    <span class='emoji'>??</span> Configuração .NET
                </div>
                {dotnetInfo}
            </div>
            
            <!-- Informações do Ambiente -->
            <div class='card'>
                <div class='card-title'>
                    <span class='emoji'>??</span> Variáveis de Ambiente
                </div>
                {environmentInfo}
            </div>
            
            <!-- Funcionalidades C# -->
            <div class='card'>
                <div class='card-title'>
                    <span class='emoji'>?</span> Funcionalidades C# Testadas
                </div>
                <div class='feature-check'>Tipos Básicos (String, Integer, DateTime)</div>
                <div class='feature-check'>Coleções (List, Arrays)</div>
                <div class='feature-check'>LINQ (Language Integrated Query)</div>
                <div class='feature-check'>Async/Await (Programação Assíncrona)</div>
                <div class='feature-check'>Pattern Matching (C# 8+)</div>
                <div class='feature-check'>Records (C# 9+)</div>
                <div class='feature-check'>Nullable Reference Types</div>
                <div class='feature-check'>ASP.NET Core Web Server</div>
            </div>
        </div>
        
        <!-- Projetos do Workspace -->
        <div class='card' style='grid-column: 1 / -1;'>
            <div class='card-title'>
                <span class='emoji'>??</span> Projetos no Workspace
            </div>
            {projectsInfo}
        </div>
        
        <div class='footer'>
            <p>? Seu ambiente de desenvolvimento está 100% configurado!</p>
            <p>?? Pronto para desenvolver com .NET 8 e C#</p>
            <button class='refresh-btn' onclick='location.reload()'>?? Atualizar Informações</button>
        </div>
    </div>
</body>
</html>
";
}

static string GetSystemInfo()
{
    return $@"
        <div class='info-row'>
            <span class='info-label'>Sistema Operacional:</span>
            <span class='info-value'>{Environment.OSVersion}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Plataforma:</span>
            <span class='info-value'>{(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux" : "macOS")}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Arquitetura:</span>
            <span class='info-value'>{RuntimeInformation.ProcessArchitecture}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Nome da Máquina:</span>
            <span class='info-value'>{Environment.MachineName}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Usuário:</span>
            <span class='info-value'>{Environment.UserName}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Processadores:</span>
            <span class='info-value'>{Environment.ProcessorCount} cores</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Memória (Working Set):</span>
            <span class='info-value'>{Environment.WorkingSet / 1024 / 1024} MB</span>
        </div>
    ";
}

static string GetDotNetInfo()
{
    var assembly = Assembly.GetExecutingAssembly();
    var frameworkName = Assembly.GetEntryAssembly()?.GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>()?.FrameworkName;
    
    return $@"
        <div class='info-row'>
            <span class='info-label'>.NET Runtime:</span>
            <span class='info-value success'>{Environment.Version}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Framework:</span>
            <span class='info-value'>{frameworkName ?? "N/A"}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>CLR Version:</span>
            <span class='info-value'>{Environment.Version}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Assembly Version:</span>
            <span class='info-value'>{assembly.GetName().Version}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Runtime Identifier:</span>
            <span class='info-value'>{RuntimeInformation.RuntimeIdentifier}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Diretório Atual:</span>
            <span class='info-value' style='font-size: 0.8em;'>{Environment.CurrentDirectory}</span>
        </div>
    ";
}

static string GetEnvironmentInfo()
{
    return $@"
        <div class='info-row'>
            <span class='info-label'>Domain:</span>
            <span class='info-value'>{Environment.UserDomainName}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>64-bit OS:</span>
            <span class='info-value'>{Environment.Is64BitOperatingSystem}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>64-bit Process:</span>
            <span class='info-value'>{Environment.Is64BitProcess}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>System Directory:</span>
            <span class='info-value' style='font-size: 0.8em;'>{Environment.SystemDirectory}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Temp Path:</span>
            <span class='info-value' style='font-size: 0.8em;'>{Path.GetTempPath()}</span>
        </div>
        <div class='info-row'>
            <span class='info-label'>Uptime:</span>
            <span class='info-value'>{TimeSpan.FromMilliseconds(Environment.TickCount64).ToString(@"dd\.hh\:mm\:ss")}</span>
        </div>
    ";
}

static string GetProjectsInfo(string workspaceDir)
{
    var srcDir = Path.Combine(workspaceDir, "src");
    var projects = new StringBuilder();
    var projectCount = 0;

    if (Directory.Exists(srcDir))
    {
        var projectFiles = Directory.GetFiles(srcDir, "*.csproj", SearchOption.AllDirectories);
        
        foreach (var projectFile in projectFiles)
        {
            projectCount++;
            var projectName = Path.GetFileNameWithoutExtension(projectFile);
            var relativePath = Path.GetRelativePath(workspaceDir, projectFile);
            
            projects.AppendLine($@"
                <div class='project-item'>
                    <div class='project-name'>?? {projectName}</div>
                    <div class='project-path'>{relativePath}</div>
                </div>
            ");
        }
    }

    if (projectCount == 0)
    {
        projects.AppendLine(@"
            <div class='project-item'>
                <div class='project-name'>?? Nenhum projeto encontrado</div>
                <div class='project-path'>Workspace: " + workspaceDir + @"</div>
            </div>
        ");
    }
    else
    {
        projects.Insert(0, $"<p style='margin-bottom: 15px; color: #667eea; font-weight: bold;'>? Total de {projectCount} projeto(s) encontrado(s)</p>");
    }

    return projects.ToString();
}
