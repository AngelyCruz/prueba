# WPF Vehitanques

Aplicación WPF para .NET Framework 4.5, creada con Visual Studio 2013.

## Configuración

1. Restaura los paquetes NuGet declarados en `WPF_Vehitanques\packages.config`.
2. Copia `WPF_Vehitanques\App.config.example` como `WPF_Vehitanques\App.config`.
3. Configura en `App.config` las cadenas de conexión y las credenciales de Elibom para tu entorno. No guardes ese archivo con credenciales en Git.
4. Abre `WPF_Vehitanques.sln` y compila la solución.

El proyecto también hace referencia a bibliotecas externas que no están incluidas en este repositorio, entre ellas Elibom y Microsoft Office Interop Excel.
