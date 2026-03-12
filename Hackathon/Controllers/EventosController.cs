using Hackathon.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Hackathon.Controllers
{
    public class EventosController : Controller
    {
        private List<Evento> Eventos
        {
            get
            {
                var eventosJson = HttpContext.Session.GetString("Eventos");

                if (string.IsNullOrEmpty(eventosJson))
                {
                    var listaNueva = new List<Evento>();
                    HttpContext.Session.SetString("Eventos", JsonSerializer.Serialize(listaNueva));
                    return listaNueva;
                }

                return JsonSerializer.Deserialize<List<Evento>>(eventosJson) ?? new List<Evento>();
            }
            set
            {
                HttpContext.Session.SetString("Eventos", JsonSerializer.Serialize(value));
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetEventos()
        {
            var eventos = Eventos.Select(e => new
            {
                id = e.Id,
                title = e.Titulo,
                start = e.FechaInicio.ToString("s"),
                end = e.FechaFin.ToString("s"),
                color = e.Color
            });

            return Json(eventos);
        }

        [HttpPost]
        public JsonResult CreateEvento(Evento ev)
        {
            var lista = Eventos;

            ev.Id = lista.Count > 0 ? lista.Max(x => x.Id) + 1 : 1;
            lista.Add(ev);

            Eventos = lista;

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult UpdateEvento(Evento ev)
        {
            var lista = Eventos;
            var existente = lista.FirstOrDefault(x => x.Id == ev.Id);

            if (existente != null)
            {
                existente.Titulo = ev.Titulo;
                existente.FechaInicio = ev.FechaInicio;
                existente.FechaFin = ev.FechaFin;
                existente.Color = ev.Color;

                Eventos = lista;
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult DeleteEvento(int id)
        {
            var lista = Eventos;
            var ev = lista.FirstOrDefault(x => x.Id == id);

            if (ev != null)
            {
                lista.Remove(ev);
                Eventos = lista;
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult MoveEvento(int id, DateTime nuevaFecha)
        {
            var lista = Eventos;
            var ev = lista.FirstOrDefault(x => x.Id == id);

            if (ev != null)
            {
                var duracion = ev.FechaFin - ev.FechaInicio;
                ev.FechaInicio = nuevaFecha;
                ev.FechaFin = nuevaFecha.Add(duracion);

                Eventos = lista;
            }

            return Json(new { success = true });
        }
    }
}
