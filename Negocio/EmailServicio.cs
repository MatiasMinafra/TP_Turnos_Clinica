using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


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

            
            if (!enabled)
                return;

            host = Get("SMTP_HOST");
            user = Get("SMTP_USER");
            pass = Get("SMTP_PASS");
            from = Get("SMTP_FROM");

            string portTxt = Get("SMTP_PORT");
            if (!int.TryParse(portTxt, out port))
                throw new Exception($"SMTP_PORT inválido en web.config. Debe ser número (ej: 587). Valor actual: '{portTxt}'");

            string sslTxt = Get("SMTP_SSL");
            if (!bool.TryParse(sslTxt, out ssl))
                throw new Exception($"SMTP_SSL inválido en web.config. Debe ser true/false. Valor actual: '{sslTxt}'");

            if (string.IsNullOrWhiteSpace(host)) throw new Exception("Falta SMTP_HOST en web.config.");
            if (string.IsNullOrWhiteSpace(user)) throw new Exception("Falta SMTP_USER en web.config.");
            if (string.IsNullOrWhiteSpace(pass)) throw new Exception("Falta SMTP_PASS en web.config.");
            if (string.IsNullOrWhiteSpace(from)) throw new Exception("Falta SMTP_FROM en web.config.");

       
            if (user.Equals("tuemail@gmail.com", StringComparison.OrdinalIgnoreCase) ||
                from.Equals("tuemail@gmail.com", StringComparison.OrdinalIgnoreCase))
                throw new Exception("SMTP_USER/SMTP_FROM siguen con el placeholder 'tuemail@gmail.com'. Poné tu Gmail real.");

            if (pass.IndexOf("TU_APP_PASSWORD", StringComparison.OrdinalIgnoreCase) >= 0 ||
                pass.IndexOf("ACA_TU_APP_PASSWORD_REAL", StringComparison.OrdinalIgnoreCase) >= 0)
                throw new Exception("SMTP_PASS sigue con placeholder. Tenés que poner la APP PASSWORD real de Gmail (16 caracteres).");
        }

        private string Get(string key) => (ConfigurationManager.AppSettings[key] ?? "").Trim();

        private void ValidarMail(string mail, string nombreCampo)
        {
            try { _ = new MailAddress(mail); }
            catch { throw new Exception($"{nombreCampo} inválido: '{mail}'"); }
        }

        private void Enviar(string para, string asunto, string html)
        {
            
            if (!enabled) return;

            para = (para ?? "").Trim();

            if (string.IsNullOrWhiteSpace(para))
                throw new Exception("El paciente no tiene email cargado. No se puede enviar el mail.");

            ValidarMail(para, "Email del paciente");
            ValidarMail(from, "SMTP_FROM");

            using (var msg = new MailMessage())
            {
                msg.From = new MailAddress(from, "Clinica Turnos");
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
                    smtp.Timeout = 15000; // 15s

                    try
                    {
                        smtp.Send(msg);
                    }
                    catch (SmtpException ex)
                    {
                        throw new Exception("Fallo SMTP: " + ex.Message);
                    }
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