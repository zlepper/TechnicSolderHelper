using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using ModpackHelper.Shared.Web.Api;
using ModpackHelper.webmods.db;
using ModpackHelper.webmods.Helpers;

namespace ModpackHelper.webmods.Controllers
{
    /// <summary>
    /// Api containing all the mods currently stored
    /// </summary>
    public class ModsController : ApiController
    {
        private ModpackHelperContext db = new ModpackHelperContext();

        // GET: api/Mods
        /// <summary>
        /// Get a list of all the mods in the database
        /// </summary>
        /// <returns></returns>
        public IQueryable<Mod> GetMods()
        {
            return db.Mods;
        }

        // GET: api/Mods/{md5 value}
        /// <summary>
        /// Get the specific mod from the db, provided it has been accepted. 
        /// </summary>
        /// <param name="md5">The md5 value of the mod</param>
        /// <returns>The mod with the specific md5 value</returns>
        [ResponseType(typeof(Mod))]
        [Route("api/Mods/{md5}")]
        public IHttpActionResult GetMod(string md5)
        {
            // Find this not, if it has been accepted only, any send it back to the user
            Mod mod = db.Mods.FirstOrDefault(m => m.JarMd5.Equals(md5) && m.Status == Status.Accepted);
            if (mod == null)
            {
                return NotFound();
            }

            return Ok(mod);
        }

        //// PUT: api/Mods/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutMod(int id, Mod mod)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != mod.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(mod).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ModExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Mods
        /// <summary>
        /// Add a mod to the database
        /// </summary>
        /// <param name="mod">The mod to add to the database</param>
        /// <returns>Returns the uri of the new mod</returns>
        [ResponseType(typeof(Mod))]
        public IHttpActionResult PostMod(Mod mod)
        {
            // Make sure it was proper data we got
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Do whatever needs to be done to the mod
            ApiHelpers.AddModToDB(mod);

            // Tell the user where they can find the data
            return Ok();
        }

        // DELETE: api/Mods/5
        //[ResponseType(typeof(Mod))]
        //public IHttpActionResult DeleteMod(int id)
        //{
        //    Mod mod = db.Mods.Find(id);
        //    if (mod == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Mods.Remove(mod);
        //    db.SaveChanges();

        //    return Ok(mod);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModExists(int id)
        {
            return db.Mods.Count(e => e.Id == id) > 0;
        }
    }
}