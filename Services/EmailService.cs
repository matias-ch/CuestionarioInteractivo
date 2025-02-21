using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace CuestionarioInteractivo.Services;
public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> EnviarCorreoAsync(string destinatario, string asunto, string cuerpo)
    {
        if (string.IsNullOrWhiteSpace(destinatario))
            return $"Tenés que escribir un email para poder recibir la respuesta de la pregunta.";
        destinatario = destinatario.Trim();
        if (!EsValidoElEmail(destinatario))
            return $"El formato del email es incorrecto. No se envió el email con la solución.";

        var smtpSettings = _configuration.GetSection("SmtpSettings");

        using (var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
        {
            try
            {
                client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                client.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                var mensaje = new MailMessage
                {
                    From = new MailAddress(smtpSettings["Username"]),
                    Subject = asunto,
                    Body = cuerpo,
                    IsBodyHtml = true
                };

                mensaje.To.Add(destinatario);
                await client.SendMailAsync(mensaje);
                return $"Enviado email a {destinatario} con la solución del ejercicio.";
            }
            catch (Exception e)
            {
                Console.Write($"Error al enviar email con solución: {e.Message}");
                return $"No se pudo enviar el email con la solución. En general esto se debe a un problema de configuración, revisar la consola.";
            }
        }
    }

    private bool EsValidoElEmail(string email)=>Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    
}
