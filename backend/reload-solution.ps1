# =====================================================================
# SCRIPT PARA RECARREGAR SOLUÇÃO NO VISUAL STUDIO
# =====================================================================

$ProjectRoot = "C:\Users\User\Desktop\Sistemas 2026\ecommerce-platform"
$SolutionName = "ProjetoEcommerce"

$infoColor = "Cyan"
$successColor = "Green"
$warningColor = "Yellow"

Write-Host "
========================================" -ForegroundColor $infoColor
Write-Host "RECARREGANDO SOLUÇÃO" -ForegroundColor $infoColor
Write-Host "========================================" -ForegroundColor $infoColor

function Reload-VisualStudio {
    Write-Host "
[OPÇÃO 1] Fechando Visual Studio e reabrindo..." -ForegroundColor $infoColor
    
    Write-Host "Fechando Visual Studio..." -ForegroundColor $warningColor
    Stop-Process -Name devenv -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 3
    
    Write-Host "Reabrindo solução..." -ForegroundColor $warningColor
    $solutionPath = "$ProjectRoot\$SolutionName.sln"
    
    if (Test-Path $solutionPath) {
        & $solutionPath
        Write-Host "✓ Solução aberta" -ForegroundColor $successColor
    } else {
        Write-Host "❌ Arquivo não encontrado: $solutionPath" -ForegroundColor "Red"
    }
}

function Reload-Dotnet {
    Write-Host "
[OPÇÃO 2] Recarregando via dotnet CLI..." -ForegroundColor $infoColor
    
    Set-Location $ProjectRoot
    
    Write-Host "Restaurando pacotes..." -ForegroundColor $warningColor
    dotnet restore
    
    Write-Host "
✓ Solução recarregada" -ForegroundColor $successColor
}

function Clean-AndReload {
    Write-Host "
[OPÇÃO 3] Limpando cache e recarregando..." -ForegroundColor $infoColor
    
    Set-Location $ProjectRoot
    
    Write-Host "Limpando arquivos temporários..." -ForegroundColor $warningColor
    
    Get-ChildItem -Recurse -Filter "bin" -Directory | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    Get-ChildItem -Recurse -Filter "obj" -Directory | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    
    Write-Host "Removendo pasta .vs..." -ForegroundColor $warningColor
    Remove-Item -Path "$ProjectRoot\.vs" -Recurse -Force -ErrorAction SilentlyContinue
    
    Write-Host "Limpando cache NuGet..." -ForegroundColor $warningColor
    dotnet nuget locals all --clear
    
    Write-Host "Restaurando pacotes..." -ForegroundColor $warningColor
    dotnet restore
    
    Write-Host "
✓ Cache limpo e solução recarregada" -ForegroundColor $successColor
}

function Build-Solution {
    Write-Host "
[OPÇÃO 4] Compilando solução..." -ForegroundColor $infoColor
    
    Set-Location $ProjectRoot
    
    Write-Host "Compilando..." -ForegroundColor $warningColor
    dotnet build "$SolutionName.sln"
    
    Write-Host "
✓ Solução compilada" -ForegroundColor $successColor
}

function Do-Everything {
    Write-Host "
[OPÇÃO 5] Executando todas as operações..." -ForegroundColor $infoColor
    
    Clean-AndReload
    Build-Solution
    Reload-VisualStudio
    
    Write-Host "
✓ Todas as operações concluídas" -ForegroundColor $successColor
}

Write-Host "
Escolha uma opção:" -ForegroundColor $infoColor
Write-Host "[1] Fechar VS e abrir novamente (Recomendado)" -ForegroundColor $warningColor
Write-Host "[2] Restaurar via dotnet CLI" -ForegroundColor $warningColor
Write-Host "[3] Limpar cache e restaurar" -ForegroundColor $warningColor
Write-Host "[4] Compilar solução" -ForegroundColor $warningColor
Write-Host "[5] Fazer tudo (3 + 4 + 1)" -ForegroundColor $warningColor

$choice = Read-Host "
Digite o número da opção"

switch ($choice) {
    "1" { Reload-VisualStudio }
    "2" { Reload-Dotnet }
    "3" { Clean-AndReload }
    "4" { Build-Solution }
    "5" { Do-Everything }
    default { Write-Host "Opção inválida" -ForegroundColor "Red" }
}

Write-Host "
========================================" -ForegroundColor $infoColor
Write-Host "✓ PROCESSO CONCLUÍDO" -ForegroundColor $successColor
Write-Host "========================================" -ForegroundColor $infoColor
