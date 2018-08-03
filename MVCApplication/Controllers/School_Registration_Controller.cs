using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCApplication.Models;

namespace MVCApplication.Controllers
{
    public class School_Registration_Controller : Controller
    {
        OAF_DBEntities db = new OAF_DBEntities();
        // GET: School_Registration_
        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert(Application ap,ProcessedApplication pa)
        {
            //if(string.IsNullOrEmpty("txtname"))
            //    ModelState.AddModelError("", "Name Cannot be Empty!!");
            //if (Request.Form["ddlbranch"]=="select")
            //    ModelState.AddModelError("", "Select a Branch!!");
            //if (Request.Form["ddlclassid"] == "select")
            //    ModelState.AddModelError("", "Select a ClassID!!");
           

            int branchid = int.Parse(Request.Form["ddlbranch"].ToString());
            int classid = int.Parse(Request.Form["ddlclassid"].ToString());
            string name = Request.Form["txtname"].ToString();
            int age = int.Parse(Request.Form["txtage"].ToString());
            DateTime dob = DateTime.Parse(Request.Form["txtdob"].ToString());
            string addr = Request.Form["txtaddress"].ToString();
            ap.branch_Id = branchid;
            ap.class_Id = classid;
            ap.name = name;
            ap.age = age;
            ap.dob = dob;
            ap.address = addr;
           pa.comments = "Submitted, But Not Yet Processed";
            db.Applications.Add(ap);
            db.ProcessedApplications.Add(pa);
            var res = db.SaveChanges();
            if (res > 0)
            {
                ModelState.AddModelError("", "Data Inserted");
                ModelState.AddModelError("","Your Application has been submitted. Your Application id = "+ ap.id);
                
            }
            return View();
        }
        [HttpGet]
        public ActionResult Status()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Status(ProcessedApplication p)
        {
            int id = int.Parse(Request.Form["txtid"].ToString());
            var data = db.ProcessedApplications.Where(x=>x.application_id==id).SingleOrDefault();
            string st = data.comments;
            string rt = data.resolved_by;
            if(st== "Submitted, But Not Yet Processed")
            {
                ModelState.AddModelError("", st+" Check Later.");

            }
            else
            {
                ModelState.AddModelError("", st + " Please Contact :"+rt);
            }
            
            return View();
        }
        [HttpGet]
        public ActionResult UpdateStatus()
        {

            return View();
        }
        [HttpPost]
        public ActionResult UpdateStatus(ProcessedApplication p,string Command)
        {
            int id = int.Parse(Request.Form["txtid"].ToString());
            var data = db.ProcessedApplications.Where(x => x.application_id == id).SingleOrDefault();

            if (Command=="Status")
            {
                
                ModelState.AddModelError("", data.comments);
                return View();
            }
           
            if(Command=="Update")
            {
                string comment = Request.Form["txtcomment"].ToString();
                data.comments = comment;
                data.date_of_resolve = DateTime.Now;
                var prstatus = (from t in db.Applications join b in db.Branches on t.branch_Id equals b.branch_id  where t.id==id select b.contact_person ).SingleOrDefault();
                data.resolved_by = prstatus;
                var res = db.SaveChanges();
                if (res > 0)
                {
                    ModelState.AddModelError("", "Status Updated");
                }
                return View();
            }
               return View();
        }
        [HttpGet]
        public ActionResult Report()
        {
            var data = from t in db.Applications
                      
                       orderby t.id ascending
                       select t;
            List<poco> lst = new List<poco>();
            foreach (var j in data)
            {
                poco pm = new poco();
                pm.id = j.id;
                pm.name = j.name;
                pm.age = j.age;
                pm.dob = j.dob;
                pm.address = j.address;
                pm.branch_Id = j.branch_Id;
                pm.class_Id = j.class_Id;
                lst.Add(pm);
            }
            int data7 = db.Applications.Count();
            Session["totalcount"] = data7;
            int res = db.ProcessedApplications.Count(x=>x.comments=="Processed");
            Session["processedcount"] = res;
            return View(lst);
        }
        [HttpPost]
        public ActionResult Report(poco poo)
        {
            if (Request.Form["ddlpr"].ToString() == "Processed")
            {

                var data3 = from t in db.Applications
                           join p in db.ProcessedApplications on t.id equals p.application_id
                           where p.comments == "Processed"
                           orderby t.id ascending
                           select t;
                List<poco> lst = new List<poco>();

                foreach (var j in data3)
                {
                    poco pm = new poco();
                    pm.id = j.id;
                    pm.name = j.name;
                    pm.age = j.age;
                    pm.dob = j.dob;
                    pm.address = j.address;
                    pm.branch_Id = j.branch_Id;
                    pm.class_Id = j.class_Id;

                    lst.Add(pm);
                }
                // Session["datta"] = lst;
                return View(lst);
            }
            else
            {
                var data1 = (from t in db.Applications
                             join p in db.ProcessedApplications on t.id equals p.application_id
                             where p.comments != "Processed"
                             select t).ToList();

                Session["datta1"] = data1;
                List<poco> lst = new List<poco>();

                foreach (var j in data1)
                {
                    poco pm = new poco();
                    pm.id = j.id;
                    pm.name = j.name;
                    pm.age = j.age;
                    pm.dob = j.dob;
                    pm.address = j.address;
                    pm.branch_Id = j.branch_Id;
                    pm.class_Id = j.class_Id;

                    lst.Add(pm);
                }
                // Session["datta"] = lst;
                return View(lst);
            }
            //var data = from t in db.Applications
            //           join p in db.ProcessedApplications on t.id equals p.application_id
            //           where p.comments == "Processed"
            //           orderby t.id ascending
            //           select t;
            // return View();
        }
    }

}