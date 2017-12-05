using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DbConnection;
using System;

namespace AjaxNotes.Controllers
{
    public class NoteController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet]
        [Route("fetchdata")]
        public JsonResult AllData()
        {
            return Json(DbConnector.Query("SELECT * FROM notes;"));
        }

        [HttpGet]
        [Route("fetchdata/{id}")]
        public JsonResult OneData(int id)
        {
            try
            {
                return Json(DbConnector.Query($"SELECT {id} FROM notes;")[0]);
            }
            catch(ArgumentOutOfRangeException e)
            {
                return Json(e.Message);
            }
        }

        [HttpPost]
        [Route("adddata")]
        public JsonResult AddNote(string title)
        {
            DbConnector.Execute($"INSERT INTO notes (title, text, created_at, updated_at) VALUES ('{title}', '', NOW(), NOW());");
            return Json(DbConnector.Query("SELECT * FROM notes ORDER BY id DESC LIMIT 1;"));
        }

        [HttpPost]
        [Route("editdata")]
        public JsonResult EditNote(int id, string title, string text)
        {
            try
            {
                DbConnector.Query($"SELECT {id} FROM notes");
            }
            catch(ArgumentOutOfRangeException e)
            {
                return Json(e.Message);
            }
            DbConnector.Execute($"UPDATE notes SET title = '{title}', text = '{text}' WHERE id = {id};");
            return OneData(id);
        }

        [HttpGet]
        [Route("deletedata/{id}")]
        public IActionResult DeleteNote(int id)
        {
            try
            {
                DbConnector.Query($"SELECT {id} FROM notes");
            }
            catch(ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index");
            }
            DbConnector.Execute($"DELETE FROM notes WHERE id = {id};");
            return RedirectToAction("Index");
        }
    }
}