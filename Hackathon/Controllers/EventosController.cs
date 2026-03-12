using Hackathon.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Controllers
{
    public class EventosController : Controller
    {
    private List<Evento> Eventos
        {
            get
            {
                if (Session["Eventos"] == null)
                    Session["Eventos"] = new List<Evento>();
                return (List<Evento>)Session["Eventos"];
            }
            set { Session["Eventos"] = value; }
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEventos()
        {
            var eventos = Eventos.Select(e => new {
                id = e.Id,
                title = e.Titulo,
                start = e.FechaInicio.ToString("s"),
                end = e.FechaFin.ToString("s"),
                color = e.Color
            });
            return Json(eventos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateEvento(Evento ev)
        {
            ev.Id = Eventos.Count > 0 ? Eventos.Max(x => x.Id) + 1 : 1;
            Eventos.Add(ev);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult UpdateEvento(Evento ev)
        {
            var existente = Eventos.FirstOrDefault(x => x.Id == ev.Id);
            if (existente != null)
            {
                existente.Titulo = ev.Titulo;
                existente.FechaInicio = ev.FechaInicio;
                existente.FechaFin = ev.FechaFin;
                existente.Color = ev.Color;
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult DeleteEvento(int id)
        {
            var ev = Eventos.FirstOrDefault(x => x.Id == id);
            if (ev != null) Eventos.Remove(ev);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult MoveEvento(int id, DateTime nuevaFecha)
        {
            var ev = Eventos.FirstOrDefault(x => x.Id == id);
            if (ev != null)
            {
                var duracion = ev.FechaFin - ev.FechaInicio;
                ev.FechaInicio = nuevaFecha;
                ev.FechaFin = nuevaFecha.Add(duracion);
            }
            return Json(new { success = true });
        }
    }

}
}
