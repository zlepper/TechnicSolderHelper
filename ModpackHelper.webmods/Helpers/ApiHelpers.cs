using System.Linq;
using System.Web;
using ModpackHelper.Shared.Web.Api;
using ModpackHelper.webmods.db;

namespace ModpackHelper.webmods.Helpers
{
    /// <summary>
    /// A bunch of helper method for the api
    /// </summary>
    public static class ApiHelpers
    {
        /// <summary>
        /// Adds a mod to the database, and does some intelligent 
        /// stuff to make sure it hasn't already been added
        /// </summary>
        /// <param name="mod">The mod to add</param>
        public static void AddModToDB(Mod mod)
        {
            using (ModpackHelperContext db = new ModpackHelperContext())
            {
                // Find the user uploading the data
                string userip = HttpContext.Current.Request.UserHostAddress;
                HelperUser user = db.HelperUsers.FirstOrDefault(u => u.Ip.Equals(userip));
                IQueryable<Mod> mods = db.Mods.Where(m => m.JarMd5.Equals(mod.JarMd5));

                // Check how many already submitted a mod with this exact data
                IQueryable<Mod> modsLike = mods.Where(m => m.Equals(mod));
                int c = modsLike.Count();

                if (user == null)
                {
                    // This is the first time ever the user uploaded something
                    user = new HelperUser(userip);
                    db.HelperUsers.Add(user);
                }
                mod.HelperUser = user;

                if (c == 0)
                {
                    // Nobody submitted a mod quite like this

                    // Check if there actually are any mods in the first place
                    if (!mods.Any())
                    {
                        // This is the first time ever this mod has been added
                        mod.Status = Status.Awaiting;
                    }
                    else
                    {

                        //Check if we have any mods who are already accepted
                        // If it is accepted, then the mod must be wrong, otherwise
                        // await someone trusty to come by
                        mod.Status = mods.Any(m => m.Status == Status.Accepted) ? Status.Denied : Status.Awaiting;
                    }
                }
                else
                {
                    // Somebody has already submitted this data

                    // Check if this data has already been accepted
                    if (modsLike.Any(m => m.Status == Status.Accepted))
                    {
                        // That means this mods data is good to go
                        mod.Status = Status.Accepted;
                    }
                    else
                    {
                        // If this mods data has already been denied
                        // then no need to check it again by a human
                        if (modsLike.Any(m => m.Status == Status.Denied))
                        {
                            mod.Status = Status.Denied;
                        }
                        else
                        {
                            // Check if this mods data has already been decided
                            // If there is any data for the same mod which has already been
                            // accepted, then this must be wrong
                            if (mods.Any(m => m.Status == Status.Accepted))
                            {
                                mod.Status = Status.Denied;
                            }
                            else
                            {
                                // A value indicating how likely this data is to be correct
                                int goodCount = 0;
                                // Check how many mods were added by people with a good reputation
                                goodCount +=
                                    modsLike.Count(
                                        m =>
                                            m.HelperUser.Mods.Count(mo => mo.Status == Status.Accepted) > 20) * 2;

                                // Check how many mods were added by people with a bad reputation
                                goodCount -=
                                    modsLike.Count(m => m.HelperUser.Mods.Count(mo => mo.Status == Status.Denied) > 20) * 2;

                                // Add how many times this data has been submitted in total
                                goodCount += modsLike.Count();

                                // Check if the mod data appear to be good enough
                                if (goodCount > 30)
                                {
                                    // Accept all the mod data
                                    mod.Status = Status.Accepted;
                                    foreach (Mod mod1 in modsLike)
                                    {
                                        mod1.Status = Status.Accepted;
                                    }

                                    // Deny everything else
                                    foreach (Mod mod1 in mods)
                                    {
                                        if(modsLike.Contains(mod1)) continue;
                                        mod1.Status = Status.Denied;
                                    }
                                }
                                else
                                {
                                    // A human have too look at this one
                                    mod.Status = Status.Awaiting;
                                }
                            }
                        }
                    }
                }

                // Save the data to the db
                db.Mods.Add(mod);
                db.SaveChanges();
            }
        }
    }
}
