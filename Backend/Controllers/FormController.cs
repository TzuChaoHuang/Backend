using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Data;
using Dapper;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : ControllerBase
    {
        private readonly DbConnection _db;
        private const string UploadFolder = "uploads";

        public FormController(DbConnection db)
        {
            _db = db;
        }

        /// <summary>
        /// Create a new Form
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateForm([FromBody] CreateFormRequest request)
        {
            using var connection = _db.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var formSql = @"
                    INSERT INTO Form (Id, IsPassword, Password, Description, ExpiredDate, CreatedDate)
                    VALUES (@Id, @IsPassword, @Password, @Description, @ExpiredDate, @CreatedDate);";

                var formId = Guid.NewGuid();
                var formParameters = new
                {
                    Id = formId,
                    request.IsPassword,
                    request.Password,
                    request.Description,
                    ExpiredDate = DateTime.UtcNow.AddDays(request.ExpiredDays),
                    CreatedDate = DateTime.UtcNow
                };

                await connection.ExecuteAsync(formSql, formParameters, transaction);

                if (request.FileNames != null && request.FileNames.Any())
                {
                    var filesSql = @"
                        INSERT INTO FormFiles (FormId, FileName)
                        VALUES (@FormId, @FileName)";

                    foreach (var fileName in request.FileNames)
                    {
                        await connection.ExecuteAsync(filesSql, new { FormId = formId, FileName = fileName }, transaction);
                    }
                }

                transaction.Commit();
                return Ok(new { Id = formId });
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Get the Form Infomation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForm(Guid id)
        {
            using var connection = _db.CreateConnection();
            var sql = "SELECT IsPassword, Description FROM Form WHERE Id = @Id";
            
            var form = await connection.QueryFirstOrDefaultAsync<FormResponse>(sql, new { Id = id });
            if (form == null)
            {
                return NotFound();
            }

            return Ok(form);
        }

        /// <summary>
        /// Validate the password for a form
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{id}/validate")]
        public async Task<IActionResult> ValidatePassword(Guid id, [FromBody] ValidatePasswordRequest request)
        {
            using var connection = _db.CreateConnection();
            
            var formSql = @"
                SELECT f.*
                FROM Form f 
                WHERE f.Id = @Id
                And f.Password = @Password";

            var form = await connection.QueryFirstOrDefaultAsync<dynamic>(formSql, new { Id = id, Password = request.Password });
            
            if (form == null)
            {
                return NotFound("Form not found");
            }

            if (DateTime.UtcNow > form.ExpiredDate)
            {
                return BadRequest("Form has expired");
            }

            var filesSql = "SELECT FileName FROM FormFiles WHERE FormId = @FormId";
            var files = await connection.QueryAsync<string>(filesSql, new { FormId = id });
            
            var fileInfos = new List<FileInfo>();
            foreach (var fileName in files)
            {
                var filePath = Path.Combine(UploadFolder, fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    continue;
                }
                fileInfos.Add(new FileInfo { FileName = fileName, FilePath = filePath });
            }

            return Ok(new { Files = fileInfos });
        }
    }

    public class FileInfo
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}