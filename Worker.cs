using AlertasInventarioWorker.Data;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace AlertasInventarioWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de alertas iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Revisando stock crítico: {time}", DateTimeOffset.Now);

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<InventarioContext>();

                    var repuestosCriticos = await context.Repuestos
                        .Where(r => r.CantidadStock <= r.NivelCritico && !r.AlertaEnviada)
                        .ToListAsync(stoppingToken);

                    if (repuestosCriticos.Any())
                    {
                        _logger.LogWarning("{count} repuestos en stock crítico encontrados.", repuestosCriticos.Count);
                        await EnviarCorreo(repuestosCriticos);

                        foreach (var r in repuestosCriticos)
                            r.AlertaEnviada = true;

                        await context.SaveChangesAsync(stoppingToken);
                    }
                    else
                    {
                        _logger.LogInformation("Sin repuestos críticos pendientes.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al revisar stock.");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task EnviarCorreo(List<Repuesto> repuestos)
        {
            var email = _config.GetSection("EmailSettings");

            var mensaje = new MimeMessage();
            mensaje.From.Add(MailboxAddress.Parse(email["Remitente"]));
            mensaje.To.Add(MailboxAddress.Parse(email["Destinatario"]));
            mensaje.Subject = "⚠ Alerta de Stock Crítico — Planta Industrial";

            var cuerpo = "<h2>Repuestos en Stock Crítico</h2><table border='1' cellpadding='6'>";
            cuerpo += "<tr><th>ID</th><th>Nombre</th><th>Stock Actual</th><th>Nivel Crítico</th></tr>";
            foreach (var r in repuestos)
                cuerpo += $"<tr><td>{r.Id}</td><td>{r.Nombre}</td><td style='color:red'>{r.CantidadStock}</td><td>{r.NivelCritico}</td></tr>";
            cuerpo += "</table><p>Revisar inventario a la brevedad.</p>";

            mensaje.Body = new TextPart("html") { Text = cuerpo };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(email["SmtpServer"], int.Parse(email["SmtpPort"]!), false);
            await smtp.AuthenticateAsync(email["Remitente"], email["Contrasena"]);
            await smtp.SendAsync(mensaje);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Correo de alerta enviado a {dest}", email["Destinatario"]);
        }
    }
}