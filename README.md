# Alertas Inventario Worker

Worker Service en C# .NET 8 que se ejecuta en segundo plano monitoreando el inventario industrial. Cuando detecta repuestos con stock igual o menor al nivel crítico, envía automáticamente un correo de alerta al jefe de planta.

## Tecnologías
- C# / .NET 8
- Worker Service (Background Service)
- Entity Framework Core
- SQL Server 2025
- MailKit / MimeKit

## Funcionalidades
- Consulta la base de datos cada 5 minutos
- Detecta repuestos con stock crítico no alertados
- Envía correo HTML con tabla de repuestos críticos
- Marca los repuestos como alertados para no enviar duplicados


git add README.md
git commit -m "docs: agregar README del proyecto"
git push
cat > README.md << 'EOF'
# Alertas Inventario Worker

Worker Service en C# .NET 8 que se ejecuta en segundo plano monitoreando el inventario industrial. Cuando detecta repuestos con stock igual o menor al nivel crítico, envía automáticamente un correo de alerta al jefe de planta.

## Tecnologías
- C# / .NET 8
- Worker Service (Background Service)
- Entity Framework Core
- SQL Server 2025
- MailKit / MimeKit

## Funcionalidades
- Consulta la base de datos cada 5 minutos
- Detecta repuestos con stock crítico no alertados
- Envía correo HTML con tabla de repuestos críticos
- Marca los repuestos como alertados para no enviar duplicados

## Parte del ecosistema
Este proyecto es parte de un ecosistema de 3 proyectos:
1. [InventarioIndustrialAPI](https://github.com/luisfelipems20/InventarioIndustrialAPI) - API REST en C# .NET 8
2. [DashboardInventario](https://github.com/luisfelipems20/DashboardInventario) - Dashboard web HTML/Bootstrap
3. **AlertasInventarioWorker** - Worker Service de alertas (este proyecto)

## Configuración
Actualizar `appsettings.json` con la cadena de conexión SQL Server y credenciales de correo Gmail (contraseña de aplicación).

## Autor
Luis Felipe - Tecnico en Programacion y Analisis de Sistemas
