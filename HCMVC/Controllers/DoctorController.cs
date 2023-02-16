using HCMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HCMVC.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IConfiguration _configuration;
        public DoctorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> DoctorDetails()
        {
            List<DoctorViewModel> doc = new();
            using(var client=new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Doctor/GetallDoctors");
                if (result.IsSuccessStatusCode)
                {
                    doc=await result.Content.ReadAsAsync<List<DoctorViewModel>>();
                }
               
            }
            return View(doc);
        }
        [Route("Doctor/Details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            DoctorViewModel doc = null;
            using(var client=new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Doctor/GetDoctorById/{id}");
                if(result.IsSuccessStatusCode)
                {
                    doc=await result.Content.ReadAsAsync<DoctorViewModel>();
                }
            }
            return View(doc);
        }
        [NonAction]
        public async Task<DoctorViewModel> FormById(int id)
        {
            DoctorViewModel doc = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Doctor/GetDoctorById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    doc = await result.Content.ReadAsAsync<DoctorViewModel>();
                }
            }
            return doc;
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            DoctorViewModel doc = await this.FormById(id);
            if (doc != null)
            {
                return View(doc);
            }
            ModelState.AddModelError("", "Server Error. Please try later");
            return View(doc);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DoctorViewModel doc)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Doctor/DeleteDoctor/{doc.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("DoctorDetails");
                }
            }
            return View();
        }
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(DoctorViewModel doc)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Doctor/CreateDoctor", doc);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("DoctorDetails");
                    }
                }
            }
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
