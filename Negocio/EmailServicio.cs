using Dominio;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Negocio
{
    public class EmailServicio
    {
        private readonly bool enabled;

        private readonly string host;
        private readonly int port;
        private readonly bool ssl;
        private readonly string user;
        private readonly string pass;
        private readonly string from;

        public EmailServicio()
        {
            enabled = string.Equals(
                (ConfigurationManager.AppSettings["SMTP_ENABLED"] ?? "").Trim(),
                "true",
                StringComparison.OrdinalIgnoreCase
            );

            if (!enabled) return;

            host = Get("SMTP_HOST");
            user = Get("SMTP_USER");
            pass = (Get("SMTP_PASS") ?? "").Replace(" ", "").Trim(); 
            from = Get("SMTP_FROM");

            string portTxt = Get("SMTP_PORT");
            if (!int.TryParse(portTxt, out port))
                throw new Exception($"SMTP_PORT inválido. Valor: '{portTxt}'");

            string sslTxt = Get("SMTP_SSL");
            if (!bool.TryParse(sslTxt, out ssl))
                throw new Exception($"SMTP_SSL inválido. Valor: '{sslTxt}'");

            if (string.IsNullOrWhiteSpace(host)) throw new Exception("Falta SMTP_HOST.");
            if (string.IsNullOrWhiteSpace(user)) throw new Exception("Falta SMTP_USER.");
            if (string.IsNullOrWhiteSpace(pass)) throw new Exception("Falta SMTP_PASS.");
            if (string.IsNullOrWhiteSpace(from)) throw new Exception("Falta SMTP_FROM.");
        }

        private string Get(string key) => (ConfigurationManager.AppSettings[key] ?? "").Trim();

      
        private string NormalizarEmail(string email)
        {
            if (email == null) return "";

            email = email.Trim();

            
            email = email.Replace("\u00A0", " ")
                         .Replace("\u200B", "")
                         .Replace("\u200C", "")
                         .Replace("\u200D", "")
                         .Replace("\uFEFF", "")
                         .Replace(" ", "");

            return email;
        }

        private void ValidarMail(string mail, string nombreCampo)
        {
            mail = NormalizarEmail(mail);

            try { _ = new MailAddress(mail); }
            catch (Exception ex)
            {
             
                throw new Exception($"{nombreCampo} inválido: '{mail}'. Detalle: {ex.Message}");
            }
        }

        private void Enviar(string para, string asunto, string html)
        {
            if (!enabled) return;

            para = NormalizarEmail(para);
            string fromLimpio = NormalizarEmail(from);

            if (string.IsNullOrWhiteSpace(para))
                throw new Exception("El paciente no tiene email cargado. No se puede enviar el mail.");

            
            try { _ = new MailAddress(para); }
            catch { throw new Exception($"Email del paciente inválido: '{para}'"); }

            try { _ = new MailAddress(fromLimpio); }
            catch { throw new Exception($"SMTP_FROM inválido: '{fromLimpio}'"); }

            using (var msg = new MailMessage())
            {
                msg.From = new MailAddress(fromLimpio, "Clinica Turnos");
                msg.To.Add(new MailAddress(para));
                msg.Subject = asunto;
                msg.Body = html;
                msg.IsBodyHtml = true;
                msg.BodyEncoding = Encoding.UTF8;
                msg.SubjectEncoding = Encoding.UTF8;

                using (var smtp = new SmtpClient(host, port))
                {
                    smtp.EnableSsl = ssl;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(user, pass);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Timeout = 15000;

                    smtp.Send(msg);
                }
            }
        }
        public void EnviarConfirmacionTurno(DtoTurnoMail t)
        {
            if (!enabled) return;

            string asunto = $"Confirmación de turno #{t.TurnoID} - Clínica Turnos";

            string html = $@"
<div style='font-family: Arial; line-height: 1.4'>
  <h2>Confirmación de turno</h2>
  <p>Hola <b>{t.PacienteNombre}</b>, tu turno quedó confirmado.</p>
  <hr/>
  <p><b>N° Turno:</b> {t.TurnoID}</p>
  <p><b>Especialidad:</b> {t.Especialidad}</p>
  <p><b>Médico:</b> {t.MedicoNombre}</p>
  <p><b>Fecha:</b> {t.Fecha:dd/MM/yyyy}</p>
  <p><b>Horario:</b> {t.HoraInicio:hh\\:mm} - {t.HoraFin:hh\\:mm}</p>
  <p><b>Motivo:</b> {System.Net.WebUtility.HtmlEncode(t.MotivoConsulta)}</p>
  <p><b>Importe:</b> {t.Importe:C}</p>
  <p><b>Medio de pago:</b> {t.MedioPago}</p>
  <hr/>
  <p style='color:#666'>Este mail es automático (TP).</p>
</div>";

            Enviar(t.PacienteEmail, asunto, html);
        }
        public void EnviarReprogramacionTurno(DtoTurnoMail t, DateTime fechaAnterior, TimeSpan horaAnterior)
        {
            if (!enabled) return;

            string asunto = $"Turno reprogramado #{t.TurnoID} - Clínica Turnos";

            string html = $@"
<div style='font-family: Arial; line-height: 1.4'>
  <h2>Turno reprogramado</h2>
  <p>Hola <b>{t.PacienteNombre}</b>, tu turno fue reprogramado.</p>
  <hr/>
  <p><b>N° Turno:</b> {t.TurnoID}</p>
  <p><b>Especialidad:</b> {t.Especialidad}</p>
  <p><b>Médico:</b> {t.MedicoNombre}</p>

  <h4>Antes</h4>
  <p><b>Fecha:</b> {fechaAnterior:dd/MM/yyyy}</p>
  <p><b>Hora:</b> {horaAnterior:hh\\:mm}</p>

  <h4>Ahora</h4>
  <p><b>Fecha:</b> {t.Fecha:dd/MM/yyyy}</p>
  <p><b>Horario:</b> {t.HoraInicio:hh\\:mm} - {t.HoraFin:hh\\:mm}</p>

  <hr/>
  <p style='color:#666'>Este mail es automático (TP).</p>
</div>";

            Enviar(t.PacienteEmail, asunto, html);
        }
    }
}