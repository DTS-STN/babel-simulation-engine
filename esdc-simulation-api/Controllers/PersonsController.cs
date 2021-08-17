using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using esdc_simulation_base.Src.Storage;
using esdc_simulation_base.Src.Classes;
using maternity_benefits;
using maternity_benefits.Storage.Mock;

using esdc_simulation_classes.MaternityBenefits;

namespace esdc_simulation_api.Controllers
{
    [ServiceFilter(typeof(PasswordFilterAttribute))]
    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IStorePersons<MaternityBenefitsPerson> _personStore;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(
            IStorePersons<MaternityBenefitsPerson> personStore,
            ILogger<PersonsController> logger)
        {
            _personStore = personStore;
            _logger = logger;
        }

        /// <summary>
        /// Get all the persons from the simulation store that are used for running a simulation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<MaternityBenefitsPerson>> GetPersons()
        {
            var persons = _personStore.GetAllPersons();
            return Ok(persons);
        }

        /// <summary>
        /// Add a list of persons to the simulation store
        /// </summary>
        /// <param name="personsRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPersons(List<MaternityBenefitsPersonRequest> personsRequest)
        {
            _logger.LogInformation("Adding {0} Persons", personsRequest.Count);
            var persons = personsRequest.Select(Convert);
            _personStore.AddPersons(persons);
            return Ok();
        }

        /// <summary>
        /// Delete all persons from the simulation store
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult DeletePersons()
        {
            _logger.LogInformation("Clearing the persons data store");
            _personStore.Clear();
            return Ok();
        }

        /// <summary>
        /// Delete a single person (by Id) from the simulation store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult DeletePerson(Guid id)
        {
            try {
                _personStore.DeletePerson(id);
                return Ok();
            } catch (NotFoundException e) {
                return BadRequest(e.Message);
            } 
        }

        private MaternityBenefitsPerson Convert(MaternityBenefitsPersonRequest req) {
            return new MaternityBenefitsPerson() {
                Id = Guid.NewGuid(),
                AverageIncome = req.AverageIncome,
                SpokenLanguage = req.SpokenLanguage,
                EducationLevel = req.EducationLevel,
                Province = req.Province,
                Age = req.Age
            };
        }

    }
}
