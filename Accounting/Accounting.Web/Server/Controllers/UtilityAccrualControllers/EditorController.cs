using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.DataAccess.Base;
using UtilityAccrual.Shared.Models;

namespace Accounting.Web.Server.Controllers.UtilityAccrualControllers
{
    [Authorize(Roles = "UtilityAccrualUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class EditorController : ControllerBase
    {
        private readonly SqlClient _sql;

        public EditorController(SqlClient sql)
        {
            _sql = sql;
        }

        [HttpGet]
        public async Task<IEnumerable<Editor>> Get()
        {
            return await _sql.GetEditors();
        }

        [HttpGet("{id}")]
        public async Task<Editor> Get(int id)
        {
            return await _sql.GetEditor(id);
        }

        [HttpGet("{name}")]
        public async Task<IEnumerable<Editor>> Get(string name)
        {
            return await _sql.GetEditorsByName(name);
        }
        [Route("[action]/{segment}")]
        [HttpGet]
        public async Task<IEnumerable<Editor>> GetEditorsBySegment(int segment)
        {
            return await _sql.GetEditorsBySegment(segment);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Editor editor)
        {
            if (editor is null)
            {
                return BadRequest("Editor is null.");
            }

            var editors = await _sql.GetEditorsBySegment((int)editor.Segment);
            if (editors.Any())
            {
                if (editors.Select(e => e.User.ToLower()).Contains(editor.User.ToLower()))
                {
                    return BadRequest("This user already exists.");
                }
            }

            await _sql.Insert(editor);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateEditor([FromBody] Editor editor)
        {
            if (editor is null)
                return BadRequest("Editor is null.");
            try { await _sql.UpdateEditor(editor); }
            catch { return BadRequest("Unable to update the editor"); }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _sql.DeleteEditor(id);
        }
    }
}
