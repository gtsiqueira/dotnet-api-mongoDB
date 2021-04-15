using System;
using api.Data.Collections;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace api.Controllers
{      
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);
            
            return StatusCode(201, "Infectado adicionado com sucesso!");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados);
        }

        [HttpPut]
        public ActionResult AtalizarInfectado([FromBody] InfectadoDto dto)
        {            
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("sexo", dto.Sexo));
                        
            return Ok("Atualização feita com sucesso!");
        }


        [HttpDelete("{dataNasc}")]
        public ActionResult Delete(DateTime dataNasc)
        {
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dataNasc));
                        
            return Ok("Registro deletado com sucesso!");
        }
    }   
}