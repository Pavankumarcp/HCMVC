using HCMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HCMVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly IConfiguration _configuration;
        public PatientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> PatientDetails()
        {
            List<PatientViewModel> pat = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Patient/GetallPatients");
                if (result.IsSuccessStatusCode)
                {
                    pat = await result.Content.ReadAsAsync<List<PatientViewModel>>();
                }

            }
            return View(pat);
        }
        [Route("Patient/Details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            PatientViewModel pat = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Patient/GetPatientById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    pat = await result.Content.ReadAsAsync<PatientViewModel>();
                }
            }
            return View(pat);
        }
        [NonAction]
        public async Task<PatientViewModel> FormById(int id)
        {
            PatientViewModel pat = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Patient/GetPatientById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    pat = await result.Content.ReadAsAsync<PatientViewModel>();
                }
            }
            return pat;
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            PatientViewModel pat = await this.FormById(id);
            if (pat != null)
            {
                return View(pat);
            }
            ModelState.AddModelError("", "Server Error. Please try later");
            return View(pat);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(PatientViewModel pat)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Patient/DeletePatient/{pat.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("PatientDetails");
                }
            }
            return View();
        }
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(PatientViewModel pat)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Patient/CreatePatient", pat);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("PatientDetails");
                    }
                }
            }
            return View();
        }
        [HttpGet]

        public async Task<IActionResult> Edit(int Id)
        {
            if(ModelState.IsValid)
            {
                PatientViewModel pat = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Patient/GetPatientById/{Id}");
                    if (result.IsSuccessStatusCode)
                    {
                        pat = await result.Content.ReadAsAsync<PatientViewModel>();
                        return View(pat);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Patient doesn't exists");

                    }

                }
                
            }
            return View();

        }
        [HttpPost("Patient/UpdateDoctor/{id}")]

        public async Task<IActionResult> Edit(PatientViewModel form)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Patient/UpdateDoctor/{form.Id}", form);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("PatientDetails");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Server Error. Please try later");
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
