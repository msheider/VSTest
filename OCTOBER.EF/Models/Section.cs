﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using static System.Collections.Specialized.BitVector32;

namespace OCTOBER.Server.Controllers.UD
{
    public class SchoolController : BaseController, GenericRestController<SectionDTO>
    {
        public SchoolController(OCTOBEROracleContext context,
                                IHttpContextAccessor httpContextAccessor,
                                IMemoryCache memoryCache)
                : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("Delete/{SchoolID}")]

        public async Task<IActionResult> Delete(int SectionID)
        {
            Debugger.Launch();
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Sections.Where(x => x.SectionId == SectionID).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Sections.Remove(itm);
                }
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Sections.Select(sp => new SectionDTO
                {
                    Capacity = sp.Capacity,
                    CourseNo = sp.CourseNo,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    InstructorId = sp.InstructorId,
                    Location = sp.Location,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    SchoolId = sp.SchoolId,
                    SectionId = sp.SectionId,
                    SectionNo = sp.SectionNo,
                    StartDateTime = sp.StartDateTime

                })
                .ToListAsync();
                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }


        [HttpGet]
        [Route("Get/{SchoolID}/{SectionID}")]
        public async Task<IActionResult> Get(int SchoolID, int SectionID)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                SectionDTO? result = await _context.Sections
                    .Where(x => x.SchoolId == SchoolID)
                    .Where(x => x.SectionId == SectionID)
                    .Select(sp => new SectionDTO
                    {
                        Capacity = sp.Capacity,
                        CourseNo = sp.CourseNo,
                        CreatedBy = sp.CreatedBy,
                        CreatedDate = sp.CreatedDate,
                        InstructorId = sp.InstructorId,
                        Location = sp.Location,
                        ModifiedBy = sp.ModifiedBy,
                        ModifiedDate = sp.ModifiedDate,
                        SchoolId = sp.SchoolId,
                        SectionId = sp.SectionId,
                        SectionNo = sp.SectionNo,
                        StartDateTime = sp.StartDateTime
                    })
                .SingleOrDefaultAsync();
                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        public Task<IActionResult> Get(int KeyVal)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] SectionDTO _SectionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Sections.Where(x => x.SectionId == _SectionDTO.SectionId).FirstOrDefaultAsync();

                if (itm == null)
                {
                    Section s = new Section
                    {

                        SectionId = _SectionDTO.SectionId,
                        SectionNo = _SectionDTO.SectionNo,
                        CourseNo = _SectionDTO.CourseNo,
                        StartDateTime = _SectionDTO.StartDateTime,
                        Location = _SectionDTO.Location,
                        InstructorId = _SectionDTO.InstructorId,
                        Capacity = _SectionDTO.Capacity
                    };
                    _context.Sections.Add(s);
                    await _context.SaveChangesAsync();
                    await _context.Database.CommitTransactionAsync();
                }
                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }


        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] SectionDTO _SectionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Sections.Where(x => x.SectionId == _SectionDTO.SectionId).FirstOrDefaultAsync();

                itm.SectionNo = _SectionDTO.SectionNo;
                itm.CourseNo = _SectionDTO.CourseNo;
                itm.StartDateTime = _SectionDTO.StartDateTime;
                itm.Location = _SectionDTO.Location;
                itm.InstructorId = _SectionDTO.InstructorId;
                itm.Capacity = _SectionDTO.Capacity;

                _context.Sections.Update(itm);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
    }

}